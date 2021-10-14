using Dragablz;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;

namespace XFiler;

internal sealed class TabsViewModel : BaseViewModel, ITabsViewModel, IDisposable
{
    #region Private Fields

    private ITabFactory _tabFactory;
    private IWindowFactory _windowFactory;

    #endregion

    #region Public Properties

    public IInterTabClient InterTabClient { get; private set; } 

    public ObservableCollection<ITabItemModel> TabItems { get; private set; } = null!;

    public ItemActionCallback ClosingTabItemHandler { get; private set; }

    public ITabItemModel? CurrentTabItem { get; set; }

    public IReadOnlyCollection<IMenuItemViewModel> Bookmarks { get; private set; }

    public Func<ITabItemModel> Factory { get; private set; }

    #endregion

    #region Commands

    public ICommand CreateNewTabItemCommand { get; }

    public ICommand DuplicateTabCommand { get; }

    public ICommand CreateSettingsTabCommand { get; }

    public ICommand CreateBookmarksDispatcherPageCommand { get; }

    public ICommand CloseAllTabsCommand { get; }

    public DelegateCommand<object> OpenTabItemInNewWindowCommand { get; }

    public DelegateCommand<object> CloseOtherTabsCommand { get; }

    #endregion

    #region Constructor

    public TabsViewModel(IInterTabClient tabClient,
        ITabFactory tabFactory,
        IWindowFactory windowFactory,
        IBookmarksManager bookmarksManager)
    {
        _tabFactory = tabFactory;
        _windowFactory = windowFactory;

        InterTabClient = tabClient;
        Bookmarks = bookmarksManager.Bookmarks;
        ClosingTabItemHandler = ClosingTabItemHandlerImpl;
        
        CreateNewTabItemCommand = new DelegateCommand<object>(OnCreateNewTabItem);
       
        OpenTabItemInNewWindowCommand = new DelegateCommand<object>(OnOpenTabItemInNewWindow, OnCanOpenTabItemInNewWindow);
       
        DuplicateTabCommand = new DelegateCommand<object>(OnDuplicate);
        
        CloseOtherTabsCommand = new DelegateCommand<object>(OnCloseOtherTabs, CanCloseAllTabs);

        CreateSettingsTabCommand = new DelegateCommand(OnOpenSettings);
        
        CreateBookmarksDispatcherPageCommand = new DelegateCommand(OnOpenBookmarksDispatcher);

        CloseAllTabsCommand = new DelegateCommand(OnCloseAllTabs);

        Factory = CreateTabVm;
    }

    #endregion

    #region Public Methods

    public void Init(IEnumerable<ITabItemModel> initTabs)
    {
        TabItems = new ObservableCollection<ITabItemModel>(initTabs);
        TabItems.CollectionChanged += TabItemsOnCollectionChanged;
    }

    public void OnOpenNewTab(IFileSystemModel fileEntityViewModel, bool isSelectNewTab = false)
    {
        if (fileEntityViewModel is DirectoryViewModel directoryViewModel)
        {
            var tab = _tabFactory.CreateExplorerTab(directoryViewModel.DirectoryInfo);

            if (tab == null)
                return;

            TabItems.Add(tab);

            if (isSelectNewTab)
                CurrentTabItem = tab;
        }
    }

    public void OnOpenNewTab(IEnumerable<IFileSystemModel> models, bool isSelectNewTab = false)
    {
        foreach (var model in models)
        {
            if (model.Info is DirectoryInfo info)
            {
                var tab = _tabFactory.CreateExplorerTab(info);
                if (tab == null)
                    continue;

                TabItems.Add(tab);

                if (isSelectNewTab)
                    CurrentTabItem = tab;
            }
        }
    }

    public void Dispose()
    {
        //CurrentTabItem?.Dispose();

        CurrentTabItem = null!;
        InterTabClient = null!;
        Bookmarks = null!;
        Factory =null!;
        ClosingTabItemHandler = null!;
        _tabFactory = null!;
        _windowFactory = null!;
    }

    #endregion

    #region Private Methods

    private void OnCreateNewTabItem(object? obj) 
        => TabItems.Add(_tabFactory.CreateMyComputerTab());

    private bool OnCanOpenTabItemInNewWindow(object? obj) 
        => TabItems.Count > 1;

    private void OnOpenTabItemInNewWindow(object? obj)
    {
        if (obj is not ITabItemModel directoryTabItem)
            return;

        TabItems.Remove(directoryTabItem);

        _windowFactory.OpenTabInNewWindow(directoryTabItem);
    }

    private void OnDuplicate(object? obj)
    {
        if (obj is not ITabItemModel directoryTabItem)
            return;

        var tab = _tabFactory.CreateTab(directoryTabItem.Route);

        if (tab != null)
            TabItems.Add(tab);
    }

    private bool CanCloseAllTabs(object? obj) 
        => TabItems.Count > 1;

    private void OnCloseOtherTabs(object? obj)
    {
        if (obj is not ITabItemModel tabItem)
            return;

        var removedItems = TabItems.Where(i => i != tabItem).ToList();

        foreach (var item in removedItems)
            TabablzControl.CloseItem(item);
    }

    private void OnCloseAllTabs()
    {
        var removedItems = TabItems.ToList();

        foreach (var item in removedItems)
            TabablzControl.CloseItem(item);
    }

    private void OnOpenSettings()
    {
        var tab = _tabFactory.CreateTab(SpecialRoutes.Settings);

        if (tab == null)
            return;

        TabItems.Add(tab);
        CurrentTabItem = tab;
    }

    private void OnOpenBookmarksDispatcher()
    {
        var tab = _tabFactory.CreateTab(SpecialRoutes.BookmarksDispatcher);

        if (tab == null)
            return;

        TabItems.Add(tab);
        CurrentTabItem = tab;
    }

    private ITabItemModel CreateTabVm() => _tabFactory.CreateMyComputerTab();

    private void TabItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OpenTabItemInNewWindowCommand.RaiseCanExecuteChanged();
        CloseOtherTabsCommand.RaiseCanExecuteChanged();
    }

    private static void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
    {
        var viewModel = args.DragablzItem.DataContext as ITabItemModel;
        viewModel?.Dispose();
    }

    #endregion

   
}