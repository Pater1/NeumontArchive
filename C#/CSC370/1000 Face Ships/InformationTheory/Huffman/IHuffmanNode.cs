using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InformationTheory.Huffman {
    internal interface IHuffmanNode {
        [JsonIgnore]
        IHuffmanNode Parent { get; set; }
        [JsonIgnore]
        int MatchCount { get; set; }

        BitList GetEncodedValue(string value);

        [JsonIgnore]
        IHuffmanNode UltimateParent { get; }

        IHuffmanNode DeepClone();
    }
}
