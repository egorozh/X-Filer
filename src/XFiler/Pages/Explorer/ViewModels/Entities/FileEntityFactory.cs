using System.IO;
using Autofac.Features.Indexed;
using XFiler.SDK;

namespace XFiler
{
    public class FileEntityFactory : IFileEntityFactory
    {
        private readonly IIndex<EntityType, FileEntityViewModel> _factory;

        public FileEntityFactory(IIndex<EntityType, FileEntityViewModel> factory)
        {
            _factory = factory;
        }

        public FileEntityViewModel CreateDirectory(DirectoryInfo directoryInfo, string? @group = null)
        {
            var model = _factory[EntityType.Directory];
            model.Init(new XFilerRoute(directoryInfo), directoryInfo);
            model.Group = @group;
            return model;
        }

        public FileEntityViewModel CreateFile(FileInfo fileInfo, string? @group = null)
        {
            var model = _factory[EntityType.File];
            model.Init(new XFilerRoute(fileInfo), fileInfo);
            model.Group = @group;
            return model;
        }
    }

    public enum EntityType
    {
        File,
        Directory
    }
}