namespace Windows.ImageOperations.Internal;

internal static class Shell32
{
    public const int SHIL_LARGE = 0x0;
    public const int SHIL_SMALL = 0x1;
    public const int SHIL_EXTRALARGE = 0x2;
    public const int SHIL_SYSSMALL = 0x3;
    public const int SHIL_JUMBO = 0x4;
    public const int SHIL_LAST = 0x4;

    public const int ILD_TRANSPARENT = 0x00000001;
    public const int ILD_IMAGE = 0x00000020;

    [DllImport("shell32.dll", EntryPoint = "#727")]
    public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

    [DllImport("user32.dll", EntryPoint = "DestroyIcon", SetLastError = true)]
    public static extern unsafe int DestroyIcon(IntPtr hIcon);

    [DllImport("shell32.dll")]
    public static extern uint SHGetIDListFromObject([MarshalAs(UnmanagedType.IUnknown)] object iUnknown,
        out IntPtr ppidl);

    [DllImport("Shell32.dll")]
    public static extern IntPtr SHGetFileInfo(string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbFileInfo,
        uint uFlags
    );
}