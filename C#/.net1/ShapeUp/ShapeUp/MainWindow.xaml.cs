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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Random rand = new Random();
        private byte EasyRand(int min, int max)
        {
           return (byte)rand.Next(min, max + 1);
        }
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Shapes.Shape rect;

            if (EasyRand(0, 100) % 2 == 0)
            {
                rect = new System.Windows.Shapes.Rectangle();
            }
            else
            {
                rect = new System.Windows.Shapes.Ellipse();
            }

            rect.Stroke = new SolidColorBrush(Colors.Black);
            rect.Fill = new SolidColorBrush(Color.FromArgb(255, EasyRand(0, 255), EasyRand(0, 255), EasyRand(0, 255)));
            rect.StrokeThickness = 1;

            Point p = Mouse.GetPosition(mainCanvas);
            int maxWidth = (int)(mainCanvas.ActualWidth / 4);
            try
            {
                rect.Width = EasyRand(10, (p.X < maxWidth ? (int)p.X : (mainCanvas.ActualWidth - p.X) < maxWidth ? (int)(mainCanvas.ActualWidth - p.X) : maxWidth));
            }
            catch
            {
                rect.Width = 10;
            }
            int maxHeight = (int)(mainCanvas.ActualHeight / 4);
            try
            {
                rect.Height = EasyRand(10, (p.Y < maxHeight ? (int)p.Y : (mainCanvas.ActualHeight - p.Y) < maxHeight ? (int)(mainCanvas.ActualHeight - p.Y) : maxHeight));
            }
            catch
            {
                rect.Height = 10;
            }
            Canvas.SetLeft(rect, p.X - rect.Width/2);
            Canvas.SetTop(rect, p.Y - rect.Height / 2);

            rect.MouseRightButtonDown += (MouseButtonEventHandler)((obj, dat) =>
            {
                mainCanvas.Children.Remove(obj as UIElement);
            });

            mainCanvas.Children.Add(rect);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainCanvas.Children.Clear();
        }

        private void UpdateCanvasColor(object sender, RoutedEventArgs e)
        {
            ColorBuild.ForceUpdate((c =>
                mainCanvas.Background = new SolidColorBrush(c))
            );
        }
    }
}
