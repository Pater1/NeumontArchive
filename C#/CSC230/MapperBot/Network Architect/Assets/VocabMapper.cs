using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets {
    public class VocabMapper: MonoBehaviour {
        public List<MappedVocab> mappeds = new List<MappedVocab>();
        [System.Serializable]
        public class MappedVocab {
            public string mapped;
            public GrammarComponent to;

            public MappedVocab(string mapped, GrammarComponent to) {
                this.mapped = mapped;
                this.to = to;
            }
            public MappedVocab() {
                this.mapped = "";
                this.to = GrammarComponent.Unknown;
            }
        }

        private struct Reeval{
            public int[] indexes;
            public int insert;

            public Reeval(int[] indexes, int insert) {
                this.indexes = indexes;
                this.insert = insert;
            }
        }
        private struct Swap{
            public int[] pre, post;
            public string combo;
            public Reeval rev;
            public MappedVocab mapper;

            private static List<int> Pre = new List<int>();
            private static List<int> Post = new List<int>();
            public static bool FromRange(out Swap swap, int lower, int upper, string[] tokens, Reeval reEval, List<MappedVocab> mappeds) {
                string combo = "";
                Pre.Clear();
                Post.Clear();
                for(int k = 0; k < reEval.indexes.Length; k++) {
                    if(k < lower) {
                        Pre.Add(reEval.indexes[k]);
                    } else if(k > upper) {
                        Post.Add(reEval.indexes[k]);
                    } else {
                        if(!string.IsNullOrEmpty(combo)) {
                            combo += " ";
                        }
                        combo += tokens[reEval.indexes[k]];
                    }
                }

                MappedVocab mapper = mappeds.Where(x => x.mapped.ToLowerInvariant() == combo.ToLowerInvariant()).FirstOrDefault();
                if(mapper != null) {
                    swap = new Swap(Pre.ToArray(), Post.ToArray(), combo, reEval, mapper);
                    return true;
                } else {
                    swap = default(Swap);
                    return false;
                }
            }
            public static bool FromRange(out Swap swap, int lower, int upper, string[] tokens, Reeval reEval, GrammarComponent swapTo) {
                string combo = "";
                Pre.Clear();
                Post.Clear();
                for(int k = 0; k < reEval.indexes.Length; k++) {
                    if(k < lower) {
                        Pre.Add(reEval.indexes[k]);
                    } else if(k > upper) {
                        Post.Add(reEval.indexes[k]);
                    } else {
                        if(!string.IsNullOrEmpty(combo)){
                            combo += " ";
                        }
                        combo += tokens[reEval.indexes[k]];
                    }
                }

                swap = new Swap(Pre.ToArray(), Post.ToArray(), combo, reEval, new MappedVocab(combo, swapTo));
                return true;
            }

            public Swap(int[] pre, int[] post, string combo, Reeval rev, MappedVocab mapper) {
                this.pre = pre;
                this.post = post;
                this.combo = combo;
                this.rev = rev;
                this.mapper = mapper;
            }
            public void Execute(ref List<Reeval> reEval, ref List<GrammarTreeNode<string>> ret) {
                reEval.Remove(this.rev);
                int index = this.rev.insert;
                ret.Insert(index, new GrammarTreeNode<string>(this.mapper.to, this.combo));
                for(int e = 0; e < reEval.Count; e++) {
                    if(reEval[e].insert > index) {
                        Reeval edit = reEval[e];
                        edit.insert++;
                        reEval[e] = edit;
                    }
                }
                if(this.pre.Any()) {
                    reEval.Add(new Reeval(this.pre, index));
                }
                if(this.post.Any()) {
                    reEval.Add(new Reeval(this.post, index + 1));
                }
            }
        }
        public List<GrammarTreeNode<string>> FullMap(string[] tokenized) {
            //explicit grammarized words
            List<GrammarTreeNode<string>> ret = new List<GrammarTreeNode<string>>();
            List<int> reEvalRegister = new List<int>();
            List<Reeval> reEval = new List<Reeval>();
            for(int i = 0; i < tokenized.Length; i++){
                MappedVocab mapper = mappeds.Where(x => x.mapped.ToLowerInvariant() == tokenized[i].ToLowerInvariant()).FirstOrDefault();
                if(mapper != null){
                    ret.Add(new GrammarTreeNode<string>(mapper.to, tokenized[i]));
                    if(reEvalRegister.Any()){
                        reEval.Add(new Reeval(reEvalRegister.ToArray(), ret.Count-1));
                        reEvalRegister.Clear();
                    }
                }else{
                    reEvalRegister.Add(i);
                }
            }
            if(reEvalRegister.Any()) {
                reEval.Add(new Reeval(reEvalRegister.ToArray(), ret.Count));
                reEvalRegister.Clear();
            }

            //explicit and cached grammarized multi-word components
            List<Swap> swaps = new List<Swap>();
            //List<int> pre = new List<int>(), post = new List<int>();
            do {
                swaps.Clear();
                for(int h = reEval.Count-1; h >= 0; h--) {
                    int[] reEv = reEval[h].indexes;
                    if(reEv.Length > 1) {
                        for(int i = 0; i < reEv.Length; i++) {
                            for(int j = i + 1; j < reEv.Length; j++) {
                                Swap s;
                                if(Swap.FromRange(out s, i, j, tokenized, reEval[h], mappeds)){
                                    swaps.Add(s);
                                }
                            }
                        }
                        if(swaps.Any()){
                            Swap swap = swaps.OrderByDescending(x => x.combo.Length).First();
                            swap.Execute(ref reEval, ref ret);
                            swaps.Clear();
                        }
                    }
                }
            } while(swaps.Any());

            //numbers
            for(int h = reEval.Count - 1; h >= 0; h--) {
                int[] reEv = reEval[h].indexes;
                for(int i = 0; i < reEv.Length; i++) {
                    double d;
                    if(double.TryParse(tokenized[reEv[i]], out d)){ 
                        Swap s;
                        if(Swap.FromRange(out s, i, i, tokenized, reEval[h], GrammarComponent.Number)) {
                            s.Execute(ref reEval, ref ret);
                        }
                    }
                }
            }

            //implicit cities
            for(int h = reEval.Count - 1; h >= 0; h--) {
                int[] reEv = reEval[h].indexes;
                int lower = -1, upper = -1;
                swaps.Clear();
                for(int i = 0; i < reEv.Length; i++) {
                    if(char.IsUpper(tokenized[reEv[i]][0])) {
                        if(lower < 0){
                            lower = i;
                            upper = i;
                        }else{
                            upper = i;
                        }
                    }else if(lower >= 0 && upper >= 0){
                        Swap s;
                        if(Swap.FromRange(out s, lower, upper, tokenized, reEval[h], GrammarComponent.City)) {
                            swaps.Add(s);
                        }
                        lower = -1;
                        upper = -1;
                    }
                }
                if(lower >= 0 && upper >= 0) {
                    Swap s;
                    if(Swap.FromRange(out s, lower, upper, tokenized, reEval[h], GrammarComponent.City)) {
                        swaps.Add(s);
                    }
                    lower = -1;
                    upper = -1;
                }
                foreach(Swap s in swaps){
                    s.Execute(ref reEval, ref ret);
                    mappeds.Add(s.mapper);
                }
            }

            for(int h = ret.Count - 1; h >= 0; h--){
                if(ret[h].componentType == GrammarComponent.Unknown){
                    ret.Remove(ret[h]);
                }
            }

            return ret;
        }
    }
}