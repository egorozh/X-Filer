using Autofac;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs\\app_logger.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();

            services.RegisterInstance(Log.Logger).As<ILogger>().SingleInstance();
        }
    }
}