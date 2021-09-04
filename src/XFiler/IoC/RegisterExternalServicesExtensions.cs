using System.IO;
using Autofac;
using Serilog;
using XFiler.SDK;

namespace XFiler
{
    internal static class RegisterExternalServicesExtensions
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
}