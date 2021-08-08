using System.Windows;
using XFiler.SDK;

namespace XFiler
{
    public partial class BrowserPage
    {
        public BrowserPage()
        {
            InitializeComponent();

            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is BrowserPageModel model)
                model.InjectBrowser(WebBrowser);
        }
    }
}