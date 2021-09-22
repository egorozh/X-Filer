namespace XFiler
{
    internal sealed class SettingsPageModel : BasePageModel, ISettingsPageModel
    {
        public SettingsPageModel() : base(typeof(SettingsPage), SpecialRoutes.Settings)
        {
        }
    }
}