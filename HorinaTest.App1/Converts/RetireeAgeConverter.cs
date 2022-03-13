using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HorinaTest.App1.Converts
{
    [ValueConversion(typeof(int), typeof(String))]
    public class RetireeAgeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int year = (int)value;
            if (year <= 0)
                return $"{Math.Abs(year)} г. на пенсии";
            return $"{year} г. до пенсии";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
