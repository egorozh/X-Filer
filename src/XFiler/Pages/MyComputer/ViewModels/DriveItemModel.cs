using System.IO;

namespace XFiler.MyComputer;

public sealed class DriveItemModel : BaseItemModel
{
    public long TotalFreeSpace { get; }

    public long TotalSize { get; }

    public double UsedPercentage { get; set; }

    public DriveItemModel(string drivePath, IIconLoader iconLoader, DelegateCommand<Route> openCommand)
        : base(new Route(new DriveInfo(drivePath)), iconLoader, openCommand)
    {
        try
        {
            var driveInfo = new DriveInfo(drivePath);

            TotalSize = driveInfo.TotalSize;
            TotalFreeSpace = driveInfo.TotalFreeSpace;

            UsedPercentage = (TotalSize - TotalFreeSpace) / (double)TotalSize * 100;
        }
        catch (Exception)
        {
            // It is Empty CD-Rom
        }
    }
}