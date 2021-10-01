namespace Windows.FileOperations.FileOperation;

[ComImport]
[Guid(Guids.IFileOperationProgressSink)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface IFileOperationProgressSink
{
    void StartOperations();
    void FinishOperations(uint hrResult);

    void PreRenameItem(uint dwFlags, IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
    void PostRenameItem(uint dwFlags, IShellItem psiItem, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrRename, IShellItem psiNewlyCreated);

    void PreMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
    void PostMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrMove, IShellItem psiNewlyCreated);

    void PreCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
    void PostCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, uint hrCopy, IShellItem psiNewlyCreated);

    void PreDeleteItem(uint dwFlags, IShellItem psiItem);
    void PostDeleteItem(uint dwFlags, IShellItem psiItem, uint hrDelete, IShellItem psiNewlyCreated);

    void PreNewItem(uint dwFlags, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName);
    void PostNewItem(uint dwFlags, IShellItem psiDestinationFolder, [MarshalAs(UnmanagedType.LPWStr)] string pszNewName, [MarshalAs(UnmanagedType.LPWStr)] string pszTemplateName, uint dwFileAttributes, uint hrNew, IShellItem psiNewItem);

    void UpdateProgress(uint iWorkTotal, uint iWorkSoFar);

    void ResetTimer();
    void PauseTimer();
    void ResumeTimer();
}

internal class FileOperationProgressSink : IFileOperationProgressSink
{
    public void StartOperations()
    {
    }

    public void FinishOperations(uint hrResult)
    {
    }

    public void PreRenameItem(uint dwFlags, IShellItem psiItem, string pszNewName)
    {
    }

    public void PostRenameItem(uint dwFlags, IShellItem psiItem, string pszNewName, uint hrRename, IShellItem psiNewlyCreated)
    {
    }

    public void PreMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName)
    {
    }

    public void PostMoveItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName, uint hrMove,
        IShellItem psiNewlyCreated)
    {
    }

    public void PreCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName)
    {
    }

    public void PostCopyItem(uint dwFlags, IShellItem psiItem, IShellItem psiDestinationFolder, string pszNewName, uint hrCopy,
        IShellItem psiNewlyCreated)
    {
    }

    public void PreDeleteItem(uint dwFlags, IShellItem psiItem)
    {
    }

    public void PostDeleteItem(uint dwFlags, IShellItem psiItem, uint hrDelete, IShellItem psiNewlyCreated)
    {
    }

    public void PreNewItem(uint dwFlags, IShellItem psiDestinationFolder, string pszNewName)
    {
    }

    public void PostNewItem(uint dwFlags, IShellItem psiDestinationFolder, string pszNewName, string pszTemplateName,
        uint dwFileAttributes, uint hrNew, IShellItem psiNewItem)
    {
    }

    public void UpdateProgress(uint iWorkTotal, uint iWorkSoFar)
    {
    }

    public void ResetTimer()
    {
    }

    public void PauseTimer()
    {
    }

    public void ResumeTimer()
    {
    }
}