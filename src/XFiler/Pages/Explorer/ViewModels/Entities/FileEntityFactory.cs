﻿using Autofac.Features.Indexed;
using System.IO;

namespace XFiler
{
    public class FileEntityFactory : IFileEntityFactory
    {
        private readonly IIndex<EntityType, FileEntityViewModel> _factory;

        public FileEntityFactory(IIndex<EntityType, FileEntityViewModel> factory)
        {
            _factory = factory;
        }

        public IFileSystemModel CreateDirectory(DirectoryInfo directoryInfo, IFilesGroup filesGroup, IconSize iconSize)
        {
            var model = _factory[EntityType.Directory];
            model.Init(new XFilerRoute(directoryInfo), directoryInfo, filesGroup, iconSize);
            
            return model;
        }

        public IFileSystemModel CreateFile(FileInfo fileInfo, IFilesGroup filesGroup, IconSize iconSize)
        {
            var model = _factory[EntityType.File];
            model.Init(new XFilerRoute(fileInfo), fileInfo, filesGroup,iconSize);
          
            return model;
        }
    }
}