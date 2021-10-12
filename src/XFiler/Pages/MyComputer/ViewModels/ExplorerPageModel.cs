using System.IO;

namespace XFiler.MyComputer;

public sealed class MyComputerPageModel : BasePageModel
{
    #region Private Fields

    private IIconLoader _iconLoader;
    private IDriveDetector _driveDetector;

    #endregion

    #region Public Methods

    public ObservableCollection<FolderItemModel> Folders { get; }

    public ObservableCollection<DriveItemModel> Drives { get; }

    #endregion

    #region Commands

    public DelegateCommand<Route> OpenCommand { get; }

    #endregion

    #region Constructor

    public MyComputerPageModel(IIconLoader iconLoader, IDriveDetector driveDetector)
    {
        _iconLoader = iconLoader;
        _driveDetector = driveDetector;

        Init(typeof(MyComputerPage), SpecialRoutes.MyComputer);

        OpenCommand = new DelegateCommand<Route>(OnOpen);

        Folders = new ObservableCollection<FolderItemModel>(
            SpecialRoutes.GetFolders().Select(r => new FolderItemModel(r, iconLoader, OpenCommand)));

        Drives = new ObservableCollection<DriveItemModel>(
            Directory.GetLogicalDrives().Select(p => new DriveItemModel(p, iconLoader, OpenCommand)));

        _driveDetector.DriveChanged += DriveDetectorOnDriveChanged;
    }

    #endregion

    #region Public Methods

    public override void Dispose()
    {
        base.Dispose();

        foreach (var folder in Folders)
            folder.Dispose();
        foreach (var folder in Drives)
            folder.Dispose();

        _driveDetector.DriveChanged -= DriveDetectorOnDriveChanged;
        _driveDetector = null!;
        _iconLoader = null!;
    }

    #endregion

    #region Private Methods

    private void OnOpen(Route route)
    {
        GoTo(route);
    }

    private void DriveDetectorOnDriveChanged(EventType type, string driveName)
    {
        DriveInfo info = new (driveName);

        if (type == EventType.Added)
        {
            Drives.Add(new DriveItemModel(info.Name, _iconLoader, OpenCommand));
        }
        else
        {
            var removedDrive = Drives.FirstOrDefault(d => d.Route.FullName == info.Name);

            if (removedDrive != null)
            {
                Drives.Remove(removedDrive);
                removedDrive.Dispose();
            }
        }
    }

    #endregion
}