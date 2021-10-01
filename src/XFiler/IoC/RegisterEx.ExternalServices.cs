using Autofac;
using System.IO;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterExternalServices(this ContainerBuilder services)
    {
        services.RegisterLogger();
    }

    private static void RegisterLogger(this ContainerBuilder services)
    {
        services.Register(c =>
        {
            var storage = c.Resolve<IStorage>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(storage.LogDirectory, "app_logger.txt"),
                    rollingInterval: RollingInterval.Month)
                .CreateLogger();
            return Log.Logger;
        }).As<ILogger>().SingleInstance();
    }
}