using System.Collections.Generic;
using System.IO;
using FileOperationsEx;
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
           
            //var sourceBuilder = new List<string>();
            //var targetPathBuilder = new List<string>();

            //foreach (var source in sourceItems)
            //{
            //    sourceBuilder.Add(source.FullName);
            //    targetPathBuilder.Add(Path.Combine(targetDir, source.Name));
            //}
            
            //var srcPaths = Path.Join(sourceBuilder.ToArray());
            //var targetPaths = Path.Join(targetPathBuilder.ToArray());

            //FileSystemEx.CopyFile(
            //    srcPaths,
            //    targetPaths,
            //    UIOption.AllDialogs,
            //    UICancelOption.DoNothing);

            foreach (var fileSystemInfo in sourceItems)
            {
                switch (fileSystemInfo)
                {
                    case DirectoryInfo directoryInfo:

                        FileSystemEx.CopyDirectory(
                            directoryInfo.FullName,
                            Path.Combine(targetDir, directoryInfo.Name),
                            UIOption.AllDialogs,
                            UICancelOption.DoNothing);

                        break;
                    case FileInfo fileInfo:

                        FileSystemEx.CopyFile(fileInfo.FullName,
                            Path.Combine(targetDir, fileInfo.Name),
                            UIOption.AllDialogs,
                            UICancelOption.DoNothing);

                        break;
                }
            }
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}