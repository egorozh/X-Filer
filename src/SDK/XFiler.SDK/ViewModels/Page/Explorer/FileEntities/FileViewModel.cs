using System;
using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo FileInfo { get; }

        public double Size => FileInfo.Length / 1024.0;

        public FileViewModel(FileInfo info, IIconLoader iconLoader) : base(new XFilerRoute(info), iconLoader, info)
        {
            FileInfo = info;
        }

        public override DateTime ChangeDateTime => Info.LastWriteTime;

        public override string? GetRootName()
            => Info.GetRootName().FullName;
    }
}