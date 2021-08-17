using System.Collections.Generic;
using System.IO;
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

            FileSystemEx.MoveFiles(srcPaths, targetDir, UICancelOption.DoNothing);
        }

        public void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            var srcPaths = new List<string>();

            foreach (var source in sourceItems) 
                srcPaths.Add(source.FullName);

            FileSystemEx.CopyFiles(srcPaths, targetDir, UICancelOption.DoNothing);
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}