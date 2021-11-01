using System.IO;
using System.Windows.Controls;
using Windows.ImageOperations;
using Windows.Storage.FileProperties;

namespace XFiler;

public class AddNewContextMenuModel : IAddNewContextMenuModel
{
    public string Extension { get; }
    public string Name { get; }
    public byte[]? Data { get; }
    public string? Template { get; }
    public Image? Icon { get; }

    public AddNewContextMenuModel(string extension, string displayType, byte[]? data,
        string? template, StorageItemThumbnail? thumbnail)
    {
        Extension = extension;
        Name = displayType;
        Data = data;
        Template = template;

        if (thumbnail != null)
        {
            Icon = new Image
            {
                Source = ImageSystem.FromStream(thumbnail.AsStream()),
            };
        }
    }
}