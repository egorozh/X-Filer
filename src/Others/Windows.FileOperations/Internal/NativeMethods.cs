namespace Windows.FileOperations;

[ComVisible(false)]
internal static class NativeMethods
{
    [PreserveSig]
    [DllImport("kernel32")]
    public static extern int CloseHandle(IntPtr hObject);

    [DllImport("kernel32",
        CharSet = CharSet.Auto,
        PreserveSig = true,
        BestFitMapping = false,
        ThrowOnUnmappableChar = true)]
    public static extern int GetVolumeInformation(
        [MarshalAs(UnmanagedType.LPTStr)] string lpRootPathName,
        StringBuilder lpVolumeNameBuffer,
        int nVolumeNameSize,
        ref int lpVolumeSerialNumber,
        ref int lpMaximumComponentLength,
        ref int lpFileSystemFlags,
        IntPtr lpFileSystemNameBuffer,
        int nFileSystemNameSize);

    /// <summary>
    /// Given a 32-bit SHFILEOPSTRUCT, call the appropriate SHFileOperation function
    /// to perform shell file operation.
    /// </summary>
    /// <param name="lpFileOp">32-bit SHFILEOPSTRUCT</param>
    /// <returns>0 if successful, non-zero otherwise.</returns>
    public static int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp)
    {
        if (IntPtr.Size == 4)
            return SHFileOperation32(ref lpFileOp);

        SHFILEOPSTRUCT64 lpFileOp1 = new()
        {
            hwnd = lpFileOp.hwnd,
            wFunc = lpFileOp.wFunc,
            pFrom = lpFileOp.pFrom,
            pTo = lpFileOp.pTo,
            fFlags = lpFileOp.fFlags,
            fAnyOperationsAborted = lpFileOp.fAnyOperationsAborted,
            hNameMappings = lpFileOp.hNameMappings,
            lpszProgressTitle = lpFileOp.lpszProgressTitle
        };

        int num = SHFileOperation64(ref lpFileOp1);
        lpFileOp.fAnyOperationsAborted = lpFileOp1.fAnyOperationsAborted;
        return num;
    }

    /// <summary>
    /// Copies, moves, renames or deletes a file system object on 32-bit platforms.
    /// </summary>
    /// <param name="lpFileOp">Pointer to an SHFILEOPSTRUCT structure that contains information this function needs
    ///       to carry out the specified operation. This parameter must contain a valid value that is not NULL.
    ///       You are responsible for validating the value. If you do not, you will experience unexpected result.</param>
    /// <returns>Returns zero if successful, non zero otherwise.</returns>
    /// <remarks>
    /// You should use fully-qualified path names with this function. Using it with relative path names is not thread safe.
    /// You cannot use SHFileOperation to move special folders My Documents and My Pictures from a local drive to a remote computer.
    /// File deletion is recursive unless you set the FOF_NORECURSION flag.
    /// </remarks>
    [DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation",
        SetLastError = true, ThrowOnUnmappableChar = true)]
    private static extern int SHFileOperation32(ref SHFILEOPSTRUCT lpFileOp);

    /// <summary>
    /// Copies, moves, renames or deletes a file system object on 64-bit platforms.
    /// </summary>
    [DllImport("shell32.dll", CharSet = CharSet.Auto, EntryPoint = "SHFileOperation",
        SetLastError = true, ThrowOnUnmappableChar = true)]
    private static extern int SHFileOperation64(ref SHFILEOPSTRUCT64 lpFileOp);

    /// <summary>
    /// Notifies the system of an event that an application has performed.
    /// An application should use this function if it performs an action that may affect the shell.
    /// </summary>
    /// <param name="wEventId">Describes the event that has occurred. Typically, only one event is specified at a time.
    ///       If more than one event is specified, the values contained in dwItem1 and dwItem2 must be the same,
    ///       respectively, for all specified events. See ShellChangeNotificationEvents.</param>
    /// <param name="uFlags">Flags that indicate the meaning of the dwItem1 and dwItem2 parameter. See ShellChangeNotificationFlags.</param>
    /// <param name="dwItem1">First event-dependent value.</param>
    /// <param name="dwItem2">Second event-dependent value.</param>
    /// <remarks>
    /// Win 95/98/Me: SHChangeNotify is supported by Microsoft Layer for Unicode.
    /// To use this http://msdn.microsoft.com/library/default.asp?url=/library/en-us/mslu/winprog/microsoft_layer_for_unicode_on_windows_95_98_me_systems.asp
    /// </remarks>
    [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern void SHChangeNotify(
        uint wEventId,
        uint uFlags,
        IntPtr dwItem1,
        IntPtr dwItem2);

    /// <summary>
    /// The MoveFileEx function moves an existing file or directory.
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/fileio/fs/movefileex.asp
    /// </summary>
    [DllImport("kernel32", PreserveSig = true,
        CharSet = CharSet.Auto, EntryPoint = "MoveFileEx",
        BestFitMapping = false, ThrowOnUnmappableChar = true,
        SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool MoveFileEx(
        string lpExistingFileName,
        string lpNewFileName,
        int dwFlags);

    /// <summary>
    /// Contains information that the SHFileOperation function uses to perform file operations on 32-bit platforms.
    /// </summary>
    /// <remarks>
    /// * For detail documentation: http://msdn.microsoft.com/library/default.asp?url=/library/en-us/shellcc/platform/shell/reference/structures/shfileopstruct.asp.
    /// Members:
    ///   hwnd: Window handle to the dialog box to display information about the status of the operation.
    ///   wFunc: Value indicates which operation (copy, move, rename, delete) to perform.
    ///   pFrom: Buffer for 1 or more source file names. Each name ends with a NULL separator + additional NULL at the end.
    ///   pTo: Buffer for destination name(s). Same rule as pFrom.
    ///   fFlags: Flags that control details of the operation.
    ///   fAnyOperationsAborted: Out param. TRUE if user aborted any file operations. Otherwise, FALSE.
    ///   hNameMappings: Handle to name mapping object containing old and new names of renamed files (not used).
    ///   lpszProgressTitle: Address of a string to use as title of progress dialog box. (not used).
    /// typedef struct _SHFILEOPSTRUCT {
    ///    HWND hwnd;
    ///    UINT wFunc;
    ///    LPCTSTR pFrom;
    ///    LPCTSTR pTo;
    ///    FILEOP_FLAGS fFlags; (WORD)
    ///    BOOL fAnyOperationsAborted;
    ///    LPVOID hNameMappings;
    ///    LPCTSTR lpszProgressTitle;
    /// } SHFILEOPSTRUCT, *LPSHFILEOPSTRUCT;
    ///   If no steps are taken, the last 3 variables will not be passed correctly. Hence the Pack:=1.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Auto)]
    public struct SHFILEOPSTRUCT
    {
        public IntPtr hwnd;
        public uint wFunc;
        [MarshalAs(UnmanagedType.LPTStr)] public string pFrom;
        [MarshalAs(UnmanagedType.LPTStr)] public string? pTo;
        public uint fFlags;
        public bool fAnyOperationsAborted;
        public IntPtr hNameMappings;
        [MarshalAs(UnmanagedType.LPTStr)] public string lpszProgressTitle;
    }

    /// <summary>
    /// Contains information that the SHFileOperation function uses to perform file operations
    /// on 64-bit platforms, where the structure is unpacked.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct SHFILEOPSTRUCT64
    {
        internal IntPtr hwnd;
        internal uint wFunc;
        [MarshalAs(UnmanagedType.LPTStr)] internal string pFrom;
        [MarshalAs(UnmanagedType.LPTStr)] internal string pTo;
        internal uint fFlags;
        internal bool fAnyOperationsAborted;
        internal IntPtr hNameMappings;
        [MarshalAs(UnmanagedType.LPTStr)] internal string lpszProgressTitle;
    }

    /// <summary>
    /// Values that indicate which file operation to perform. Used in SHFILEOPSTRUCT
    /// </summary>
    public enum SHFileOperationType : uint
    {
        FO_MOVE = 1,
        FO_COPY = 2,
        FO_DELETE = 3,
        FO_RENAME = 4,
    }

    /// <summary>
    /// Flags that control the file operation. Used in SHFILEOPSTRUCT.
    /// </summary>
    [Flags]
    public enum ShFileOperationFlags : uint
    {
        Default = 0,

        //The pTo member specifies multiple destination files (one for each source file)
        //rather than one directory where all source files are to be deposited.
        FOF_MULTIDESTFILES = 1,

        //Not currently used.
        FOF_CONFIRMMOUSE = 2,

        //Do not display a progress dialog box.
        FOF_SILENT = 4,

        //Give the file being operated on a new name in a move, copy, or rename operation
        //if a file with the target name already exists.
        FOF_RENAMEONCOLLISION = 8,

        // Respond with "Yes to All" for any dialog box that is displayed.
        FOF_NOCONFIRMATION = 16, // 0x0010

        // If FOF_RENAMEONCOLLISION is specified and any files were renamed,
        // assign a name mapping object containing their old and new names to the hNameMappings member.
        FOF_WANTMAPPINGHANDLE = 32, // 0x0020

        // Preserve Undo information, if possible. Undone can only be done from the same process.
        // If pFrom does not contain fully qualified path and file names, this flag is ignored.
        // NOTE: Not setting this flag will let the file be deleted permanently, unlike the doc says.
        FOF_ALLOWUNDO = 64, // 0x0040

        // Perform the operation on files only if a wildcard file name (*.*) is specified.
        FOF_FILESONLY = 128, // 0x0080

        // Display a progress dialog box but do not show the file names.
        FOF_SIMPLEPROGRESS = 256, // 0x0100

        // Do not confirm the creation of a new directory if the operation requires one to be created.
        FOF_NOCONFIRMMKDIR = 512, // 0x0200

        // Do not display a user interface if an error occurs.
        FOF_NOERRORUI = 1024, // 0x0400

        // Do not copy the security attributes of the file.
        FOF_NOCOPYSECURITYATTRIBS = 2048, // 0x0800

        // Only operate in the local directory. Don//t operate recursively into subdirectories.
        FOF_NORECURSION = 4096, // 0x1000

        // Do not move connected files as a group. Only move the specified files.
        FOF_NO_CONNECTED_ELEMENTS = 8192, // 0x2000

        // Send a warning if a file is being destroyed during a delete operation rather than recycled.
        // This flag partially overrides FOF_NOCONFIRMATION.
        FOF_WANTNUKEWARNING = 16384, // 0x4000

        // Treat reparse points as objects, not containers.
        FOF_NORECURSEREPARSE = 32768, // 0x8000,

        FOFX_NOSKIPJUNCTIONS = 0x00010000, // Don't avoid binding to junctions (like Task folder, Recycle-Bin)
        FOFX_PREFERHARDLINK = 0x00020000, // Create hard link if possible

        FOFX_SHOWELEVATIONPROMPT =
            0x00040000, // Show elevation prompts when error UI is disabled (use with FOF_NOERRORUI)

        FOFX_EARLYFAILURE =
            0x00100000, // Fail operation as soon as a single error occurs rather than trying to process other items (applies only when using FOF_NOERRORUI)

        FOFX_PRESERVEFILEEXTENSIONS =
            0x00200000, // Rename collisions preserve file extns (use with FOF_RENAMEONCOLLISION)
        FOFX_KEEPNEWERFILE = 0x00400000, // Keep newer file on naming conflicts
        FOFX_NOCOPYHOOKS = 0x00800000, // Don't use copy hooks
        FOFX_NOMINIMIZEBOX = 0x01000000, // Don't allow minimizing the progress dialog

        FOFX_MOVEACLSACROSSVOLUMES =
            0x02000000, // Copy security information when performing a cross-volume move operation
        FOFX_DONTDISPLAYSOURCEPATH = 0x04000000, // Don't display the path of source file in progress dialog
        FOFX_DONTDISPLAYDESTPATH = 0x08000000, // Don't display the path of destination file in progress dialog
    }

    /// <summary>
    /// Describes the event that has occurred. Used in SHChangeNotify.
    /// There are more values in shellapi.h. Only include the relevant ones.
    /// </summary>
    public enum SHChangeEventTypes : uint
    {
        //Specifies a combination of all of the disk event identifiers.
        SHCNE_DISKEVENTS = 145439, // 0x0002381F

        // All events have occurred.
        SHCNE_ALLEVENTS = 2147483647, // 0x7FFFFFFF
    }

    /// <summary>
    /// Indicates the meaning of dwItem1 and dwItem2 parameters in SHChangeNotify method.
    /// There are more values in shellapi.h. Only include the relevant one.
    /// </summary>
    public enum SHChangeEventParameterFlags : uint
    {
        // The dwItem1 and dwItem2 parameters are DWORD values.
        SHCNF_DWORD = 3,
    }

    [Flags]
    internal enum SIATTRIBFLAGS
    {
        SIATTRIBFLAGS_AND = 0x00000001,
        SIATTRIBFLAGS_OR = 0x00000002,
        SIATTRIBFLAGS_APPCOMPAT = 0x00000003,
        SIATTRIBFLAGS_MASK = 0x00000003,
        SIATTRIBFLAGS_ALLITEMS = 0x00004000
    }
}