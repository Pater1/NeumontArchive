using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoDataStructures.Interfaces
{
    public interface ITree<T>: ICollection<T>, IEnumerable<T> {
        IEnumerable<T> InOrder { get; }
        IEnumerable<T> PreOrder { get; }
        IEnumerable<T> PostOrder { get; }
        IEnumerable<T> BreadthOrder { get; }

        int Height();
    }
}
