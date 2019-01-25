using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using h2n_test.Common;

namespace h2n_test
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private double _field = 0;
        public double Field
        {
            get => _field;
            set
            {
                if (_field == value)
                    return;
                _field = value;
                OnPropertyChanged(nameof(Field));
            }
        }


        #region methods

        private void Action()
        {

        }

        RelayCommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(param => Action()));
            }
        }

        #endregion methods

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
       
        #endregion INotifyPropertyChanged
    }
}
