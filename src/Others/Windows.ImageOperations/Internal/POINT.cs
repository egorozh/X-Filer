using System.Runtime.InteropServices;

namespace Windows.ImageOperations.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct POINT
    {
        int x;
        int y;
    }
}