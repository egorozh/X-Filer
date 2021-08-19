using System.Collections.Generic;
using System.IO;

namespace XFiler.SDK
{
    public interface IFileOperations
    {
        void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void Delete(IReadOnlyList<FileSystemInfo> items, DirectoryInfo targetDirectory,
            bool isDeletePermanently = false);

        void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void Rename(FileSystemInfo modelInfo, string newName);
    }
}