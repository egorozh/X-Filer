using System.IO;
using XFiler.Resources.Localization;

namespace XFiler.Sorting;

internal class FilesSortingOfDateOfChange : DisposableViewModel, IFilesSorting
{
    public string Name { get; } = Strings.Sorting_DateOfChange;
    
    public string Id => "90064064-30a8-4353-b60b-c12f5bf0f6b8";

    public IOrderedEnumerable<T> OrderBy<T>(IEnumerable<T> dirs) where T : FileSystemInfo
    {
        return dirs.OrderBy(d => d.LastWriteTime);
    }
}