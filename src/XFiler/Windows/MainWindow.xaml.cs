namespace XFiler
{
    public sealed partial class MainWindow : IMainWindow
    {
        public MainWindow() => InitializeComponent();

        public void NormalizeAndActivate()
        {
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;

            Activate();
        }
    }
}