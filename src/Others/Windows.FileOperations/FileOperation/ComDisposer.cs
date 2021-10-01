namespace Windows.FileOperations.FileOperation;

/// <summary>
/// IDisposable wrapper for COM objects.
/// </summary>
/// <typeparam name="T">Type of COM object.</typeparam>
internal class ComDisposer<T> : IDisposable where T : class
{
    /// <summary>
    /// Initialize a COM disposer for the specified object.
    /// </summary>
    /// <param name="obj">COM object that needs to be released.</param>
    internal ComDisposer(T obj)
    {
        if (obj != null && !obj.GetType().IsCOMObject)
            throw new ArgumentOutOfRangeException(nameof(obj), "Object must be a COM object.");
        Value = obj;
    }

    /// <summary>
    /// The wrapped COM object.
    /// </summary>
    internal T Value { get; private set; }

    /// <summary>
    /// Release the COM object if it hasn't already been released.
    /// </summary>
    public void Dispose()
    {
        if (Value != null)
        {
            Marshal.FinalReleaseComObject(Value);
            Value = null;
        }
    }
}