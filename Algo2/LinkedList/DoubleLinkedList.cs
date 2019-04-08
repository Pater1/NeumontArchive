using AlgoDataStructures.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AlgoDataStructures {
    public class DoubleLinkedList<T>: ILinkedList<T> {
        private DoubleLinkedListNode<T> head, tail;
        public DoubleLinkedList() { }
        public DoubleLinkedList(IEnumerable<T> source) { 
            foreach(T t in source){
                Add(t);
            }
        }
        public DoubleLinkedList(params T[] source): this((IEnumerable<T>)source) { }

        public int Count { get; private set; }
        public bool IsReadOnly => false;

        public T this[int index] {
            get {
                return NodeAt(index).data;
            }

            set {
                NodeAt(index).data = value;
            }
        }
        public T Get(int index) {
            return this[index];
        }
        private DoubleLinkedListNode<T> NodeAt(int index) {
            if(index < 0 || index >= Count) {
                throw new IndexOutOfRangeException();
            }
            DoubleLinkedListNode<T> ret = head;
            while(index > 0) {
                ret = ret.next;
                index--;
            }
            return ret;
        }
        private DoubleLinkedListNode<T> NodeWith(T index) {
            DoubleLinkedListNode<T> cur = head;
            while(cur != null) {
                if(cur.data.Equals(index)){
                    return cur;
                }
                cur = cur.next;
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator() {
            DoubleLinkedListNode<T> n = head;
            while(n != null){
                yield return n.data;
                n = n.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            DoubleLinkedListNode<T> n = head;
            while(n != null) {
                yield return n.data;
                n = n.next;
            }
        }

        public void Add(T item) {
            DoubleLinkedListNode<T> n = new DoubleLinkedListNode<T>(item);
            if(tail == null || head == null) {
                tail = n;
                head = n;
            }else{
                tail.next = n;
                n.previous = tail;
                tail = n;
            }
            Count++;
        }

        public void Clear() {
            tail = null; head = null; Count = 0;
        }

        public bool Contains(T item) {
            return System.Linq.Enumerable.Contains(this, item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            DoubleLinkedListNode<T> cur = head;
            int count = 0;
            while(cur != null && count < array.Length) {
                array[count] = cur.data;
                cur = cur.next;
            }
        }
        
        private T Remove(DoubleLinkedListNode<T> what) {
            if(what != null) {
                T ret = what.data;

                DoubleLinkedListNode<T> nxt = what.next, prv = what.previous;

                if(nxt == null) {//what is tail
                    tail = what.previous;
                    if(tail != null) {
                        tail.next = null;
                    }else{
                        head = null;
                    }
                } else if(prv == null) {//what is head
                    head = what.next;
                    if(head != null) {
                        head.previous = null;
                    }else{
                        tail = null;
                    }
                } else {
                    nxt.previous = prv;
                    prv.next = nxt;
                }

                Count--;
                return ret;
            } else {
                return default(T);
            }
        }
        public bool Remove(T item) {
            return Remove(NodeWith(item)) != null;
        }
        public T Remove() {
            return Remove(head);
        }
        public T RemoveLast() {
            return Remove(tail);
        }
        public T RemoveAt(int index) {
            return Remove(NodeAt(index));
        }
        void IList<T>.RemoveAt(int index) {
            RemoveAt(index);
        }
        
        public int Search(T val) {
            return IndexOf(val);
        }
        public int IndexOf(T item) {
            DoubleLinkedListNode<T> cur = head;
            int count = 0;
            while(cur != null) {
                if(cur.data.Equals(item)){
                    return count;
                }

                cur = cur.next;
                count++;
            }
            return -1;
        }

        public void Insert(T val, int index) => ((IList<T>)this).Insert(index, val);
        void IList<T>.Insert(int index, T item) {
            if(index < Count) {
                DoubleLinkedListNode<T> nxt = NodeAt(index), prv = nxt.previous, nw = new DoubleLinkedListNode<T>(item);

                nw.next = nxt;
                nw.previous = prv;
                nxt.previous = nw;
                if(prv != null) {
                    prv.next = nw;
                } else {
                    head = nw;
                }

                Count++;
            }else{
                throw new IndexOutOfRangeException();
            }
        }
        
        public override string ToString() {
            string ret = "";
            foreach(T t in this){
                ret += t.ToString() + ", ";
            }
            return ret.Length > 0? ret.Remove(ret.Length-2): ret;
        }
    }
    internal class DoubleLinkedListNode<D> {
        public DoubleLinkedListNode<D> next, previous;
        public D data;

        public DoubleLinkedListNode(D data, DoubleLinkedListNode<D> prev = null) {
            this.data = data;
            this.previous = prev;
        }
    }
}