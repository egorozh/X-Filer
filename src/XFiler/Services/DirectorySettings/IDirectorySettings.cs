namespace XFiler;

public interface IDirectorySettings
{
    DirectorySettingsInfo GetSettings(string directoryFullName);
       
    void SetSettings(string directoryFullName, DirectorySettingsInfo info);
}