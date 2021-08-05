using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace XFiler.SDK
{
    [ContentProperty(nameof(SettingsItems))]
    public class ToolBarControl : Control
    {
        public static readonly DependencyProperty SettingsItemsProperty = DependencyProperty.Register(
            nameof(SettingsItems), typeof(ObservableCollection<Control>), typeof(ToolBarControl),
            new PropertyMetadata(new ObservableCollection<Control>()));

        public ObservableCollection<Control> SettingsItems
        {
            get => (ObservableCollection<Control>)GetValue(SettingsItemsProperty);
            set => SetValue(SettingsItemsProperty, value);
        }

        #region Static Constructor

        static ToolBarControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBarControl),
                new FrameworkPropertyMetadata(typeof(ToolBarControl)));
        }

        #endregion
    }
}