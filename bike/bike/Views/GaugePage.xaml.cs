using Infrastructure;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace bike.Views
{
    public partial class GaugePage : ContentPage
    {
        public GaugePage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            ((Element)sender).GetParent<Grid>().GetChildren<Picker>().FirstOrDefault()?.Focus();

        }
        private void Picker_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ((IVisualElementController)sender).InvalidateMeasure(InvalidationTrigger.MeasureChanged);
        }
    }

}
