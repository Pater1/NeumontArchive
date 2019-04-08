using GameOfLife;
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

namespace ShapeUp
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class LabeledSlider : UserControl
    {
        public delegate void OnUpdate();
        public event OnUpdate callOnUpdate;

        public double Maximum
        {
            get
            {
                return (double)Slider.Maximum;
            }
            set
            {
                Slider.Maximum = (double)value;
            }
        }
        public double Minimum
        {
            get
            {
                return (double)Slider.Minimum;
            }
            set
            {
                Slider.Minimum = (double)value;
            }
        }


        public double Value
        {
            get
            {
                return Slider.Value;
            }
        }

        public string TextLabel
        {
            get { return (string)TypeLabel.Content; }
            set { TypeLabel.Content = value; }
        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider.Value = Slider.Value;
            if (callOnUpdate != null) callOnUpdate();
        }

        public LabeledSlider()
        {
            InitializeComponent();


            LabelConverter b2b = new LabelConverter();
            Binding bind = new Binding("Value")
            {
                Converter = b2b,
                Mode = BindingMode.TwoWay
            };
            ValueLabel.DataContext = Slider;
            BindingOperations.SetBinding(ValueLabel, Label.ContentProperty, bind);
        }
    }
}
