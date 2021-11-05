using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace XFiler.SDK;

public interface IFilesSorting : INotifyPropertyChanged, IDisposable, ICheckedItem
{
    string Id { get; }
    
    IOrderedEnumerable<T> OrderBy<T>(IEnumerable<T> dirs) where T : FileSystemInfo;
}