using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Prism.Commands;

namespace XFiler.SDK;

public interface IClipboardService
{
    event EventHandler<FileClipboardEventArgs> ClipboardChanged;

    DelegateCommand<object> CutCommand { get; }
    DelegateCommand<object> CopyCommand { get; }
    DelegateCommand<object> PasteCommand { get; }
        
    bool IsCutted(FileSystemInfo info);
}

public class FileClipboardEventArgs : EventArgs
{
    public DragDropEffects Action { get; }
    public IReadOnlyList<FileSystemInfo> Items { get; }

    public FileClipboardEventArgs(DragDropEffects action, IReadOnlyList<FileSystemInfo> items)
    {
        Action = action;
        Items = items;
    }
}