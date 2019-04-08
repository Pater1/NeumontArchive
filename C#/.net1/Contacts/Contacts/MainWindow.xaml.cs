using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
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
using forms = System.Windows.Forms;

namespace Contacts {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ContactsData contacts = new ContactsData();
        ContactsData _Contacts {
            get {
                if(contacts == null) {
                    contacts = new ContactsData();
                }
                return contacts;
            }
            set {
                if(value != null) {
                    contacts = value;
                } else if(contacts == null){
                    contacts = new ContactsData();
                }
            }
        }

        //not null means building new Contact.
        Contact activeContact = null;
        Contact _ActiveContact {
            get {
                if (activeContact != null) {
                    SaveContact.Visibility = Visibility.Visible;
                    NewContact.Visibility = Visibility.Collapsed;

                    return activeContact;
                } else {
                    SaveContact.Visibility = Visibility.Collapsed;
                    NewContact.Visibility = Visibility.Visible;

                    return (Contact)Contacts.SelectedItem;
                }
            }
            set {
                if (activeContact != null) {
                    activeContact = value;

                    SaveContact.Visibility = Visibility.Visible;
                    NewContact.Visibility = Visibility.Collapsed;
                } else {
                    if (value != null) {
                        _Contacts._Contacts[Contacts.SelectedIndex] = value;
                    }

                    SaveContact.Visibility = Visibility.Collapsed;
                    NewContact.Visibility = Visibility.Visible;
                }
            }
        }
        public MainWindow() {
            InitializeComponent();
            DataContext = contacts;
        }

        private void ChangedSelection(object sender, SelectionChangedEventArgs e) {
            Form.DataContext = _ActiveContact;
        }

        private void DeleteContact_Click(object sender, RoutedEventArgs e) {
            if (activeContact != null) {
                activeContact = null;
            } else {
                _Contacts._Contacts.RemoveAt(Contacts.SelectedIndex);
            }
            Form.DataContext = _ActiveContact;
        }

        private void SaveContact_Click(object sender, RoutedEventArgs e) {
            if (activeContact != null) {
                if (activeContact._IsValid) {
                    _Contacts._Contacts.Add(activeContact);
                    activeContact = null;
                    Form.DataContext = _ActiveContact;
                } else {
                    MessageBox.Show("Current contact is incomplete! Please make sure to include first and last names and at least one contact info (phone or email).");
                }
            } else {
                SaveContact.Visibility = Visibility.Collapsed;
            }
        }

        private void NewContact_Click(object sender, RoutedEventArgs e) {
            activeContact = new Contact();
            Form.DataContext = _ActiveContact;
        }

        private static readonly string extention = "contacts";
        private static BinaryFormatter biFor = new BinaryFormatter();
        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e) {
            using (forms.OpenFileDialog open = new forms.OpenFileDialog()) {
                open.DefaultExt = extention;
                open.Filter =
                    $"Demo Binaries (*.{extention})|*.{extention}| All files (*.*)|*.*";

                forms.DialogResult result = open.ShowDialog();
                //load canceled
                if (result != System.Windows.Forms.DialogResult.OK) return;
                saveTo = open.FileName;

                //using (FileStream fs = new FileStream(open.FileName, FileMode.Open, FileAccess.Read))
                using (Stream fs = open.OpenFile()) {
                    _Contacts = (ContactsData)biFor.Deserialize(fs);
                    Contacts.ItemsSource = _Contacts._Contacts;
                }
            }
        }

        private string saveTo = null;
        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e) {
            if (saveTo == null) {
                SaveAsExecuted(sender, e);
            } else {
                using (Stream fs = new FileStream(saveTo, FileMode.Create)) {
                    biFor.Serialize(fs, _Contacts);
                }
            }
        }
        private void SaveAsExecuted(object sender, ExecutedRoutedEventArgs e) {
            using (forms.SaveFileDialog save = new forms.SaveFileDialog()) {
                save.DefaultExt = extention;
                save.Filter =
                    $"Demo Binaries (*.{extention})|*.{extention}| All files (*.*)|*.*";
                save.AddExtension = true;
                save.FileName = $"SavedDemoData";
                save.OverwritePrompt = true;

                forms.DialogResult result = save.ShowDialog();
                //save canceled
                if (result != System.Windows.Forms.DialogResult.OK) return;
                saveTo = save.FileName;

                //using (FileStream fs = new FileStream(save.FileName, FileMode.Create, FileAccess.Write))
                using (Stream fs = save.OpenFile()) {
                    biFor.Serialize(fs, _Contacts);
                }
            }
        }
    }
}
