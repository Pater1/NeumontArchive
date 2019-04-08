using AlgoDataStructures.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgoDataStructures
{
    public class SingleLinkedList<T> : ILinkedList<T> {
        private SingleLinkedListNode<T> head;
        private SingleLinkedListNode<T> Tail {
            get {
                SingleLinkedListNode<T> ret = head;
                if(head != null){
                    while(ret.next != null) ret = ret.next;
                }
                return ret;
            }
        }

        public SingleLinkedList() { }
        public SingleLinkedList(IEnumerable<T> source) {
            foreach(T t in source) {
                Add(t);
            }
        }
        public SingleLinkedList(params T[] source) : this((IEnumerable<T>)source) { }


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
        private SingleLinkedListNode<T> NodeAt(int index) {
            if(index < 0 || index >= Count) {
                throw new IndexOutOfRangeException();
            }
            SingleLinkedListNode<T> ret = head;
            while(index > 0) {
                ret = ret.next;
                index--;
            }
            return ret;
        }
        private SingleLinkedListNode<T> NodeWith(T index) {
            SingleLinkedListNode<T> cur = head;
            while(cur != null) {
                if(cur.data.Equals(index)) {
                    return cur;
                }
                cur = cur.next;
            }
            return null;
        }

        public IEnumerator<T> GetEnumerator() {
            SingleLinkedListNode<T> n = head;
            while(n != null) {
                yield return n.data;
                n = n.next;
            }
        }
        IEnumerator IEnumerable.GetEnumerator() {
            SingleLinkedListNode<T> n = head;
            while(n != null) {
                yield return n.data;
                n = n.next;
            }
        }

        public void Add(T item) {
            SingleLinkedListNode<T> n = new SingleLinkedListNode<T>(item);
            if(head == null) {
                head = n;
            } else {
                Tail.next = n;
            }
            Count++;
        }

        public void Clear() {
            head = null; Count = 0;
        }

        public bool Contains(T item) {
            return System.Linq.Enumerable.Contains(this, item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            SingleLinkedListNode<T> cur = head;
            int count = 0;
            while(cur != null && count < array.Length) {
                array[count] = cur.data;
                cur = cur.next;
            }
        }

        private T Remove(SingleLinkedListNode<T> what) {
            if(what != null && head != null) {
                if(what == head) {
                    head = head.next;
                } else {
                    SingleLinkedListNode<T> cur = head;
                    while(cur.next != null && cur.next != what) cur = cur.next;
                    cur.next = what.next;
                }

                T ret = what.data;
                Count--;
                return ret;
            }
            return default(T);
        }
        public bool Remove(T item) {
            return Remove(NodeWith(item)) != null;
        }
        public T Remove() {
            return Remove(head);
        }
        public T RemoveLast() {
            return Remove(Tail);
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
            SingleLinkedListNode<T> cur = head;
            int count = 0;
            while(cur != null) {
                if(cur.data.Equals(item)) {
                    return count;
                }

                cur = cur.next;
                count++;
            }
            return -1;
        }

        public void Insert(T val, int index) {
            ((IList<T>)this).Insert(index, val);
        }
        void IList<T>.Insert(int index, T item) {
            if(index < Count) {
                SingleLinkedListNode<T> nxt = NodeAt(index), prv = null, nw = new SingleLinkedListNode<T>(item);

                if(nxt != head) {
                    prv = head;
                    while(prv.next != nxt && prv != null) {
                        prv = prv.next;
                    }
                }

                nw.next = nxt;
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
            foreach(T t in this) {
                ret += t.ToString() + ", ";
            }
            return new string(ret.Reverse().Skip(2).Reverse().ToArray());
        }
    }
    internal class SingleLinkedListNode<D> {
        public SingleLinkedListNode<D> next;
        public D data;

        public SingleLinkedListNode(D data) {
            this.data = data;
        }
    }
}
