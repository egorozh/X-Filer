using System.IO;
using XFiler.Resources.Localization;

namespace XFiler.Sorting;

internal class FilesSortingOfName : DisposableViewModel, IFilesSorting
{
    private INaturalStringComparer _comparer;

    public string Name { get; } = Strings.Sorting_Name;

    public string Id => "76419503-8202-454d-8590-23a9e809ec5d";

    public FilesSortingOfName(INaturalStringComparer comparer)
    {
        _comparer = comparer;
    }

    public IOrderedEnumerable<T> OrderBy<T>(IEnumerable<T> dirs) where T : FileSystemInfo
    {
        return dirs.OrderBy(d => d.Name, _comparer);
    }

    protected override void Dispose(bool disposing)
    {
        if (!Disposed && disposing)
        {
            _comparer = null!;
        }

        base.Dispose(disposing);
    }
}