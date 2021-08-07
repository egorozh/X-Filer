using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace XFiler.SDK
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
        private readonly List<BookmarkItem> _items;

        #endregion

        #region Public Properties

        public IReadOnlyCollection<IMenuItemViewModel> Bookmarks => _bookmarks;

        #endregion

        #region Commands

        public DelegateCommand<string> AddBookmarkCommand { get; }
        public DelegateCommand<IList<object>> BookmarkClickCommand { get; }

        #endregion

        #region Constructor

        public BookmarksManager(IMenuItemFactory itemFactory)
        {
            _itemFactory = itemFactory;

            BookmarkClickCommand = new DelegateCommand<IList<object>>(OnBookmarkClicked);
            AddBookmarkCommand = new DelegateCommand<string>(OnAddBookmark);

            _items = OpenBookmarksFile();

            _bookmarks = CreateMenuItemViewModels(_items);
        }

        private ObservableCollection<IMenuItemViewModel> CreateMenuItemViewModels(IList<BookmarkItem>? items)
        {
            var menuVms = new ObservableCollection<IMenuItemViewModel>();

            if (items == null || !items.Any())
                return menuVms;

            foreach (var bookmarkItem in items)
            {
                var vm = _itemFactory
                    .CreateItem(bookmarkItem, CreateMenuItemViewModels(bookmarkItem.Children),
                        BookmarkClickCommand);

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

        private void OnAddBookmark(string path)
        {
            if (!Directory.Exists(path))
                return;

            _items.Add(new BookmarkItem
            {
                Path = path
            });

            try
            {
                var json = JsonSerializer.Serialize(_items);

                File.WriteAllText(BookmarksFileName, json);
            }
            catch (Exception e)
            {
            }

            _bookmarks.Clear();

            foreach (var viewModel in CreateMenuItemViewModels(_items))
            {
                _bookmarks.Add(viewModel);
            }
        }

        #endregion
    }
}