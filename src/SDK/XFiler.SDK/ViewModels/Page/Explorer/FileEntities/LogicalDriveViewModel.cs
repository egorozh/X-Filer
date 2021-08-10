namespace XFiler.SDK
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(XFilerRoute route, IIconLoader iconLoader)
            : base(route, iconLoader)
        {
        }

        public override string GetRootName() => FullName;
    }
}