using System.IO;
using XFiler.Resources.Localization;

namespace XFiler.Sorting;

internal class FilesSortingOfSize : DisposableViewModel, IFilesSorting
{
    public string Name { get; } = Strings.Sorting_Size;

    public string Id => "473d766d-e081-4f21-b70d-1ba4a7ab5683";

    public IOrderedEnumerable<T> OrderBy<T>(IEnumerable<T> dirs) where T : FileSystemInfo
    {
        return dirs.OrderBy(d => d is FileInfo f ? f.Length : 0);
    }
}