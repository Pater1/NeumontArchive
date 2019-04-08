using System;
using System.Collections.Generic;
using System.Linq;
using PushDownAutomaton.GrammarTree;

namespace PushDownAutomaton {
    public class Replier {
        private IEnumerable<VocabularyResponceMapping<string>> Mapping;
        public Replier(IEnumerable<VocabularyResponceMapping<string>> mapping) {
            Mapping = mapping;
        }

        public string Reply(GrammarTreeNode<string> parsed) {
            if(parsed.Count() == 1 && parsed.First().hasLeafValue && parsed.First().leafValue == "yes") return "cool!";
            IEnumerable<GrammarTreeNode<string>> leaves = parsed.Where(x => x.hasLeafValue).ToArray();
            string ret = "";
            foreach(GrammarTreeNode<string> leaf in leaves){
                IEnumerable<VocabularyResponceMapping<string>> lMap = Mapping.Where(x => x.SwapMatch(leaf)).ToArray();
                if(lMap.Any()){
                    VocabularyResponceMapping<string> mp = lMap.OrderByDescending(x => x.sentenceStructure.Length).First();
                    ret += " " + mp.mapTo;
                }else{
                    ret += " " + leaf.leafValue;
                }
            }
            return $"because{ret}?";
        }
    }
}