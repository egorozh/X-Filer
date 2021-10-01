using System.ComponentModel;
using System.Runtime.InteropServices;

namespace XFiler;

internal sealed class WindowsNaturalStringComparer : INaturalStringComparer
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
    public static extern int StrCmpLogicalW(string psz1, string psz2);

    public int Compare(string? a, string? b)
        => StrCmpLogicalW(a, b);

    public int Compare(object? x, object? y)
    {
        if (x is FileEntityViewModel one && y is FileEntityViewModel two)
            return SortDirection == ListSortDirection.Ascending
                ? Compare(one.Name, two.Name)
                : -Compare(one.Name, two.Name);

        return Compare(x as string, y as string);
    }

    public ListSortDirection SortDirection { get; set; }
}