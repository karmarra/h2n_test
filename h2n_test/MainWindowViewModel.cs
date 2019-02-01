using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
        private bool _prepared;
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

        public ICommand AddNumberCommand { get; }
        public ICommand DeleteNumberCommand { get; }
        public ICommand CECommand { get; }
        public ICommand ClearCommand { get; }
        public ICommand AddPointCommand { get; }
        public ICommand UnaryOperCommand { get; }
        public ICommand PrepareBinaryOperCommand { get; }
        public ICommand BinaryOperCommand { get; }
        public ICommand PercentOperCommand { get; }


        public MainWindowViewModel()
        {
            AddNumberCommand = new RelayCommand(param => RunMethod(() => AddNumber(param.ToString())));
            DeleteNumberCommand = new RelayCommand(param => RunMethod(DeleteNumber));
            CECommand = new RelayCommand(param => RunMethod(CE));
            ClearCommand = new RelayCommand(param => RunMethod(Clear));
            AddPointCommand = new RelayCommand(param => RunMethod(AddPoint));
            UnaryOperCommand = new RelayCommand(param => RunMethod(() => UnaryOper(param.ToString())));
            PrepareBinaryOperCommand = new RelayCommand(param => RunMethod(() => PrepareBinaryOper(param.ToString())));
            BinaryOperCommand = new RelayCommand(param => RunMethod(BinaryOper));
            PercentOperCommand = new RelayCommand(param => RunMethod(PreparePercentOper));
        }

        #region methods

        private string Format(double value)
        {
            return value.ToString("G17");
        }

        private void AddNumber(string number)
        {
            if (_needClear) Field = "";
            if (Field.Length >= 16)
                return;
            Field += number;
            _needClear = false;
        }

        private void AddPoint()
        {
            if (_needClear)
            {
                Field = "0,";
                _needClear = false;
                return;
            }
            if (Field.Length >= 16 || Field.Contains(','))
                return;
            Field += ',';
        }

        private void DeleteNumber()
        {
            if (!Field.Any())
                return;
            if (Field.Length == 1)
            {
                CE();
                return;
            }
            Field = Field.Remove(Field.Length - 1).Trim();
        }

        private void CE()
        {
            Field = "0";
            _repeat = false;
            _needClear = true;
            _prepared = false;
        }

        private void Clear()
        {
            CE();
            _left = _right = 0;
            _operation = Operations.None;
        }

        private void UnaryOper(string operType)
        {
            PrepareOper(operType);
            _left = Calculator.CalcOperation(_operation, _left);
            Field = Format(_left);
            _needClear = true;
            _prepared = false;
        }

        private void PrepareOper(string operType)
        {
            if (_prepared) BinaryOper();
            if (!Double.TryParse(Field.Trim(), out _left))
                throw new Exception("Ошибка парсинга!");
            _operation = (Operations)Enum.Parse(typeof(Operations), operType);
            _needClear = true;
        }

        private void PrepareBinaryOper(string operType)
        {
            PrepareOper(operType);
            _repeat = false;
            _prepared = true;
        }

        private void BinaryOper()
        {
            if (_operation == Operations.None) return;
            if (!_repeat) Double.TryParse(Field, out _right);
            _left = Calculator.CalcOperation(_operation, _left, _right);
            Field = Format(_left);
            _repeat = true;
            _needClear = true;
            _prepared = false;
        }

        private void PreparePercentOper()
        {
            Double.TryParse(Field, out _right);
            _right = Calculator.CalcOperation(Operations.Percent, _left, _right);
            Field = Format(_right);
            _needClear = true;
        }

        private void RunMethod(Action action)
        {
            RunMethod(() => { action(); return 0; });
        }

        private T RunMethod<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ee)
            {
                Field = ee.Message;
                _needClear = true;
            }
            return default(T);
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
