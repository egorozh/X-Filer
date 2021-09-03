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

        public DelegateCommand AddFolderCommand { get; }
        public DelegateCommand RemoveCommand { get; }

        #endregion

        #region Constructor

        public BookmarksManager(IMenuItemFactory itemFactory)
        {
            _itemFactory = itemFactory;

            BookmarkClickCommand = new DelegateCommand<IList<object>>(OnBookmarkClicked);
            AddBookmarkCommand = new DelegateCommand<IPageModel>(OnAddBookmark);

            RemoveCommand = new DelegateCommand(OnRemove, CanRemove);
            AddFolderCommand = new DelegateCommand(OnAddFolder);

            var items = OpenBookmarksFile();

            _bookmarks = CreateMenuItemViewModels(items);
            _bookmarks.CollectionChanged += BookmarksOnCollectionChanged;
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
            var removedVm = SelectedItem 
                            ?? throw new ArgumentNullException(nameof(SelectedItem));

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
        
        private void OnAddFolder()
        {
            IList<IMenuItemViewModel> parentCollection = _bookmarks;
            var insertIndex = _bookmarks.Count;

            if (SelectedItem != null)
            {
                var parent = FindParent(_bookmarks, SelectedItem);

                if (parent != null)
                    parentCollection = parent.Items;

                insertIndex = parentCollection.IndexOf(SelectedItem) + 1;
            }
            
            parentCollection.Insert(insertIndex, CreateItem(new BookmarkItem
            {
                BookmarkFolderName = "Новая папка"
            }));
        }

        #endregion

        #region Private Methods

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

        private static List<BookmarkItem> OpenBookmarksFile()
        {
            if (File.Exists(BookmarksFileName))
            {
                var json = File.ReadAllText(BookmarksFileName);

                try
                {
                    var res = JsonSerializer.Deserialize<List<BookmarkItem>>(json);
                    if (res != null)
                        return res;
                }
                catch (Exception)
                {
                    // ignored
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
            catch (Exception)
            {
                // ignored
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

        private MenuItemViewModel CreateItem(BookmarkItem bookmarkItem)
        {
            var vm = _itemFactory
                .CreateItem(bookmarkItem, CreateMenuItemViewModels(bookmarkItem.Children),
                    BookmarkClickCommand);

            vm.IsSelectedChanged += VmOnIsSelectedChanged;

            return vm;
        }

        #endregion
    }
}