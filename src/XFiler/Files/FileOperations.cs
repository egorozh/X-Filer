using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
                        Task.Run(() =>
                        {
                            FileSystem.MoveDirectory(
                                directoryInfo.FullName,
                                Path.Combine(targetDir, directoryInfo.Name),
                                UIOption.AllDialogs,
                                UICancelOption.DoNothing);
                        });
                        break;
                    case FileInfo fileInfo:
                        Task.Run(() =>
                        {
                            FileSystem.MoveFile(fileInfo.FullName,
                                Path.Combine(targetDir, fileInfo.Name),
                                UIOption.AllDialogs,
                                UICancelOption.DoNothing);
                        });
                        break;
                }
            }
        }

        public void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
            var targetDir = targetDirectory.FullName;

            foreach (var fileSystemInfo in sourceItems)
            {
                switch (fileSystemInfo)
                {
                    case DirectoryInfo directoryInfo:
                        Task.Run(() =>
                        {
                            FileSystem.CopyDirectory(
                                directoryInfo.FullName,
                                Path.Combine(targetDir, directoryInfo.Name),
                                UIOption.AllDialogs,
                                UICancelOption.DoNothing);
                        });
                        break;
                    case FileInfo fileInfo:
                        Task.Run(() =>
                        {
                            FileSystem.CopyFile(fileInfo.FullName,
                                Path.Combine(targetDir, fileInfo.Name),
                                UIOption.AllDialogs,
                                UICancelOption.DoNothing);
                        });

                        break;
                }
            }
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}