using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AlgoDataStructures
{
    public class MaxHeapPriorityQueue {
        public MaxHeapPriorityQueue(int capacity = 16){
            arr = new PQNode[capacity];
        }

        private PQNode[] arr;
        public int Count { get; private set; } = 0;

        private int Parent(int i) => (i - 1) / 2; 
        private int LeftChild(int i) => (2 * i + 1);
        private int RightChild(int i) => (2 * i + 2); 
        private void Swap(int from, int to){
            PQNode tmp = arr[from];
            arr[from] = arr[to];
            arr[to] = tmp;
        }

        public void Enqueue(int priority, int value) {
            PQNode n = new PQNode(priority, value);
            arr[Count] = n;
            int index = Count;
            Count++;

            if(Count >= arr.Length){
                PQNode[] ew = new PQNode[arr.Length * 2];
                arr.CopyTo(ew, 0);
                arr = ew;
            }

            while(arr[index].Priority > arr[Parent(index)].Priority){
                Swap(index, Parent(index));
                index = Parent(index);
            }
        }
        public PQNode Dequeue() {
            PQNode ret = arr[0];

            int index = 0;
            Swap(index, Count-1);
            Count--;
            while((LeftChild(index) < Count && arr[LeftChild(index)] != null && arr[LeftChild(index)].Priority > arr[index].Priority) ||
            (RightChild(index) < Count && arr[RightChild(index)] != null && arr[RightChild(index)].Priority > arr[index].Priority)) {
                int largerChild =   (LeftChild(index) >= Count && arr[LeftChild(index)] == null)? RightChild(index):
                                    (RightChild(index) >= Count && arr[RightChild(index)] == null) ? LeftChild(index) :
                                    arr[LeftChild(index)].Priority > arr[RightChild(index)].Priority ?
                                        LeftChild(index) : RightChild(index);
                Swap(index, largerChild);
                index = largerChild;
            }

            return ret;
        }
        public PQNode Peek() {
            return arr[0];
        }

        public PQNode[] ToSortedArray(){
            PQNode[] ret = new PQNode[Count];
            for(int i = Count-1; i >= 0; i--){
                ret[i] = Dequeue();
            }
            return ret;
        }

        public override string ToString() {
            string ret = "";
            for(int i = 0; i < Count; i++){
                ret += arr[i].ToString();
                if(i < Count - 1) ret += ",";
            }
            return ret;
        }
    }

    public class PQNode{
        public PQNode(int priority, int value) {
            Priority = priority;
            Value = value;
        }

        public int Priority { get; set; }
        public int Value { get; set; }

        public override string ToString() {
            return $"{Priority}:{Value}";
        }
    }
}
