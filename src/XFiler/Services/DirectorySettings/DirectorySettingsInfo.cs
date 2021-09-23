namespace XFiler
{
    public readonly struct DirectorySettingsInfo
    {
        public string? GroupId { get; }
            
        public string? PresenterId { get; }

        public DirectorySettingsInfo(string groupId, string presenterId)
        {
            GroupId = groupId;
            PresenterId = presenterId;
        }
    }
}