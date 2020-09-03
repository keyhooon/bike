using System;
using Xamarin.Forms;


namespace bike.Views
{
    public partial class ErrorLogPage 
    {
        public ErrorLogPage()
        {
            this.InitializeComponent();
            listView.ItemGenerator = new Infrastructure.ItemGeneratorExt(listView);
        }
    }
}