using System;
using System.Collections.Generic;
using System.Linq;
using PushDownAutomaton.GrammarTree;

namespace PushDownAutomaton {
    public class Vocabulary<T> {
        protected Dictionary<T, GrammarComponent> TranslateDictionary { get; set; }
        protected Vocabulary() {
            TranslateDictionary = null;
        }
        public Vocabulary(Dictionary<T, GrammarComponent> translateDictionary) {
            TranslateDictionary = translateDictionary;
        }
        
        public virtual IEnumerable<GrammarTreeNode<T>> Translate(IEnumerable<T> split) {
            return split.Select(x => new GrammarTreeNode<T>(TranslateDictionary[x], x)).ToArray();
        }
    }
    public struct VocabularyResponceMapping<T>{
        public GrammarComponent[] sentenceStructure;
        public T mapFrom, mapTo;

        public VocabularyResponceMapping(IEnumerable<GrammarComponent> sentenceStructure, T mapFrom, T mapTo) {
            this.sentenceStructure = sentenceStructure.ToArray();
            this.mapFrom = mapFrom;
            this.mapTo = mapTo;
        }

        public bool SwapMatch(GrammarTreeNode<T> match, out T swap) {
            bool ret = SwapMatch(match);
            swap = ret ? mapTo : match.leafValue;
            return ret;
        }
        public bool SwapMatch(GrammarTreeNode<T> match) {
            if(!match.hasLeafValue) return false;
            if(!EqualityComparer<T>.Default.Equals(match.leafValue, mapFrom)) return false;
            return KernelMatch(match.FindParentChain().Select(x => x.componentType));
        }
        public bool KernelMatch(IEnumerable<GrammarComponent> chain) {
            GrammarComponent[] test = chain.ToArray();
            if(test.Length < sentenceStructure.Length) return false;

            for(int i = 0; i < (test.Length - sentenceStructure.Length); i++){
                if(KernelMatch(test, i)){
                    return true;
                }
            }
            return false;
        }
        public bool KernelMatch(GrammarComponent[] chain, int start) {
            for(int i = 0; i < sentenceStructure.Length; i++){
                if((sentenceStructure[i] & chain[i + start]) == 0){
                    return false;
                }
            }
            return true;
        }
    }
}