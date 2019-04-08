using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ItemControls
{
    class StringToImagePathConverter : IValueConverter
    {
        private string fileExtention = "png";
        public string FileExtention
        {
            get { return fileExtention; }
            set { if (value != null) fileExtention = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Replace(" ", "") + '.' + FileExtention;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
