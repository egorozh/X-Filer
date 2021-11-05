using XFiler.Resources.Localization;

namespace XFiler;

public class FilesGroupOfNone : DisposableViewModel, IFilesGroup
{
    public string Name { get; } = Strings.Grouping_None;

    public string? GetGroup(IFileItem item)
    {
        return null;
    }

    public string Id => "21a6615b-61e1-4dc9-acde-95a8bc532cbe";
}