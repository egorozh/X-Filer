using System.Windows;
using XFiler.SDK;

namespace XFiler
{
    public partial class MainWindow : IXFilerWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
            
        public void NormalizeAndActivate()
        {
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;

            Activate();
        }
    }
}