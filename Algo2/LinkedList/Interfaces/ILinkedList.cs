using System.Collections.Generic;

namespace AlgoDataStructures.Interfaces {
    public interface ILinkedList<T>: IList<T> {
        void Insert(T val, int index);//redundant (backwards parameter list)
        T Get(int index);//redundant (indexer)
        T Remove();
        new T RemoveAt(int index);//conflicts with IList<T>'s 'void RemoveAt(int index)' /hence the 'new'/
        T RemoveLast();
        int Search(T val);//redundant (IndexOf)
    }
}
