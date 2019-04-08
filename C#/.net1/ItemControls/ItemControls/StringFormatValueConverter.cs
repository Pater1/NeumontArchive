using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ItemControls
{
    class StringFormatValueConverter : IValueConverter
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { if (value != null) text = value; }
        }

        private string[] properties;
        public string[] Properties
        {
            get { return properties; }
            set { if (value != null) properties = value; }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            Type t = value.GetType();
            string[] propVals = new string[Properties.Length];

            for(int i = 0; i < Properties.Length; i++)
            {
                string prop = Properties[i];
                PropertyInfo p = t.GetProperty(prop);
                var v = p.GetValue(value);
                propVals[i] = v.ToString();
            }

            return string.Format(text, propVals);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
