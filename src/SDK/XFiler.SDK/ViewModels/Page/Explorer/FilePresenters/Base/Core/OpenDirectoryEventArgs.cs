using System;

namespace XFiler.SDK;

public class OpenDirectoryEventArgs : EventArgs
{
    public IFileSystemModel FileEntityViewModel { get; }

    public OpenDirectoryEventArgs(IFileSystemModel fileEntityViewModel)
    {
        FileEntityViewModel = fileEntityViewModel;
    }
}