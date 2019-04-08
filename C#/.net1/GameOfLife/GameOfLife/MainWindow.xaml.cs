using ShapeUp;
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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            xSlider.callOnUpdate += (() => gameGrid._XDim = (int)xSlider.Value);
            ySlider.callOnUpdate += (() => gameGrid._YDim = (int)ySlider.Value);

            TimeConverter b2b = new TimeConverter()
            {
                _Min = 0,
                _Max = 5000
            };
            Binding bind = new Binding("TickDelay")
            {
                Converter = b2b,
                Mode = BindingMode.TwoWay
            };
            DelaySlider.DataContext = gameGrid;
            BindingOperations.SetBinding(DelaySlider.Slider, Slider.ValueProperty, bind);
        }
        
        private void Randomize(object sender, RoutedEventArgs e)
        {
            gameGrid.ToggleAllCells();
        }

        private void Step(object sender, RoutedEventArgs e)
        {
            gameGrid.UpdateAsync().GetAwaiter().GetResult();
        }

        private void Rebuild(object sender, RoutedEventArgs e)
        {
            gameGrid.RebuildGrid();
        }

        private void Run(object sender, RoutedEventArgs e)
        {
            gameGrid.ToggleAutoRun();
        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gameGrid.Shutdown();
        }
    }
}
