using System.Collections.Generic;
using System.IO;

namespace XFiler.SDK
{
    public interface IFileOperations
    {
        void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);
        
        void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

        void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);
    }
}
