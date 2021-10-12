using System;
using System.Windows;

namespace XFiler.SDK;

public interface IPageModel : IDisposable
{
    event EventHandler<HyperlinkEventArgs> GoToUrl;

    DataTemplate Template { get; }

    Route Route { get; }
}