using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XFiler.SDK;

public interface IFileOperations
{
    void Move(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

    void Copy(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

    void Delete(IReadOnlyList<FileSystemInfo> items, DirectoryInfo targetDirectory,
        bool isDeletePermanently = false);

    void CreateLink(IReadOnlyList<FileSystemInfo> sourceItems, DirectoryInfo targetDirectory);

    void Rename(FileSystemInfo modelInfo, string newName);

    void CreateFolder(string targetFolder, string name = "Новая папка");

    void CreateEmptyTextFile(string targetFolder, string name = "Новый текстовый документ");

    Task<string> CreateFile(string targetFolder, string name, string extension);
        
    Task<string> CreateFileFromTemplate(string targetFolder, string name, string extension, string template);
}   