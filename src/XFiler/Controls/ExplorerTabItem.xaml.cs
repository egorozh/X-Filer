using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace XFiler
{
    public partial class ExplorerTabItem
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