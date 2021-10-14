using System.Windows.Input;

namespace XFiler.TrayIcon;

public sealed class TrayIconViewModel
{
    private readonly IWindowFactory _windowFactory;

    public ICommand ShowWindowCommand { get; }

    public ICommand ExitApplicationCommand { get; }

    public TrayIconViewModel(IWindowFactory windowFactory)
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
        var window = Application.Current.Windows.OfType<IMainWindow>().FirstOrDefault();

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