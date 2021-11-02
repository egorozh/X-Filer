using System;

namespace XFiler.SDK;

public interface IDriveDetector : IInitializeService
{
    event Action<EventType, string> DriveChanged;
}

public enum EventType
{
    Added = 2,
    Removed = 3
}