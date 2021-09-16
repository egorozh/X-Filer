using Autofac.Features.Indexed;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using XFiler.SDK;

namespace XFiler.Base
{
    public class GridFilesPresenterFactory : BaseFilesPresenterFactory
    {
        private IIndex<string, IFilesPresenter> _presenterFactory;

        public GridFilesPresenterFactory(IIndex<string, IFilesPresenter> presenterFactory)
            : base("Таблица", CreateTemplate())
        {
            _presenterFactory = presenterFactory;
        }

        public override IFilesPresenter CreatePresenter(DirectoryInfo currentDirectory, IFilesGroup group)
        {
            var presenter = _presenterFactory["grid"];
            presenter.Init(currentDirectory, group);
            return presenter;
        }

        public override void Dispose()
        {
            base.Dispose();

            _presenterFactory = null!;
        }

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