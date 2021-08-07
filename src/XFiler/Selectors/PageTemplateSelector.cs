using System.Windows;
using System.Windows.Controls;
using XFiler.SDK;

namespace XFiler
{
    internal class PageTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IPageModel page)
                return page.Template;

            return base.SelectTemplate(item, container);
        }
    }
}