using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets {
    public class Parser: MonoBehaviour {
        public VocabMapper vocabMap;
        public MapCommandInterpreter interpreter;
        public Map map;
        public void Parse(string[] tokenized) {
            IEnumerable<GrammarTreeNode<string>> leaves = vocabMap.FullMap(tokenized);
            GrammarTreeNode<string> sentance = ParseInternal(leaves);
            Command<Map> c = interpreter.Interpret(sentance);
            c.Execute(map);
        }

        public List<GrammarComponentMapping> MappingSet = new List<GrammarComponentMapping>();

        public GrammarTreeNode<string> ParseInternal(IEnumerable<GrammarTreeNode<string>> leaves) {
            List<GrammarTreeNode<string>> output = leaves.ToList();
            IEnumerable<GrammarComponentMappingPDA<string>> mappingPDAs;
            do {
                mappingPDAs = new GrammarComponentMappingPDA<string>[0];
                foreach(GrammarTreeNode<string> g in output) {
                    mappingPDAs = mappingPDAs.Concat(MappingSet.Select(x => new GrammarComponentMappingPDA<string>(x, MappingSet.IndexOf(x)))).Where(x => x.Match(g));
                }
                mappingPDAs = mappingPDAs.OrderBy(x => x.priority)/*.ThenByDescending(x => x.MatchPointer).ThenByDescending(x => output.IndexOf(x.MatchedMapping.Peek()))*/.ToArray();
                GrammarComponentMappingPDA<string> toSub = mappingPDAs.FirstOrDefault();
                if(toSub != null) {
                    List<GrammarTreeNode<string>> children = new List<GrammarTreeNode<string>>();
                    int reinsert = 0;
                    while(toSub.MatchedMapping.Any()) {
                        GrammarTreeNode<string> popped = toSub.MatchedMapping.Pop();
                        children.Insert(0, popped);
                        reinsert = output.IndexOf(popped);
                        output.Remove(popped);
                    }
                    output.Insert(reinsert, new GrammarTreeNode<string>(toSub.MappingMatch.MapUp, children));
                }
            } while(mappingPDAs.Count() > 0);

            if(output.Count != 1) {
                throw new FormatException("Input is not valid for the provided grammar!");
            } else {
                return output.First();
            }
        }
    }

    public class GrammarComponentMappingPDA<T> {
        public int priority;
        public GrammarComponentMappingPDA(GrammarComponentMapping mappingMatch, int priority) {
            MappingMatch = mappingMatch;
            MatchedMapping = new Stack<GrammarTreeNode<T>>();
            this.priority = priority;
        }

        public GrammarComponentMapping MappingMatch { get; set; }
        public Stack<GrammarTreeNode<T>> MatchedMapping { get; set; }
        public int MatchPointer {
            get; set;
        }

        public bool Match(GrammarTreeNode<T> to) {
            if(MatchPointer == MappingMatch.MapDown.Length) return true;

            VariadicGrammarComponent vgc = MappingMatch.MapDown[MatchPointer];
            if(vgc.MapTo == to.componentType) {
                MatchedMapping.Push(to);
                if(!vgc.isVariadic){
                    MatchPointer++;
                }
                return true;
            } else {
                if(vgc.isVariadic) {
                    MatchPointer++;
                    return Match(to);
                } else {
                    return false;
                }
            }
        }
    }
    [System.Serializable]
    public class VariadicGrammarComponent{
        public GrammarComponent MapTo;
        public bool isVariadic = false;
    }
    [System.Serializable]
    public class GrammarComponentMapping {
        public GrammarComponentMapping(GrammarComponent mapUp, IEnumerable<GrammarComponent> mapDown) {
            MapUp = mapUp;
            MapDown = mapDown.Select(x => new VariadicGrammarComponent() { MapTo = x, isVariadic = false}).ToArray();
        }
        public GrammarComponentMapping() {
            MapUp = GrammarComponent.Unknown;
            MapDown = new VariadicGrammarComponent[0];
        }

        public GrammarComponent MapUp;
        public VariadicGrammarComponent[] MapDown;
    }
}