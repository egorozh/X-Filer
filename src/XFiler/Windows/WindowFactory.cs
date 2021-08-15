using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler
{
    internal class WindowFactory : IWindowFactory
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

        public IXFilerWindow GetWindowWithRootTab()
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

            var activeWindow = (currentApp.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                                ?? currentApp.MainWindow)
                               ?? throw new ArgumentNullException("Application.Current.MainWindow");

            MainWindow mainWindow = new()
            {
                DataContext = mvm,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = activeWindow.Left + location.X,
                Top = activeWindow.Top + location.Y
            };

            mainWindow.Show();
        }

        private void OnOpenNewWindow(object parameter)
        {
            if (parameter is IDirectoryModel model)
            {
                var vm = _explorerTabFactory
                    .Invoke()
                    .CreateExplorerTab(model.DirectoryInfo);

                OpenTabInNewWindow(vm);
            }
            else if (parameter is IEnumerable items)
            {
                var tabs = items.OfType<IDirectoryModel>()
                    .Select(directoryModel => _explorerTabFactory.Invoke()
                        .CreateExplorerTab(directoryModel.DirectoryInfo))
                    .ToList();

                OpenTabInNewWindow(tabs);
            }
        }
    }
}