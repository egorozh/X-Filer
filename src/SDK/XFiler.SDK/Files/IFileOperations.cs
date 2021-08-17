using System;
using System.Collections.Generic;
using System.IO;

namespace XFiler.SDK
{
    public interface IFileOperations
    {
        event EventHandler<FileOperationArgs> OperationHappened;

        void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void Delete(IReadOnlyList<FileSystemInfo> items, DirectoryInfo targetDirectory,
            bool isDeletePermanently = false);

        void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);
    }

    public class FileOperationArgs : EventArgs
    {
        public FileOperationArgs(IReadOnlyList<FileSystemInfo>? newItems, IReadOnlyList<FileSystemInfo>? removedItems,
            DirectoryInfo targetDirectory)
        {
            NewItems = newItems;
            RemovedItems = removedItems;
            TargetDirectory = targetDirectory;
        }

        public DirectoryInfo TargetDirectory { get; }

        public IReadOnlyList<FileSystemInfo>? NewItems { get; }
        public IReadOnlyList<FileSystemInfo>? RemovedItems { get; }
    }
}