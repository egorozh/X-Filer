using System.Windows;

namespace XFiler.SDK
{
    internal class SettingsTabViewModel : TabItemViewModel, ISettingsTabViewModel
    {
        public SettingsTabViewModel() : base("Настройки", CreateTemplate())
        {
        }

        private static DataTemplate CreateTemplate() => new()
        {
            DataType = typeof(SettingsTabViewModel),
            VisualTree = new FrameworkElementFactory(typeof(SettingsTabItem))
        };

    }
}