using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtentionMethods_Again
{
    public static class ExtentionMethods {
        public static string Concantonate(this string[] strs) {
            string ret = "";
            foreach (string s in strs) {
                ret += s;
            }
            return ret;
        }
        public static string[] SplitOn(this string strs, params string[] on) {
            foreach (string s in on) {
                strs.Replace(s, Encoding.UTF8.GetString(new byte[] { 2 })[0].ToString());
            }

            return strs.Split(Encoding.UTF8.GetString(new byte[] { 2 })[0]);
        }
        public static long Square(long i) {
            return i * i;
        }
    }
}
