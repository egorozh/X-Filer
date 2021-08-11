using System.Linq;
using System.Windows;
using System.Windows.Input;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler.NotifyIcon
{
    public class NotifyIconViewModel
    {
        private readonly IWindowFactory _windowFactory;

        public ICommand ShowWindowCommand { get; }

        public ICommand ExitApplicationCommand { get; }

        public NotifyIconViewModel(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
            ShowWindowCommand = new DelegateCommand(OnShowWindow);
            ExitApplicationCommand = new DelegateCommand(Exit);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }
        
        private void OnShowWindow()
        {
            var window = Application.Current.Windows.OfType<IXFilerWindow>().FirstOrDefault();

            if (window != null)
            {
                window.NormalizeAndActivate();
            }
            else
            {
                _windowFactory.GetWindowWithRootTab().Show();
            }
        }
    }
}