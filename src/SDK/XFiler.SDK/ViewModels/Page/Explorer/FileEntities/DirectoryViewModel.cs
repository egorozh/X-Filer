using System;
using System.IO;

namespace XFiler.SDK
{
    public class DirectoryViewModel : FileEntityViewModel
    {
        public DirectoryInfo DirectoryInfo { get; }

        public DirectoryViewModel(DirectoryInfo directoryInfo, IIconLoader iconLoader)
            : base(new XFilerRoute(directoryInfo), iconLoader, directoryInfo)
        {
            DirectoryInfo = directoryInfo;
        }

        public override DateTime ChangeDateTime => DirectoryInfo.LastWriteTime;

        public override string GetRootName() => Info.GetRootName().FullName;
    }
}