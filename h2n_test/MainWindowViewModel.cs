using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using h2n_test.Common;
using Microsoft.SqlServer.Server;

namespace h2n_test
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string _field = "";
        public string Field
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

        private double _left;
        private double _right;
        private BinaryOperations _operation;


        #region methods

        private string Format(double value)
        {
            return $"{value:0 0.####}";
        }

        private void AddNumber(string number)
        {
            if (Field.Length >= 21 || Field.Contains(','))
                return;

            Field += number;
        }

        private void AddPoint()
        {
            if (Field.Length >= 21 || Field.Contains(','))
                return;
            Field += ',';
        }


        private void DeleteNumber()
        {
            if (!Field.Any())
                return;
            Field.Remove(Field.Length - 1);
        }

        private void UnaryOper(string operType)
        {
            try
            {
                _left = Calculator.CalcUnaryOperation(0, (UnaryOperations)Enum.Parse(typeof(UnaryOperations), operType));
                Field = Format(_left);
            }
            catch (Exception ee)
            {
                Field = ee.Message;
            }

        }

        private void PrepareBinaryOper(string operType)
        {
            try
            {
                Double.TryParse(Field, out _left);
                _operation = (BinaryOperations)Enum.Parse(typeof(BinaryOperations), operType);
                Field = "0";

                //_left = Calculator.CalcUnaryOperation(0, (UnaryOperations)Enum.Parse(typeof(UnaryOperations), operType));
                //Field = Format(_left);
            }
            catch (Exception ee)
            {
                Field = ee.Message;
            }

        }

        private void CalcBinaryOper()
        {
            _left = Calculator.CalcBinaryOperation(0, _right, _operation);
        }

        private RelayCommand _addNumberCommand;
        public ICommand AddNumberCommand
        {
            get
            {
                return _addNumberCommand ?? (_addNumberCommand = new RelayCommand(param => AddNumber(param.ToString())));
            }
        }

        private RelayCommand _unaryOperCommand;
        public ICommand UnaryOperCommand
        {
            get
            {
                return _unaryOperCommand ?? (_unaryOperCommand = new RelayCommand(param => UnaryOper(param.ToString())));
            }
        }

        private RelayCommand _binaryOperCommand;
        public ICommand BinaryOperCommand
        {
            get
            {
                return _binaryOperCommand ?? (_binaryOperCommand = new RelayCommand(param => UnaryOper(param.ToString())));
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
