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
    public class Contact: INotifyPropertyChanged {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void FieldChanged([CallerMemberName] string field = null) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
        #endregion

        private string nameFirst;
        public string _NameFirst {
            get {
                if (nameFirst == null) {
                    nameFirst = "";
                }
                return nameFirst; }
            set {
                if (value != null) {
                    nameFirst = value;
                }else if (nameFirst == null) {
                    nameFirst = "";
                }
                FieldChanged();
            }
        }
        private string nameLast;
        public string _NameLast {
            get {
                if(nameLast == null) {
                    nameLast = "";
                }
                return nameLast; }
            set {
                if (value != null) {
                    nameLast = value;
                }else if (nameLast == null) {
                    nameLast = "";
                }
                FieldChanged();
            }
        }

        private ObservableCollection<Phone> phones;
        public ObservableCollection<Phone> _Phones {
            get {
                if(phones == null) {
                    phones = new ObservableCollection<Phone>();
                }
                return phones;
            }
            set {
                if (value != null) {
                    phones = value;
                }else if (phones == null) {
                    phones = new ObservableCollection<Phone>();
                }
            }
        }

        private ObservableCollection<Email> emails;
        public ObservableCollection<Email> _Emails {
            get {
                if (emails == null) {
                    emails = new ObservableCollection<Email>();
                }
                return emails;
            }
            set {
                if (value != null) {
                    emails = value;
                } else if (emails == null) {
                    emails = new ObservableCollection<Email>();
                }
            }
        }

        private Group group;
        public Group _Group {
            get { return group; }
            set { group = value; FieldChanged(); }
        }

        public bool _IsValid {
            get {
                return !string.IsNullOrEmpty(_NameFirst) && !string.IsNullOrWhiteSpace(_NameFirst)
                    && !string.IsNullOrEmpty(_NameLast) && !string.IsNullOrWhiteSpace(_NameLast)
                    && (
                        (_Phones.Count() > 0 && ((Func<bool>)(() => {
                            foreach (Phone p in _Phones) {
                                if (!p._IsValid) return false;
                            }
                            return true;
                        }))())
                        || 
                        (_Emails.Count() > 0 && ((Func<bool>)(() => {
                            foreach (Email e in _Emails) {
                                if (!e._IsValid) return false;
                            }
                            return true;
                        }))())
                    );
            }
        }
    }

    public enum Group {
        Family,
        Friend,
        Coworker
    }

    [System.Serializable]
    public class Email : INotifyPropertyChanged {
        #region INotifyPropertyChanged
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void FieldChanged([CallerMemberName] string field = null) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
        #endregion

        private string address;
        public string _Address {
            get {
                if(address == null) {
                    address = "";
                }
                return address;
            }
            set {
                if (value != null) {
                    address = value;
                }else if (address == null) {
                    address = "";
                }
                FieldChanged();
            }
        }

        public EmailType type;
        public EmailType _Type {
            get { return type; }
            set { type = value; FieldChanged(); }
        }

        public bool _IsValid {
            get {
                return !string.IsNullOrEmpty(_Address) && !string.IsNullOrWhiteSpace(_Address);
            }
        }
    }
    public enum EmailType {
        work,
        personal
    }

    [System.Serializable]
    public class Phone : INotifyPropertyChanged {
        #region INotifyPropertyChanged
        [field:NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private void FieldChanged([CallerMemberName] string field = null) {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
        #endregion

        private string number;
        public string _Number {
            get {
                if(number == null) {
                    number = "";
                }
                return number;
            }
            set {
                if (value != null) {
                    number = value;
                }else if (number == null) {
                    number = "";
                }
                FieldChanged(); }
        }

        public PhoneType type;
        public PhoneType _Type {
            get { return type; }
            set { type = value; FieldChanged(); }
        }

        public bool _IsValid {
            get {
                return !string.IsNullOrEmpty(_Number) && !string.IsNullOrWhiteSpace(_Number);
            }
        }
    }
    public enum PhoneType {
        work,
        home,
        cell
    }
}
