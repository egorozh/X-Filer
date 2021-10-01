using Autofac;

namespace XFiler.SDK.Plugins;

public interface IPlugin
{
    void Load(ContainerBuilder services);
}