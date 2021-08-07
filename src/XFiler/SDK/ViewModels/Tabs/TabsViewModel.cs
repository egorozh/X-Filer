using Dragablz;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace XFiler.SDK
{
    public class TabsViewModel : BaseViewModel, ITabsViewModel
    {
        #region Private Fields

        private readonly ITabFactory _tabFactory;
        private readonly IWindowFactory _windowFactory;
        private readonly ISettingsTabFactory _settingsFactory;

        #endregion

        #region Public Properties

        public ITabClient InterTabClient { get; }

        public ObservableCollection<ITabItemModel> TabItems { get; }

        public ItemActionCallback ClosingTabItemHandler { get; }

        public ITabItemModel? CurrentTabItem { get; set; }

        public IReadOnlyCollection<IMenuItemViewModel> Bookmarks { get; }

        public Func<ITabItemModel> Factory { get; }
            
        #endregion

        #region Commands

        public DelegateCommand<object> CreateNewTabItemCommand { get; }
        public DelegateCommand<object> OpenTabItemInNewWindowCommand { get; }
        public DelegateCommand<object> DuplicateTabCommand { get; }
        public DelegateCommand<object> CloseAllTabsCommand { get; }
        public DelegateCommand CreateSettingsTabCommand { get; }

        #endregion

        #region Constructor

        public TabsViewModel(ITabClient tabClient,
            ITabFactory tabFactory,
            IWindowFactory windowFactory,
            IBookmarksManager bookmarksManager,
            IEnumerable<ITabItemModel> init,
            ISettingsTabFactory settingsFactory)
        {
            _tabFactory = tabFactory;
            _windowFactory = windowFactory;
            _settingsFactory = settingsFactory;


            InterTabClient = tabClient;
            Bookmarks = bookmarksManager.Bookmarks;
            ClosingTabItemHandler = ClosingTabItemHandlerImpl;
            CreateNewTabItemCommand = new DelegateCommand<object>(OnCreateNewTabItem);
            OpenTabItemInNewWindowCommand =
                new DelegateCommand<object>(OnOpenTabItemInNewWindow, OnCanOpenTabItemInNewWindow);
            DuplicateTabCommand = new DelegateCommand<object>(OnDuplicate);
            CloseAllTabsCommand = new DelegateCommand<object>(OnCloseAllTabs, CanCloseAllTabs);

            CreateSettingsTabCommand = new DelegateCommand(OnOpenSettings);

            TabItems = new ObservableCollection<ITabItemModel>(init);
            //CurrentTabItem = TabItems.FirstOrDefault();

            Factory = CreateTabVm;

            TabItems.CollectionChanged += TabItemsOnCollectionChanged;
        }

        #endregion

        #region Public Methods

        public void OnOpenNewTab(FileEntityViewModel fileEntityViewModel, bool isSelectNewTab = false)
        {
            if (fileEntityViewModel is DirectoryViewModel directoryViewModel)
            {
                var tab = _tabFactory.CreateExplorerTab(directoryViewModel.DirectoryInfo);
                TabItems.Add(tab);

                if (isSelectNewTab)
                    CurrentTabItem = tab;
            }
        }

        #endregion

        #region Private Methods

        private void OnCreateNewTabItem(object? obj)
        {
            TabItems.Add(_tabFactory.CreateMyComputerTab());
        }

        private bool OnCanOpenTabItemInNewWindow(object? obj) => TabItems.Count > 1;

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

            TabItems.Add(_tabFactory
                .CreateTab(directoryTabItem.Url));
        }

        private bool CanCloseAllTabs(object? obj) => TabItems.Count > 1;

        private void OnCloseAllTabs(object? obj)
        {
            if (obj is not ITabItemModel tabItem)
                return;

            var removedItems = TabItems.Where(i => i != tabItem).ToList();

            foreach (var item in removedItems)
                TabItems.Remove(item);
        }

        private void OnOpenSettings()
        {
            _settingsFactory.OpenSettingsTab();
        }

        private ITabItemModel CreateTabVm() => _tabFactory.CreateMyComputerTab();

        private void TabItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OpenTabItemInNewWindowCommand.RaiseCanExecuteChanged();
            CloseAllTabsCommand.RaiseCanExecuteChanged();
        }

        private void ClosingTabItemHandlerImpl(ItemActionCallbackArgs<TabablzControl> args)
        {
            //in here you can dispose stuff or cancel the close

            //here's your view model:
            var viewModel = args.DragablzItem.DataContext as ITabItemModel;
            viewModel?.Dispose();
            //here's how you can cancel stuff:
            //args.Cancel(); 
        }

        #endregion
    }
}