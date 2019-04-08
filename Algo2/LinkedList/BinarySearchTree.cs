using AlgoDataStructures.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlgoDataStructures {
    public class BinarySearchTree<D>: ICollection<D> where D : IComparable<D>, IComparable {
        private BinarySearchTreeNode<D> headNode;
        public BinarySearchTree() { }

        public int Count => headNode != null? headNode.Count: 0;
        public int Height() => headNode != null ? headNode.Height() +1: 0;
        public bool IsReadOnly => false;

        public void Add(D item) {
            if(headNode == null) {
                headNode = new BinarySearchTreeNode<D>(item);
            } else {
                headNode.Add(new BinarySearchTreeNode<D>(item));
            }
        }

        public void Clear() {
            headNode = null;
        }

        public bool Contains(D item) {
            return headNode.Contains(new BinarySearchTreeNode<D>(item));
        }

        public void CopyTo(D[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(D item) {
            return headNode.Remove(new BinarySearchTreeNode<D>(item));
        }

        public string InOrder() => new string(inOrder.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        public string PreOrder() => new string(preOrder.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        public string PostOrder() => new string(postOrder.Select(x => x.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());

        public IEnumerable<D> inOrder => headNode != null ? headNode.InOrder.Select(x => x.Data) : new D[0];
        public IEnumerable<D> preOrder => headNode != null ? headNode.PreOrder.Select(x => x.Data) : new D[0];
        public IEnumerable<D> postOrder => headNode != null ? headNode.BreadthOrder.Select(x => x.Data) : new D[0];

        public IEnumerator<D> GetEnumerator() {
            return inOrder.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public override string ToString() {
            return headNode.ToString();
        }
        public D[] ToArray(){
            return inOrder.ToArray();
        }
    }
    internal class BinarySearchTreeNode<D>: ITree<BinarySearchTreeNode<D>>, 
                                            IComparable<BinarySearchTreeNode<D>>
                                where D : IComparable<D>, IComparable {
        private BinarySearchTreeNode<D> lesser, greater, parent;//equal goes into greater; parent used to simplify remove logic
        public D Data{ get; private set; }
        
        public BinarySearchTreeNode(D data, BinarySearchTreeNode<D> parent = null) {
            this.Data = data;
            this.parent = parent;
        }

        public int Count { 
            get {
                return  (greater != null ? greater.Count : 0) +
                        (lesser != null ? lesser.Count : 0) +
                        1;//this
            }
        }
        public int Height(){ 
            if(lesser == null && greater == null) {
                return 0;
            } else if(lesser == null) {
                return greater.Height() + 1;
            } else if(greater == null) {
                return lesser.Height() + 1;
            } else {
                return Math.Max(lesser.Height() + 1, greater.Height() + 1);
            }
        }

        public bool IsReadOnly => false;

        public void Add(BinarySearchTreeNode<D> item) {
            BinarySearchTreeNode<D> cur = this;
            while(cur != null) {
                int comp = cur.CompareTo(item);
                if(comp > 0) {
                    if(cur.lesser == null) {
                        item.parent = cur;
                        cur.lesser = item;
                        break;
                    } else {
                        cur = cur.lesser;/*.Add(item);*/
                    }
                } else {
                    if(cur.greater == null) {
                        item.parent = cur;
                        cur.greater = item;
                        break;
                    } else {
                        cur = cur.greater;/*.Add(item);*/
                    }
                }
            }
        }

        public bool Contains(BinarySearchTreeNode<D> item) {
            if(Equals(item)){
                return true;
            }

            int comp = CompareTo(item);
            if(comp > 0){
                return lesser != null && lesser.Contains(item);
            }else {
                return greater != null && greater.Contains(item);
            }
        }
        
        public void Clear() {
            throw new NotImplementedException();
        }
        public void CopyTo(BinarySearchTreeNode<D>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(BinarySearchTreeNode<D> item) {
            if(item == null) return false; //no item to remove

            BinarySearchTreeNode<D> toDelete = this;
            int comp = 0;
            while(toDelete != null) {
                if(toDelete.Equals(item)) {
                    break;
                }
                comp = toDelete.CompareTo(item);
                if(comp > 0) { //goes into lesser
                    toDelete = toDelete.lesser;
                } else {
                    toDelete = toDelete.greater;
                }
            }
            if(toDelete == null) return false; //item not contained

            if(toDelete.lesser == null && toDelete.greater == null) {
                int c = toDelete.parent.CompareTo(toDelete);
                if(c > 0){
                    toDelete.parent.lesser = null;
                }else {
                    toDelete.parent.greater = null;
                }
            } else if(toDelete.lesser == null) {
                toDelete.Data = toDelete.greater.Data;
                toDelete.greater = greater.greater;
                toDelete.lesser = greater.lesser;
            } else if(toDelete.greater == null) {
                toDelete.Data = toDelete.lesser.Data;
                toDelete.greater = lesser.greater;
                toDelete.lesser = lesser.lesser;
            } else {
                BinarySearchTreeNode<D> inOrderPredecesor = toDelete.greater;
                while(inOrderPredecesor.lesser != null) {
                    inOrderPredecesor = inOrderPredecesor.lesser;
                }

                D data = inOrderPredecesor.Data;
                Remove(inOrderPredecesor);

                toDelete.Data = data;
            }

            return true;
        }

        public IEnumerable<BinarySearchTreeNode<D>> InOrder {
            get {
                if(lesser != null) foreach(BinarySearchTreeNode<D> d in lesser.InOrder) {
                    yield return d;
                }
                yield return this;
                if(greater != null) foreach(BinarySearchTreeNode<D> d in greater.InOrder) {
                    yield return d;
                }
            }
        }
        public IEnumerable<BinarySearchTreeNode<D>> PreOrder {
            get {
                yield return this;
                if(lesser != null) foreach(BinarySearchTreeNode<D> d in lesser.PreOrder) {
                    yield return d;
                }
                if(greater != null) foreach(BinarySearchTreeNode<D> d in greater.PreOrder) {
                    yield return d;
                }
            }
        }
        public IEnumerable<BinarySearchTreeNode<D>> PostOrder {
            get {
                if(lesser != null) foreach(BinarySearchTreeNode<D> d in lesser.PostOrder) {
                    yield return d;
                }
                if(greater != null) foreach(BinarySearchTreeNode<D> d in greater.PostOrder) {
                    yield return d;
                }
                yield return this;
            }
        }
        public IEnumerable<BinarySearchTreeNode<D>> BreadthOrder { 
            get {
                IEnumerable<BinarySearchTreeNode<D>> staging = new BinarySearchTreeNode<D>[] { this };
                while(staging.Any()){
                    foreach(BinarySearchTreeNode<D> d in staging){
                        yield return d;
                    }
                    staging = staging.Select(x => new BinarySearchTreeNode<D>[] { x.lesser, x.greater }).SelectMany(x => x).Where(x => x!= null);
                }
            }
        }
        public IEnumerator<BinarySearchTreeNode<D>> GetEnumerator() {
            return InOrder.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public int CompareTo(BinarySearchTreeNode<D> other) {
            return Data.CompareTo(other.Data);
        }

        public override bool Equals(object obj) {
            if(obj is BinarySearchTreeNode<D> other){
                return Data.Equals(other.Data);
            }
            return false;
        }
        public override int GetHashCode() {
            return Data.GetHashCode();
        }
        public override string ToString() {
            return new string(this.Select(x => x.Data.ToString() + ", ").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());
        }
    }
}