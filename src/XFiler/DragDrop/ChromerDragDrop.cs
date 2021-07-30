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
            if (dropInfo?.DragInfo == null)
            {
                return;
            }

            if (!dropInfo.IsSameDragDropContextAsSource)
            {
                return;
            }

            //var debugWindow = Application.Current.Windows.OfType<DebugWindow>().FirstOrDefault();

            //if (debugWindow != null)
            //{
            //    debugWindow.TextBlock.Text =
            //        $"{dropInfo.VisualTarget} ; {dropInfo.VisualTargetItem}";
            //}

            if (dropInfo.TargetCollection is ListCollectionView listCollectionView &&
                listCollectionView.SourceCollection is ObservableCollection<FileEntityViewModel> targetCollection)
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
            if (dropInfo.TargetCollection is ListCollectionView listCollectionView &&
                listCollectionView.SourceCollection is ObservableCollection<FileEntityViewModel> targetCollection &&
                dropInfo.DragInfo.SourceCollection is ListCollectionView sourceListCollectionView &&
                sourceListCollectionView.SourceCollection is ObservableCollection<FileEntityViewModel> sourceCollection)
            {
                var targetVisual = (ItemsControl)dropInfo.VisualTarget;

                if (targetVisual.DataContext is IFilesPresenter filesPresenter)
                {
                    if (dropInfo.Data is FileEntityViewModel sourceItem)
                        DropItem(sourceItem, sourceCollection, dropInfo, dropInfo.TargetItem as FileEntityViewModel,
                            targetCollection,
                            filesPresenter.CurrentDirectoryPathName);
                    if (dropInfo.Data is ICollection<object> sourceItems)
                        DropItems(sourceItems.Cast<FileEntityViewModel>().ToList(), sourceCollection, dropInfo,
                            dropInfo.TargetItem as FileEntityViewModel,
                            targetCollection,
                            filesPresenter.CurrentDirectoryPathName);
                }
            }
        }

        #endregion

        #region Private Methods

        #region Drag

        private bool CanDrag(IDropInfo dropInfo, ObservableCollection<FileEntityViewModel> targetCollection)
            => dropInfo.Data switch
            {
                FileEntityViewModel sourceItem => CanDragOneSourceItem(dropInfo, targetCollection, sourceItem),
                ICollection<object> sourceItems => CanDragManySourceItems(dropInfo, targetCollection,
                    sourceItems.Cast<FileEntityViewModel>().ToList()),
                _ => false
            };

        private bool CanDragOneSourceItem(IDropInfo dropInfo,
            ObservableCollection<FileEntityViewModel> targetCollection,
            FileEntityViewModel sourceItem)
        {
            var sourceRoot = sourceItem.GetRootName();

            //var debugWindow = Application.Current.Windows.OfType<DebugWindow>().FirstOrDefault();

            //if (debugWindow != null)
            //{
            //    debugWindow.TextBlock.Text =
            //        $"{dropInfo.TargetItem}";
            //}

            if (dropInfo.TargetItem is DirectoryViewModel targetFolder && targetFolder != sourceItem)
            {
                var targetRoot = targetFolder.GetRootName();

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetFolder.Name}";
                }
                else if (sourceRoot == targetRoot)
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

                return true;
            }

            if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItem))
            {
                var targetVisual = (ItemsControl)dropInfo.VisualTarget;
                var viewModel = targetVisual.DataContext as IFilesPresenter;
                DirectoryInfo targetDirectory = new(viewModel.CurrentDirectoryPathName);

                var targetRoot = targetDirectory.Root.Name;

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetInsertAdorner);

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else if (sourceRoot == targetRoot)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                    dropInfo.EffectText = "Переместить в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    dropInfo.EffectText = "Копировать в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }

                return true;
            }

            return false;
        }

        private bool CanDragManySourceItems(IDropInfo dropInfo,
            ObservableCollection<FileEntityViewModel> targetCollection,
            ICollection<FileEntityViewModel> sourceItems)
        {
            var sourceItem = sourceItems.First();
            var sourceRoot = sourceItem.GetRootName();

            if (dropInfo.TargetItem == null && !targetCollection.Contains(sourceItems.First()))
            {
                var targetVisual = (ItemsControl)dropInfo.VisualTarget;
                var viewModel = targetVisual.DataContext as IFilesPresenter;
                DirectoryInfo targetDirectory = new(viewModel.CurrentDirectoryPathName);

                var targetRoot = targetDirectory.Root.Name;

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetInsertAdorner);

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else if (sourceRoot == targetRoot)
                {
                    dropInfo.Effects = DragDropEffects.Move;
                    dropInfo.EffectText = "Переместить в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }
                else
                {
                    dropInfo.Effects = DragDropEffects.Copy;
                    dropInfo.EffectText = "Копировать в";
                    dropInfo.DestinationText = $"{targetDirectory.Name}";
                }

                return true;
            }

            if (dropInfo.TargetItem is DirectoryViewModel targetFolder &&
                !sourceItems.Contains(targetFolder))
            {
                var targetRoot = targetFolder.GetRootName();

                dropInfo.DropTargetAdorner = typeof(ChromerDropTargetHighlightAdorner);

                if (sourceItem is LogicalDriveViewModel logicalDrive)
                {
                    dropInfo.Effects = DragDropEffects.Link;
                    dropInfo.EffectText = "Создать ссылку в";
                    dropInfo.DestinationText = $"{targetFolder.Name}";
                }
                else if (sourceRoot == targetRoot)
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

                return true;
            }

            return false;
        }

        #endregion

        #region Drop

        private void DropItem(FileEntityViewModel sourceItem,
            ObservableCollection<FileEntityViewModel> sourceCollection, IDropInfo dropInfo,
            FileEntityViewModel? targetItem,
            ObservableCollection<FileEntityViewModel> targetCollection, string targetDirectory)
        {
            var copyAndMove = WpfCopyAndMove.Instance;

            bool result;

            if (dropInfo.Effects == DragDropEffects.Move)
            {
                result = copyAndMove.Move(sourceItem,
                    targetItem as DirectoryViewModel ??
                    _fileEntityFactory.CreateDirectory(new DirectoryInfo(targetDirectory)));

                if (result)
                {
                    sourceCollection.Remove(sourceItem);

                    if (targetItem == null)
                        targetCollection.Add(sourceItem);
                }
            }
            else if (dropInfo.Effects == DragDropEffects.Copy)
            {
                result = copyAndMove.Copy(sourceItem,
                    targetItem as DirectoryViewModel ??
                    _fileEntityFactory.CreateDirectory(new DirectoryInfo(targetDirectory)));

                if (result)
                {
                    if (targetItem == null)
                        targetCollection.Add(sourceItem.Clone());
                }
            }
            else if (dropInfo.Effects == DragDropEffects.Link)
            {
            }
        }

        private void DropItems(ICollection<FileEntityViewModel> sourceItems,
            ObservableCollection<FileEntityViewModel> sourceCollection, IDropInfo dropInfo,
            FileEntityViewModel? targetItem,
            ObservableCollection<FileEntityViewModel> targetCollection, string targetDirectory)
        {
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