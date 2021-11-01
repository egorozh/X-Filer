using System;

namespace XFiler.SDK;

public interface IDriveDetector
{
    event Action<EventType, string> DriveChanged;

    void Init();
}

public enum EventType
{
    Added = 2,
    Removed = 3
}