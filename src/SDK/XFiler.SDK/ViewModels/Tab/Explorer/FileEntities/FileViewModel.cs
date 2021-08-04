using System;
using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo Info { get; }

        public double Size => Info.Length / 1024.0;

        public FileViewModel(FileInfo fileInfo, IIconLoader iconLoader) : base(fileInfo.Name, fileInfo.FullName, iconLoader)
        {
            Info = fileInfo;
        }

        public override DateTime ChangeDateTime => Info.LastWriteTime;

        public override string? GetRootName()
            => new FileInfo(FullName).Directory?.Root.Name;

        public override FileEntityViewModel Clone()
            => new FileViewModel(new FileInfo(Info.FullName), IconLoader);
    }
}