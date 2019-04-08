using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ExtentionMethods
{
    public static class Extentions
    {
        #region Prescribed
        /// <summary>
        /// Prints out and given IEnumerable to console in CSV fasion.
        /// </summary>
        /// <typeparam name="T">The Type of the collection to print. May be infered.</typeparam>
        /// <param name="toPrint">The collection to be printed</param>
        public static void Print<T>(this IEnumerable<T> toPrint)
        {
            string print = "";
            bool first = true;
            foreach(T t in toPrint)
            {
                print += (first ? "" : ",") + t.ToString();
                first = false;
            }
            Console.WriteLine(print);
        }

        /// <summary>
        /// Raises a (the base) to the power of exp if exp is positive.
        /// If exp is negative, this function will return the exp-th root of a
        /// </summary>
        /// <param name="a">The int to act as the base for this exponentiation</param>
        /// <param name="exp">The exponent by which to raise the base. NOTE: negative values here will result in returning the exp-th root of the base, NOT the technically correct mathematical definition of 1/(base^-exp).</param>
        /// <returns>If exp >= 0, base^exp; else, base^(1/exp) (I.E. the exp-th root)</returns>
        public static int ToPower(this int a, int exp)
        {
            if (exp == 0) return 1;
            double pow = exp < 0 ? (1.0 / -exp) : exp;
            return (int)Math.Pow(a, pow);
        }
        /// <summary>
        /// Tests if the given string is a palindrome
        /// </summary>
        /// <param name="palindrome">the string to test if is a palindrome</param>
        /// <returns>bool representing if the provided string is a palindrome</returns>
        public static bool IsPalindrome(this string palindrome)
        {
            string test = palindrome.Replace(" ", String.Empty).ToLower();
            IEnumerable<char> e1 = test.AsEnumerable();
            IEnumerable<char> e2 = e1.Reverse();

            for(int i = 0; i < e1.Count(); i++)
            {
                if (e1.ElementAt(i) != e2.ElementAt(i)) return false;
            }
            return true;
        }
        #endregion

        #region XML Inject/Eject
        /// <summary>
        /// Serializes the given toSerialize object to Xml, and appends the serialized object to the provided Xml tree at the given parentNode.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized and apended into the parentNode XmlNode. Can be infered</typeparam>
        /// <param name="parentNode">The XmlNode to append the serialized object to</param>
        /// <param name="toSerialize">The object to serialize, and append to the Xml tree at the given node.</param>
        public static void Inject<T>(this XmlNode parentNode, T toSerialize)
        {
            XmlSerializer ser = new XmlSerializer(toSerialize.GetType());

            XmlDocument xd = null;

            using (MemoryStream memStm = new MemoryStream())
            {
                ser.Serialize(memStm, toSerialize);

                memStm.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                using (var xtr = XmlReader.Create(memStm, settings))
                {
                    xd = new XmlDocument();
                    xd.Load(xtr);
                }
            }

            XmlDocument doc = null;
            if (parentNode is XmlDocument)
            {
                doc = ((XmlDocument)parentNode);
            }
            else
            {
                //necessary for crossing XmlDocument contexts
                doc = parentNode.OwnerDocument;
            }
            XmlNodeList chlns = xd.ChildNodes;

            for (int i = 0; i < chlns.Count; i++)
            {
            //    Console.WriteLine(chlns[i]);

                XmlNode importNode = doc.ImportNode(chlns[i], true);

                parentNode.AppendChild(importNode);
            }

            return;
        }
        /// <summary>
        /// Deserializes the given XmlNode into an object of type T. Does not catch any failed deserialization exceptions.
        /// </summary>
        /// <typeparam name="T">The datatype expected to be deserialized from the given dataNode. Can NOT be infered.</typeparam>
        /// <param name="dataNode">the Xml node to deserialized into and object of type T</param>
        /// <returns>The object deserialized from the gives dataNode, cast to the provided type T</returns>
        public static T Eject<T>(this XmlNode dataNode)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlReader reader = new XmlNodeReader(dataNode);
            T t = (T)ser.Deserialize(reader);
            return t;
        }
        #endregion
     
        #region Easy Rands
        private static Random rand = new Random();
        /// <summary>
        /// Returns a random int inbetween max (inclusive) and min (inclusive)
        /// </summary>
        /// <param name="max">The maximum of the range within which to select a random value</param>
        /// <param name="min">The minimum of the range within which to select a random value. Defaults to 1</param>
        /// <returns>a random int inbetween max (inclusive) and min (inclusive)</returns>
        public static int GetRandom(this int max, int min = 1)
        {
            return rand.Next(min, (max + 1));
        }
        /// <summary>
        /// Returns a random value from the given collection
        /// </summary>
        /// <typeparam name="T">The type of object to return. Can be infered.</typeparam>
        /// <param name="collection">The collection from which to select a random value</param>
        /// <returns>a random value from the given collection</returns>
        public static T GetRandom<T>(this IEnumerable<T> collection)
        {
            int index = collection.Count().GetRandom() - 1;
            return collection.ElementAt(index);
        }
        #endregion

        #region Homemade Syntactic Sugar
        /// <summary>
        /// Syntactic sugar to replace 'source == this || source == that || source == theOtherThing ||...'
        /// </summary>
        /// <typeparam name="T">The type of object given to compare. Can (and probably should) be infered.</typeparam>
        /// <param name="source">the single value to check if it equals any of the provided params</param>
        /// <param name="check">the array of params the check source against</param>
        /// <returns>if source is equal to any of the provided params</returns>
        public static bool EqualsAny<T>(this T source, params T[] check)
        {
            return check.Contains(source);
        }
        /// <summary>
        /// Syntactic sugar to add multiple items to a collection in a single line.
        /// </summary>
        /// <typeparam name="T">The type of object contained within, and to add to the given collection. Can be infered</typeparam>
        /// <param name="list">The collection to which to add the provided parameters</param>
        /// <param name="values">The values to add to the provided list</param>
        public static void AddRange<T>(this ICollection<T> list, params T[] values)
        {
            foreach (T value in values)
            {
                list.Add(value);
            }
        }
        /// <summary>
        /// Syntactic sugar to check if the given object is null, and throw an ArgumentNullException if it is. Allows for an optional customMessage.
        /// </summary>
        /// <typeparam name="T">The type of object to check if null. Can (and probably should) be infered. Used in constructing the default error message.</typeparam>
        /// <param name="obj">the object to check if null</param>
        /// <param name="customMessage">Optional. The message with which to build the thrown exeption if one is thrown. A default error message will be provided if null. Defaults to null.</param>
        public static void ThrowIfNull<T>(this T obj, string customMessage = null) where T: class
        {
            if (obj == null) throw new ArgumentNullException(typeof(T).FullName, (customMessage == null) ? string.Format("Parameter of Type:{0} may not be null!", typeof(T).Name) : customMessage);
        }
        #endregion
    }
}