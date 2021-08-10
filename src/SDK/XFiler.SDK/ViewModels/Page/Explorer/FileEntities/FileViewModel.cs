using System;
using System.IO;

namespace XFiler.SDK
{
    public sealed class FileViewModel : FileEntityViewModel
    {
        public FileInfo Info { get; }

        public double Size => Info.Length / 1024.0;

        public FileViewModel(XFilerRoute route, IIconLoader iconLoader) : base(route, iconLoader)
        {
            Info = new FileInfo(route.FullName);
        }

        public override DateTime ChangeDateTime => Info.LastWriteTime;

        public override string? GetRootName()
            => new FileInfo(FullName).Directory?.Root.Name;

        public override FileEntityViewModel Clone()
            => new FileViewModel(Route, IconLoader);
    }
}