using GongSolutions.Wpf.DragDrop;
using System.IO;

namespace XFiler.DragDrop;

internal static class DragDropExtensions
{
    #region Public Methods

    public static IReadOnlyList<FileSystemInfo> GetSources(this IDropInfo dropInfo) => dropInfo.Data switch
    {
        IFileSystemModel sourceItem => new List<FileSystemInfo> { sourceItem.Info },
        ICollection<object> sourceItems => sourceItems.Cast<IFileSystemModel>()
            .Select(fs => fs.Info).ToList(),
        IDataObject dataObject => dataObject.GetFileDropData(),
        _ => Enumerable.Empty<FileSystemInfo>().ToList()
    };

    public static DirectoryInfo? GetTarget(this IDropInfo dropInfo)
    {
        if (dropInfo.VisualTarget is not FrameworkElement { DataContext: IFilesPresenter filesPresenter })
            return null;

        return dropInfo.TargetItem is IDirectoryModel dirVm
            ? dirVm.DirectoryInfo
            : filesPresenter.DirectoryInfo;
    }

    public static bool CanDragItems(this IDropInfo dropInfo,
        IReadOnlyList<FileSystemInfo> sourceItems,
        DirectoryInfo targetDirectory)
    {
        if (!sourceItems.Any())
            return false;

        var sourceItem = sourceItems.First();

        dropInfo.DropTargetAdorner = dropInfo.TargetItem switch
        {
            null  => typeof(ToFolderAdorner),
            IDirectoryModel => typeof(ToItemAdorner),
            _ => null
        };

        var isValid = dropInfo.Validation(sourceItems, targetDirectory, sourceItem);

        if (!isValid)
            return false;

        SetDropInfoParams(dropInfo, sourceItem, targetDirectory);
        return true;
    }

    #endregion

    #region Private Methods

    private static bool Validation(this IDropInfo dropInfo,
        IEnumerable<FileSystemInfo> sourceItems,
        DirectoryInfo target, FileSystemInfo source) => dropInfo.TargetItem switch
    {
        null when source.SourceValid(target) => true,
        IDirectoryModel when sourceItems.SourcesValid(target) =>true,
        _ => false
    };

    private static bool SourceValid(this FileSystemInfo sourceItem, DirectoryInfo targetDirectory)
    {
        IEnumerable<string> targetItems = targetDirectory
            .EnumerateFileSystemInfos()
            .Select(d => d.FullName);

        return !targetItems.Contains(sourceItem.FullName);
    }

    private static bool SourcesValid(this IEnumerable<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
    {
        return !sourceItems
            .Select(fi => fi.FullName)
            .Contains(targetDirectory.FullName);
    }

    private static void SetDropInfoParams(IDropInfo dropInfo, FileSystemInfo sourceItem, DirectoryInfo targetFolder)
    {
        var sourceRoot = sourceItem.GetRootName();
        var targetRoot = targetFolder.Root;
            
        if (sourceItem is DirectoryInfo { Parent: null })
        {
            dropInfo.Effects = DragDropEffects.Link;
            dropInfo.EffectText = "Создать ссылку в";
            dropInfo.DestinationText = $"{targetFolder.Name}";
        }
        else if (sourceRoot.FullName == targetRoot.FullName)
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

    private static IReadOnlyList<FileSystemInfo> GetFileDropData(this IDataObject dataObject)
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
}