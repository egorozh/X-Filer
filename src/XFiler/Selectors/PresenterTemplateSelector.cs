using System.Windows;
using System.Windows.Controls;
using XFiler.SDK;

namespace XFiler
{
    internal class PresenterTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is IFilesPresenterFactory factory)
            {
                return factory.Template;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
