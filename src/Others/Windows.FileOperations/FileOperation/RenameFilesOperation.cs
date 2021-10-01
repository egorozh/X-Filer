namespace Windows.FileOperations.FileOperation;

public class RenameFilesOperation : IDisposable
{
    private static readonly Type _fileOperationType = Type.GetTypeFromCLSID(new Guid(Guids.CLSID_FileOperation));
        
    private readonly IFileOperation _fileOperation;
    private readonly ShellItemFactory _shellItemFactory;

    public RenameFilesOperation() : this(CreateHandle())
    {
    }

    public RenameFilesOperation(IntPtr handle)
    {
        _fileOperation = (IFileOperation)Activator.CreateInstance(_fileOperationType);
        
        _fileOperation.SetOperationFlags(FileOperationFlags.FOFX_SHOWELEVATIONPROMPT);
        _fileOperation.SetOwnerWindow((uint)handle);

        _shellItemFactory = new ShellItemFactory();
    }

    public void Rename(string source, string newName)
    {
        using ComDisposer<IShellItem> sourceItem = _shellItemFactory.Create(source);
           
        _fileOperation.RenameItem(sourceItem.Value, newName+"\0", null);
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

    private static IntPtr CreateHandle()
    {
        try
        {
            return Process.GetCurrentProcess().MainWindowHandle;
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case SecurityException _:
                case InvalidOperationException _:
                    return IntPtr.Zero;
                default:
                    throw;
            }
        }
    }
}