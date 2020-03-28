using Syncfusion.SfNavigationDrawer.XForms;
using System;

namespace bike.Commands
{
    public class ToggleDrawerCommand : System.Windows.Input.ICommand
    {

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)=> parameter is SfNavigationDrawer;


        public void Execute(object parameter) => ((SfNavigationDrawer)parameter).ToggleDrawer();
    }
}
