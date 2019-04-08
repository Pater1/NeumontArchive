using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItemControls
{
    public class Item: DependencyObject
    {
        private static DependencyProperty name = 
                DependencyProperty.Register("name", typeof(string), typeof(Item));
        public string Name
        {
            get { return (string)GetValue(name); }
            set { if (value != null) SetValue(name, value); }
        }

        private static DependencyProperty effect =
                DependencyProperty.Register("effect", typeof(string), typeof(Item));
        public string Effect
        {
            get { return (string)GetValue(effect); }
            set { if (value != null) SetValue(effect, value); }
        }

        private static DependencyProperty cost =
                DependencyProperty.Register("cost", typeof(int), typeof(Item));
        public int Cost{
            get { return (int)GetValue(cost); }
            set { SetValue(cost, value); }
        }

        private static DependencyProperty description =
                DependencyProperty.Register("description", typeof(string), typeof(Item));
        public string Description
        {
            get { return (string)GetValue(description); }
            set { if (value != null) SetValue(description, value); }
        }

        private static DependencyProperty equiped =
                DependencyProperty.Register("equiped", typeof(bool), typeof(Item));
        public bool Equiped {
            get { return (bool)GetValue(equiped); }
            set { SetValue(equiped, value); }
        }

        public Item()
        {
            Name = "";
            Effect = "";
            Cost = 0;
            Description = "";
            Equiped = false;
        }
        public Item(string _name, string _effect, int cost, string _description, bool equiped) : this()
        {
            Name = _name;
            Effect = _effect;
            Cost = cost;
            Description = _description;
            Equiped = equiped;
        }
    }
}
