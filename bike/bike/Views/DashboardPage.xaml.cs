using Syncfusion.SfPicker.XForms;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Infrastructure;
using System.Linq;

namespace bike.Views.Main
{
    public partial class DashboardPage 
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ((IVisualElementController)sender).InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }

        private void ClickGestureRecognizer_Clicked(object sender, System.EventArgs e)
        {
            ((Element)sender).GetParent<Grid>().GetChildren<Picker>().FirstOrDefault()?.Focus();
        }
    }
}
