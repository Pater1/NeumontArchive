using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ItemControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<Character> chars = new ObservableCollection<Character>()
        {
            new Character("Itsa Hammar", 8, CharacterClass.Fighter, Gender.Female, 15, 7, 12, 512){
                Inventory = new ObservableCollection<Item>()
                {
                    new Item("A Hammer",
                            "2d20+5 damage",
                            68616,
                            "Turns solid things into squishy things.",
                            true)
                }
            },
            new Character("Lance Polk", 18, CharacterClass.Fighter, Gender.Male, 18, 4, 12, 1024),
            new Character("Phyre Bendre", 4, CharacterClass.Mage, Gender.Female, 8, 16, 10, 2048),
            new Character("Eyr Geisehr", 12, CharacterClass.Mage, Gender.Male, 4, 18, 12, 4096),
            new Character("Seano Allarms", 6, CharacterClass.Ranger, Gender.Female, 12, 4, 18, 8192),
            new Character("Boe Strehng", 2, CharacterClass.Ranger, Gender.Male, 12, 4, 18, 16384),
        };
        public MainWindow()
        {
            InitializeComponent();
            characterSelect.ItemsSource = chars;
        }
    }
}
