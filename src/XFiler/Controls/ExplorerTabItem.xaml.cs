using System;
using System.Windows.Input;
using XFiler.SDK;

namespace XFiler
{
    public partial class ExplorerTabItem : IExplorerTabItem
    {
        public ExplorerTabItem()
        {
            InitializeComponent();

            //var dpd = DependencyPropertyDescriptor.FromProperty(ContentProperty, typeof(ContentControl));

            //dpd?.AddValueChanged(ContentControl, OnContentControlChanged);
        }

        private void OnContentControlChanged(object? sender, EventArgs e)
        {
            Keyboard.Focus(this);
        }
    }
}