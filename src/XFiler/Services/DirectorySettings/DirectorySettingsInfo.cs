namespace XFiler;

public readonly struct DirectorySettingsInfo
{
    public string? GroupId { get; }
            
    public string? PresenterId { get; }
    public string? SortingId { get;  }

    public DirectorySettingsInfo(string groupId, string presenterId, string? sortingId)
    {
        GroupId = groupId;
        PresenterId = presenterId;
        SortingId = sortingId;
    }
}