using System;
using System.IO;

namespace XFiler.SDK
{
    public class DirectoryViewModel : FileEntityViewModel
    {
        public DirectoryInfo DirectoryInfo { get; }

        public DirectoryViewModel(DirectoryInfo directoryName, IIconLoader iconLoader)
            : base(directoryName.Name, directoryName.FullName, iconLoader)
        {
            DirectoryInfo = directoryName;
        }

        public override DateTime ChangeDateTime => DirectoryInfo.LastWriteTime;

        public override string GetRootName()
            => new DirectoryInfo(FullName).Root.Name;

        public override FileEntityViewModel Clone()
            => new DirectoryViewModel(new DirectoryInfo(DirectoryInfo.FullName), IconLoader);
    }
}