namespace Windows.ImageOperations.Internal;

[StructLayout(LayoutKind.Sequential)]
internal struct SHFILEINFO
{
    public const int NAMESIZE = 80;
    public IntPtr hIcon;
    public int iIcon;
    public uint dwAttributes;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szDisplayName;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
    public string szTypeName;
};