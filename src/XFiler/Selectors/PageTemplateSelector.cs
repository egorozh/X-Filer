using System.Windows.Controls;

namespace XFiler;

internal sealed class PageTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is IPageModel page)
            return page.Template;

        return base.SelectTemplate(item, container);
    }
}