using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EcommerceSite1.Models {
    public class UserRoleChangeModel {
        [Required]
        [Display(Name = "User's username")]
        public string UserName { get; set; }
        [Required]
        [Display(Name = "User's new role")]
        public string NewRole { get; set; }
    }
}