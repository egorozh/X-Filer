namespace Windows.FileOperations;

///  <summary>
///  Specify which UI dialogs to show.
///  </summary>
///  <remarks>
///  To fix common issues; avoid Integer values that VB Compiler
///  will convert Boolean to (0 and -1).
///  </remarks>
public enum UIOption
{
    OnlyErrorDialogs = 2,
    AllDialogs = 3,
}