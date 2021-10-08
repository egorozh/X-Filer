using System.Management;

namespace XFiler
{
    internal class DriveDetector : IDriveDetector
    {
        private const string Query = "SELECT * FROM Win32_VolumeChangeEvent WHERE EventType = 2 or EventType = 3";

        public event Action<EventType, string>? DriveChanged;

        public DriveDetector() => Init();

        private void Init()
        {
            ManagementEventWatcher watcher = new()
            {
                Query = new WqlEventQuery(Query)
            };

            watcher.EventArrived += WatcherOnEventArrived;

            watcher.Start();
        }

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
    }
}