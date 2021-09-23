using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Windows.FileOperations.Resources;

namespace Windows.FileOperations
{
    public class FileSystemEx
    {
        private static readonly char[] MSeparatorChars =
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar,
            Path.VolumeSeparatorChar
        };

        #region Public Methods

        public static string GetParentPath(string path)
        {
            Path.GetFullPath(path);
            if (IsRoot(path))
                throw ExceptionUtils.GetArgumentExceptionWithArgName(nameof(path),
                    "Could not get parent path since the given path is a root directory: '{0}'.", path);
            return Path.GetDirectoryName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }

        public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName,
            bool overwrite = false)
            => CopyOrMoveDirectory(CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, overwrite,
                UiOptionInternal.NO_UI, UICancelOption.ThrowException);

        public static void CopyDirectory(string sourceDirectoryName, string destinationDirectoryName,
            UIOption showUi, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => CopyOrMoveDirectory(CopyOrMove.Copy, sourceDirectoryName, destinationDirectoryName, false,
                ToUiOptionInternal(showUi), onUserCancel);

        public static void CopyFile(string sourceFileName, string destinationFileName, bool overwrite = false)
            => CopyOrMoveFile(CopyOrMove.Copy, sourceFileName, destinationFileName, overwrite, UiOptionInternal.NO_UI,
                UICancelOption.ThrowException);

        public static void CopyFile(string sourceFileName, string destinationFileName,
            UIOption showUi, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => CopyOrMoveFile(CopyOrMove.Copy, sourceFileName, destinationFileName, false,
                ToUiOptionInternal(showUi), onUserCancel);

        public static void CopyFiles(IReadOnlyList<string> sources, string destinationFileName,
            UICancelOption onUserCancel)
            => CopyOrMoveFiles(CopyOrMove.Copy, sources, destinationFileName, onUserCancel);

        public static void DeleteDirectory(string directory, DeleteDirectoryOption onDirectoryNotEmpty) =>
            DeleteDirectoryInternal(directory, onDirectoryNotEmpty, UiOptionInternal.NO_UI,
                RecycleOption.DeletePermanently, UICancelOption.ThrowException);

        public static void DeleteDirectory(string directory, UIOption showUi,
            RecycleOption recycle = RecycleOption.DeletePermanently,
            UICancelOption onUserCancel = UICancelOption.ThrowException)
            => DeleteDirectoryInternal(directory, DeleteDirectoryOption.DeleteAllContents,
                ToUiOptionInternal(showUi), recycle, onUserCancel);

        public static void DeleteFile(string file) => DeleteFileInternal(file, UiOptionInternal.NO_UI,
            RecycleOption.DeletePermanently, UICancelOption.ThrowException);

        public static void DeleteFile(string file, UIOption showUi,
            RecycleOption recycle, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => DeleteFileInternal(file, ToUiOptionInternal(showUi), recycle, onUserCancel);

        public static void DeleteFiles(IReadOnlyList<string> items, UIOption showUi,
            RecycleOption recycle, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => DeleteFileInternal(items, showUi, recycle, onUserCancel);

        public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName,
            bool overwrite = false)
            => CopyOrMoveDirectory(CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, overwrite,
                UiOptionInternal.NO_UI, UICancelOption.ThrowException);

        public static void MoveDirectory(string sourceDirectoryName, string destinationDirectoryName,
            UIOption showUi, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => CopyOrMoveDirectory(CopyOrMove.Move, sourceDirectoryName, destinationDirectoryName, false,
                ToUiOptionInternal(showUi), onUserCancel);

        public static void MoveFile(string sourceFileName, string destinationFileName, bool overwrite = false)
            => CopyOrMoveFile(CopyOrMove.Move, sourceFileName, destinationFileName, overwrite,
                UiOptionInternal.NO_UI, UICancelOption.ThrowException);

        public static void MoveFile(string sourceFileName, string destinationFileName,
            UIOption showUi, UICancelOption onUserCancel = UICancelOption.ThrowException)
            => CopyOrMoveFile(CopyOrMove.Move, sourceFileName, destinationFileName,
                false, ToUiOptionInternal(showUi), onUserCancel);

        public static void MoveFiles(IReadOnlyList<string> sources, string destinationFileName,
            UICancelOption onUserCancel)
            => CopyOrMoveFiles(CopyOrMove.Move, sources, destinationFileName, onUserCancel);

        public static void RenameDirectory(string directory, string newName)
        {
            directory = Path.GetFullPath(directory);
            ThrowIfDevicePath(directory);
            if (IsRoot(directory))
                throw ExceptionUtils.GetIoException(SR.IO_DirectoryIsRoot_Path, directory);
            if (!Directory.Exists(directory))
                throw ExceptionUtils.GetDirectoryNotFoundException(SR.IO_DirectoryNotFound_Path, directory);
            string str = !string.IsNullOrEmpty(newName)
                ? GetFullPathFromNewName(GetParentPath(directory), newName, nameof(newName))
                : throw ExceptionUtils.GetArgumentNullException(nameof(newName), SR.General_ArgumentEmptyOrNothing_Name,
                    nameof(newName));
            Debug.Assert(GetParentPath(str).Equals(GetParentPath(directory), StringComparison.OrdinalIgnoreCase),
                "Invalid FullNewPath");
            EnsurePathNotExist(str);
            Directory.Move(directory, str);
        }

        public static void RenameFile(string file, string newName)
        {
            file = NormalizeFilePath(file, nameof(file));
            ThrowIfDevicePath(file);
            if (!File.Exists(file))
                throw ExceptionUtils.GetFileNotFoundException(file, "Could not find file '{0}'.", file);
            string str = !string.IsNullOrEmpty(newName)
                ? GetFullPathFromNewName(GetParentPath(file), newName, nameof(newName))
                : throw ExceptionUtils.GetArgumentNullException(nameof(newName), SR.General_ArgumentEmptyOrNothing_Name,
                    nameof(newName));
            Debug.Assert(GetParentPath(str).Equals(GetParentPath(file), StringComparison.OrdinalIgnoreCase),
                "Invalid FullNewPath");
            EnsurePathNotExist(str);
            File.Move(file, str);
        }

        public static void RenameFile(string filePath, string newName, UIOption showUi,
            UICancelOption onUserCancel = UICancelOption.ThrowException)
        {
            var operationFlags = GetOperationFlags(ToUiOptionInternal(showUi), true);

            ShellFileOperation(NativeMethods.SHFileOperationType.FO_RENAME,
                operationFlags,
                NormalizeFilePath(filePath, nameof(filePath)),
                NormalizeFilePath(newName, nameof(newName)),
                onUserCancel);
        }

        public static void RenameDirectory(string directory, string newName, UIOption showUi,
            UICancelOption onUserCancel = UICancelOption.ThrowException)
        {
            var operationFlags = GetOperationFlags(ToUiOptionInternal(showUi));

            ShellFileOperation(NativeMethods.SHFileOperationType.FO_RENAME,
                operationFlags, NormalizePath(directory),
                NormalizeFilePath(newName, nameof(newName)), onUserCancel);
        }

        #endregion

        #region Private Methods

        private static string NormalizeFilePath(string path, string paramName)
        {
            CheckFilePathTrailingSeparator(path, paramName);
            return NormalizePath(path);
        }

        private static string NormalizePath(string path) => GetLongPath(RemoveEndingSeparator(Path.GetFullPath(path)));

        private static void CheckFilePathTrailingSeparator(string path, string paramName)
        {
            if (string.IsNullOrEmpty(path))
                throw ExceptionUtils.GetArgumentNullException(paramName);
            if (path.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal) |
                path.EndsWith(Path.AltDirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                throw ExceptionUtils.GetArgumentExceptionWithArgName(paramName, SR.IO_FilePathException);
        }

        private static void AddToStringCollection(ICollection<string>? strCollection,
            string[]? strArray)
        {
            Debug.Assert(strCollection != null, "StrCollection is NULL");

            if (strArray == null)
                return;

            foreach (var str in strArray)
            {
                if (!strCollection.Contains(str))
                    strCollection.Add(str);
            }
        }

        private static void CopyOrMoveDirectory(
            CopyOrMove operation,
            string sourceDirectoryName,
            string destinationDirectoryName,
            bool overwrite,
            UiOptionInternal showUi,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation), "Invalid Operation");
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);
            string str1 = NormalizePath(sourceDirectoryName);
            string str2 = NormalizePath(destinationDirectoryName);
            ThrowIfDevicePath(str1);
            ThrowIfDevicePath(str2);
            if (!Directory.Exists(str1))
                throw ExceptionUtils.GetDirectoryNotFoundException(SR.IO_DirectoryNotFound_Path, sourceDirectoryName);
            if (IsRoot(str1))
                throw ExceptionUtils.GetIoException(SR.IO_DirectoryIsRoot_Path, sourceDirectoryName);
            if (File.Exists(str2))
                throw ExceptionUtils.GetIoException(SR.IO_FileExists_Path, destinationDirectoryName);
            if (str2.Equals(str1, StringComparison.OrdinalIgnoreCase))
                throw ExceptionUtils.GetIoException(SR.IO_SourceEqualsTargetDirectory);
            if (str2.Length > str1.Length &&
                str2.Substring(0, str1.Length).Equals(str1, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Assert(str2.Length > str1.Length, "Target path should be longer");
                if (str2[str1.Length] == Path.DirectorySeparatorChar)
                    throw ExceptionUtils.GetInvalidOperationException(SR.IO_CyclicOperation);
            }

            if (showUi != UiOptionInternal.NO_UI && Environment.UserInteractive)
                ShellCopyOrMove(operation, FileOrDirectory.Directory, str1, str2, showUi, onUserCancel);
            else
                FxCopyOrMoveDirectory(operation, str1, str2, overwrite);
        }

        private static void FxCopyOrMoveDirectory(
            CopyOrMove operation,
            string sourceDirectoryPath,
            string targetDirectoryPath,
            bool overwrite)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation), "Invalid Operation");
            Debug.Assert(!string.IsNullOrEmpty(sourceDirectoryPath) &
                         Path.IsPathRooted(sourceDirectoryPath), "Invalid     Source");
            Debug.Assert(!string.IsNullOrEmpty(targetDirectoryPath) &
                         Path.IsPathRooted(targetDirectoryPath), "Invalid Target");
            if (operation == CopyOrMove.Move & !Directory.Exists(targetDirectoryPath) &
                IsOnSameDrive(sourceDirectoryPath, targetDirectoryPath))
            {
                Directory.CreateDirectory(GetParentPath(targetDirectoryPath));
                try
                {
                    Directory.Move(sourceDirectoryPath, targetDirectoryPath);
                    return;
                }
                catch (IOException ex)
                {
                }
                catch (UnauthorizedAccessException ex)
                {
                }
            }

            Directory.CreateDirectory(targetDirectoryPath);
            Debug.Assert(Directory.Exists(targetDirectoryPath), "Should be able to create Target Directory");
            DirectoryNode sourceDirectoryNode = new(sourceDirectoryPath, targetDirectoryPath);
            ListDictionary exceptions = new();
            CopyOrMoveDirectoryNode(operation, sourceDirectoryNode, overwrite, exceptions);
            if (exceptions.Count > 0)
            {
                IOException ioException = new(SR.IO_CopyMoveRecursive);
                foreach (object obj in exceptions)
                {
                    DictionaryEntry dictionaryEntry = obj != null ? (DictionaryEntry)obj : new DictionaryEntry();
                    ioException.Data.Add(RuntimeHelpers.GetObjectValue(dictionaryEntry.Key),
                        RuntimeHelpers.GetObjectValue(dictionaryEntry.Value));
                }

                throw ioException;
            }
        }

        private static void CopyOrMoveDirectoryNode(
            CopyOrMove operation,
            DirectoryNode sourceDirectoryNode,
            bool overwrite,
            ListDictionary exceptions)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation), "Invalid Operation");
            Debug.Assert(exceptions != null, "Null exception list");
            Debug.Assert(sourceDirectoryNode != null, "Null source node");
            try
            {
                if (!Directory.Exists(sourceDirectoryNode.TargetPath))
                    Directory.CreateDirectory(sourceDirectoryNode.TargetPath);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                int num;
                switch (exception)
                {
                    case IOException _:
                    case UnauthorizedAccessException _:
                    case NotSupportedException _:
                        num = 1;
                        break;
                    default:
                        num = exception is SecurityException ? 1 : 0;
                        break;
                }

                if (num != 0)
                {
                    exceptions.Add(sourceDirectoryNode.Path, exception.Message);
                    return;
                }

                throw;
            }

            Debug.Assert(Directory.Exists(sourceDirectoryNode.TargetPath),
                "TargetPath should have existed or exception should be thrown");
            if (!Directory.Exists(sourceDirectoryNode.TargetPath))
            {
                exceptions.Add(sourceDirectoryNode.TargetPath,
                    ExceptionUtils.GetDirectoryNotFoundException(SR.IO_DirectoryNotFound_Path,
                        sourceDirectoryNode.TargetPath));
            }
            else
            {
                string[] files = Directory.GetFiles(sourceDirectoryNode.Path);
                int index = 0;
                while (index < files.Length)
                {
                    string str = files[index];
                    try
                    {
                        CopyOrMoveFile(operation, str,
                            Path.Combine(sourceDirectoryNode.TargetPath, Path.GetFileName(str)), overwrite,
                            UiOptionInternal.NO_UI, UICancelOption.ThrowException);
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        int num;
                        switch (exception)
                        {
                            case IOException _:
                            case UnauthorizedAccessException _:
                            case SecurityException _:
                                num = 1;
                                break;
                            default:
                                num = exception is NotSupportedException ? 1 : 0;
                                break;
                        }

                        if (num != 0)
                        {
                            exceptions.Add(str, exception.Message);
                        }
                        else
                            throw;
                    }

                    checked
                    {
                        ++index;
                    }
                }

                foreach (DirectoryNode subDir in sourceDirectoryNode.SubDirs)
                    CopyOrMoveDirectoryNode(operation, subDir, overwrite, exceptions);

                if (operation == CopyOrMove.Move)
                {
                    try
                    {
                        Directory.Delete(sourceDirectoryNode.Path, false);
                    }
                    catch (Exception ex)
                    {
                        Exception exception = ex;
                        int num;
                        switch (exception)
                        {
                            case IOException _:
                            case UnauthorizedAccessException _:
                            case SecurityException _:
                                num = 1;
                                break;
                            default:
                                num = exception is DirectoryNotFoundException ? 1 : 0;
                                break;
                        }

                        if (num != 0)
                        {
                            exceptions.Add(sourceDirectoryNode.Path, exception.Message);
                        }
                        else
                            throw;
                    }
                }
            }
        }

        private static void CopyOrMoveFile(
            CopyOrMove operation,
            string sourceFileName,
            string destinationFileName,
            bool overwrite,
            UiOptionInternal showUi,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation), "Invalid Operation");
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);
            string str1 = NormalizeFilePath(sourceFileName, nameof(sourceFileName));
            string str2 = NormalizeFilePath(destinationFileName, nameof(destinationFileName));
            ThrowIfDevicePath(str1);
            ThrowIfDevicePath(str2);
            if (!File.Exists(str1))
                throw ExceptionUtils.GetFileNotFoundException(sourceFileName, "Could not find file '{0}'.",
                    sourceFileName);
            if (Directory.Exists(str2))
                throw ExceptionUtils.GetIoException(SR.IO_DirectoryExists_Path, destinationFileName);
            Directory.CreateDirectory(GetParentPath(str2));
            if (showUi != UiOptionInternal.NO_UI && Environment.UserInteractive)
                ShellCopyOrMove(operation, FileOrDirectory.File, str1, str2, showUi, onUserCancel);
            else if (operation == CopyOrMove.Copy || str1.Equals(str2, StringComparison.OrdinalIgnoreCase))
                File.Copy(str1, str2, overwrite);
            else if (overwrite)
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    try
                    {
                        if (!NativeMethods.MoveFileEx(str1, str2, 11))
                            ThrowWinIoError(Marshal.GetLastWin32Error());
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else
                {
                    File.Delete(str2);
                    File.Move(str1, str2);
                }
            }
            else
                File.Move(str1, str2);
        }

        private static void CopyOrMoveFiles(
            CopyOrMove operation,
            IReadOnlyCollection<string> sourcePaths,
            string destinationFolderPath,
            UICancelOption onUserCancel,
            UiOptionInternal options = UiOptionInternal.AllDialogs)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation), "Invalid Operation");
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);

            string str2 = NormalizePath(destinationFolderPath);

            if (!Directory.Exists(str2))
                Directory.CreateDirectory(str2);

            if (Environment.UserInteractive)
            {
                ShellCopyOrMove(operation, FileOrDirectory.File, sourcePaths, str2,
                    options, onUserCancel);
            }
        }

        private static void DeleteDirectoryInternal(
            string directory,
            DeleteDirectoryOption onDirectoryNotEmpty,
            UiOptionInternal showUi,
            RecycleOption recycle,
            UICancelOption onUserCancel)
        {
            VerifyDeleteDirectoryOption(nameof(onDirectoryNotEmpty), onDirectoryNotEmpty);
            VerifyRecycleOption(nameof(recycle), recycle);
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);
            string fullPath = Path.GetFullPath(directory);
            ThrowIfDevicePath(fullPath);
            if (!Directory.Exists(fullPath))
                throw ExceptionUtils.GetDirectoryNotFoundException(SR.IO_DirectoryNotFound_Path, directory);
            if (IsRoot(fullPath))
                throw ExceptionUtils.GetIoException(SR.IO_DirectoryIsRoot_Path, directory);
            if (showUi != UiOptionInternal.NO_UI && Environment.UserInteractive)
                ShellDelete(fullPath, showUi, recycle, onUserCancel, FileOrDirectory.Directory);
            else
                Directory.Delete(fullPath, onDirectoryNotEmpty == DeleteDirectoryOption.DeleteAllContents);
        }

        private static void DeleteFileInternal(
            string file,
            UiOptionInternal showUi,
            RecycleOption recycle,
            UICancelOption onUserCancel)
        {
            VerifyRecycleOption(nameof(recycle), recycle);
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);
            string str = NormalizeFilePath(file, nameof(file));
            ThrowIfDevicePath(str);
            if (!File.Exists(str))
                throw ExceptionUtils.GetFileNotFoundException(file, "Could not find file '{0}'.", file);
            if (showUi != UiOptionInternal.NO_UI && Environment.UserInteractive)
                ShellDelete(str, showUi, recycle, onUserCancel, FileOrDirectory.File);
            else
                File.Delete(str);
        }

        private static void DeleteFileInternal(
            IReadOnlyList<string> files,
            UIOption showUi,
            RecycleOption recycle,
            UICancelOption onUserCancel)
        {
            VerifyRecycleOption(nameof(recycle), recycle);
            VerifyUiCancelOption(nameof(onUserCancel), onUserCancel);

            ShellDelete(files, showUi, recycle, onUserCancel);
        }

        private static void EnsurePathNotExist(string path)
        {
            if (File.Exists(path))
                throw ExceptionUtils.GetIoException(
                    "Could not complete operation since a file already exists in this path '{0}'.", path);
            if (Directory.Exists(path))
                throw ExceptionUtils.GetIoException(
                    "Could not complete operation since a directory already exists in this path '{0}'.", path);
        }

        private static string GetFullPathFromNewName(string path, string newName, string argumentName)
        {
            Debug.Assert(!string.IsNullOrEmpty(path) && Path.IsPathRooted(path), path);
            Debug.Assert(path.Equals(Path.GetFullPath(path)), path);
            Debug.Assert(!string.IsNullOrEmpty(newName), "Null NewName");
            Debug.Assert(!string.IsNullOrEmpty(argumentName), "Null argument name");
            string path2 = newName.IndexOfAny(MSeparatorChars) < 0
                ? RemoveEndingSeparator(Path.GetFullPath(Path.Combine(path, newName)))
                : throw ExceptionUtils.GetArgumentExceptionWithArgName(argumentName,
                    "Argument '{0}' must be a name, and not a relative or absolute path: '{1}'.", argumentName,
                    newName);
            return GetParentPath(path2).Equals(path, StringComparison.OrdinalIgnoreCase)
                ? path2
                : throw ExceptionUtils.GetArgumentExceptionWithArgName(argumentName,
                    "Argument '{0}' must be a name, and not a relative or absolute path: '{1}'.", argumentName,
                    newName);
        }

        private static string GetLongPath(string fullPath)
        {
            Debug.Assert(!string.IsNullOrEmpty(fullPath) && Path.IsPathRooted(fullPath),
                "Must be full path");
            string str;
            try
            {
                if (IsRoot(fullPath))
                {
                    str = fullPath;
                }
                else
                {
                    DirectoryInfo directoryInfo = new(GetParentPath(fullPath));
                    if (File.Exists(fullPath))
                    {
                        Debug.Assert(directoryInfo.GetFiles(Path.GetFileName(fullPath)).Length == 1,
                            "Must found exactly 1");
                        str = directoryInfo.GetFiles(Path.GetFileName(fullPath))[0].FullName;
                    }
                    else if (Directory.Exists(fullPath))
                    {
                        Debug.Assert(directoryInfo.GetDirectories(Path.GetFileName(fullPath)).Length == 1,
                            "Must found exactly 1");
                        str = directoryInfo.GetDirectories(Path.GetFileName(fullPath))[0].FullName;
                    }
                    else
                        str = fullPath;
                }
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                int num1;
                switch (exception)
                {
                    case ArgumentException _:
                    case PathTooLongException _:
                    case NotSupportedException _:
                    case DirectoryNotFoundException _:
                    case SecurityException _:
                        num1 = 1;
                        break;
                    default:
                        num1 = exception is UnauthorizedAccessException ? 1 : 0;
                        break;
                }

                if (num1 != 0)
                {
                    int num2;
                    switch (exception)
                    {
                        case ArgumentException _:
                        case PathTooLongException _:
                            num2 = 0;
                            break;
                        default:
                            num2 = !(exception is NotSupportedException) ? 1 : 0;
                            break;
                    }

                    Debug.Assert(num2 != 0, "These exceptions should be caught above");
                    str = fullPath;
                }
                else
                    throw;
            }

            return str;
        }

        private static bool IsOnSameDrive(string path1, string path2)
        {
            path1 = path1.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            path2 = path2.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return string.Compare(Path.GetPathRoot(path1), Path.GetPathRoot(path2),
                StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static bool IsRoot(string path)
        {
            if (!Path.IsPathRooted(path))
                return false;
            path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return string.Compare(path, Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase) == 0;
        }

        private static string RemoveEndingSeparator(string path)
        {
            if (Path.IsPathRooted(path) && path.Equals(Path.GetPathRoot(path), StringComparison.OrdinalIgnoreCase))
                return path;
            return path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private static void ShellCopyOrMove(
            CopyOrMove operation,
            FileOrDirectory targetType,
            string fullSourcePath,
            string fullTargetPath,
            UiOptionInternal showUi,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation));
            Debug.Assert(Enum.IsDefined(typeof(FileOrDirectory), targetType));
            Debug.Assert(
                !string.IsNullOrEmpty(fullSourcePath) & Path.IsPathRooted(fullSourcePath),
                "Invalid FullSourcePath");
            Debug.Assert(
                !string.IsNullOrEmpty(fullTargetPath) & Path.IsPathRooted(fullTargetPath),
                "Invalid FullTargetPath");
            Debug.Assert(showUi != UiOptionInternal.NO_UI, "Why call ShellDelete if ShowUI is NoUI???");
            NativeMethods.SHFileOperationType operationType = operation != CopyOrMove.Copy
                ? NativeMethods.SHFileOperationType.FO_MOVE
                : NativeMethods.SHFileOperationType.FO_COPY;
            NativeMethods.ShFileOperationFlags operationFlags = GetOperationFlags(showUi);
            string fullSource = fullSourcePath;
            if (targetType == FileOrDirectory.Directory)
            {
                if (Directory.Exists(fullTargetPath))
                    fullSource = Path.Combine(fullSourcePath, "*");
                else
                    Directory.CreateDirectory(GetParentPath(fullTargetPath));
            }

            ShellFileOperation(operationType, operationFlags, fullSource, fullTargetPath, onUserCancel);
            if (!(operation == CopyOrMove.Move & targetType == FileOrDirectory.Directory) ||
                !Directory.Exists(fullSourcePath) || Directory.GetDirectories(fullSourcePath).Length != 0 ||
                Directory.GetFiles(fullSourcePath).Length != 0)
                return;
            Directory.Delete(fullSourcePath, false);
        }

        private static void ShellCopyOrMove(
            CopyOrMove operation,
            FileOrDirectory targetType,
            IReadOnlyCollection<string>? sourcePaths,
            string fullTargetPath,
            UiOptionInternal showUi,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(CopyOrMove), operation));
            Debug.Assert(Enum.IsDefined(typeof(FileOrDirectory), targetType));
            //Debug.Assert(
            //     Path.IsPathRooted(fullSourcePath),
            //    "Invalid FullSourcePath");
            Debug.Assert(
                !string.IsNullOrEmpty(fullTargetPath) && Path.IsPathRooted(fullTargetPath),
                "Invalid FullTargetPath");
            Debug.Assert(showUi != UiOptionInternal.NO_UI, "Why call ShellDelete if ShowUI is NoUI???");
            NativeMethods.SHFileOperationType operationType = operation != CopyOrMove.Copy
                ? NativeMethods.SHFileOperationType.FO_MOVE
                : NativeMethods.SHFileOperationType.FO_COPY;
            NativeMethods.ShFileOperationFlags operationFlags = GetOperationFlags(showUi);
            //string fullSource = fullSourcePath;
            //if (targetType == FileOrDirectory.Directory)
            //{
            //    if (Directory.Exists(fullTargetPath))
            //        fullSource = Path.Combine(fullSourcePath, "*");
            //    else
            //        Directory.CreateDirectory(GetParentPath(fullTargetPath));
            //}

            ShellFileOperation(operationType, operationFlags, sourcePaths, fullTargetPath, onUserCancel);
            //if (!(operation == CopyOrMove.Move & targetType == FileOrDirectory.Directory) ||
            //    !Directory.Exists(fullSourcePath) || Directory.GetDirectories(fullSourcePath).Length != 0 ||
            //    Directory.GetFiles(fullSourcePath).Length != 0)
            //    return;
            //Directory.Delete(fullSourcePath, false);
        }

        private static void ShellDelete(
            IReadOnlyCollection<string> files,
            UIOption showUi,
            RecycleOption recycle,
            UICancelOption onUserCancel)
        {
            var operationFlags = GetOperationFlags(ToUiOptionInternal(showUi));

            if (recycle == RecycleOption.SendToRecycleBin)
                operationFlags |= NativeMethods.ShFileOperationFlags.FOF_ALLOWUNDO;

            ShellFileOperation(NativeMethods.SHFileOperationType.FO_DELETE, operationFlags, files, null,
                onUserCancel);
        }

        private static void ShellDelete(
            string fullPath,
            UiOptionInternal showUi,
            RecycleOption recycle,
            UICancelOption onUserCancel,
            FileOrDirectory fileOrDirectory)
        {
            Debug.Assert(!string.IsNullOrEmpty(fullPath) && Path.IsPathRooted(fullPath),
                "FullPath must be a full path");
            Debug.Assert(showUi != UiOptionInternal.NO_UI, "Why call ShellDelete if ShowUI is NoUI???");
            NativeMethods.ShFileOperationFlags operationFlags = GetOperationFlags(showUi);
            if (recycle == RecycleOption.SendToRecycleBin)
                operationFlags |= NativeMethods.ShFileOperationFlags.FOF_ALLOWUNDO;
            ShellFileOperation(NativeMethods.SHFileOperationType.FO_DELETE, operationFlags, fullPath, null,
                onUserCancel);
        }

        private static void ShellFileOperation(
            NativeMethods.SHFileOperationType operationType,
            NativeMethods.ShFileOperationFlags operationFlags,
            string fullSource,
            string fullTarget,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(NativeMethods.SHFileOperationType), operationType));
            //Debug.Assert(operationType != NativeMethods.SHFileOperationType.FO_RENAME, "Don't call Shell to rename");
            Debug.Assert(!string.IsNullOrEmpty(fullSource) && Path.IsPathRooted(fullSource),
                "Invalid FullSource path");
            //Debug.Assert(
            //    operationType == NativeMethods.SHFileOperationType.FO_DELETE ||
            //    !string.IsNullOrEmpty(fullTarget) && Path.IsPathRooted(fullTarget),
            //    "Invalid FullTarget path");

            var shellOperationInfo = GetShellOperationInfo(
                operationType, operationFlags, fullSource, fullTarget);

            var errorCode = NativeMethods.SHFileOperation(ref shellOperationInfo);
            NativeMethods.SHChangeNotify(145439U, 3U, IntPtr.Zero, IntPtr.Zero);

            if (shellOperationInfo.fAnyOperationsAborted)
            {
                if (onUserCancel == UICancelOption.ThrowException)
                    throw new OperationCanceledException();
            }
            else if (errorCode != 0)
            {
                ThrowWinIoError(errorCode);
            }
        }

        private static void ShellFileOperation(
            NativeMethods.SHFileOperationType operationType,
            NativeMethods.ShFileOperationFlags operationFlags,
            IReadOnlyCollection<string>? sourcePaths,
            string? fullTarget,
            UICancelOption onUserCancel)
        {
            Debug.Assert(Enum.IsDefined(typeof(NativeMethods.SHFileOperationType), operationType));
            Debug.Assert(operationType != NativeMethods.SHFileOperationType.FO_RENAME, "Don't call Shell to rename");
            Debug.Assert(sourcePaths != null,
                "Invalid FullSource path");
            Debug.Assert(
                operationType == NativeMethods.SHFileOperationType.FO_DELETE ||
                !string.IsNullOrEmpty(fullTarget) && Path.IsPathRooted(fullTarget),
                "Invalid FullTarget path");

            var shellOperationInfo = GetShellOperationInfo(
                operationType, operationFlags, sourcePaths, fullTarget);

            var errorCode = NativeMethods.SHFileOperation(ref shellOperationInfo);
            NativeMethods.SHChangeNotify(145439U, 3U, IntPtr.Zero, IntPtr.Zero);

            if (shellOperationInfo.fAnyOperationsAborted)
            {
                if (onUserCancel == UICancelOption.ThrowException)
                    throw new OperationCanceledException();
            }
            else if (errorCode != 0)
            {
                ThrowWinIoError(errorCode);
            }
        }

        private static NativeMethods.SHFILEOPSTRUCT GetShellOperationInfo(
            NativeMethods.SHFileOperationType operationType,
            NativeMethods.ShFileOperationFlags operationFlags,
            string sourcePath,
            string? targetPath = null)
        {
            Debug.Assert(!string.IsNullOrEmpty(sourcePath) && Path.IsPathRooted(sourcePath),
                "Invalid SourcePath");
            return GetShellOperationInfo(operationType, operationFlags, new[]
            {
                sourcePath
            }, targetPath);
        }

        private static NativeMethods.SHFILEOPSTRUCT GetShellOperationInfo(
            NativeMethods.SHFileOperationType operationType,
            NativeMethods.ShFileOperationFlags operationFlags,
            IReadOnlyCollection<string>? sourcePaths,
            string? targetPath = null)
        {
            Debug.Assert(Enum.IsDefined(typeof(NativeMethods.SHFileOperationType), operationType),
                "Invalid OperationType");
            //Debug.Assert(Path.IsPathRooted(targetPath),
            //    "Invalid TargetPath");
            Debug.Assert(sourcePaths is { Count: > 0 }, "Invalid SourcePaths");

            NativeMethods.SHFILEOPSTRUCT shfileopstruct = new()
            {
                wFunc = (uint)operationType,
                fFlags = (uint)operationFlags,
                pFrom = GetShellPath(sourcePaths),
                pTo = targetPath != null ? GetShellPath(targetPath) : null,
                hNameMappings = IntPtr.Zero
            };

            try
            {
                shfileopstruct.hwnd = Process.GetCurrentProcess().MainWindowHandle;
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case SecurityException _:
                    case InvalidOperationException _:
                        shfileopstruct.hwnd = IntPtr.Zero;
                        break;
                    default:
                        throw;
                }
            }

            shfileopstruct.lpszProgressTitle = string.Empty;
            return shfileopstruct;
        }

        private static NativeMethods.ShFileOperationFlags GetOperationFlags(
            UiOptionInternal showUi, bool forRenameOp = false)
        {
            var fileOperationFlags = !forRenameOp
                ? NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMMKDIR |
                  NativeMethods.ShFileOperationFlags.FOF_NO_CONNECTED_ELEMENTS
                : NativeMethods.ShFileOperationFlags.FOFX_PRESERVEFILEEXTENSIONS;
            
            if (showUi == UiOptionInternal.OnlyErrorDialogs)
                fileOperationFlags |= NativeMethods.ShFileOperationFlags.FOF_SILENT |
                                      NativeMethods.ShFileOperationFlags.FOF_NOCONFIRMATION;

            return fileOperationFlags;
        }

        private static string GetShellPath(string fullPath)
        {
            Debug.Assert(!string.IsNullOrEmpty(fullPath) && Path.IsPathRooted(fullPath),
                "Must be full path");

            return GetShellPath(new[]
            {
                fullPath
            });
        }

        private static string GetShellPath(IReadOnlyCollection<string>? fullPaths)
        {
            Debug.Assert(fullPaths != null, "FullPaths is NULL");
            Debug.Assert(fullPaths.Count > 0, "FullPaths() is empty array");

            foreach (var fullPath in fullPaths)
                Debug.Assert(!string.IsNullOrEmpty(fullPath) && Path.IsPathRooted(fullPath), fullPath);

            StringBuilder multiString = new();

            const string nullChar = "\0";

            foreach (var fullPath in fullPaths)
                multiString.Append(fullPath + nullChar);

            multiString.Append("\0");

            var shellPath = multiString.ToString();

            Debug.Assert(shellPath.EndsWith(nullChar, StringComparison.Ordinal));


            return shellPath;
        }

        private static void ThrowIfDevicePath(string path)
        {
            if (path.StartsWith("\\\\.\\", StringComparison.Ordinal))
                throw ExceptionUtils.GetArgumentExceptionWithArgName(nameof(path),
                    "The given path is a Win32 device path. Don't use paths starting with '\\\\.\\'.");
        }

        private static void ThrowWinIoError(int errorCode)
        {
            switch (errorCode)
            {
                case 2:
                    throw new FileNotFoundException();
                case 3:
                    throw new DirectoryNotFoundException();
                case 5:
                    throw new UnauthorizedAccessException();
                case 15:
                    throw new DriveNotFoundException();
                case 206:
                    throw new PathTooLongException();
                case 995:
                case 1223:
                    throw new OperationCanceledException();
                default:
                    throw new IOException(new Win32Exception(errorCode).Message, Marshal.GetHRForLastWin32Error());
            }
        }

        private static UiOptionInternal ToUiOptionInternal(UIOption showUi) => showUi switch
        {
            UIOption.OnlyErrorDialogs => UiOptionInternal.OnlyErrorDialogs,
            UIOption.AllDialogs => UiOptionInternal.AllDialogs,
            _ => throw new InvalidEnumArgumentException(nameof(showUi), (int)showUi, typeof(UIOption))
        };

        private static void VerifyDeleteDirectoryOption(string argName, DeleteDirectoryOption argValue)
        {
            if (argValue != DeleteDirectoryOption.DeleteAllContents &&
                argValue != DeleteDirectoryOption.ThrowIfDirectoryNonEmpty)
                throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(DeleteDirectoryOption));
        }

        private static void VerifyRecycleOption(string argName, RecycleOption argValue)
        {
            if (argValue != RecycleOption.DeletePermanently && argValue != RecycleOption.SendToRecycleBin)
                throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(RecycleOption));
        }

        private static void VerifySearchOption(string argName, SearchOption argValue)
        {
            if (argValue != SearchOption.SearchAllSubDirectories && argValue != SearchOption.SearchTopLevelOnly)
                throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(SearchOption));
        }

        private static void VerifyUiCancelOption(string argName, UICancelOption argValue)
        {
            if (argValue != UICancelOption.DoNothing && argValue != UICancelOption.ThrowException)
                throw new InvalidEnumArgumentException(argName, (int)argValue, typeof(UICancelOption));
        }

        #endregion

        #region Structs

        private enum CopyOrMove
        {
            Copy,
            Move,
        }

        private enum FileOrDirectory
        {
            File,
            Directory,
        }

        private enum UiOptionInternal
        {
            OnlyErrorDialogs = 2,
            AllDialogs = 3,
            NO_UI = 4,
        }

        private class DirectoryNode
        {
            internal string Path { get; }

            internal string TargetPath { get; }

            internal ICollection<DirectoryNode> SubDirs { get; }

            internal DirectoryNode(string directoryPath, string targetDirectoryPath)
            {
                Debug.Assert(Directory.Exists(directoryPath), "Directory does not exist");
                Debug.Assert(
                    !string.IsNullOrEmpty(targetDirectoryPath) &&
                    System.IO.Path.IsPathRooted(targetDirectoryPath), "Invalid TargetPath");
                Path = directoryPath;
                TargetPath = targetDirectoryPath;
                SubDirs = new List<DirectoryNode>();

                foreach (var subDirPath in Directory.GetDirectories(Path))
                {
                    string subTargetDirPath = System.IO.Path.Combine(TargetPath,
                        System.IO.Path.GetFileName(subDirPath));
                    SubDirs.Add(new DirectoryNode(subDirPath, subTargetDirPath));
                }
            }
        }

        #endregion
    }
}