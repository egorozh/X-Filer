using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.FileOperations;
using XFiler.SDK;

namespace XFiler
{
    internal class FileOperations : IFileOperations
    {
        public void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            var srcPaths = new List<string>();

            foreach (var source in sourceItems)
                srcPaths.Add(source.FullName);

            Task.Run(() => { FileSystemEx.MoveFiles(srcPaths, targetDir, UICancelOption.DoNothing); });
        }

        public void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            var srcPaths = sourceItems.Select(source => source.FullName).ToList();
            Task.Run(() => { FileSystemEx.CopyFiles(srcPaths, targetDir, UICancelOption.DoNothing); });
        }

        public void Delete(IReadOnlyList<FileSystemInfo> items, DirectoryInfo targetDirectory,
            bool isDeletePermanently = false)
        {
            Task.Run(() =>
            {
                FileSystemEx.DeleteFiles(items.Select(source => source.FullName).ToList(),
                    UIOption.AllDialogs,
                    isDeletePermanently ? RecycleOption.DeletePermanently : RecycleOption.SendToRecycleBin,
                    UICancelOption.DoNothing);
            });
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}