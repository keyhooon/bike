using Prism.Commands;
using Syncfusion.SfNavigationDrawer.XForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace bike
{
    public static class NavigationDrawerCommand
    {
        public static DelegateCommand<SfNavigationDrawer> ToggleDrawerCommand = new DelegateCommand<SfNavigationDrawer>(o=>o.ToggleDrawer(),o=>o != null);


    }
}
