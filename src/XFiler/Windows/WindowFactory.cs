using System;
using System.Linq;
using System.Windows;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
{
    internal class WindowFactory : IWindowFactory
    {
        private readonly Func<ITabsFactory> _tabsFactory;
        private readonly Func<IExplorerTabFactory> _explorerTabFactory;

        public DelegateCommand<FileEntityViewModel> OpenNewWindowCommand { get; }

        public WindowFactory(Func<ITabsFactory> tabsFactory, Func<IExplorerTabFactory> explorerTabFactory)
        {
            _tabsFactory = tabsFactory;
            _explorerTabFactory = explorerTabFactory;

            OpenNewWindowCommand = new DelegateCommand<FileEntityViewModel>(OnOpenNewWindow);
        }

        public void OpenTabInNewWindow(IExplorerTabItemViewModel tabItem)
        {
            var tabsVm = _tabsFactory.Invoke().CreateTabsViewModel(new[]
            {
                tabItem
            });

            ShowNewWindow(tabsVm, new Point(24, 24));
        }

        public IXFilerWindow GetWindowWithRootTab()
        {
            var tabsViewModel = _tabsFactory.Invoke().CreateTabsViewModel(new[]
            {
                _explorerTabFactory.Invoke().CreateRootTab()
            });

            return new ExplorerWindow
            {
                DataContext = tabsViewModel
            };
        }

        private static void ShowNewWindow(ITabsViewModel mvm, Point location)
        {
            var currentApp = Application.Current ?? throw new ArgumentNullException("Application.Current");

            var activeWindow = (currentApp.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                                ?? currentApp.MainWindow)
                               ?? throw new ArgumentNullException("Application.Current.MainWindow");

            ExplorerWindow mainWindow = new()
            {
                DataContext = mvm,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = activeWindow.Left + location.X,
                Top = activeWindow.Top + location.Y
            };

            mainWindow.Show();
        }

        private void OnOpenNewWindow(FileEntityViewModel fileEntityViewModel)
        {
            if (fileEntityViewModel is DirectoryViewModel directoryViewModel)
            {
                var vm = _explorerTabFactory
                    .Invoke()
                    .CreateExplorerTab(directoryViewModel.DirectoryInfo);

                OpenTabInNewWindow(vm);
            }
        }
    }
}