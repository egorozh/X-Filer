using GongSolutions.Wpf.DragDrop;
using System.Collections.Specialized;
using System.IO;
using System.Text.Json;
using XFiler.DragDrop;

namespace XFiler;

internal sealed class BookmarksManager : BaseViewModel, IBookmarksManager
{
    #region Private Fields

    private readonly IMenuItemFactory _itemFactory;
    private readonly ILogger _logger;
    private readonly IStorage _storage;

    #endregion

    #region Private Fields

    private readonly ObservableCollection<IMenuItemViewModel> _bookmarks;

    #endregion

    #region Public Properties

    public IReadOnlyCollection<IMenuItemViewModel> Bookmarks => _bookmarks;
    public IMenuItemViewModel? SelectedItem { get; private set; }

    public IDragSource DragSource { get; }
    public IBookmarksDispatcherDropTarget DropTarget { get; }

    #endregion

    #region Commands

    public DelegateCommand<object> AddBookmarkCommand { get; }

    public DelegateCommand<IList<object>> BookmarkClickCommand { get; }

    public DelegateCommand AddFolderCommand { get; }
    public DelegateCommand RemoveCommand { get; }

    #endregion

    #region Constructor

    public BookmarksManager(IMenuItemFactory itemFactory,
        ILogger logger,
        IStorage storage,
        IDragSource dragSource,
        IBookmarksDispatcherDropTarget dropTarget)
    {
        _itemFactory = itemFactory;
        _logger = logger;
        _storage = storage;

        DragSource = dragSource;
        DropTarget = dropTarget;

        BookmarkClickCommand = new DelegateCommand<IList<object>>(OnBookmarkClicked);
        AddBookmarkCommand = new DelegateCommand<object>(OnAddBookmark);

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
            parameters[0] is Route url &&
            parameters[1] is ITabItemModel tabItemViewModel)
        {
            tabItemViewModel.Open(url);
        }
    }

    private void OnAddBookmark(object parameter)
    {
        var vm = parameter switch
        {
            IPageModel page => CreateItem(new BookmarkItem
            {
                Path = page.Route.FullName
            }),
            IFileSystemModel fileSystemModel => CreateItem(new BookmarkItem
            {
                Path = fileSystemModel.Info.FullName
            }),
            _ => null
        };

        if (vm != null)
            _bookmarks.Add(vm);
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

        var vm = CreateItem(new BookmarkItem
        {
            BookmarkFolderName = "Новая папка"
        });

        if (vm != null)
            parentCollection.Insert(insertIndex, vm);
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

            if (vm != null)
                menuVms.Add(vm);
        }

        return menuVms;
    }

    private List<BookmarkItem> OpenBookmarksFile()
    {
        if (File.Exists(_storage.Bookmarks))
        {
            var json = File.ReadAllText(_storage.Bookmarks);

            try
            {
                var res = JsonSerializer.Deserialize<List<BookmarkItem>>(json);
                if (res != null)
                    return res;
            }
            catch (Exception e)
            {
                _logger.Error(e, "OpenBookmarksFile");
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

            File.WriteAllText(_storage.Bookmarks, json);

            _logger.Debug("Save Bookmarks File success");
        }
        catch (Exception e)
        {
            _logger.Error(e, "Save Bookmarks File");
        }
    }

    private void VmOnIsSelectedChanged(object? sender, EventArgs e)
    {
        if (sender is IMenuItemViewModel {IsSelected: true} menuItem)
        {
            SelectedItem = menuItem;
        }

        RemoveCommand.RaiseCanExecuteChanged();
    }

    private IMenuItemViewModel? CreateItem(BookmarkItem bookmarkItem)
    {
        var vm = _itemFactory
            .CreateItem(bookmarkItem, CreateMenuItemViewModels(bookmarkItem.Children),
                BookmarkClickCommand);

        if (vm is not null)
            vm.IsSelectedChanged += VmOnIsSelectedChanged;

        return vm;
    }

    #endregion
}