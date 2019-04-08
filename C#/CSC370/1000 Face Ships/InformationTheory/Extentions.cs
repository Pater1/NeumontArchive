using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformationTheory {
    public static class Extentions {
        public static int Occurences(this string file, string value) {
            if(string.IsNullOrEmpty(file) || string.IsNullOrEmpty(value)) return 0;

            string copy = file.Replace(value, "");

            int instancesOf = (file.Length - copy.Length) / value.Length;

            return instancesOf;
        }

        public static int CountAndRemoveOccurences(ref string file, string value) {
            if(string.IsNullOrEmpty(file) || string.IsNullOrEmpty(value)) return 0;
            
            string copy = file.Replace(value, "");

            int instancesOf = (file.Length - copy.Length) / value.Length;

            file = copy;

            return instancesOf;
        }

        internal static int CountAndRemoveOccurences(ref StringBuilder builder, string value) {
            if(string.IsNullOrEmpty(value)) return 0;

            int oldLength = builder.Length;

            builder.Replace(value, "");

            int instancesOf = (oldLength - builder.Length) / value.Length;

            return instancesOf;
        }
    }
}
