using System;
using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        private readonly FileInfo _fileInfo;

        public double Size => _fileInfo.Length / 1024.0;

        public FileViewModel(FileInfo fileInfo, IIconLoader iconLoader) : base(fileInfo.Name, fileInfo.FullName, iconLoader)
        {
            _fileInfo = fileInfo;
        }

        public override DateTime ChangeDateTime => _fileInfo.LastWriteTime;

        public override string? GetRootName()
            => new FileInfo(FullName).Directory?.Root.Name;

        public override FileEntityViewModel Clone()
            => new FileViewModel(new FileInfo(_fileInfo.FullName), IconLoader);
    }
}