using System;

namespace XFiler.SDK
{
    public interface ITabItem : IDisposable
    {
        string Header { get; set; }

        bool IsSelected { get; set; }

        bool LogicalIndex { get; set; }
    }
}