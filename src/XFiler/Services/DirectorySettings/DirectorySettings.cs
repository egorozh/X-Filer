using LiteDB;
using System.IO;

namespace XFiler;

internal class DirectorySettings : IDirectorySettings
{
    private readonly string _dbPath;

    public DirectorySettings(IStorage storage)
    {
        _dbPath = Path.Combine(storage.DbDirectory, "dirInfo.db");
    }

    public DirectorySettingsInfo GetSettings(string directoryFullName)
    {
        using var db = new LiteDatabase(_dbPath);

        var col = db.GetCollection<DirectorySettingsInfoDto>("dirInfos");

        var info = col.FindOne(x => x.Id == directoryFullName.ToLower());

        if (info != null)
            return new DirectorySettingsInfo(info.GroupId, info.PresenterId, info.SortingId);

        return new DirectorySettingsInfo();
    }

    public void SetSettings(string directoryFullName, DirectorySettingsInfo info)
    {
        using var db = new LiteDatabase(_dbPath);

        var col = db.GetCollection<DirectorySettingsInfoDto>("dirInfos");

        var oldInfo = col.FindOne(x => x.Id == directoryFullName.ToLower());

        if (oldInfo != null)
        {
            oldInfo.GroupId = info.GroupId;
            oldInfo.PresenterId = info.PresenterId;
            oldInfo.SortingId = info.SortingId;
            col.Update(oldInfo);
        }
        else
        {
            DirectorySettingsInfoDto newInfo = new()
            {
                Id = directoryFullName.ToLower(),
                GroupId = info.GroupId,
                PresenterId = info.PresenterId,
                SortingId = info.SortingId
            };

            col.Insert(newInfo);
        }
    }
}

internal sealed class DirectorySettingsInfoDto
{
    public string Id { get; set; }

    public string GroupId { get; set; }

    public string PresenterId { get; set; }

    public string SortingId { get; set; }
}