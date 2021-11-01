using System.IO;
using System.Management;

namespace XFiler;

internal class DriveDetector : IDriveDetector
{
    #region Private Fields

    private readonly Func<ITabFactory> _tabFactory;
    private readonly IWindowFactory _windowFactory;
    private const string Query = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3";

    #endregion

    #region Events

    public event Action<EventType, string>? DriveChanged;

    #endregion

    #region Constructor

    public DriveDetector(Func<ITabFactory> tabFactory, IWindowFactory windowFactory)
    {
        _tabFactory = tabFactory;
        _windowFactory = windowFactory;
    }

    #endregion

    #region Public Methods

    public void Init()
    {
        ManagementEventWatcher watcher = new()
        {
            Query = new WqlEventQuery(Query)
        };

        watcher.EventArrived += WatcherOnEventArrived;

        watcher.Start();

        DriveChanged += OnDriveChanged;
    }

    #endregion

    #region Private Methods

    private void WatcherOnEventArrived(object sender, EventArrivedEventArgs e)
    {
        var driveName = e.NewEvent.Properties["DriveName"].Value.ToString();

        var res = int.TryParse(e.NewEvent.Properties["EventType"].Value.ToString(), out var eventTypeValue);

        if (res && driveName != null)
            RaiseDriveChanged(driveName, (EventType) eventTypeValue);
    }

    private void RaiseDriveChanged(string driveName, EventType eventType)
    {
        Application.Current.Dispatcher.Invoke(() => { DriveChanged?.Invoke(eventType, driveName); });
    }

    private void OnDriveChanged(EventType type, string driveName)
    {
        if (type == EventType.Added)
        {
            DirectoryInfo info = new(driveName);

            var tab = _tabFactory.Invoke().CreateExplorerTab(info);

            if (tab != null)
                _windowFactory.OpenTabInNewWindow(tab);
        }
    }

    #endregion
}