using System;
using System.ComponentModel;

namespace XFiler.SDK;

public interface IFilesGroup : INotifyPropertyChanged, IDisposable, ICheckedItem
{
    string? GetGroup(IFileItem fileEntityViewModel);
            
    string Id { get; }
}