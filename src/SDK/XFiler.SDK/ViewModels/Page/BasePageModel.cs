using System;
using System.Windows;

namespace XFiler.SDK
{
    public abstract class BasePageModel : BaseViewModel, IPageModel
    {
        public DataTemplate Template { get; }

        public event EventHandler<HyperlinkEventArgs>? GoToUrl;

        protected BasePageModel(DataTemplate template)
        {
            Template = template;
        }

        protected void GoTo(XFilerUrl url, bool isOpenInNewTab = false)
        {
            GoToUrl?.Invoke(this, new HyperlinkEventArgs(url, isOpenInNewTab));
        }

        public virtual void Dispose()
        {
        }
    }
}