using System.Diagnostics;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace XFiler;

public class ContextMenu : IDisposable
{
    #region Private Fields

    private Shell32.IContextMenu _cMenu;
    private User32.SafeHMENU _hMenu;

    #endregion

    #region Public Properties

    public IReadOnlyList<ContextMenuItem> Items { get; private set; }

    public IReadOnlyList<string> ItemsPath { get; }

    #endregion

    #region Constructor

    public ContextMenu(Shell32.IContextMenu cMenu,
        User32.SafeHMENU hMenu,
        IReadOnlyList<string> itemsPath,
        IReadOnlyList<ContextMenuItem> items)
    {
        _cMenu = cMenu;
        _hMenu = hMenu;
        ItemsPath = itemsPath;
        Items = items;
    }

    #endregion

    #region Public Methods

    public bool InvokeVerb(string verb)
    {
        if (string.IsNullOrEmpty(verb))
            return false;

        try
        {
            var currentWindows = Win32Api.GetDesktopWindows();
            var pici = new Shell32.CMINVOKECOMMANDINFOEX
            {
                lpVerb = new SafeResourceId(verb, CharSet.Ansi),
                nShow = ShowWindowCommand.SW_SHOWNORMAL
            };
            pici.cbSize = (uint) Marshal.SizeOf(pici);
            _cMenu.InvokeCommand(pici);
            Win32Api.BringToForeground(currentWindows);
            return true;
        }
        catch (Exception ex) when (ex is COMException or UnauthorizedAccessException)
        {
            Debug.WriteLine(ex);
        }

        return false;
    }

    public void InvokeItem(int itemId)
    {
        if (itemId < 0)
            return;

        try
        {
            //var currentWindows = Win32Api.GetDesktopWindows();
            var pici = new Shell32.CMINVOKECOMMANDINFOEX
            {
                lpVerb = Macros.MAKEINTRESOURCE(itemId),
                nShow = ShowWindowCommand.SW_SHOWNORMAL,
            };
            pici.cbSize = (uint) Marshal.SizeOf(pici);
            _cMenu.InvokeCommand(pici);
            //Win32Api.BringToForeground(currentWindows);
        }
        catch (Exception ex) when (
            ex is COMException or UnauthorizedAccessException)
        {
            Debug.WriteLine(ex);
        }
    }

    #endregion

    #region IDisposable Support

    // To detect redundant calls
    private bool _disposedValue;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;

        if (disposing)
        {
            // TODO: dispose managed state (managed objects).

            foreach (var si in Items) 
                (si as IDisposable)?.Dispose();

            Items = null!;
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        User32.DestroyMenu(_hMenu);
        _hMenu = null!;

        Marshal.ReleaseComObject(_cMenu);
        _cMenu = null!;

        _disposedValue = true;
    }

    ~ContextMenu()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion IDisposable Support

    public enum HbitmapHmenu : long
    {
        HBMMENU_CALLBACK = -1,
        HBMMENU_MBAR_CLOSE = 5,
        HBMMENU_MBAR_CLOSE_D = 6,
        HBMMENU_MBAR_MINIMIZE = 3,
        HBMMENU_MBAR_MINIMIZE_D = 7,
        HBMMENU_MBAR_RESTORE = 2,
        HBMMENU_POPUP_CLOSE = 8,
        HBMMENU_POPUP_MAXIMIZE = 10,
        HBMMENU_POPUP_MINIMIZE = 11,
        HBMMENU_POPUP_RESTORE = 9,
        HBMMENU_SYSTEM = 1
    }
}