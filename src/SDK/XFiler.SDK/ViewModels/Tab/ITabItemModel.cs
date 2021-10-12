using System;

namespace XFiler.SDK;

public interface ITabItemModel : IDisposable
{
    IPageModel Page { get; }
    Route Route { get; }
    string Header { get; }
    bool IsSelected { get; }
    bool LogicalIndex { get; }

    void Init(Route route);

    void Open(Route route);
}