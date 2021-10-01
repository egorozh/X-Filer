namespace Windows.FileOperations;

/// <summary>
/// Specify the action to do when deleting a directory and it is not empty.
/// </summary>
/// <remarks>
/// Again, avoid Integer values that VB Compiler will convert Boolean to (0 and -1).
/// IMPORTANT: Change VerifyDeleteDirectoryOption if this enum is changed.
/// Also, values in DeleteDirectoryOption must be different from UIOption.
/// </remarks>
public enum DeleteDirectoryOption
{
    ThrowIfDirectoryNonEmpty = 4,
    DeleteAllContents = 5,
}