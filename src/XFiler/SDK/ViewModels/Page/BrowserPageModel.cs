using System.Windows;

namespace XFiler.SDK
{
    internal class BrowserPageModel : BasePageModel
    {
        public string Url { get; }

        public BrowserPageModel(string url) : base(CreateTemplate())
        {
            Url = url;
        }

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(BrowserPageModel),
            VisualTree = new FrameworkElementFactory(typeof(BrowserPage))
        };
    }
}