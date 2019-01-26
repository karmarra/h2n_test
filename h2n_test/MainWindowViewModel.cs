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
        private double _left;
        private double _right;
        private bool _repeat;
        private bool _needClear = true;
        private Operations _operation;

        private string _field = "0";
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


        #region methods

        private string Format(double value)
        {
            return $"{value:#### ### ### ##0.####}";
        }

        private void AddNumber(string number)
        {
            if(_needClear) Field = "";
            if (Field.Length >= 21)
                return;

            Field += number;
            _needClear = false;
        }

        private void AddPoint()
        {
            if (_needClear)
            {
                Field = "0,";
                return;
            }
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

        private void CE()
        {
            Field = "0";
        }

        private void Clear()
        {
            Field = "0";
            _left = _right = 0;
            _operation = Operations.None;
            _repeat = false;
        }



        private void UnaryOper(string operType)
        {
            try
            {
                PrepareOper(operType);
                _left = Calculator.CalcOperation(_operation, _left);
                Field = Format(_left);
            }
            catch (Exception ee)
            {
                Field = ee.Message;
            }
            finally
            {
                _needClear = true;
            }
        }

        private void PrepareOper(string operType)
        {
                if(!Double.TryParse(Field, out _left))
                    throw new Exception("Ошибка парсинга!");
                _operation = (Operations)Enum.Parse(typeof(Operations), operType);
                _needClear = true;
                _repeat = false;
        }

        private void BinaryOper()
        {
            try
            {
                if(_operation == Operations.None) return;
                if(!_repeat) Double.TryParse(Field, out _right);
                _left = Calculator.CalcOperation(_operation, _left, _right);
                Field = Format(_left);
                _repeat = true;
            }
            catch (Exception ee)
            {
                Field = ee.Message;
            }
            finally
            {
                _needClear = true;
            }
        }

        private void PreparePercentOper()
        {
            try
            {
                Double.TryParse(Field, out _right);
                _right = Calculator.CalcOperation(Operations.Percent, _left, _right);
                Field = Format(_right);
            }
            catch (Exception ee)
            {
                Field = ee.Message;
            }
            finally
            {
                _needClear = true;
            }
        }


        private RelayCommand _addNumberCommand;
        public ICommand AddNumberCommand
        {
            get
            {
                return _addNumberCommand ?? (_addNumberCommand = new RelayCommand(param => AddNumber(param.ToString())));
            }
        }

        private RelayCommand _addPointCommand;
        public ICommand AddPointCommand
        {
            get
            {
                return _addPointCommand ?? (_addPointCommand = new RelayCommand(param =>AddPoint()));
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

        private RelayCommand _prepareBinaryOperCommand;
        public ICommand PrepareBinaryOperCommand
        {
            get
            {
                return _prepareBinaryOperCommand ?? (_prepareBinaryOperCommand = new RelayCommand(param => PrepareOper(param.ToString())));
            }
        }

        private RelayCommand _binaryOperCommand;
        public ICommand BinaryOperCommand
        {
            get
            {
                return _binaryOperCommand ?? (_binaryOperCommand = new RelayCommand(param => BinaryOper()));
            }
        }

        private RelayCommand _percentOperCommand;
        public ICommand PercentOperCommand
        {
            get
            {
                return _percentOperCommand ?? (_percentOperCommand = new RelayCommand(param => PreparePercentOper()));
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
