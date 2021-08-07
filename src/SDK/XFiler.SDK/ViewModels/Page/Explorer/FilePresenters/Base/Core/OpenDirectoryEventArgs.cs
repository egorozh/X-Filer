using System;

namespace XFiler.SDK
{
    public class OpenDirectoryEventArgs : EventArgs
    {
        public FileEntityViewModel FileEntityViewModel { get; }

        public OpenDirectoryEventArgs(FileEntityViewModel fileEntityViewModel)
        {
            FileEntityViewModel = fileEntityViewModel;
        }
    }
}