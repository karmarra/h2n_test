using System;
using System.Windows.Input;

namespace h2n_test.Common
{
    public class RelayCommand : ICommand
    {
        #region Fields 

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        #endregion

        
        #region Ctors

        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        #endregion

        
        #region ICommand 

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter) { _execute(parameter); }

        #endregion ICommand
    }
}
