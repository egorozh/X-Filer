﻿using System.IO;
using Windows.FileOperations;
using Windows.FileOperations.FileOperation;
using RecycleOption = Windows.FileOperations.RecycleOption;
using UICancelOption = Windows.FileOperations.UICancelOption;
using UIOption = Windows.FileOperations.UIOption;

namespace XFiler;

internal sealed class FileOperations : IFileOperations
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

    public void Rename(FileSystemInfo info, string? newName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            return;
        }

        Task.Run(() =>
        {
            using var renameOp = new RenameFilesOperation();

            renameOp.Rename(info.FullName, newName);
        });
    }

    public void CreateFolder(string targetFolder, string name = "Новая папка")
    {
        Task.Run(() =>
        {
            string folderPath = Path.Combine(targetFolder, name);

            var index = 2;

            while (Directory.Exists(folderPath))
                folderPath = Path.Combine(targetFolder, $"{name} ({index++})");

            TryAction(() => Directory.CreateDirectory(folderPath));
        });
    }

    public void CreateEmptyTextFile(string targetFolder, string name = "Новый текстовый документ")
    {
        Task.Run(() =>
        {
            string filePath = Path.Combine(targetFolder, $"{name}.txt");

            var index = 2;

            while (File.Exists(filePath))
                filePath = Path.Combine(targetFolder, $"{name} ({index++}).txt");

            TryAction(() => File.WriteAllText(filePath, ""));
        });
    }

    private static void TryAction(Action action)
    {
        try
        {
            action.Invoke();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Ошибка");
        }
    }
}