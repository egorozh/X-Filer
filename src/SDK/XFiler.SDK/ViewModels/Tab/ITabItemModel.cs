using System;

namespace XFiler.SDK
{
    public interface ITabItemModel : IDisposable
    {
        IPageModel Page { get; }
        XFilerUrl Url { get; }
        string Header { get; }
        bool IsSelected { get; }
        bool LogicalIndex { get; }

        void Open(XFilerUrl url);
    }
}