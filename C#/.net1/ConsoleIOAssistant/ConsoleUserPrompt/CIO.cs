using System;
using System.Collections.Generic;

namespace CSC160_ConsoleMenu
{
    public static class CIO
    {
        /// <summary>
        /// Generates a console-based menu using the strings in options as the menu items.
        /// Automatically numbers each option starting at 1 and incrementing by 1.
        /// Reserves the number 0 for the "quit" option when withQuit is true.
        /// </summary>
        /// <param name="options">strings representing the menu options</param>
        /// <param name="withQuit">adds option 0 for "quit" when true</param>
        /// <returns>the int of the selection made by the user</returns>
        public static int PromptForMenuSelection(IEnumerable<string> options, bool withQuit = false)
        {
            int i = 0;
            if (withQuit)
            {
                Console.WriteLine(string.Format("   {0} - {1}", i.ToString(), "Quit"));
            }
            i++;
            foreach(string st in options)
            {
                Console.WriteLine(string.Format("   {0} - {1}", i.ToString(), st));
                i++;
            }
            int ret = PromptForInt("Please make your selection: ", withQuit?0:1, i - 1);
            return ret;
        }

        /// <summary>
        /// Generates a prompt that expects the user to enter one of two responses that will equate
        /// to a boolean value. The trueString represents the case insensitive response that will equate to true. 
        /// The falseString acts similarly, but for a false boolean value.
        /// 	Example: Assume this method is called with a trueString argument of "yes" and a falseString
        /// 	argument of "no". If the user enters "YES", the method returns true. If the user enters "no",
        /// 	the method returns false. All other inputs are considered invalid, the user will be informed, 
        /// 	and the prompt will repeat.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="trueString">the case insensitive value that will evaluate to true</param>
        /// <param name="falseString">the case insensitive value that will evaluate to false</param>
        /// <returns>the boolean value</returns>
        public static bool PromptForBool(string message, string trueString = "Y", string falseString = "N", bool caseSenesitive = false)
        {
            string yes = trueString;
            string no = falseString;
            if (!caseSenesitive)
            {
                yes = yes.ToLower();
                no = no.ToLower();
            }
            while (true)
            {
                string responce = PromptForInput(message);
                if (!caseSenesitive)
                {
                    responce = responce.ToLower();
                }
                bool isYes = responce.Equals(yes);
                if (isYes || responce.Equals(no)) return isYes;

                Console.WriteLine(string.Format("Error! Please respond either {0} or {1}\n", trueString, falseString));
            }
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a byte value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the byte value</returns>
        public static byte PromptForByte(string message, byte min = byte.MinValue, byte max = byte.MaxValue)
        {
            return (byte)PromptForLong(message, min, max);
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a short value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the short value</returns>
        public static short PromptForShort(string message, short min = short.MinValue, short max = short.MaxValue)
        {
            return (short)PromptForLong(message, min, max);
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a int value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the int value</returns>
        public static int PromptForInt(string message, int min = int.MinValue, int max = int.MaxValue)
        {
            return (int)PromptForLong(message, min, max);
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a long value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the long value</returns>
        public static long PromptForLong(string message, long min = long.MinValue, long max = long.MaxValue)
        {
            long l;
            while (true)
            {
                string inp = PromptForInput(message);
                if (long.TryParse(inp, out l))
                {
                    if (l >= min && l <= max)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Error! Please keep within range {0} to {1}\n", min, max));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Error! Please respond with an integer value between {0} and {1}\n", min, max));
                }
            }
            return l;
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a float value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the float value</returns>
        public static float PromptForFloat(string message, float min = float.MinValue, float max = float.MaxValue)
        {
            return (float)PromptForDouble(message, min, max);
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a double value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the double value</returns>
        public static double PromptForDouble(string message, double min = double.MinValue, double max = double.MaxValue)
        {
            double l;
            while (true)
            {
                string inp = PromptForInput(message);
                if (double.TryParse(inp, out l))
                {
                    if (l >= min && l <= max)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Error! Please keep within range {0} to {1}\n", min, max));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Error! Please respond with a decimal value between {0} and {1}\n", min, max));
                }
            }
            return l;
        }

        /// <summary>
        /// Generates a prompt that expects a numeric input representing a decimal value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the decimal value</returns>
        public static decimal PromptForDecimal(string message, decimal min = decimal.MinValue, decimal max = decimal.MaxValue)
        {
            decimal l;
            while (true)
            {
                string inp = PromptForInput(message);
                if (decimal.TryParse(inp, out l))
                {
                    if (l >= min && l <= max)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Error! Please keep within range {0} to {1}\n", min, max));
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Error! Please respond with a decimal value between {0} and {1}\n", min, max));
                }
            }
            return l;
        }

        /// <summary>
        /// Generates a prompt that allows the user to enter any response and returns the string.
        /// When allowEmpty is true, empty responses are valid. When false, responses must contain
        /// at least one character (including whitespace).
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user.</param>
        /// <param name="allowEmpty">when true, makes empty responses valid</param>
        /// <returns>the input from the user as a String</returns>
        public static string PromptForInput(string message, bool allowEmpty = false)
        {
            string ret;
            while (true)
            {
                Console.Write(message);
                ret = Console.ReadLine();
                if (allowEmpty || !ret.Equals("")) break;
            }
            return ret;
        }

        /// <summary>
        /// Generates a prompt that expects a single character input representing a char value.
        /// This method loops until valid input is given.
        /// </summary>
        /// <param name="message">the prompt to be displayed to the user</param>
        /// <param name="min">the inclusive minimum boundary</param>
        /// <param name="max">the inclusive maximum boundary</param>
        /// <returns>the char value</returns>
        public static char PromptForChar(string message, char min = char.MinValue, char max = char.MaxValue)
        {
            char c;
            while (true)
            {
                string inp = PromptForInput(message);
                try
                {
                    c = inp[1];
                    Console.WriteLine(string.Format("Error! Please respond with a single letter.\n"));
                }
                catch
                {
                    c = inp[0];
                    if (c >= min && c <= max) break;

                    Console.WriteLine(string.Format("Error! Please respond with a single letter within the range [{0}, {1}].\n", min, max));
                }
            }
            return c;
        }
    }
}
