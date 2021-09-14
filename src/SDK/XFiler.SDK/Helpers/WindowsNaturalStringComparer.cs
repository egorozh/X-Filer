using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace XFiler.SDK
{
    public sealed class WindowsNaturalStringComparer : IComparer<string>
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        public static extern int StrCmpLogicalW(string psz1, string psz2);

        public int Compare(string? a, string? b) 
            => StrCmpLogicalW(a, b);
    }
}