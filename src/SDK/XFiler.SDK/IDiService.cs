using System;

namespace XFiler.SDK;

public interface IDIService
{
    void RegisterSingleton<TImplementer, TTypedService>()
        where TImplementer : TTypedService
        where TTypedService : notnull;
        
    void RegisterSingleton<TTypedService>(Func<IDIContext, TTypedService> @delegate)
        where TTypedService : notnull;

    void Register<TImplementer, TTypedService>()
        where TImplementer : TTypedService
        where TTypedService : notnull;

    void Register<TImplementer, TTypedService>(object serviceKey)
        where TImplementer : TTypedService
        where TTypedService : notnull;
}

public interface IDIContext
{
    T Resolve<T>() where T : notnull; 
}