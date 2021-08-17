using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.FileOperations;
using XFiler.SDK;

namespace XFiler
{
    internal class FileOperations : IFileOperations
    {
        public void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            foreach (var fileSystemInfo in sourceItems)
            {
                switch (fileSystemInfo)
                {
                    case DirectoryInfo directoryInfo:

                        FileSystemEx.MoveDirectory(
                            directoryInfo.FullName,
                            Path.Combine(targetDir, directoryInfo.Name),
                            UIOption.AllDialogs,
                            UICancelOption.DoNothing);

                        break;
                    case FileInfo fileInfo:

                        FileSystemEx.MoveFile(fileInfo.FullName,
                            Path.Combine(targetDir, fileInfo.Name),
                            UIOption.AllDialogs,
                            UICancelOption.DoNothing);

                        break;
                }
            }
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