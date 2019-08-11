using System;
using System.Windows.Input;

namespace AppCenterWPF
{
    public class SimpleCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        readonly Action<object> func;

        public SimpleCommand(Action<object> func)
        {
            this.func = func;
        }

        public bool CanExecute(object parameter)
            => true;

        public void Execute(object parameter)
        {
            func?.Invoke(parameter);
        }
    }
}
