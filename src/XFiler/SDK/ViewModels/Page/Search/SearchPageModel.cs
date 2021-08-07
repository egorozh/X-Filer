using System;
using System.Windows;

namespace XFiler.SDK
{
    internal class SearchPageModel : BaseViewModel, IPageModel
    {
        public DataTemplate Template { get; }

        public event EventHandler<HyperlinkEventArgs>? GoToUrl;

        public SearchPageModel(XFilerUrl url)
        {
            Template = CreateTemplate();
        }

        public void Dispose()
        {
        }
        
        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(SearchPageModel),
            VisualTree = new FrameworkElementFactory(typeof(SearchPage))
        };
    }
}