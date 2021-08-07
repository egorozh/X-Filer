using System;

namespace XFiler.SDK
{
    public interface ITabItemModel : IDisposable
    {
        IPageModel Page { get; }
        XFilerRoute Route { get; }
        string Header { get; }
        bool IsSelected { get; }
        bool LogicalIndex { get; }

        void Open(XFilerRoute route);
    }
}