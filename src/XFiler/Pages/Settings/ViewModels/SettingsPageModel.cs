using System;
using System.Windows;

namespace XFiler.SDK
{
    internal class SettingsPageModel : BaseViewModel, ISettingsPageModel
    {
        public event EventHandler<HyperlinkEventArgs>? GoToUrl;
        public DataTemplate Template { get; }

        public SettingsPageModel()
        {
            Template = CreateTemplate();
        }

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(SettingsPageModel),
            VisualTree = new FrameworkElementFactory(typeof(SettingsPage))
        };

        public void Dispose()
        {
        }
    }
}