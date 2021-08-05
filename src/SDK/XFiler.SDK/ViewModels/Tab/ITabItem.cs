using System;
using System.Windows;

namespace XFiler.SDK
{
    public interface ITabItem : IDisposable
    {
        DataTemplate Template { get; }

        string Header { get; }

        bool IsSelected { get; set; }

        bool LogicalIndex { get; set; }
    }
}