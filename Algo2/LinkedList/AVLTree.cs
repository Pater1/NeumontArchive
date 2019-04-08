using AlgoDataStructures.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoDataStructures
{
    public class AVLTree<D>: ITree<AVLTree<D>>, ITree<D>, IComparable<AVLTree<D>> where D : IComparable<D>, IComparable {
        private AVLTree<D> lesser, greater;

        private bool dataSet;
        private D data;
        public D Data{ 
            get {
                return data;
            }
            private set{
                data = value;
                dataSet = true;
            }
        }

        public AVLTree() { }
        public AVLTree(D data) {
            Data = data;
        }
        public AVLTree(IEnumerable<D> data) {
            foreach(D d in data){
                Add(d);
            }
        }
        public AVLTree(params D[] data) : this((IEnumerable<D>)data) { }
        
        public int CompareTo(AVLTree<D> other) => Data.CompareTo(other.Data);
        public override bool Equals(object obj) => Data.Equals((obj as AVLTree<D>).Data);
        public override int GetHashCode() => Data.GetHashCode();
        public override string ToString() => new string(((ITree<AVLTree<D>>)this).BreadthOrder.Select(x => x.dataSet ? x.Data.ToString() + ", " : "").SelectMany(x => x).Reverse().Skip(2).Reverse().ToArray());

        #region ITree-common-
        public int Height() => Math.Max((lesser != null ? lesser.Height() : 0),
                                        (greater != null ? greater.Height() : 0))
                             + (dataSet ? 1 : 0);//this
        public int Count => (lesser != null ? lesser.Count : 0) +
                            (greater != null ? greater.Count : 0) +
                            (dataSet? 1: 0);//this
        public bool IsReadOnly => false;

        public void Clear() {
            lesser = null;
            greater = null;
        }

        public D[] ToArray(){
            List<D> l = new List<D>();
            foreach(D d in BreadthOrder){
                l.Add(d);
            }
            return l.ToArray();
        }
        #endregion

        #region ITree<AVLTree<D>>
        IEnumerable<AVLTree<D>> ITree<AVLTree<D>>.InOrder {
            get {
                if(lesser != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)lesser).InOrder) {
                        yield return d;
                    }
                }
                yield return this;

                if(greater != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)greater).InOrder) {
                        yield return d;
                    }
                }
            }
        }
        IEnumerable<AVLTree<D>> ITree<AVLTree<D>>.PreOrder {
            get {
                yield return this;
                if(lesser != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)lesser).InOrder) {
                        yield return d;
                    }
                }
                if(greater != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)greater).InOrder) {
                        yield return d;
                    }
                }
            }
        }
        IEnumerable<AVLTree<D>> ITree<AVLTree<D>>.PostOrder {
            get {

                if(lesser != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)lesser).InOrder) {
                        yield return d;
                    }
                }
                if(greater != null) {
                    foreach(AVLTree<D> d in ((ITree<AVLTree<D>>)greater).InOrder) {
                        yield return d;
                    }
                }
                yield return this;
            }
        }
        IEnumerable<AVLTree<D>> ITree<AVLTree<D>>.BreadthOrder {
            get {
                IEnumerable<AVLTree<D>> staging = new AVLTree<D>[] { this };
                while(staging.Any()) {
                    foreach(AVLTree<D> d in staging) {
                        yield return d;
                    }
                    staging = staging.Select(x => new AVLTree<D>[] { x.lesser, x.greater }).SelectMany(x => x).Where(x => x != null);
                }
            }
        }

        private readonly Stack<AVLTree<D>> rebal = new Stack<AVLTree<D>>();
        public void Add(AVLTree<D> item){ 
            if(item == null) return;
            
            AVLTree<D> cur = this;
            while(cur != null) {
                rebal.Push(cur);
                int comp = cur.CompareTo(item);
                if(comp > 0) { //goes into lesser
                    if(cur.lesser == null) {
                        cur.lesser = item;
                        break;
                    } else {
                        cur = cur.lesser;
                    }
                } else {
                    if(cur.greater == null) {
                        cur.greater = item;
                        break;
                    } else {
                        cur = cur.greater;
                    }
                }
            }
            
            while(rebal.Count > 0) {
                rebal.Pop().Rebalance();
            }
        }

        public bool Contains(AVLTree<D> item) {
            if(Equals(item)){
                return true;
            }else{
                int comp = CompareTo(item);
                if(comp > 0 && lesser != null) { //goes into lesser
                    return lesser.Contains(item);
                }else if(greater != null){
                    return greater.Contains(item);
                }
            }
            return false;
        }

        public void CopyTo(AVLTree<D>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(AVLTree<D> item) {
            if(item == null) return false; //no item to remove

            AVLTree<D> toDelete = this;
            int comp = 0;
            while(toDelete != null) {
                rebal.Push(toDelete);
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
            
            if(toDelete.lesser == null && toDelete.greater == null){
                toDelete.dataSet = false;
            } else if(toDelete.lesser == null) {
                toDelete.Data = toDelete.greater.Data;
                toDelete.greater = greater.greater;
                toDelete.lesser = greater.lesser;
            } else if(toDelete.greater == null) {
                toDelete.Data = toDelete.lesser.Data;
                toDelete.greater = lesser.greater;
                toDelete.lesser = lesser.lesser;
            } else {
                AVLTree<D> inOrderPredecesor = toDelete.greater;
                while(inOrderPredecesor.lesser != null) {
                    rebal.Push(inOrderPredecesor);
                    inOrderPredecesor = inOrderPredecesor.lesser;
                }

                D data = inOrderPredecesor.Data;
                Remove(inOrderPredecesor);

                toDelete.Data = data;
            }

            while(rebal.Count > 0) {
                rebal.Pop().Rebalance();
            }
            return true;
        }

        private const int UnbalanceLimit = 1;
        private int Balance => (lesser != null ? lesser.Height() : 0) -
                            (greater != null ? greater.Height() : 0);
        private void Rebalance() {
            if(Balance > UnbalanceLimit) {//left rotation
                if(lesser.Balance < 0){//left-right rotation
                    lesser.RightRotation();
                }
                LeftRotation();
            }else if(Balance < -UnbalanceLimit) {//right rotation
                if(greater.Balance > 0){//right-left rotation
                    greater.LeftRotation();
                }
                RightRotation();
            }
        }
        internal void LeftRotation() {
            AVLTree<D> g = new AVLTree<D>(Data);
            g.greater = greater;
            g.lesser = lesser.greater;

            AVLTree<D> l = lesser.lesser;

            Data = lesser.Data;
            greater = g;
            lesser = l;
        }
        internal void RightRotation() {
            AVLTree<D> l = new AVLTree<D>(Data);
            l.lesser = lesser;
            l.greater = greater.lesser;

            AVLTree<D> g = greater.greater;

            Data = greater.Data;
            greater = g;
            lesser = l;
        }
        
        IEnumerator<AVLTree<D>> IEnumerable<AVLTree<D>>.GetEnumerator() => ((IEnumerable<AVLTree<D>>)BreadthOrder).GetEnumerator();
        #endregion

        #region ITree<D>
        public IEnumerable<D> InOrder => ((ITree<AVLTree<D>>)this).InOrder.Select(x => x.Data);
        public IEnumerable<D> PreOrder => ((ITree<AVLTree<D>>)this).PreOrder.Select(x => x.Data);
        public IEnumerable<D> PostOrder => ((ITree<AVLTree<D>>)this).PostOrder.Select(x => x.Data);
        public IEnumerable<D> BreadthOrder {
            get {
                List<AVLTree<D>> staging = new List<AVLTree<D>>() { this };
                while(staging.Count > 0) {
                    foreach(AVLTree<D> d in staging) {
                        if(d.dataSet) yield return d.Data;
                    }
                    staging = staging.Select(x => new AVLTree<D>[] { x.lesser, x.greater }).SelectMany(x => x).Where(x => x != null).ToList();
                }
            }
        }

        public void Add(D item) {
            if(dataSet) {
                Add(new AVLTree<D>(item));
            }else{
                Data = item;
            }
        }

        public bool Contains(D item) => Contains(new AVLTree<D>(item));

        public void CopyTo(D[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public bool Remove(D item) => Remove(new AVLTree<D>(item));

        public IEnumerator<D> GetEnumerator() {
            return BreadthOrder.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
        #endregion
    }
}
