using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Contacts {
    [System.Serializable]
    public class ContactsData: INotifyPropertyChanged {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void FieldChanged([CallerMemberName] string field = null) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
        #endregion

        private ObservableCollection<Contact> contacts;
        public ObservableCollection<Contact> _Contacts {
            get {
                if(contacts == null) {
                    contacts = new ObservableCollection<Contact>();
                }
                return contacts;
            }
            set {
                if(value != null) {
                    contacts = value;
                } else if (contacts == null) {
                    contacts = new ObservableCollection<Contact>();
                }
            }
        }
    }
}
