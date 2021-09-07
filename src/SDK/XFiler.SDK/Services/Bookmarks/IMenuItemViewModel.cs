using System;
using System.Collections.Generic;

namespace XFiler.SDK
{
    public interface IMenuItemViewModel : IDisposable
    {
        IList<IMenuItemViewModel> Items { get; set; }

        XFilerRoute? Route { get; }

        bool IsSelected { get; }
        string? Header { get; }

        event EventHandler IsSelectedChanged;

        BookmarkItem GetItem();
    }
}