using System;
using System.IO;

namespace XFiler.SDK
{
    public class DirectoryViewModel : FileEntityViewModel
    {
        public DirectoryInfo DirectoryInfo { get; }

        public DirectoryViewModel(XFilerRoute route, IIconLoader iconLoader)
            : base(route, iconLoader)
        {
            DirectoryInfo = new DirectoryInfo(route.FullName);
        }

        public override DateTime ChangeDateTime => DirectoryInfo.LastWriteTime;

        public override string GetRootName()
            => new DirectoryInfo(FullName).Root.Name;

        public override FileEntityViewModel Clone()
            => new DirectoryViewModel(Route, IconLoader);
    }
}