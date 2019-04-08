using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HelloMVC_take4 {
    public class OnePlusFilledValidator : ValidationAttribute {
        public OnePlusFilledValidator(params string[] _fields) {
            fields = _fields;
        }
        private readonly string[] fields;
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            if (Convert.ToString(value).Length > 0) return null; //first field has value, therefore pass.

            IEnumerable<PropertyInfo> properties = this.fields.Select(validationContext.ObjectType.GetProperty);
            IEnumerable<string> values = properties.Select(p => p.GetValue(validationContext.ObjectInstance, null)).OfType<string>();
            foreach(string v in values) {
                if (v.Length > 0) return null;
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}