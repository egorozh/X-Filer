using Autofac.Features.Indexed;
using System.IO;

namespace XFiler;

public sealed class FileEntityFactory : IFileEntityFactory
{
    private readonly IIndex<EntityType, FileEntityViewModel> _factory;

    public FileEntityFactory(IIndex<EntityType, FileEntityViewModel> factory)
    {
        _factory = factory;
    }

    public async Task<IFileSystemModel> CreateDirectory(DirectoryInfo directoryInfo, IFilesGroup filesGroup,
        IconSize iconSize)
    {
        var model = _factory[EntityType.Directory];
        await model.Init(new DirectoryRoute(directoryInfo), directoryInfo, filesGroup, iconSize);

        return model;
    }

    public async Task<IFileSystemModel> CreateFile(FileInfo fileInfo, IFilesGroup filesGroup, IconSize iconSize)
    {
        var model = _factory[EntityType.File];
        await model.Init(new FileRoute(fileInfo), fileInfo, filesGroup, iconSize);

        return model;
    }
}