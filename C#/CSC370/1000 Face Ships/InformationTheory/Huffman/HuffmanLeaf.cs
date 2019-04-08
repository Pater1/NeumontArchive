using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformationTheory.Huffman {
    [System.Serializable]
    internal class HuffmanLeaf: IHuffmanNode {
        public string value;
        public BitList encodedValue;

        public int BitsSaved {
            get {
                if(encodedValue == null){
                    return int.MinValue;
                }

                int bitsRemoved = value.Length * 8 * MatchCount;
                int bitsAdded = Encode.Length * 8 + encodedValue.BitLength * MatchCount;

                return bitsRemoved - bitsAdded;
            }
        }

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
        
        public HuffmanLeaf(string value, int matchCount) {
            this.value = value;
            this.MatchCount = matchCount;
            Parent = null;
            encodedValue = new BitList();
        }
        public HuffmanLeaf() {
            value = null;
            this.MatchCount = 0;
            Parent = null;
            encodedValue = new BitList();
        }
        public HuffmanLeaf(string rawValue, BitList encodedValue) {
            value = rawValue;
            this.encodedValue = encodedValue;

            this.MatchCount = 0;
            Parent = null;
        }

        public BitList EncodedValue {
            get {
                if((encodedValue == null || encodedValue.BitLength <= 0) && Parent != null) {
                    if(encodedValue == null){
                        encodedValue = new BitList();
                    }

                    IHuffmanNode last = this;
                    IHuffmanNode cur = Parent;
                    while(cur != null) {
                        ((HuffmanTree)cur).EncodedValue_Buildup(encodedValue, last);
                        last = cur;
                        cur = cur.Parent;
                    }
                    encodedValue = encodedValue.Reversed();
                }

                return encodedValue;
            }
        }

        public BitList GetEncodedValue(string value) {
            if(value == this.value) {
                return EncodedValue;
            } else {
                return null;
            }
        }
        
        public IHuffmanNode DeepClone() {
            HuffmanLeaf ret = new HuffmanLeaf();
            ret.value = value;
            ret.encodedValue = encodedValue.DeepClone();
            ret.MatchCount = MatchCount;
            return ret;
        }

        public override string ToString() {
            return "\"" + value + "\" <-> \'" + encodedValue.ToString() + "\'";
        }

        public byte[] Encode {
            get {
                byte[] encoded = encodedValue.Encode;
                byte[] raw = Encoding.ASCII.GetBytes(value);
                byte[] rawLength = BitConverter.GetBytes(raw.Length);

                int bitLength = raw.Length;
                byte bitlengthByteLength;
                if(bitLength == (byte)bitLength) {
                    bitlengthByteLength = 1;
                    rawLength = new byte[] { (byte)bitLength };
                } else if(bitLength == (short)bitLength) {
                    bitlengthByteLength = 2;
                    rawLength = BitConverter.GetBytes((short)bitLength);
                } else {
                    bitlengthByteLength = 4;
                    rawLength = BitConverter.GetBytes(bitLength);
                }

                if(BitConverter.IsLittleEndian) {
                    Array.Reverse(rawLength);
                }

                return (new byte[] { bitlengthByteLength }).Concat(rawLength).Concat(raw).Concat(encoded).ToArray();
            }
        }
        public static HuffmanLeaf Decode(IEnumerable<byte> bytes) {
            int refPntr = 0;
            return Decode(bytes, ref refPntr);
        }
        internal static HuffmanLeaf Decode(IEnumerable<byte> bytes, ref int refPntr) {
            int bitlengthByteLength = bytes.First();

            byte[] bigEndianBitLength = bytes.Skip(1).Take(bitlengthByteLength).ToArray();
            if(BitConverter.IsLittleEndian) {
                Array.Reverse(bigEndianBitLength);
            }

            int rawLength;
            switch(bigEndianBitLength.Length) {
                case 1:
                    rawLength = bigEndianBitLength.First();
                    break;
                case 2:
                    rawLength = BitConverter.ToInt16(bigEndianBitLength);
                    break;
                case 4:
                    rawLength = BitConverter.ToInt32(bigEndianBitLength);
                    break;
                //case 8:
                //    rawLength = BitConverter.ToInt64(bigEndianBitLength);
                //    break;
                default:
                    throw new ArgumentException();
            }
            string raw = Encoding.ASCII.GetString(
                bytes.Skip(bitlengthByteLength + 1).Take(rawLength).ToArray()
            );
            refPntr += rawLength + bitlengthByteLength + 1;
            BitList encoded = BitList.Decode(bytes.Skip(rawLength + bitlengthByteLength + 1), ref refPntr);

            HuffmanLeaf ret = new HuffmanLeaf();

            ret.encodedValue = encoded;
            ret.value = raw;

            return ret;
        }
        public static IEnumerable<HuffmanLeaf> DecodeAll(IEnumerable<byte> header) {
            int enumPntr = 0;
            return DecodeAll(header, ref enumPntr);
        }

        internal static IEnumerable<HuffmanLeaf> DecodeAll(IEnumerable<byte> header, ref int refPntr) {
            int headerLength = header.Count();
            List<HuffmanLeaf> ret = new List<HuffmanLeaf>();
            while(refPntr < headerLength) {
                ret.Add(HuffmanLeaf.Decode(header.Skip(refPntr), ref refPntr));
            }
            return ret;
        }
    }
}
