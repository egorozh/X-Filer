using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text.Json;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
{
    internal class BookmarksManager : BaseViewModel, IBookmarksManager
    {
        #region Private Fields

        private readonly IMenuItemFactory _itemFactory;

        #endregion

        #region Constants

        private const string BookmarksFileName = "bookmarks.json";

        #endregion

        #region Private Fields

        private readonly ObservableCollection<IMenuItemViewModel> _bookmarks;

        #endregion

        #region Public Properties

        public IReadOnlyCollection<IMenuItemViewModel> Bookmarks => _bookmarks;
        public IMenuItemViewModel? SelectedItem { get; private set; }

        #endregion

        #region Commands

        public DelegateCommand<IPageModel> AddBookmarkCommand { get; }
        public DelegateCommand<IList<object>> BookmarkClickCommand { get; }

        public DelegateCommand RemoveCommand { get; }

        #endregion

        #region Constructor

        public BookmarksManager(IMenuItemFactory itemFactory)
        {
            _itemFactory = itemFactory;

            BookmarkClickCommand = new DelegateCommand<IList<object>>(OnBookmarkClicked);
            AddBookmarkCommand = new DelegateCommand<IPageModel>(OnAddBookmark);

            RemoveCommand = new DelegateCommand(OnRemove, CanRemove);

            var items = OpenBookmarksFile();

            _bookmarks = CreateMenuItemViewModels(items);
            _bookmarks.CollectionChanged += BookmarksOnCollectionChanged;
        }

        private ObservableCollection<IMenuItemViewModel> CreateMenuItemViewModels(IList<BookmarkItem>? items)
        {
            var menuVms = new ObservableCollection<IMenuItemViewModel>();

            if (items == null || !items.Any())
                return menuVms;

            foreach (var bookmarkItem in items)
            {
                var vm = CreateItem(bookmarkItem);

                menuVms.Add(vm);
            }

            return menuVms;
        }

        private List<BookmarkItem> OpenBookmarksFile()
        {
            if (File.Exists(BookmarksFileName))
            {
                var json = File.ReadAllText(BookmarksFileName);

                try
                {
                    return JsonSerializer.Deserialize<List<BookmarkItem>>(json);
                }
                catch (Exception e)
                {
                }
            }


            return new List<BookmarkItem>();
        }

        private void BookmarksOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            try
            {
                List<BookmarkItem> items = new();

                foreach (var model in _bookmarks)
                {
                    items.Add(model.GetItem());
                }

                var json = JsonSerializer.Serialize(items);

                File.WriteAllText(BookmarksFileName, json);
            }
            catch (Exception e)
            {
            }
        }

        private void VmOnIsSelectedChanged(object? sender, EventArgs e)
        {
            if (sender is IMenuItemViewModel { IsSelected: true } menuItem)
            {
                SelectedItem = menuItem;
            }

            RemoveCommand.RaiseCanExecuteChanged();
        }

        private MenuItemViewModel CreateItem(BookmarkItem? bookmarkItem)
        {
            var vm = _itemFactory
                .CreateItem(bookmarkItem, CreateMenuItemViewModels(bookmarkItem.Children),
                    BookmarkClickCommand);

            vm.IsSelectedChanged += VmOnIsSelectedChanged;

            return vm;
        }

        #endregion

        #region Commands Methods

        private void OnBookmarkClicked(IList<object> parameters)
        {
            if (parameters.Count == 2 &&
                parameters[0] is XFilerRoute url &&
                parameters[1] is ITabItemModel tabItemViewModel)
            {
                tabItemViewModel.Open(url);
            }
        }

        private void OnAddBookmark(IPageModel page)
        {
            var route = page.Route;

            _bookmarks.Add(CreateItem(new BookmarkItem
            {
                Path = route.FullName
            }));
        }

        private bool CanRemove() => SelectedItem != null;

        private void OnRemove()
        {
            var removedVm = SelectedItem;

            removedVm.IsSelectedChanged -= VmOnIsSelectedChanged;
            removedVm.Dispose();

            var parent = FindParent(_bookmarks, removedVm);

            if (parent == null)
                _bookmarks.Remove(removedVm);
            else
                parent.Items.Remove(removedVm);
        }

        private static IMenuItemViewModel? FindParent(IEnumerable<IMenuItemViewModel> items,
            IMenuItemViewModel removedVm, IMenuItemViewModel? parent = null)
        {
            foreach (var model in items)
            {
                if (model == removedVm)
                    return parent;

                var par = FindParent(model.Items, removedVm, model);

                if (par != null)
                    return par;
            }

            return null;
        }

        #endregion
    }
}