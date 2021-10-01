using Autofac;

namespace XFiler;

internal static partial class RegisterEx
{
    public static void RegisterGroups(this ContainerBuilder services)
    {
        services.RegisterType<FilesGroupOfNone>().As<IFilesGroup>();
        services.RegisterType<FilesGroupOfName>().As<IFilesGroup>();
        services.RegisterType<FilesGroupOfType>().As<IFilesGroup>();
    }
}