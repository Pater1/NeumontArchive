using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushDownAutomaton {
    public class StringSplitStateMachine {
        public static IEnumerable<char> defaultDelimiters = " .!?";

        public static IEnumerable<string> SplitOut(string toSplit, IEnumerable<char> delimiters = null){
            if(delimiters == null) delimiters = defaultDelimiters;
            
            string keepRegister = "";
            bool passingDelimiters = false;
            foreach(char c in toSplit){
                if(delimiters.Contains(c)){
                    if(!passingDelimiters){
                        yield return keepRegister;
                        keepRegister = "";
                    }
                    passingDelimiters = passingDelimiters ? passingDelimiters : !passingDelimiters;
                } else{
                    keepRegister += c;
                    passingDelimiters = false;
                }
            }
            if(!string.IsNullOrEmpty(keepRegister)) yield return keepRegister;
        }
    }
}
