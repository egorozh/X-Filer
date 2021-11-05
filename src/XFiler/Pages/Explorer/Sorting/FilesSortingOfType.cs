using System.IO;
using XFiler.Resources.Localization;

namespace XFiler.Sorting;

internal class FilesSortingOfType : DisposableViewModel, IFilesSorting
{
    public string Name { get; } = Strings.Sorting_Type;

    public string Id => "492f4249-ec83-4a47-abbf-a29a8cf58473";

    public IOrderedEnumerable<T> OrderBy<T>(IEnumerable<T> dirs) where T : FileSystemInfo
    {
        return dirs.OrderBy(d => d.Extension);
    }
}