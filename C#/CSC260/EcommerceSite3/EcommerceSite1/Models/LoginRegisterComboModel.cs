using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceSite1.Models {
    public class LoginRegisterComboModel {
        public bool? ToRegister { get {
                bool regNull = Register == null;
                bool logNull = Login == null;

                if (regNull && logNull) {
                    return null;
                } else if(regNull) {
                    return false;
                }else if (logNull) {
                    return true;
                } else {
                    return null;
                }
            }
        }
        public RegisterViewModel Register { get; set; } = null;
        public LoginViewModel Login { get; set; } = null;
        public string LoginReturnURL { get; set; } = null;
    }
}