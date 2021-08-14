using System.Collections.Generic;
using System.IO;
using XFiler.SDK;

namespace XFiler
{
    internal class FileOperations : IFileOperations
    {
        public void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }

        public void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }

        public void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory)
        {
        }
    }
}