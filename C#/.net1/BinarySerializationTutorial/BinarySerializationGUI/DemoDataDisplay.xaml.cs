using BinarySerializationTutorial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BinarySerializationGUI
{
    /// <summary>
    /// Interaction logic for DemoDataDisplay.xaml
    /// </summary>
    public partial class DemoDataDisplay : UserControl
    {
        public static readonly DependencyProperty data =
            DependencyProperty.Register("data", typeof(DemoData), typeof(DemoDataDisplay));
        public DemoData Data {
            get
            {
                return (DemoData)GetValue(data);
            }
            set
            {
                if (value != null)
                {
                    SetValue(data, value);
                    BindToData();
                }
            }
        }
        private bool twoWay = false;
        public bool TwoWay {
            get
            {
                return twoWay;
            }
            set
            {
                twoWay = value;
                BindToData();
            }
        }

        public DemoDataDisplay()
        {
            InitializeComponent();
            Data = new DemoData();
        }

        private void BindToData()
        {
            IDLabel.DataContext = Data;
            Binding idBind = new Binding("ID");
            idBind.Source = Data;
            idBind.Mode = TwoWay ? BindingMode.TwoWay : BindingMode.OneWay;
            BindingOperations.SetBinding(IDLabel, TextBox.TextProperty, idBind);

            DataLabel.DataContext = Data;
            Binding dataBind = new Binding("Data");
            dataBind.Source = Data;
            dataBind.Mode = TwoWay ? BindingMode.TwoWay : BindingMode.OneWay;
            BindingOperations.SetBinding(DataLabel, TextBox.TextProperty, dataBind);
        }
    }
}
