using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace XFiler.SDK;

public interface IMenuItemViewModel : IDisposable
{
    IList<IMenuItemViewModel> Items { get; set; }

    Route? Route { get; }

    bool IsSelected { get; }
    string? Header { get; }

    event EventHandler IsSelectedChanged;

    BookmarkItem GetItem();

    void Init(Route route, ObservableCollection<IMenuItemViewModel> children, ICommand command);
    void Init(string folderHeader, ObservableCollection<IMenuItemViewModel> children);
}