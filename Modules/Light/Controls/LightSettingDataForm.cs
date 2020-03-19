using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Controls
{
    public partial class LightSettingDataForm : SfDataForm
    {
        public LightSettingDataForm()
        {
            RegisterEditor("Light1", "Segment");
            RegisterEditor("Light2", "Segment");
            RegisterEditor("Light3", "Segment");
            RegisterEditor("Light4", "Segment");
            ContainerType = ContainerType.Filled;
            LayoutOptions = LayoutType.Default;
            LabelWidth = 1;
            EditorWidth = 3;

        }
    }
}
