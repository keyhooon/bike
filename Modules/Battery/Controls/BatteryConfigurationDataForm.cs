using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.Controls
{
    public class BatteryConfigurationDataForm : SfDataForm
    {
        public BatteryConfigurationDataForm()
        {
            ContainerType = ContainerType.Filled;
            LayoutOptions = LayoutType.TextInputLayout;
        }
    }
}
