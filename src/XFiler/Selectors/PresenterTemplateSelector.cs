using System.Windows.Controls;

namespace XFiler;

internal sealed class PresenterTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is IFilesPresenterFactory factory)
        {
            return factory.Template;
        }
                
        return base.SelectTemplate(item, container);
    }
}