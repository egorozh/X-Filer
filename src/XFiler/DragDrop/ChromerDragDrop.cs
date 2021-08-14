using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GongSolutions.Wpf.DragDrop;
using XFiler.SDK;

namespace XFiler.DragDrop
{
    public class ChromerDragDrop : IDropTarget
    {
        #region Private Fields

        private readonly IFileEntityFactory _fileEntityFactory;

        #endregion

        #region Constructor

        public ChromerDragDrop(IFileEntityFactory fileEntityFactory)
        {
            _fileEntityFactory = fileEntityFactory;
        }

        #endregion

        #region Public Methods

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not IDataObject && dropInfo?.DragInfo == null)
                return;

            if (!dropInfo.IsSameDragDropContextAsSource)
                return;

            if (dropInfo.TargetCollection is ListCollectionView
            {
                SourceCollection: ObservableCollection<FileEntityViewModel> targetCollection
            })
            {
                if (CanDrag(dropInfo, targetCollection))
                    return;
            }

            dropInfo.EffectText = null;
            dropInfo.DestinationText = null;
            dropInfo.DropTargetAdorner = null;
            dropInfo.Effects = DragDropEffects.Scroll;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo.Data is not IDataObject && dropInfo?.DragInfo == null)
                return;

            if (dropInfo.TargetCollection is ListCollectionView listCollectionView &&
                listCollectionView.SourceCollection is ObservableCollection<FileEntityViewModel> targetCollection)
            {
                var targetVisual = (ItemsControl)dropInfo.VisualTarget;

                if (targetVisual.DataContext is IFilesPresenter filesPresenter)
                {
                    if (dropInfo.Data is FileEntityViewModel sourceItem)
                        DropItems(new List<FileEntityViewModel>() { sourceItem },
                            dropInfo, dropInfo.TargetItem as FileEntityViewModel,
                            targetCollection,
                            filesPresenter.CurrentDirectory.FullName);
                    if (dropInfo.Data is ICollection<object> sourceItems)
                        DropItems(sourceItems.Cast<FileEntityViewModel>().ToList(), dropInfo,
                            dropInfo.TargetItem as FileEntityViewModel,
                            targetCollection,
                            filesPresenter.CurrentDirectory.FullName);
                }
            }
        }

        #endregion

        #region Private Methods

        #region Drag

        private static bool CanDrag(IDropInfo dropInfo, ObservableCollection<FileEntityViewModel> targetCollection)
            => dropInfo.Data switch
            {
                FileEntityViewModel sourceItem => CanDragManySourceItems(dropInfo, targetCollection,
                    new[] { sourceItem }),
                ICollection<object> sourceItems => CanDragManySourceItems(dropInfo, targetCollection,
                    sourceItems.Cast<FileEntityViewModel>().ToList()),
                IDataObject dataObject => CanDragExplorerItems(
                    dropInfo,
                    targetCollection.Select(f => f.Route.FullName),
                    dataObject),
                _ => false
            };

        private static bool CanDragManySourceItems(IDropInfo dropInfo,
            IReadOnlyList<FileEntityViewModel> targetCollection,
            IReadOnlyList<FileEntityViewModel> sourceItems)
        {
            var sourceItem = sourceItems.First();
            var sourceRoot = sourceItem.GetRootName();

            if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItem))
            {
                var targetVisual = (ItemsControl)dropInfo.VisualTarget;
                var viewModel = targetVisual.DataContext as IFilesPresenter;
                DirectoryInfo targetDirectory = new(viewModel.CurrentDirectory.FullName);

                var targetRoot = targetDirectory.Root.Name;

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetInsertAdorner);

                SetDropInfoParams(dropInfo, sourceItem.FullName.ToInfo(),
                    targetDirectory,
                    new DirectoryInfo(sourceRoot),
                    targetRoot);

                return true;
            }

            if (dropInfo.TargetItem is DirectoryViewModel targetFolder &&
                !sourceItems.Contains(targetFolder))
            {
                var targetRoot = targetFolder.GetRootName();

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);

                SetDropInfoParams(dropInfo, sourceItem.FullName.ToInfo(),
                    targetFolder.DirectoryInfo,
                    new DirectoryInfo(sourceRoot),
                    targetRoot);

                return true;
            }

            return false;
        }

        private static bool CanDragExplorerItems(IDropInfo dropInfo,
            IEnumerable<string> targetCollection,
            IDataObject dataObject)
        {
            if (!dataObject.GetDataPresent(DataFormats.FileDrop) ||
                dataObject.GetData(DataFormats.FileDrop) is not string[] fullPaths)
                return false;

            IReadOnlyList<FileSystemInfo> sourceItems = fullPaths.Select(GetFileInfo).ToList();

            var sourceItem = sourceItems.First();
            var sourceRoot = sourceItem.GetRootName();

            if (dropInfo.TargetItem == null
                && !targetCollection.Contains(sourceItem.FullName)
                && dropInfo.VisualTarget is Control
                {
                    DataContext: IFilesPresenter presenter
                })
            {
                DirectoryInfo targetDirectory = presenter.CurrentDirectory;

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

        private static FileSystemInfo GetFileInfo(string path) => path.ToInfo();

        #endregion

        #region Drop
        
        private void DropItems(ICollection<FileEntityViewModel> sourceItems,
            IDropInfo dropInfo,
            FileEntityViewModel? targetItem,
            ObservableCollection<FileEntityViewModel> targetCollection, string targetDirectory)
        {
            if (dropInfo.DragInfo.SourceCollection is not ListCollectionView
            {
                SourceCollection: ObservableCollection<FileEntityViewModel> sourceCollection
            }) return;
            
            var copyAndMove = WpfCopyAndMove.Instance;

            bool result;

            if (dropInfo.Effects == DragDropEffects.Move)
            {
                result = copyAndMove.Move(sourceItems,
                    targetItem as DirectoryViewModel ??
                    _fileEntityFactory.CreateDirectory(new DirectoryInfo(targetDirectory)));

                if (result)
                {
                    sourceCollection.RemoveRange(sourceItems);

                    if (targetItem == null)
                        targetCollection.AddRange(sourceItems);
                }
            }
            else if (dropInfo.Effects == DragDropEffects.Copy)
            {
                result = copyAndMove.Copy(sourceItems,
                    targetItem as DirectoryViewModel ??
                    _fileEntityFactory.CreateDirectory(new DirectoryInfo(targetDirectory)));

                if (result)
                {
                    if (targetItem == null)
                        targetCollection.AddRange(sourceItems.Select(i => i.Clone()));
                }
            }
            else if (dropInfo.Effects == DragDropEffects.Link)
            {
            }
        }

        #endregion

        #endregion
    }

    internal class WpfCopyAndMove
    {
        #region Singleton

        private static WpfCopyAndMove? _instance;

        public static WpfCopyAndMove Instance => _instance ??= new WpfCopyAndMove();

        private WpfCopyAndMove()
        {
        }

        #endregion

        #region Public Methods

        public bool Move(FileEntityViewModel sourceItem, DirectoryViewModel targetDirectory)
            => Move(new List<FileEntityViewModel> { sourceItem }, targetDirectory);

        public bool Move(ICollection<FileEntityViewModel> sourceItems, DirectoryViewModel targetDirectory)
        {
            //foreach (var fileEntityViewModel in sourceItems)
            //{
            //    FileTransferManager.MoveWithProgress(fileEntityViewModel.FullName,
            //        Path.Combine(targetDirectory.FullName, fileEntityViewModel.Name), progress =>
            //        {

            //        });
            //}

            return true;
        }

        public bool Copy(FileEntityViewModel sourceItem, DirectoryViewModel targetDirectory)
            => Copy(new List<FileEntityViewModel> { sourceItem }, targetDirectory);

        public bool Copy(ICollection<FileEntityViewModel> sourceItems, DirectoryViewModel targetDirectory)
        {
            return true;
        }

        #endregion
    }

    internal static class Extensions
    {
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> addedElems)
        {
            foreach (var addedElem in addedElems)
                source.Add(addedElem);
        }

        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> removedElems)
        {
            foreach (var removedElem in removedElems)
                source.Remove(removedElem);
        }
    }
}