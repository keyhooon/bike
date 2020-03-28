using DataModels;
using Syncfusion.XForms.DataForm;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Controls
{
    public class PedalConfigurationDataForm : SfDataForm
    {
        public PedalConfigurationDataForm()
        {
            RegisterEditor("MagnetCount", "NumericUpDown");
            AutoGeneratingDataFormItem += PedalConfigurationDataForm_AutoGeneratingDataFormItem;
            ContainerType = ContainerType.Filled;
            LayoutOptions = LayoutType.TextInputLayout;
        }

        private void PedalConfigurationDataForm_AutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
        {
            if (e.DataFormItem.Name == "MagnetCount")
            {
                RangeAttribute attribute = (RangeAttribute) (typeof(PedalConfiguration).GetProperty("MagnetCount").GetCustomAttributes(typeof(RangeAttribute), false).First());
                (e.DataFormItem as DataFormNumericUpDownItem).Maximum = (int) attribute.Maximum;
                (e.DataFormItem as DataFormNumericUpDownItem).Minimum = (int)attribute.Minimum;
                (e.DataFormItem as DataFormNumericUpDownItem).AutoReverse = true;
                (e.DataFormItem as DataFormNumericUpDownItem).StepValue = 2;
            }
        }
    }
}
