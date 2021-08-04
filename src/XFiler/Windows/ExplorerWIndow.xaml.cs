using System.Windows;
using XFiler.SDK;

namespace XFiler
{
    public partial class ExplorerWindow : IXFilerWindow
    {
        public ExplorerWindow()
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