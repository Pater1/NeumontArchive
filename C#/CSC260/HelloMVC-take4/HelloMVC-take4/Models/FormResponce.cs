using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelloMVC_take4.Models {
    public class FormResponce {
        [Required(ErrorMessage = "Please enter your age")]
        [Range(6, int.MaxValue, ErrorMessage = "You must be older than 5")]
        public byte Age { get; set; }
        [Required(ErrorMessage = "Please enter your first name")]
        public string NameFirst { get; set; }
        [Required(ErrorMessage = "Please enter your last name")]
        public string NameLast { get; set; }

        [OnePlusFilledValidator("Email", ErrorMessage = "Phone or Email must be provided.")]
        public string PhoneNumber { get; set; }
        [RegularExpression(".+\\@.+\\..+",
            ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
    }
}