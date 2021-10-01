namespace XFiler.Controls;

/// <summary>
/// Implement an interface that enables a viewmodel to interact
/// with the <seealso cref="EditBox"/> control.
/// </summary>
public interface IEditBoxModel
{
    /// <summary>
    /// The viewmodel can fire this event to request editing of its item
    /// name in order to start the rename process via the <seealso cref="EditBox"/> control.
    /// 
    /// The control will fire the command that is bound to the Command dependency
    /// property (if any) with the new name as parameter (if editing was not cancelled
    /// (through escape) in the meantime.
    /// </summary>
    event EventHandler RequestEdit;
}