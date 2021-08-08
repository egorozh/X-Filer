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

        protected BasePageModel(Type pageType)
        {
            Template = CreateTemplate(pageType);
        }

        protected void GoTo(XFilerRoute route, bool isOpenInNewTab = false)
        {
            GoToUrl?.Invoke(this, new HyperlinkEventArgs(route, isOpenInNewTab));
        }

        public virtual void Dispose()
        {
        }

        private DataTemplate CreateTemplate(Type pageType) => new()
        {
            DataType = this.GetType(),
            VisualTree = new FrameworkElementFactory(pageType)
        };
    }
}