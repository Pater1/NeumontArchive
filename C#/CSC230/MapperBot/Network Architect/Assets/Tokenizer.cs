using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets {
    public class Tokenizer: MonoBehaviour {
        public char[] delimiters = " .!?,".ToCharArray();
        public Parser parse;

        public void Tokenize(string toTokenize) {
            foreach(string s in toTokenize.Split(Environment.NewLine.ToCharArray())) {
                if(string.IsNullOrEmpty(s)) continue;

                List<string> tokenized = new List<string>();
                string register = "";
                foreach(char c in s) {
                    if(delimiters.Contains(c) || char.IsWhiteSpace(c)) {
                        if(register != "") {
                            tokenized.Add(register);
                            register = "";
                        }
                    } else {
                        if(char.IsDigit(c)) {
                            if(register != "" && !char.IsDigit(register[register.Length - 1])) {
                                tokenized.Add(register);
                                register = "";
                            }
                        } else {
                            if(register != "" && char.IsDigit(register[register.Length - 1])) {
                                tokenized.Add(register);
                                register = "";
                            }
                        }
                        register += c;
                    }
                }
                if(register != "") {
                    tokenized.Add(register);
                    register = "";
                }
                parse.Parse(tokenized.ToArray());
            }
        }
    }
}