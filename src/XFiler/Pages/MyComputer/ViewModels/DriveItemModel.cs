using System.IO;
using Prism.Commands;
using XFiler.SDK;

namespace XFiler.MyComputer
{
    public class DriveItemModel : BaseItemModel
    {
        public long TotalFreeSpace { get; }

        public long TotalSize { get; }

        public double UsedPercentage { get; set; }

        public DriveItemModel(string drivePath, IIconLoader iconLoader, DelegateCommand<XFilerRoute> openCommand)
            : base(new XFilerRoute(new DriveInfo(drivePath)), iconLoader, openCommand)
        {
            var driveInfo = new DriveInfo(drivePath);

            TotalSize = driveInfo.TotalSize;
            TotalFreeSpace = driveInfo.TotalFreeSpace;

            UsedPercentage = (TotalSize - TotalFreeSpace) / (double)TotalSize * 100;
        }
    }
}