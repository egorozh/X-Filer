using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.FileOperations;
using XFiler.SDK;

namespace XFiler
{
    internal class FileOperations : IFileOperations
    {
        public event EventHandler<FileOperationArgs>? OperationHappened;

        public void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            var srcPaths = new List<string>();

            foreach (var source in sourceItems)
                srcPaths.Add(source.FullName);

            FileSystemEx.MoveFiles(srcPaths, targetDir, UICancelOption.DoNothing);

            OperationHappened?.Invoke(this, new FileOperationArgs(null, null, targetDirectory));
        }

        public void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            var srcPaths = sourceItems.Select(source => source.FullName).ToList();

            FileSystemEx.CopyFiles(srcPaths, targetDir, UICancelOption.DoNothing);
            OperationHappened?.Invoke(this, new FileOperationArgs(null, null, targetDirectory));
        }

        public void Delete(IReadOnlyList<FileSystemInfo> items, DirectoryInfo targetDirectory,
            bool isDeletePermanently = false)
        {
            FileSystemEx.DeleteFiles(items.Select(source => source.FullName).ToList(),
                UIOption.AllDialogs,
                isDeletePermanently ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin,
                UICancelOption.DoNothing);
            OperationHappened?.Invoke(this, new FileOperationArgs(null, null, targetDirectory));
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}