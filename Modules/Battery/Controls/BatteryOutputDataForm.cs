using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.Controls
{
    public class BatteryOutputDataForm : SfDataForm
    {
        public BatteryOutputDataForm()
        {
            ContainerType = ContainerType.Filled;
            LayoutOptions = LayoutType.TextInputLayout;
        }
    }
}
