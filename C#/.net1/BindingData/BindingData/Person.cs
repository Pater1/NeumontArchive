using BindingData.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

namespace BindingData
{
    class Person : INotifyPropertyChanged
    {
        #region Constructors and Factories
        public static Person BuildRandomPerson()
        {
            Person person = new Person();
            Person.RandomizeValues(person);
            
            return person;
        }
        public static void RandomizeValues(Person person)
        {
            person._NameFirst = GetFirstName();
            person._NameLast = GetLastName();
            person._Age = rand.Next(5, 121);
            person._Gender = (Gender)rand.Next(2);
        }
        #region Factory Assistant Sub-Fuctions & Cache
        private static Random rand = new Random();
        private static string firstNameDataPath = "Data/CSV_Database_of_First_Names.csv";
        private static string GetFirstName(int? index = null)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] strings = assembly.GetManifestResourceNames();
            foreach (string s in strings)
            {
                //MessageBox.Show(s);
            }

            using (Stream stream = new FileStream(firstNameDataPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                string[] results = result.Split(Environment.NewLine.ToArray());
                return results[index != null ? (int)index : rand.Next(1, results.Length)];
            }
        }
        private static string lastNameDataPath = "Data/CSV_Database_of_Last_Names.csv";
        private static string GetLastName(int? index = null)
        {
            using (Stream stream = new FileStream(lastNameDataPath, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                string[] results = result.Split(Environment.NewLine.ToArray());
                return results[index != null ? (int)index : rand.Next(results.Length)];
            }
        }
        #endregion
        private Person() { }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void UpdateProperty([CallerMemberName] string propName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        #region Object Overrides
        public override string ToString()
        {
            return $"{_NameFirst} {_NameLast}: {_Age}, {_Gender}";
        }
        #endregion

        #region Fields and Properties
        private string nameFirst;
        public string _NameFirst
        {
            get { return nameFirst; }
            set {
                if (value != null)
                {
                    nameFirst = value;
                    UpdateProperty();
                }
            }
        }

        private string nameLast;
        public string _NameLast
        {
            get { return nameLast; }
            set {
                if (value != null){
                    nameLast = value;
                    UpdateProperty();
                }
            }
        }

        private int age;
        public int _Age
        {
            get { return age; }
            set {
                if (value >= 5 && value <= 120)
                {
                    age = value;
                    UpdateProperty();
                }
            }
        }

        public enum Gender
        {
            male,
            female
        }
        private Gender gender;
        public Gender _Gender
        {
            get { return gender; }
            set {
                gender = value;
                UpdateProperty();
            }
        }
        #endregion
    }
}
