using System.IO;
using System.Reflection;
using XFiler.Base;
using XFiler.SDK.Plugins;

namespace XFiler;

internal static class PluginsLoadExtensions
{   
    public static IEnumerable<IPlugin> GetPlugins(this IStorage storage)
    {
        var plugins = new List<IPlugin>
        {
            new BasePlugin()
        };

        //var pluginsPaths = Directory.EnumerateFiles(storage.PluginsDirectory, "XFiler.Plus.dll");

        //var other = pluginsPaths.SelectMany(pluginPath =>
        //{
        //    var pluginAssembly = LoadPlugin(pluginPath);
        //    return CreatePlugins(pluginAssembly);
        //});

        //plugins.AddRange(other);

        return plugins;
    }

    private static IEnumerable<IPlugin> CreatePlugins(Assembly assembly)
    {
        return (from type in assembly.GetTypes()
                where typeof(IPlugin).IsAssignableFrom(type)
                select Activator.CreateInstance(type))
            .OfType<IPlugin>();
    }

    private static Assembly LoadPlugin(string pluginPath)
    {
        var loadContext = new PluginLoadContext(pluginPath);
        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginPath)));
    }
}