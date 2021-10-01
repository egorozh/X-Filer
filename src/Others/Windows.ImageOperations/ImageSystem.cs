namespace Windows.ImageOperations;

public static class ImageSystem
{
    #region Constants

    private const string IdIImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
    private const string IdIImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";

    #endregion

    #region Public Methods

    public static BitmapImage GetIcon(string fileName, bool isExtraLarge = true)
    {
        var iIndex = GetIconIndex(fileName);

        var hIcon = isExtraLarge
            ? GetJumboIcon(iIndex)
            : GetXlIcon(iIndex);

        var bmp = Icon.FromHandle(hIcon).ToBitmap();

        Shell32.DestroyIcon(hIcon);

        return bmp.ToBitmapImage();
    }

    public static async Task<Stream?> GetIconStream(string fileName, bool isExtraLarge = true)
    {
        var iIndex = GetIconIndex(fileName);

        var hIcon = isExtraLarge
            ? GetJumboIcon(iIndex)
            : GetXlIcon(iIndex);
            
        var bmp = Icon.FromHandle(hIcon).ToBitmap();

        Shell32.DestroyIcon(hIcon);

        var ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Png);

        return ms;
    }

    public static Stream ToStream(this Bitmap bitmap)
    {
        var ms = new MemoryStream();

        bitmap.Save(ms, ImageFormat.Png);

        return ms;
    }

    public static BitmapImage ToBitmapImage(this Bitmap bitmap)
    {
        using var ms = new MemoryStream();

        bitmap.Save(ms, ImageFormat.Png);

        return FromStream(ms);
    }

    public static BitmapImage FromStream(Stream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);

        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = stream;
        bitmapImage.EndInit();

        return bitmapImage;
    }

    #endregion

    #region Private Methods

    private static int GetIconIndex(string pszFile)
    {
        SHFILEINFO sfi = new();

        const SHGFI flags = SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.UseFileAttributes;

        Shell32.SHGetFileInfo(pszFile, 0, ref sfi,
            (uint)System.Runtime.InteropServices.Marshal.SizeOf(sfi),
            (uint)flags);

        return sfi.iIcon;
    }

    // 48X48
    private static IntPtr GetXlIcon(int iImage)
    {
        IImageList spiml = null;
        var guil = new Guid(IdIImageList);

        Shell32.SHGetImageList(Shell32.SHIL_EXTRALARGE, ref guil, ref spiml);

        var hIcon = IntPtr.Zero;

        spiml.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon); //

        return hIcon;
    }

    // 256*256
    private static IntPtr GetJumboIcon(int iImage)
    {
        IImageList imageList = null!;

        Guid guid = new(IdIImageList2);

        Shell32.SHGetImageList(Shell32.SHIL_JUMBO, ref guid, ref imageList);

        var hIcon = IntPtr.Zero;
        imageList.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon);

        return hIcon;
    }

    #endregion
}