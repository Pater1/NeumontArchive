using System;
using System.Collections.Generic;
using System.Linq;
using PushDownAutomaton.GrammarTree;

namespace PushDownAutomaton {
    public class PushDownParser<T> {
        private IEnumerable<GrammarComponentMapping> MappingSet{ get; set; }
        public PushDownParser(IEnumerable<GrammarComponentMapping> mapping) {
            MappingSet = mapping.OrderByDescending(x => x.MapDown.Count()).ToArray();
        }

        public GrammarTreeNode<T> Parse(IEnumerable<GrammarTreeNode<T>> leaves) {
            List<GrammarTreeNode<T>> output = leaves.ToList();
            IEnumerable<GrammarComponentMappingPDA<T>> mappingPDAs;
            do {
                mappingPDAs = new GrammarComponentMappingPDA<T>[0];
                foreach(GrammarTreeNode<T> g in output) {
                    mappingPDAs = mappingPDAs.Concat(MappingSet.Select(x => new GrammarComponentMappingPDA<T>(x))).Where(x => x.Match(g));
                }
                mappingPDAs = mappingPDAs.OrderByDescending(x => x.MatchPointer).ThenByDescending(x => output.IndexOf(x.MatchedMapping.Peek()));
                GrammarComponentMappingPDA<T> toSub = mappingPDAs.FirstOrDefault();
                if(toSub != null){
                    List<GrammarTreeNode<T>> children = new List<GrammarTreeNode<T>>();
                    int reinsert = 0;
                    while(toSub.MatchedMapping.Any()){
                        GrammarTreeNode<T> popped = toSub.MatchedMapping.Pop();
                        children.Insert(0, popped);
                        reinsert = output.IndexOf(popped);
                        output.Remove(popped);
                    }
                    output.Insert(reinsert, new GrammarTreeNode<T>(toSub.MappingMatch.MapUp, children));
                }
            } while(mappingPDAs.Count() > 0);

            if(output.Count != 1){
                throw new FormatException("Input is not valid for the provided grammar!");
            }else{
                return output.First();
            }
        }
    }

    public class GrammarComponentMappingPDA<T> {
        public GrammarComponentMappingPDA(GrammarComponentMapping mappingMatch) {
            MappingMatch = mappingMatch;
            MatchedMapping = new Stack<GrammarTreeNode<T>>();
        }

        public GrammarComponentMapping MappingMatch { get; set; }
        public Stack<GrammarTreeNode<T>> MatchedMapping { get; set; }
        public int MatchPointer => MatchedMapping.Count();

        public bool Match(GrammarTreeNode<T> to){
            if(MatchPointer == MappingMatch.MapDown.Length) return true;

            if((MappingMatch.MapDown[MatchPointer] & to.componentType) != 0){
                MatchedMapping.Push(to);
                return true;
            }else{
                return false;
            }
        }
    }
    public struct GrammarComponentMapping {
        public GrammarComponentMapping(GrammarComponent mapUp, IEnumerable<GrammarComponent> mapDown) : this() {
            MapUp = mapUp;
            MapDown = mapDown.ToArray();
        }

        public GrammarComponent MapUp { get; set; }
        public GrammarComponent[] MapDown { get; set; }
    }
}