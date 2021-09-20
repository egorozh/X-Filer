using System.Drawing;
using System.Windows.Media.Imaging;
using XFiler.Helpers;

namespace XFiler
{
    public class NativeIconResolver
    {
        const string IID_IImageList = "46EB5926-582E-4017-9FDF-E8998DAA0950";
        const string IID_IImageList2 = "192B9D83-50FC-457B-90A0-2B82A8B5DAE1";

        public static BitmapImage GetIcon(string fileName)
        {
            var hIcon = GetJumboIcon(GetIconIndex(fileName));

            using Icon ico = (Icon)Icon.FromHandle(hIcon).Clone();
            
            Shell32.DestroyIcon(hIcon);

            return ico.ToBitmapImage();
        }

        private static int GetIconIndex(string pszFile)
        {
            SHFILEINFO sfi = new();

            Shell32.SHGetFileInfo(pszFile
                , 0
                , ref sfi
                , (uint)System.Runtime.InteropServices.Marshal.SizeOf(sfi)
                , (uint)(SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.UseFileAttributes));

            return sfi.iIcon;
        }

        // 48X48
        private static IntPtr GetXLIcon(int iImage)
        {
            IImageList spiml = null;
            var guil = new Guid(IID_IImageList); //or IID_IImageList

            Shell32.SHGetImageList(Shell32.SHIL_EXTRALARGE, ref guil, ref spiml);

            var hIcon = IntPtr.Zero;

            spiml.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon); //

            return hIcon;
        }

        // 256*256
        private static IntPtr GetJumboIcon(int iImage)
        {
            IImageList spiml = null;
            var guil = new Guid(IID_IImageList2); //or IID_IImageList

            Shell32.SHGetImageList(Shell32.SHIL_JUMBO, ref guil, ref spiml);

            var hIcon = IntPtr.Zero;

            spiml.GetIcon(iImage, Shell32.ILD_TRANSPARENT | Shell32.ILD_IMAGE, ref hIcon); //

            return hIcon;
        }
    }
}