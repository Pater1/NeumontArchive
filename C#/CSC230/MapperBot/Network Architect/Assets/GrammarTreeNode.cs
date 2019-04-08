using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets {
    public class GrammarTreeNode<T>: IEnumerable<GrammarTreeNode<T>> {
        public GrammarComponent componentType;

        public IEnumerable<GrammarTreeNode<T>> children;
        public List<GrammarTreeNode<T>> ImmediateChildren{
            get{
                return children.ToList();
            }
        }
        public GrammarTreeNode<T> parent;
        public IEnumerable<GrammarTreeNode<T>> FindParentChain() {
            return FindParentChain(new List<GrammarTreeNode<T>>());
        }
        private List<GrammarTreeNode<T>> FindParentChain(List<GrammarTreeNode<T>> chain) {
            if(parent != null){
                chain = parent.FindParentChain(chain);
            }
            chain.Add(this);
            return chain;
        }

        public readonly T leafValue;
        public readonly bool hasLeafValue;

        public GrammarTreeNode(GrammarComponent componentType, IEnumerable<GrammarTreeNode<T>> children) {
            this.componentType = componentType;
            this.children = children;
            foreach(GrammarTreeNode<T> child in children){
                child.parent = this;
            }
            this.leafValue = default(T);
            this.hasLeafValue = false;
        }
        public GrammarTreeNode(GrammarComponent componentType, T endValue) {
            this.componentType = componentType;
            this.leafValue = endValue;
            this.hasLeafValue = true;
            children = new GrammarTreeNode<T>[0];
        }

        public IEnumerable<T> Values { 
            get {
                IEnumerator<GrammarTreeNode<T>> emu = this.GetEnumerator();
                while(emu.MoveNext()) {
                    if(emu.Current.hasLeafValue) yield return emu.Current.leafValue;
                }
            }
        }
        
        //depth first search
        public IEnumerator<GrammarTreeNode<T>> GetEnumerator() {
            foreach(GrammarTreeNode<T> child in children){
                IEnumerator<GrammarTreeNode<T>> emu = child.GetEnumerator();
                while(emu.MoveNext()){
                    yield return emu.Current;
                }
            }
            yield return this;
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
