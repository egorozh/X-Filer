namespace XFiler;

public sealed partial class MainWindow : IMainWindow
{
    public MainWindow()
    {
        InitializeComponent();

        //Closed += OnClosed;
    }

    public void NormalizeAndActivate()
    {
        if (WindowState == WindowState.Minimized)
            WindowState = WindowState.Normal;

        Activate();
    }

    //private void OnClosed(object? sender, EventArgs e)
    //{
    //    Closed -= OnClosed;
        
    //    if (DataContext is IDisposable disposable)
    //    {
    //        disposable.Dispose();
    //    }
    //}
}