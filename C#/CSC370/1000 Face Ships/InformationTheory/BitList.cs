using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InformationTheory {
    //true -> 1, false -> 0
    public class BitList: IEnumerable<bool>, IEnumerable<char> {
        public Guid id = Guid.NewGuid();
        private List<byte> raw = new List<byte>();
        private int _bitLength;

        public BitList() { }
        public BitList(long bitStore, long bitLength) : this() {
            Push(bitLength, bitStore);
        }

        public override bool Equals(object obj) {
            BitList bt = obj as BitList;
            if(bt == null) return false;
            if(bt.BitLength != this.BitLength) return false;
            for(int i = 0; i < BitLength; i++) {
                if(bt[i] != this[i]) return false;
            }
            return true;
        }
        public override int GetHashCode() {
            return raw.Cast<int>().Aggregate((x, y) => x ^ y);
        }
        public override string ToString() {
            string ret = "";
            for(int i = 0; i < BitLength; i++) {
                ret += (this[i] ? "1" : "0");
            }
            return ret;
        }

        public byte[] Raw => raw.ToArray();

        public byte[] Encode {
            get {
                byte[] bigEndianBitLength;

                int bitLength = BitLength;
                byte bitlengthByteLength;
                if(bitLength == (byte)bitLength) {
                    bitlengthByteLength = 1;
                    bigEndianBitLength = new byte[] { (byte)BitLength };
                } else if(bitLength == (short)bitLength) {
                    bitlengthByteLength = 2;
                    bigEndianBitLength = BitConverter.GetBytes((short)BitLength);
                } else {
                    bitlengthByteLength = 4;
                    bigEndianBitLength = BitConverter.GetBytes(BitLength);
                }

                if(BitConverter.IsLittleEndian) {
                    Array.Reverse(bigEndianBitLength);
                }

                return (new byte[] { bitlengthByteLength }).Concat(bigEndianBitLength).Concat(raw).ToArray();
            }
        }
        public static BitList Decode(IEnumerable<byte> bytes) {
            int refPntr = 0;
            return Decode(bytes, ref refPntr);
        }

        internal static BitList Decode(IEnumerable<byte> bytes, ref int refPntr) {
            BitList ret = new BitList();
            int bitlengthByteLength = bytes.First();

            byte[] bigEndianBitLength = bytes.Skip(1).Take(bitlengthByteLength).ToArray();
            if(BitConverter.IsLittleEndian) {
                Array.Reverse(bigEndianBitLength);
            }
            switch(bigEndianBitLength.Length) {
                case 1:
                    ret.BitLength = bigEndianBitLength.First();
                    break;
                case 2:
                    ret.BitLength = BitConverter.ToInt16(bigEndianBitLength);
                    break;
                case 4:
                    ret.BitLength = BitConverter.ToInt32(bigEndianBitLength);
                    break;
                //case 8:
                //    rawLength = BitConverter.ToInt64(bigEndianBitLength);
                //    break;
                default:
                    throw new ArgumentException();
            }

            int takeLength = (ret.BitLength / 8) + ((ret.BitLength % 8) == 0 ? 0 : 1);
            ret.raw = bytes.Skip(bitlengthByteLength + 1).Take(takeLength).ToList();

            refPntr += takeLength + bitlengthByteLength + 1;

            return ret;
        }

        public BitList Reversed() {
            BitList ret = new BitList();
            for(int i = BitLength-1; i >= 0; i--){
                ret.Push(this[i]);
            }
            return ret;
        }

        public bool this[int index] {
            get {
                unchecked {
                    int rawIndex = (int)(index / 8);
                    byte localIndex = (byte)(index % 8);

                    if(rawIndex >= raw.Count) {
                        return false;
                    }

                    byte localRaw = raw[rawIndex];
                    byte localMask = (byte)(1 << (7 - localIndex));

                    return (localRaw & localMask) != 0;
                }
            }
            set {
                unchecked {
                    int rawIndex = (int)(index / 8);
                    byte localIndex = (byte)(index % 8);

                    while(rawIndex >= raw.Count) {
                        raw.Add(0);
                    }

                    byte localRaw = raw[rawIndex];
                    byte localMask = (byte)(1 << (7 - localIndex));

                    if(!value) {
                        localRaw = (byte)(localRaw & ~localMask);
                    } else {
                        localRaw = (byte)(localRaw | localMask);
                    }

                    raw[rawIndex] = localRaw;
                }
            }
        }

        public int BitLength {
            get {
                return _bitLength;
            }
            private set {
                if(value == 0){

                }
                _bitLength = value;
            }
        }

        public void Push(long bitLength, long bitStore) {
            for(int i = (int)bitLength - 1; i >= 0; i--) {
                long bit = bitStore >> i;
                bit &= 1;
                Push(bit != 0);
            }
        }
        public void Push(bool bit) {
            this[BitLength] = bit;
            BitLength++;
        }

        public void Push(BitList v) {
            for(int i = 0; i < v.BitLength; i++) {
                this.Push(v[i]);
            }
        }

        public BitList DeepClone() {
            BitList ret = new BitList();
            ret.Push(this);
            return ret;
        }

        public IEnumerator<bool> GetEnumerator() {
            for(int i = 0; i < BitLength; i++) {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        IEnumerator<char> IEnumerable<char>.GetEnumerator() {
            for(int i = 0; i < BitLength; i++) {
                yield return this[i] ? '1' : '0';
            }
        }

        public void Clear() {
            BitLength = 0;
            raw.Clear();
        }
    }
}
