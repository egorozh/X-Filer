using GongSolutions.Wpf.DragDrop;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XFiler.SDK;

namespace XFiler.DragDrop
{
    internal class XFilerDragDrop : IDropTarget
    {
        #region Private Fields
            
        private readonly IFileOperations _fileOperations;

        #endregion

        #region Constructor

        public XFilerDragDrop(IFileOperations fileOperations)
        {
            _fileOperations = fileOperations;
        }

        #endregion

        #region Public Methods

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not IDataObject && dropInfo?.DragInfo == null)
                return;

            if (!dropInfo.IsSameDragDropContextAsSource)
                return;

            if (CanDrag(dropInfo))
                return;
            
            dropInfo.EffectText = null;
            dropInfo.DestinationText = null;
            dropInfo.DropTargetAdorner = null;
            dropInfo.Effects = DragDropEffects.Scroll;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.VisualTarget is not FrameworkElement { DataContext: IFilesPresenter filesPresenter })
                return;

            DirectoryInfo targetDirectory = dropInfo.TargetItem is DirectoryViewModel dirVm
                ? dirVm.DirectoryInfo
                : filesPresenter.CurrentDirectory;

            var sources = dropInfo.Data switch
            {
                FileEntityViewModel sourceItem => new List<FileSystemInfo> { sourceItem.Info },
                ICollection<object> sourceItems => sourceItems.Cast<FileEntityViewModel>()
                    .Select(fs => fs.Info).ToList(),
                IDataObject dataObject => GetFileDropData(dataObject),
                _ => null
            };

            if (sources != null && sources.Any())
                DropItems(sources, dropInfo.Effects, targetDirectory);
        }

        #endregion

        #region Private Methods

        #region Drag

        private static bool CanDrag(IDropInfo dropInfo)
        {
            var sources = dropInfo.Data switch
            {
                FileEntityViewModel sourceItem => new List<FileSystemInfo> { sourceItem.Info },
                ICollection<object> sourceItems => sourceItems.Cast<FileEntityViewModel>()
                    .Select(fs => fs.Info).ToList(),
                IDataObject dataObject => GetFileDropData(dataObject),
                _ => null
            };

            if (sources != null && sources.Any())
                return CanDragItems(dropInfo, sources);

            return false;
        }

        private static bool CanDragItems(IDropInfo dropInfo,
            IReadOnlyList<FileSystemInfo> sourceItems)
        {
            var sourceItem = sourceItems.First();
            var sourceRoot = sourceItem.GetRootName();

            if (dropInfo.TargetItem == null
                && dropInfo.VisualTarget is Control
                {
                    DataContext: IFilesPresenter presenter
                })
            {
                DirectoryInfo targetDirectory = presenter.CurrentDirectory;

                IEnumerable<string> targetCollection = targetDirectory
                    .EnumerateFileSystemInfos()
                    .Select(d => d.FullName);

                if (targetCollection.Contains(sourceItem.FullName))
                    return false;

                var targetRoot = targetDirectory.Root.Name;

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetInsertAdorner);

                SetDropInfoParams(dropInfo, sourceItem, targetDirectory, sourceRoot, targetRoot);

                return true;
            }

            if (dropInfo.TargetItem is DirectoryViewModel targetFolder &&
                !sourceItems.Select(fi => fi.FullName).Contains(targetFolder.Route.FullName))
            {
                var targetRoot = targetFolder.GetRootName();

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);

                SetDropInfoParams(dropInfo, sourceItem, targetFolder.DirectoryInfo, sourceRoot, targetRoot);

                return true;
            }

            return false;
        }

        private static void SetDropInfoParams(IDropInfo dropInfo,
            FileSystemInfo? sourceItem,
            DirectoryInfo targetFolder,
            DirectoryInfo sourceRoot,
            string? targetRoot)
        {
            if (sourceItem is DirectoryInfo { Parent: null })
            {
                dropInfo.Effects = DragDropEffects.Link;
                dropInfo.EffectText = "Создать ссылку в";
                dropInfo.DestinationText = $"{targetFolder.Name}";
            }
            else if (sourceRoot.FullName == targetRoot)
            {
                dropInfo.Effects = DragDropEffects.Move;
                dropInfo.EffectText = "Переместить в";
                dropInfo.DestinationText = $"{targetFolder.Name}";
            }
            else
            {
                dropInfo.Effects = DragDropEffects.Copy;
                dropInfo.EffectText = "Копировать в";
                dropInfo.DestinationText = $"{targetFolder.Name}";
            }
        }

        #endregion

        #region Drop

        private void DropItems(IReadOnlyList<FileSystemInfo> sourceItems,
            DragDropEffects effects,
            DirectoryInfo targetDirectory)
        {
            switch (effects)
            {
                case DragDropEffects.Move:
                    _fileOperations.Move(sourceItems, targetDirectory);
                    break;
                case DragDropEffects.Copy:
                    _fileOperations.Copy(sourceItems, targetDirectory);
                    break;
                case DragDropEffects.Link:
                    _fileOperations.CreateLink(sourceItems, targetDirectory);
                    break;
            }
        }

        private static IReadOnlyList<FileSystemInfo> GetFileDropData(IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(DataFormats.FileDrop) &&
                dataObject.GetData(DataFormats.FileDrop) is string[] fullPaths)
            {
                return fullPaths.Select(fs => fs.ToInfo())
                    .OfType<FileSystemInfo>()
                    .ToList();
            }

            return Enumerable.Empty<FileSystemInfo>().ToList();
        }

        #endregion

        #endregion
    }
}