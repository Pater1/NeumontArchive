using FSM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FiniteStateMachine {
    class Program {
        static void Main(string[] args) {
            string[] defaultResponces = new string[]{
                "What great weather!",
                "Tell me about yourself",
                "What do you like to do?",
                "What makes you sad?"
            };
            Dictionary<string, string[]> responces = new Dictionary<string, string[]>() {
                { "hi", new string[]{
                    "Hi", "sup?"
                }},
                { "hello", new string[]{
                    "Hello", "How are you?"
                }},
                { "howdy", new string[]{
                    "Hwody partner"
                }},
                { "aloha", new string[]{
                    "Aloha", "Serf's up!"
                }},
                { "thank", new string[]{
                    "You're welcome", "No, thank you!"
                }},
                { "how_are", new string[]{
                    "I'm fine, how about you?"
                }},
                { "how_do", new string[]{
                    "How do I do what?"
                }},
                { "ahoy", new string[]{
                    "YAARRR!!!!"
                }},
            };
            StateMachine<char> nd = new StateMachine<char>(new State<char>[]{
                #region Start
                new State<char>(new Transition<char>[]{
                    new EpsilonTransition<char>("Reset"),

                    new EpsilonTransition<char>("blank_hi"),
                    new EpsilonTransition<char>("blank_hello"),
                    new EpsilonTransition<char>("blank_howdy"),
                    new EpsilonTransition<char>("blank_aloha"),
                    new EpsilonTransition<char>("blank_thank"),
                    new EpsilonTransition<char>("blank_how_are"),
                    new EpsilonTransition<char>("blank_how_do"),
                    new EpsilonTransition<char>("blank_ahoy"),

                    new EpsilonTransition<char>("hi_"),
                    new EpsilonTransition<char>("hello_"),
                    new EpsilonTransition<char>("howdy_"),
                    new EpsilonTransition<char>("aloha_"),
                    new EpsilonTransition<char>("thank_"),
                    new EpsilonTransition<char>("how_are_"),
                    new EpsilonTransition<char>("how_do_"),
                    new EpsilonTransition<char>("ahoy_"),
                }, "Start", true),
                new State<char>(new Transition<char>[]{
                    new EpsilonTransition<char>("Reset")
                }, "Default Fail"),
                new State<char>(new Transition<char>[]{
                    new EpsilonTransition<char>("blank_hi"),
                    new EpsilonTransition<char>("blank_hello"),
                    new EpsilonTransition<char>("blank_howdy"),
                    new EpsilonTransition<char>("blank_aloha"),
                    new EpsilonTransition<char>("blank_thank"),
                    new EpsilonTransition<char>("blank_how_are"),
                    new EpsilonTransition<char>("blank_how_do"),
                    new EpsilonTransition<char>("blank_ahoy"),
                }, "Reset"),
                #endregion
                #region hi
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hi_", ' '),
                }, "blank_hi"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hi_h", 'h'),
                    new DirectTransition<char>("hi_h", 'H'),
                }, "hi_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hi_hi", 'i'),
                }, "hi_h"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hi_hi_", ' '),
                    new DirectTransition<char>("hi_hi_", '.'),
                    new DirectTransition<char>("hi_hi_", '!'),
                    new DirectTransition<char>("hi_hi_", '?'),
                }, "hi_hi", false, "hi"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("hi_hi_"),
                }, "hi_hi_", false, "hi"),
                #endregion
                #region hello
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_", ' '),
                }, "blank_hello"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_h", 'h'),
                    new DirectTransition<char>("hello_h", 'H'),
                }, "hello_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_he", 'e'),
                }, "hello_h"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_hel", 'l'),
                }, "hello_he"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_hell", 'l'),
                }, "hello_hel"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_hello", 'o'),
                }, "hello_hell"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("hello_hello_", ' '),
                    new DirectTransition<char>("hello_hello_", '.'),
                    new DirectTransition<char>("hello_hello_", '!'),
                    new DirectTransition<char>("hello_hello_", '?'),
                }, "hello_hello", false, "hello"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("hello_hello_"),
                }, "hello_hello_", false, "hello"),
                #endregion
                #region howdy
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_", ' '),
                }, "blank_howdy"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_h", 'h'),
                    new DirectTransition<char>("howdy_h", 'H'),
                }, "howdy_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_ho", 'o'),
                }, "howdy_h"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_how", 'w'),
                }, "howdy_ho"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_howd", 'd'),
                }, "howdy_how"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_howdy", 'y'),
                }, "howdy_howd"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("howdy_howdy_", ' '),
                    new DirectTransition<char>("howdy_howdy_", '.'),
                    new DirectTransition<char>("howdy_howdy_", '!'),
                    new DirectTransition<char>("howdy_howdy_", '?'),
                }, "howdy_howdy", false, "howdy"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("howdy_howdy_"),
                }, "howdy_howdy_", false, "howdy"),
                #endregion
                #region aloha
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_", ' '),
                }, "blank_aloha"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_a", 'a'),
                    new DirectTransition<char>("aloha_a", 'A'),
                }, "aloha_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_al", 'l'),
                }, "aloha_a"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_alo", 'o'),
                }, "aloha_al"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_aloh", 'h'),
                }, "aloha_alo"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_aloha", 'a'),
                }, "aloha_aloh"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("aloha_aloha_", ' '),
                    new DirectTransition<char>("aloha_aloha_", '.'),
                    new DirectTransition<char>("aloha_aloha_", '!'),
                    new DirectTransition<char>("aloha_aloha_", '?'),
                }, "aloha_aloha", false, "aloha"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("aloha_aloha_"),
                }, "aloha_aloha_", false, "aloha"),
                #endregion
                #region thank
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_", ' '),
                }, "blank_thank"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_t", 't'),
                    new DirectTransition<char>("thank_t", 'T'),
                }, "thank_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_th", 'h'),
                }, "thank_t"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_tha", 'a'),
                }, "thank_th"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_than", 'n'),
                }, "thank_tha"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thank", 'k'),
                }, "thank_than"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thanks", 's'),
                    new DirectTransition<char>("thank_thank_", ' '),
                }, "thank_thank"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thanks_", ' '),
                    new DirectTransition<char>("thank_thanks_", '.'),
                    new DirectTransition<char>("thank_thanks_", '!'),
                    new DirectTransition<char>("thank_thanks_", '?'),
                }, "thank_thanks", false, "thanks"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("thank_thanks_"),
                }, "thank_thanks_", false, "thanks"),

                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thank_y", 'y'),
                    new DirectTransition<char>("thank_thank_", ' '),
                }, "thank_thank_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thank_yo", 'o'),
                }, "thank_thank_y"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thank_you", 'u'),
                }, "thank_thank_yo"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("thank_thank_you_", ' '),
                    new DirectTransition<char>("thank_thank_you_", '.'),
                    new DirectTransition<char>("thank_thank_you_", '!'),
                    new DirectTransition<char>("thank_thank_you_", '?'),
                }, "thank_thank_you", false, "thanks"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("thank_thank_you_"),
                }, "thank_thank_you_", false, "thanks"),
                #endregion
                #region how_are
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_", ' '),
                }, "blank_how_are"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_h", 'h'),
                    new DirectTransition<char>("how_are_h", 'H'),
                }, "how_are_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_ho", 'o'),
                }, "how_are_h"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how", 'w'),
                }, "how_are_ho"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_", ' '),
                }, "how_are_how"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_a", 'a'),
                    new DirectTransition<char>("how_are_how_", ' '),
                }, "how_are_how_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_ar", 'r'),
                }, "how_are_how_a"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are", 'e'),
                }, "how_are_how_ar"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_", ' '),
                }, "how_are_how_are"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_y", 'y'),
                    new DirectTransition<char>("how_are_how_are_", ' '),
                }, "how_are_how_are_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_yo", 'o'),
                }, "how_are_how_are_y"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you", 'u'),
                }, "how_are_how_are_yo"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_", ' '),
                    new DirectTransition<char>("how_are_how_are_you.", '?'),
                }, "how_are_how_are_you"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("how_are_how_are_you."),
                }, "how_are_how_are_you.", false, "how_are"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_d", 'd'),
                    new DirectTransition<char>("how_are_how_are_you_", ' '),
                }, "how_are_how_are_you_", false, "how_are"),

                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_do", 'o'),
                }, "how_are_how_are_you_d"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_doi", 'i'),
                }, "how_are_how_are_you_do"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_doin", 'n'),
                }, "how_are_how_are_you_doi"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_doing", 'g'),
                }, "how_are_how_are_you_doin"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_are_how_are_you_doing_", '?'),
                }, "how_are_how_are_you_doing"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("how_are_how_are_you_doing_"),
                }, "how_are_how_are_you_doing_", false, "how_are"),
                #endregion
                #region how_do
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_", ' '),
                }, "blank_how_do"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_h", 'h'),
                    new DirectTransition<char>("how_do_h", 'H'),
                }, "how_do_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_ho", 'o'),
                }, "how_do_h"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how", 'w'),
                }, "how_do_ho"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_", ' '),
                }, "how_do_how"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_d", 'd'),
                    new DirectTransition<char>("how_do_how_", ' '),
                }, "how_do_how_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do", 'o'),
                }, "how_do_how_d"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_", ' '),
                }, "how_do_how_do"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_y", 'y'),
                    new DirectTransition<char>("how_do_how_do_", ' '),
                }, "how_do_how_do_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_yo", 'o'),
                }, "how_do_how_do_y"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_you", 'u'),
                }, "how_do_how_do_yo"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_you_", ' '),
                }, "how_do_how_do_you"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_you_d", 'd'),
                    new DirectTransition<char>("how_do_how_do_you_", ' '),
                }, "how_do_how_do_you_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_you_do", 'o'),
                }, "how_do_how_do_you_d"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("how_do_how_do_you_do_", '?'),
                }, "how_do_how_do_you_do"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("how_do_how_do_you_do_")
                }, "how_do_how_do_you_do_", false, "how_do"),
                #endregion
                #region ahoy
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_", ' '),
                }, "blank_ahoy"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_a", 'a'),
                    new DirectTransition<char>("ahoy_a", 'A'),
                }, "ahoy_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ah", 'h'),
                }, "ahoy_a"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_aho", 'o'),
                }, "ahoy_ah"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy", 'y'),
                }, "ahoy_aho"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_", ' '),
                }, "ahoy_ahoy"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_m", 'm'),
                    new DirectTransition<char>("ahoy_ahoy_", ' '),
                }, "ahoy_ahoy_"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_ma", 'a'),
                }, "ahoy_ahoy_m"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_mat", 't'),
                }, "ahoy_ahoy_ma"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_mate", 'e'),
                }, "ahoy_ahoy_mat"),
                new State<char>(new Transition<char>[]{
                    new DirectTransition<char>("ahoy_ahoy_mate_", ' '),
                    new DirectTransition<char>("ahoy_ahoy_mate_", '.'),
                    new DirectTransition<char>("ahoy_ahoy_mate_", '!'),
                    new DirectTransition<char>("ahoy_ahoy_mate_", '?'),
                }, "ahoy_ahoy_mate", false, "ahoy"),
                new State<char>(new Transition<char>[]{
                    new FailTransition<char>("ahoy_ahoy_mate_"),
                }, "ahoy_ahoy_mate_", false, "ahoy"),
                #endregion
            }, new FailTransition<char>("Default Fail"));
            StateMachine<char> d = nd.Render();

            string str = "";
            while(str != "exit"){
                str = Console.ReadLine();
                string r;
                r = new string(nd.Ingest(str).SelectMany(x => x + ",").ToArray());
                Console.WriteLine(r);
                r = d.Ingest(str).SingleOrDefault();
                Console.WriteLine((r != null && responces.ContainsKey(r)) ? responces[r].Random() : defaultResponces.Random());
                Console.WriteLine();
            }
        }
    }
}
