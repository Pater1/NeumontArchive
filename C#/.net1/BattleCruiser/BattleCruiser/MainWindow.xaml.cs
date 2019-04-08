using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;

namespace BattleCruiser {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public static MainWindow Instance { get; private set; }
        public MainWindow() {
            InitializeComponent();
            currentGame = new Game();
            currentGame.InitializeGame();
            stack.Children.Add(currentGame);
            Instance = this;
        }
        Game currentGame;

        public void NewGame(object sender, RoutedEventArgs e) {
            stack.Children.Remove(currentGame);
            currentGame = new Game();
            currentGame.InitializeGame();
            stack.Children.Add(currentGame);
        }

        private string lastSavePath;
        private void SaveGame(object sender, RoutedEventArgs e) {
            if(lastSavePath == null) {
                SaveGameAs(sender, e);
            } else {
                using (FileStream fs = new FileStream(lastSavePath, FileMode.Create)) {
                    XmlSerializer xml = new XmlSerializer(typeof(Game));
                    xml.Serialize(fs, currentGame);

                    //currentGame.WriteXml(XmlWriter.Create(fs));
                }
            }
        }
        private void SaveGameAs(object sender, RoutedEventArgs e) {
            using (SaveFileDialog save = new SaveFileDialog()) {
                save.FileName = "game.btlcrzr";
                save.Filter = "Battle Cruiser save files (*.btlcrzr)|*.btlcrzr|All files (*.*)|*.*";
                
                if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    lastSavePath = save.FileName;
                    SaveGame(sender, e);
                }
            }
        }

        public void OpenGame(object sender, RoutedEventArgs e) {
            using (OpenFileDialog open = new OpenFileDialog()) {
                open.FileName = "game.btlcrzr";
                open.Filter = "Battle Cruiser save files (*.btlcrzr)|*.btlcrzr|All files (*.*)|*.*";

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    lastSavePath = open.FileName;
                    using (FileStream fs = new FileStream(lastSavePath, FileMode.Open)) {
                        XmlSerializer xml = new XmlSerializer(typeof(Game));
                        stack.Children.Remove(currentGame);
                        currentGame = (Game)xml.Deserialize(fs);
                        stack.Children.Add(currentGame);
                    }
                }
            }
        }

        private void ToggleCheat(object sender, RoutedEventArgs e) {
            currentGame.InCheatMode = !currentGame.InCheatMode;
        }
    }
}
