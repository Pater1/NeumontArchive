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
using BinarySerializationTutorial;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.IO;

namespace BinarySerializationGUI
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

        private string extention = "bi";
        private BinaryFormatter biFor = new BinaryFormatter();
        private void Save(object sender, RoutedEventArgs e)
        {
            using (SaveFileDialog save = new SaveFileDialog())
            {
                save.DefaultExt = extention;
                save.Filter =
                    $"Demo Binaries (*.{extention})|*.{extention}| All files (*.*)|*.*";
                save.AddExtension = true;
                save.FileName = "SavedDemoData.bi";

                DialogResult result = save.ShowDialog();
                //save canceled
                if (result != System.Windows.Forms.DialogResult.OK) return;

                //using (FileStream fs = new FileStream(save.FileName, FileMode.Create, FileAccess.Write))
                using (Stream fs = save.OpenFile())
                {
                    biFor.Serialize(fs, SaveDisply.Data);
                }
            }
        }

        private void Load(object sender, RoutedEventArgs e)
        {
            using (OpenFileDialog open = new OpenFileDialog())
            {
                open.DefaultExt = extention;
                open.Filter =
                    $"Demo Binaries (*.{extention})|*.{extention}| All files (*.*)|*.*";

                DialogResult result = open.ShowDialog();
                //load canceled
                if (result != System.Windows.Forms.DialogResult.OK) return;

                //using (FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read))
                using(Stream fs = open.OpenFile())
                {
                    LoadDisply.Data = (DemoData)biFor.Deserialize(fs);
                }
            }
        }
    }
}
