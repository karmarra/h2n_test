using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace h2n_test.Common
{
    public class SizeToVisibilityConverter : IMultiValueConverter
    {
        #region Fields 

        public double SizeLimit { get; set; }

        #endregion


        #region Ctors

        public SizeToVisibilityConverter()
        {
            SizeLimit = 560000;
        }

        #endregion


        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Count() == 2)
            {
                if (!Double.TryParse(values[0].ToString(), out var w) ||
                   !Double.TryParse(values[1].ToString(), out var h))
                    throw new Exception("Некорректные параметры SizeToVisibilityConverter!");
                Boolean.TryParse(parameter?.ToString(), out var i);
                if (w * h < SizeLimit)
                    return i ? Visibility.Visible : Visibility.Collapsed;
                return i ? Visibility.Collapsed : Visibility.Visible;
            }
            else
                throw new Exception("Некорректные параметры SizeToVisibilityConverter!");
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}