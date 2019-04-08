using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GameOfLife
{
    class TimeConverter : IValueConverter
    {
        private long min;
        public long _Min
        {
            get { return min; }
            set { min = value; }
        }
        private long max;
        public long _Max
        {
            get { return max; }
            set { max = value; }
        }

        public double ConvertBack(double b)
        {
            b = b*(_Max-_Min) +_Min;
            return b;
        }
        public double Convert(double b)
        {
            b -= _Min;
            b /= (_Max - _Min);
            return b;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            double a = (double)value;
            return Convert(a);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double a = (double)value;
            return ConvertBack(a);
        }
    }
}
