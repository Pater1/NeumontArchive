using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace InformationTheory.Huffman {
    [System.Serializable]
    internal class HuffmanTree: IHuffmanNode {
        //left -> 1, right -> 0
        public IHuffmanNode left, right;
        
        [JsonIgnore]
        public IHuffmanNode Parent { get; set; }
        [JsonIgnore]
        public int MatchCount { get; set; }
        [JsonIgnore]
        public IHuffmanNode UltimateParent {
            get {
                IHuffmanNode cur = this;
                while(cur.Parent != null) {
                    cur = cur.Parent;
                }
                return (HuffmanTree)cur;
            }
        }

        public HuffmanTree(IHuffmanNode left, IHuffmanNode right) {
            this.left = left;
            this.right = right;

            MatchCount = left.MatchCount + right.MatchCount;

            left.Parent = this;
            right.Parent = this;
        }

        public void EncodedValue_Buildup(BitList bitStore, IHuffmanNode from) {
            if(from == left) {
                bitStore.Push(true);
            } else if(from == right) {
                bitStore.Push(false);
            }else{
                throw new Exception("error reading tree!");
            }
        }

        public BitList GetEncodedValue(string value) {
            BitList v = left.GetEncodedValue(value);
            if(v == null) {
                v = right.GetEncodedValue(value);
            }
            return v;
        }

        public IHuffmanNode DeepClone() {
            return new HuffmanTree(left.DeepClone(), right.DeepClone());
        }
    }
}
