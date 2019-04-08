using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets {
    public static class Extentions {
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements) {
            return elements.DifferentCombinations(elements.Count());
        }
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k) {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }
}
