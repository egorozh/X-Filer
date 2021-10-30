using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Vanara.InteropServices;
using Vanara.PInvoke;
using Vanara.Windows.Shell;

namespace XFiler;

public class ContextMenuOperations
{
    public static ContextMenu? GetContextMenu(IEnumerable<string> filePathList,
        Func<string?, bool>? itemFilter = null) => GetContextMenuForFiles(filePathList,
        Shell32.CMF.CMF_NORMAL | Shell32.CMF.CMF_SYNCCASCADEMENU, itemFilter);

    #region Private Fields

    private static ContextMenu? GetContextMenuForFiles(
        IEnumerable<string> filePathList,
        Shell32.CMF flags,
        Func<string, bool>? itemFilter = null)
    {
        List<ShellItem> shellItems = new();

        try
        {
            shellItems.AddRange(filePathList
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(fp => new ShellItem(fp)));

            return GetContextMenuForFiles(shellItems, flags, itemFilter);
        }
        catch (Exception ex) when (ex is ArgumentException or FileNotFoundException)
        {
            return null;
        }
        finally
        {
            foreach (var si in shellItems)
                si.Dispose();
        }
    }

    private static ContextMenu? GetContextMenuForFiles(IReadOnlyList<ShellItem> shellItems,
        Shell32.CMF flags, Func<string, bool>? itemFilter = null)
    {
        if (!shellItems.Any())
            return null;

        // HP: the items are all in the same folder
        using var sf = shellItems.First().Parent;

        var menu = sf.GetChildrenUIObjects<Shell32.IContextMenu>(null, shellItems.ToArray());

        var hMenu = User32.CreatePopupMenu();
        menu.QueryContextMenu(hMenu, 0, 1, 0x7FFF, flags);

        var items = EnumMenuItems(menu, hMenu, itemFilter);

        return new ContextMenu(menu, hMenu, shellItems.Select(x => x.ParsingName).ToList(), items);
    }

    private static IReadOnlyList<ContextMenuItem> EnumMenuItems(Shell32.IContextMenu cMenu,
        HMENU hMenu, Func<string?, bool>? itemFilter = null)
    {
        var menuItemsResult = new List<ContextMenuItem>();

        var itemCount = User32.GetMenuItemCount(hMenu);

        User32.MENUITEMINFO mii = new()
        {
            fMask = User32.MenuItemInfoMask.MIIM_BITMAP
                    | User32.MenuItemInfoMask.MIIM_FTYPE
                    | User32.MenuItemInfoMask.MIIM_STRING
                    | User32.MenuItemInfoMask.MIIM_ID
                    | User32.MenuItemInfoMask.MIIM_SUBMENU
        };

        mii.cbSize = (uint) Marshal.SizeOf(mii);

        for (uint ii = 0; ii < itemCount; ii++)
        {
            var container = new SafeCoTaskMemString(512);
            mii.dwTypeData = (IntPtr) container;
            mii.cch = (uint) container.Capacity - 1; // https://devblogs.microsoft.com/oldnewthing/20040928-00/?p=37723
            var retval = User32.GetMenuItemInfo(hMenu, ii, true, ref mii);
            if (!retval)
            {
                container.Dispose();
                continue;
            }

            var type = (MenuItemType) mii.fType;
            var id = (int) (mii.wID - 1);
            var label = mii.dwTypeData;
            var commandString = GetCommandString(cMenu, mii.wID - 1);
            Bitmap? icon = null;
            IReadOnlyList<ContextMenuItem>? subItems = null;

            if (type == MenuItemType.MFT_STRING && id > 0)
            {
                Debug.WriteLine("Item {0} ({1}): {2}", ii, mii.wID, mii.dwTypeData);

                if (itemFilter != null && (itemFilter(commandString) || itemFilter(label)))
                {
                    // Skip items implemented in UWP
                    container.Dispose();
                    continue;
                }

                if (mii.hbmpItem != HBITMAP.NULL &&
                    !Enum.IsDefined(typeof(ContextMenu.HbitmapHmenu), ((IntPtr) mii.hbmpItem).ToInt64()))
                {
                    icon = Win32Api.GetBitmapFromHBitmap(mii.hbmpItem);
                }

                if (mii.hSubMenu != HMENU.NULL)
                {
                    Debug.WriteLine("Item {0}: has submenu", ii);

                    try
                    {
                        (cMenu as Shell32.IContextMenu2)?.HandleMenuMsg((uint) User32.WindowMessage.WM_INITMENUPOPUP,
                            (IntPtr) mii.hSubMenu, new IntPtr(ii));
                    }
                    catch (NotImplementedException)
                    {
                        // Only for dynamic/owner drawn? (open with, etc)
                    }

                    subItems = EnumMenuItems(cMenu, mii.hSubMenu, itemFilter);
                    Debug.WriteLine("Item {0}: done submenu", ii);
                }

                menuItemsResult.Add(new ContextMenuItem(id, label, commandString, type, icon, subItems));
            }
            else
            {
                Debug.WriteLine("Item {0}: {1}", ii, mii.fType.ToString());
            }

            container.Dispose();
        }

        return menuItemsResult;
    }

    private static string? GetCommandString(Shell32.IContextMenu cMenu, uint offset,
        Shell32.GCS flags = Shell32.GCS.GCS_VERBW)
    {
        if (offset > 5000)
        {
            // Hackish workaround to avoid an AccessViolationException on some items,
            // notably the "Run with graphic processor" menu item of NVidia cards
            return null;
        }

        SafeCoTaskMemString? commandString = null;
        try
        {
            commandString = new SafeCoTaskMemString(512);
            cMenu.GetCommandString(new IntPtr(offset), flags, IntPtr.Zero, commandString,
                (uint) commandString.Capacity - 1);
            Debug.WriteLine("Verb {0}: {1}", offset, commandString);
            return commandString.ToString();
        }
        catch (Exception ex) when (ex is InvalidCastException or ArgumentException)
        {
            // TODO: investigate this..
            Debug.WriteLine(ex);
            return null;
        }
        catch (Exception ex) when (ex is COMException or NotImplementedException)
        {
            // Not every item has an associated verb
            return null;
        }
        finally
        {
            commandString?.Dispose();
        }
    }

    #endregion
}