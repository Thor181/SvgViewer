using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SvgViewer.Command
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public Action<object> Action { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (Action == null)
                throw new NotImplementedException("Action is not implemented");

            Action?.Invoke(parameter);
        }
    }
}
