using XFiler.Resources.Localization;

namespace XFiler;

public class FilesGroupOfType : DisposableViewModel, IFilesGroup
{
    public string Name { get; } = Strings.Grouping_Type;

    public string GetGroup(IFileItem item)
    {
        return item.Type;
    }

    public string Id { get; } = "ef45927b-b015-4686-a02b-935df331966f";
}