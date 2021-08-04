using System.Windows;
using System.Windows.Input;
using Prism.Commands;

namespace XFiler.SDK
{
    public class NotifyIconViewModel
    {
        private readonly IWindowFactory _windowFactory;

        public ICommand ShowWindowCommand { get; }

        public ICommand ExitApplicationCommand { get; }

        public NotifyIconViewModel(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            ShowWindowCommand = new DelegateCommand(OnShowWindow, CanShowWindow);
            ExitApplicationCommand = new DelegateCommand(Exit);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private bool CanShowWindow()
        {
            return Application.Current.MainWindow == null;
        }

        private void OnShowWindow()
        {
            _windowFactory.GetWindowWithRootTab().Show();
        }
    }
}