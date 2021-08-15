using Prism.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using WK.Libraries.SharpClipboardNS;
using XFiler.SDK;

namespace XFiler
{
    public class ClipboardService : IClipboardService
    {
        #region Private Fields

        private const string DropEffectFormat = "Preferred DropEffect";

        private readonly IFileOperations _fileOperations;

        #endregion

        #region Events

        public event EventHandler<FileClipboardEventArgs>? ClipboardChanged;

        #endregion

        #region Commands

        public DelegateCommand<object> CutCommand { get; }
        public DelegateCommand<object> CopyCommand { get; }
        public DelegateCommand<IFileSystemModel> PasteCommand { get; }

        #endregion

        #region Constructor

        public ClipboardService(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations;

            CutCommand = new DelegateCommand<object>(OnCut);
            CopyCommand = new DelegateCommand<object>(OnCopy);
            PasteCommand = new DelegateCommand<IFileSystemModel>(OnPaste, CanPaste);

            SharpClipboard clipboard = new();
            clipboard.ClipboardChanged += ClipboardOnClipboardChanged;
        }

        #endregion

        #region Private Methods

        private static void OnCut(object parameter)
        {
            switch (parameter)
            {
                case IFileSystemModel model:
                    CopyOrMove(new[] { model.Info }, DragDropEffects.Move);
                    break;
                case IEnumerable e:
                    CopyOrMove(e.OfType<IFileSystemModel>().Select(m => m.Info), DragDropEffects.Move);
                    break;
            }
        }

        private static void OnCopy(object parameter)
        {
            switch (parameter)
            {
                case IFileSystemModel model:
                    CopyOrMove(new[] { model.Info });
                    break;
                case IEnumerable e:
                    CopyOrMove(e.OfType<IFileSystemModel>().Select(m => m.Info));
                    break;
            }
        }

        private static bool CanPaste(IFileSystemModel? model)
        {
            return Clipboard.ContainsFileDropList();
        }
          
        private void OnPaste(IFileSystemModel? model)
        {
            if (model is not { Info: DirectoryInfo directory })
                return;

            var files = Clipboard.GetFileDropList();
            IReadOnlyList<FileSystemInfo> items = files.OfType<string>()
                .Select(p => p.ToInfo())
                .OfType<FileSystemInfo>().ToList();

            var action = GetAction();

            if (action.HasFlag(DragDropEffects.Copy))
                _fileOperations.Copy(items, directory);
            else if (action.HasFlag(DragDropEffects.Move))
                _fileOperations.Move(items, directory);
        }

        private void ClipboardOnClipboardChanged(object? sender, SharpClipboard.ClipboardChangedEventArgs e)
        {
            var files = Clipboard.GetFileDropList();
            IReadOnlyList<FileSystemInfo> items = files.OfType<string>()
                .Select(p => p.ToInfo())
                .OfType<FileSystemInfo>().ToList();

            var action = GetAction();
            
            PasteCommand.RaiseCanExecuteChanged();

            ClipboardChanged?.Invoke(this, new FileClipboardEventArgs(action, items));
        }

        private static void CopyOrMove(IEnumerable<FileSystemInfo> items,
            DragDropEffects effects = DragDropEffects.Copy)
        {
            StringCollection paths = new();
            paths.AddRange(items.Select(i => i.FullName).ToArray());

            var data = new DataObject();
            data.SetFileDropList(paths);
            data.SetData(DropEffectFormat, ToData(effects));
            Clipboard.SetDataObject(data);
        }

        private static DragDropEffects GetAction()
        {
            if (Clipboard.GetData(DropEffectFormat) is not MemoryStream stream)
                return DragDropEffects.None;

            byte[] aMoveEffect = new byte[4];
            stream.Read(aMoveEffect, 0, aMoveEffect.Length);
            return (DragDropEffects)BitConverter.ToInt32(aMoveEffect, 0);
        }

        private static MemoryStream ToData(DragDropEffects effects)
            => new(BitConverter.GetBytes((int)effects));

        #endregion
    }
}