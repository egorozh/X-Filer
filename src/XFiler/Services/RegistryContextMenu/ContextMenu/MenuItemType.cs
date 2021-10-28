namespace XFiler;

[Flags]
public enum MenuItemType : uint
{
    MFT_STRING = 0,
    MFT_BITMAP = 4,
    MFT_MENUBARBREAK = 32,
    MFT_MENUBREAK = 64,
    MFT_OWNERDRAW = 256,
    MFT_RADIOCHECK = 512,
    MFT_SEPARATOR = 2048,
    MFT_RIGHTORDER = 8192,
    MFT_RIGHTJUSTIFY = 16384
}