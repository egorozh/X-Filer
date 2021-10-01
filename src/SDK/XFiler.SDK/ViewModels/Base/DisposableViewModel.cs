using System;

namespace XFiler.SDK;

public class DisposableViewModel : BaseViewModel, IDisposable
{
    protected bool Disposed;

    ~DisposableViewModel()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        Disposed = true;
    }
}