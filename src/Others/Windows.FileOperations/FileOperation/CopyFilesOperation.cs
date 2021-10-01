namespace Windows.FileOperations.FileOperation;

/// <summary>
/// Queued file copy based on IFileOperation.  Requires Windows Vista or newer.
/// </summary>
public class CopyFilesOperation : IDisposable
{
    private static readonly Type _fileOperationType = Type.GetTypeFromCLSID(new Guid(Guids.CLSID_FileOperation));
    
    private readonly IFileOperation _fileOperation;
    private readonly ShellItemFactory _shellItemFactory;

    /// <summary>
    /// Create a new FileOperation object that will perform all queued operations when disposed.
    /// </summary>
    public CopyFilesOperation() : this(null)
    {
    }

    /// <summary>
    /// Create a new FileOperation object that will perform all queued operations when disposed.
    /// </summary>
    /// <param name="owner">Owner of the file operations.</param>
    public CopyFilesOperation(IWin32Window owner)
    {
        _fileOperation = (IFileOperation)Activator.CreateInstance(_fileOperationType);
        _fileOperation.SetOperationFlags(FileOperationFlags.FOF_NOCONFIRMMKDIR);
        if (owner != null)
            _fileOperation.SetOwnerWindow((uint)owner.Handle);
        _shellItemFactory = new ShellItemFactory();
    }

    /// <summary>
    /// Add a file copy operation to the queue.
    /// </summary>
    /// <param name="source">File to copy from (must exist).</param>
    /// <param name="dest">File to copy to (will be created or overwritten).</param>
    public void Queue(FileInfo source, FileInfo dest)
        => Queue(source.FullName, dest.Directory.FullName, dest.Name);

    /// <summary>
    /// Add a file copy operation to the queue.
    /// </summary>
    /// <param name="source">File to copy from (must exist).</param>
    /// <param name="destPath">Full path without filename to copy to (must exist).</param>
    /// <param name="destFilename">Filename without path to copy to (will be created or overwritten).</param>
    public void Queue(FileInfo source, string destPath, string destFilename)
        => Queue(source.FullName, destPath, destFilename);

    /// <summary>
    /// Add a file copy operation to the queue.
    /// </summary>
    /// <param name="source">Full path and filename to copy from (must exist).</param>
    /// <param name="destPath">Full path without filename to copy to (must exist).</param>
    /// <param name="destFilename">Filename without path to copy to (will be created or overwritten).</param>
    public void Queue(string source, string destPath, string destFilename)
    {
        using ComDisposer<IShellItem> sourceItem = _shellItemFactory.Create(source);
        using ComDisposer<IShellItem> destinationItem = _shellItemFactory.Create(destPath);
        _fileOperation.CopyItem(sourceItem.Value, destinationItem.Value,
            destFilename, null);
    }

    /// <summary>
    /// Start the file copy operation and release all resources.
    /// </summary>
    public void Dispose()
    {
        try
        {
            _fileOperation.PerformOperations();
        }
        catch
        {
        } // if something goes wrong the IFileOperation UI will tell the user so we don't have to

        Marshal.FinalReleaseComObject(_fileOperation);
        GC.SuppressFinalize(this);
    }
}