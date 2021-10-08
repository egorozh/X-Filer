namespace XFiler;

internal sealed class WindowFactory : IWindowFactory
{
    private readonly Func<ITabsFactory> _tabsFactory;
    private readonly Func<ITabFactory> _explorerTabFactory;

    public DelegateCommand<object> OpenNewWindowCommand { get; }

    public WindowFactory(Func<ITabsFactory> tabsFactory, Func<ITabFactory> explorerTabFactory)
    {
        _tabsFactory = tabsFactory;
        _explorerTabFactory = explorerTabFactory;

        OpenNewWindowCommand = new DelegateCommand<object>(OnOpenNewWindow);
    }

    public void OpenTabInNewWindow(ITabItemModel tabItem)
    {
        var tabsVm = _tabsFactory.Invoke().CreateTabsViewModel(new[]
        {
            tabItem
        });

        ShowNewWindow(tabsVm, new Point(24, 24));
    }

    public void OpenTabInNewWindow(IEnumerable<ITabItemModel> tabs)
    {
        var tabsVm = _tabsFactory.Invoke().CreateTabsViewModel(tabs);

        ShowNewWindow(tabsVm, new Point(24, 24));
    }

    public IMainWindow GetWindowWithRootTab()
    {
        var tabsViewModel = _tabsFactory.Invoke().CreateTabsViewModel(new[]
        {
            _explorerTabFactory.Invoke().CreateMyComputerTab()
        });

        return new MainWindow
        {
            DataContext = tabsViewModel
        };
    }

    private static void ShowNewWindow(ITabsViewModel mvm, Point location)
    {
        var currentApp = Application.Current ?? throw new ArgumentNullException("Application.Current");

        var activeWindow = currentApp.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                           ?? currentApp.MainWindow;

        if (activeWindow != null)
        {
            MainWindow mainWindow = new()
            {
                DataContext = mvm,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = activeWindow.Left + location.X,
                Top = activeWindow.Top + location.Y
            };

            mainWindow.Show();
        }
        else
        {
            MainWindow mainWindow = new()
            {
                DataContext = mvm,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            mainWindow.Show();
        }

       
    }

    private void OnOpenNewWindow(object parameter)
    {
        if (parameter is IDirectoryModel model)
        {
            var vm = _explorerTabFactory
                .Invoke()
                .CreateExplorerTab(model.DirectoryInfo);

            if (vm != null) 
                OpenTabInNewWindow(vm);
        }
        else if (parameter is IEnumerable items)
        {
            List<ITabItemModel> tabs = items.OfType<IDirectoryModel>()
                .Select(directoryModel => _explorerTabFactory.Invoke()
                    .CreateExplorerTab(directoryModel.DirectoryInfo))
                .OfType<ITabItemModel>()
                .ToList();

            OpenTabInNewWindow(tabs);
        }
    }
}