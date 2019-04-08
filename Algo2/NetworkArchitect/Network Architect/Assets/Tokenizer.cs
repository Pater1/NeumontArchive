using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tokenizer : MonoBehaviour {
    public char[] defaultDelimiters = " .?!,".ToCharArray();

    public Parser parser;

    public void PushDownTokens(string s){
        parser.Parse(Tokenize(s).ToArray());
    }

    public IEnumerable<string> Tokenize(string toSplit, IEnumerable<char> delimiters = null) {
        if(delimiters == null) delimiters = defaultDelimiters;

        string keepRegister = "";
        bool passingDelimiters = false;
        foreach(char c in toSplit) {
            if(delimiters.Contains(c)) {
                if(!passingDelimiters) {
                    yield return keepRegister;
                    keepRegister = "";
                }
                passingDelimiters = passingDelimiters ? passingDelimiters : !passingDelimiters;
            } else {
                keepRegister += c;
                passingDelimiters = false;
            }
        }
        if(!string.IsNullOrEmpty(keepRegister)) yield return keepRegister;
    }
}
