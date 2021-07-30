using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
{
    internal class BookmarksManager : BaseViewModel, IBookmarksManager
    {
        #region Constants

        private const string BookmarksFileName = "bookmarks.json";

        #endregion

        #region Private Fields

        private readonly ExtensionToImageFileConverter _converter;

        private readonly ObservableCollection<MenuItemViewModel> _bookmarks;
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

        public BookmarksManager(ExtensionToImageFileConverter converter)
        {
            _converter = converter;

            BookmarkClickCommand = new DelegateCommand<IList<object>>(OnBookmarkClicked);
            AddBookmarkCommand = new DelegateCommand<string>(OnAddBookmark);

            _items = OpenBookmarksFile();

            _bookmarks = CreateMenuItemViewModels(_items);
        }

        private ObservableCollection<MenuItemViewModel> CreateMenuItemViewModels(IList<BookmarkItem> items)
        {
            var menuVms = new ObservableCollection<MenuItemViewModel>();

            if (items == null || !items.Any())
                return menuVms;

            var applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;

            var iconsDirectory = new DirectoryInfo(Path.Combine(applicationDirectory, "Resources", "Icons"));

            foreach (var bookmarkItem in items)
            {
                var path = bookmarkItem.Path;

                var vm = new MenuItemViewModel(path)
                {
                    Items = CreateMenuItemViewModels(bookmarkItem.Children)
                };

                if (path == null)
                {
                    vm.Header = bookmarkItem.BookmarkFolderName;
                    vm.IconPath = Path.Combine(iconsDirectory.FullName, IconName.BookmarkFolder + ".svg");
                }
                else
                {
                    try
                    {
                        DefinitionPathToVm(vm, path, iconsDirectory);
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }

                menuVms.Add(vm);
            }

            return menuVms;
        }

        private void DefinitionPathToVm(MenuItemViewModel vm, string path, DirectoryInfo iconsDirectory)
        {
            vm.Command = BookmarkClickCommand;

            var attr = File.GetAttributes(path);

            if (attr.HasFlag(FileAttributes.Directory))
            {
                vm.Header = new DirectoryInfo(path).Name;
                vm.IconPath = Path.Combine(iconsDirectory.FullName, IconName.Folder + ".svg");
            }
            else
            {
                var extension = new FileInfo(path).Extension;

                vm.Header = new FileInfo(path).Name;
                vm.IconPath =
                    _converter.GetImagePath(string.IsNullOrEmpty(extension) ? "" : extension.Substring(1))
                        .FullName;
            }
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
                parameters[0] is string path &&
                parameters[1] is ExplorerTabItemViewModel tabItemViewModel)
            {
                tabItemViewModel.OpenBookmark(path);
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