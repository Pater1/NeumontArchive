using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ColorBuilder.xaml
    /// </summary>
    public partial class ColorBuilder : UserControl
    {
        public delegate void UpdateColor(Color c);
        public event UpdateColor onColorUpdate;
        public ColorBuilder()
        {
            InitializeComponent();

            RVal.callOnUpdate += OnChangeUpdate;
            GVal.callOnUpdate += OnChangeUpdate;
            BVal.callOnUpdate += OnChangeUpdate;
            AVal.callOnUpdate += OnChangeUpdate;
        }

        public void ForceUpdate(UpdateColor on)
        {
            Color c = BuildColor();
            on(c);
        }

        private void OnChangeUpdate()
        {
            Color c = BuildColor();
            Preveiw.Background = new SolidColorBrush(c);
            if(onColorUpdate != null) onColorUpdate(c);
        }

        public Color BuildColor()
        {
            Color c = new Color();

            c.R = (byte)RVal.Value;
            c.G = (byte)GVal.Value;
            c.B = (byte)BVal.Value;
            c.A = (byte)AVal.Value;

            return c;
        }
    }
}
