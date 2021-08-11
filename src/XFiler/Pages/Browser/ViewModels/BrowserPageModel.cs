using CefSharp.Wpf;
using System.ComponentModel;

namespace XFiler.SDK
{
    internal class BrowserPageModel : BasePageModel
    {
        private ChromiumWebBrowser _webBrowser = null!;

        public string Url { get; set; }

        public BrowserPageModel(string url) : base(typeof(BrowserPage))
        {
            Url = url;

            PropertyChanged += BrowserModelPropertyChanged;
        }

        public void InjectBrowser(ChromiumWebBrowser webBrowser)
        {
            _webBrowser = webBrowser;
        }

        public override void Dispose()
        {
            base.Dispose();

            PropertyChanged -= BrowserModelPropertyChanged;

            _webBrowser.Dispose();
            _webBrowser = null!;
        }

        private void BrowserModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Url))
            {
                GoTo(new XFilerRoute(Url, Url, RouteType.WebLink));
            }
        }
    }
}