using Autofac;

namespace XFiler;

internal class AutofacDiService : IDIService
{
    private readonly ContainerBuilder _services;

    public AutofacDiService()
    {
        _services = new ContainerBuilder();
    }


    public IContainer Build()
    {
        var host = _services.Build();

        return host;
    }

    public void RegisterSingleton<TImplementer, TTypedService>()
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        _services.RegisterType<TImplementer>().As<TTypedService>().SingleInstance();
    }

    public void RegisterInitializeSingleton<TImplementer, TTypedService>()
        where TImplementer : TTypedService
        where TTypedService : IInitializeService
    {
        _services.RegisterType<TImplementer>().As<TTypedService>().As<IInitializeService>().SingleInstance();
    }

    public void RegisterSingleton<TTypedService>(Func<IDIContext, TTypedService> @delegate)
        where TTypedService : notnull
    {   
        _services.Register(c =>
        {
            var context = new AutofacDiContext(c);

            return @delegate.Invoke(context);
        }).As<TTypedService>().SingleInstance();
    }

    public void Register<TImplementer, TTypedService>()
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        _services.RegisterType<TImplementer>().As<TTypedService>().ExternallyOwned();
    }

    public void Register<TImplementer, TTypedService>(object serviceKey)
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        _services.RegisterType<TImplementer>().Keyed<TTypedService>(serviceKey).ExternallyOwned();
    }
}
    
internal class AutofacDiContext : IDIContext
{
    private readonly IComponentContext _componentContext;

    public AutofacDiContext(IComponentContext componentContext)
    {
        _componentContext = componentContext;
    }

    public T Resolve<T>() where T : notnull
    {
        return _componentContext.Resolve<T>();
    }
}