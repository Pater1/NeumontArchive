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

namespace Contacts {
    /// <summary>
    /// Interaction logic for ContactForm.xaml
    /// </summary>
    public partial class ContactForm : UserControl {
        //private static DependencyProperty contact = 
        //    DependencyProperty.Register("contact", typeof(Contact), typeof(ContactForm));
        //public Contact LocalContact {
        //    get { return (Contact)GetValue(contact); }
        //    set {
        //        if (value != null) {
        //            SetValue(contact, value);
        //            DataContext = LocalContact;
        //        }
        //    }
        //}

        public Contact LocalContact {
            get {
                return (Contact)DataContext;
            }
            set {
                if (value != null) {
                    DataContext = value;
                }
            }
        }

        //private Contact contact;
        //public Contact LocalContact {
        //    get { return contact; }
        //    set { contact = value; DataContext = contact; }
        //}

        public ContactForm() {
            InitializeComponent();
        }

        private void DeleteCurrentPhone(object sender, RoutedEventArgs e) {
            Phone phone = (Phone)PhoneSeletor.SelectedItem;
            LocalContact._Phones.Remove(phone);
        }

        private void AddNewPhone(object sender, RoutedEventArgs e) {
            Phone phone = new Phone();
            LocalContact._Phones.Add(phone);
            PhoneSeletor.SelectedIndex = LocalContact._Phones.Count() - 1;
        }

        private void DeleteCurrentEmail(object sender, RoutedEventArgs e) {
            Email email = (Email)EmailSeletor.SelectedItem;
            LocalContact._Emails.Remove(email);
        }

        private void AddNewEmail(object sender, RoutedEventArgs e) {
            Email email = new Email();
            LocalContact._Emails.Add(email);
            EmailSeletor.SelectedIndex = LocalContact._Emails.Count() - 1;
        }
    }
}
