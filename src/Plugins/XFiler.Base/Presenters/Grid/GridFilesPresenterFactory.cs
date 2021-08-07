using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using GongSolutions.Wpf.DragDrop;
using XFiler.SDK;

namespace XFiler.Base
{
    public class GridFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private readonly IFileEntityFactory _fileEntityFactory;
        private readonly IDropTarget _dropTarget;
        private readonly IDragSource _dragSource;
        private readonly IWindowFactory _windowFactory;

        public GridFilesPresenterFactory(IFileEntityFactory fileEntityFactory,
            IDropTarget dropTarget,
            IDragSource dragSource,
            IWindowFactory windowFactory) : base("Таблица", CreateTemplate())
        {
            _fileEntityFactory = fileEntityFactory;
            _dropTarget = dropTarget;
            _dragSource = dragSource;
            _windowFactory = windowFactory;
        }

        public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory)
            => new GridFilesPresenterViewModel(currentDirectory, _fileEntityFactory,
                _dropTarget, _dragSource, _windowFactory);

        private static DataTemplate CreateTemplate()
        {
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(
                @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                                  xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" 
                                  xmlns:base=""clr-namespace:XFiler.Base;assembly=XFiler.Base""
                                  DataType=""{ x:Type base:GridFilesPresenterViewModel}"">
                        <base:GridFilesPresenter />
                    </DataTemplate>"));

            return (DataTemplate)XamlReader.Load(ms);
        }
    }
}