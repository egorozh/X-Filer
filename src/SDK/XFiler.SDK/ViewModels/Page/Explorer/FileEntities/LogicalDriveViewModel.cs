using System.IO;

namespace XFiler.SDK
{
    public sealed class LogicalDriveViewModel : DirectoryViewModel
    {
        public LogicalDriveViewModel(DirectoryInfo route, IIconLoader iconLoader)
            : base(route, iconLoader)
        {
        }

        public override string GetRootName() => FullName;
    }
}