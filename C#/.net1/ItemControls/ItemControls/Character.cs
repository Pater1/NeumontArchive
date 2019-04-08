using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItemControls
{
    public class Character: DependencyObject
    {
        private static DependencyProperty name = DependencyProperty.Register("name", typeof(string), typeof(Character));
        public string Name
        {
            get { return (string)GetValue(name); }
            set { if (value != null) SetValue(name, value); }
        }

        private static DependencyProperty level = DependencyProperty.Register("level", typeof(int), typeof(Character));
        public int Level {
            get
            {
                return (int)GetValue(level);
            }
            set
            {
                SetValue(level, value);
            }
        }

        public CharacterClass CharClass { get; set; }

        public Gender CharGender { get; set; }

        public int Strength { get; set; }
        public int Intelligence { get; set; }
        public int Dexterity { get; set; }
        public int Gold { get; set; }

        private ObservableCollection<Item> inventory = new ObservableCollection<Item>();

        public ObservableCollection<Item> Inventory
        {
            get { return inventory; }
            set { if (value != null) inventory = value; }
        }

        public Character(string name, int level, CharacterClass charClass, Gender charGender, int strength, int intelligence, int dexterity, int gold, ObservableCollection<Item> inventory = null)
        {
            Name = name;
            Level = level;
            CharClass = charClass;
            CharGender = charGender;
            Strength = strength;
            Intelligence = intelligence;
            Dexterity = dexterity;
            Gold = gold;
            Inventory = inventory;
        }
    }
}
