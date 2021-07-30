using System.IO;

namespace XFiler.SDK
{
    public interface IIconPathProvider
    {
        FileInfo GetIconPath(FileEntityViewModel viewModel);
    }   
}