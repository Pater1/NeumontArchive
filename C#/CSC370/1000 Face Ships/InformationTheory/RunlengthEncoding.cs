using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace InformationTheory {
    public class RunlengthEncoding: IEncoder {
        public void Decode(FileInfo source, FileInfo destination) {
            using(Stream s = source.OpenRead()) {
                using(Stream d = destination.Exists ? destination.Open(FileMode.Truncate) : destination.Create()) {
                    Decode(s, d);
                }
            }
        }
        public void Encode(FileInfo source, FileInfo destination) {
            using(Stream s = source.OpenRead()){
                using(Stream d = destination.Exists? destination.Open(FileMode.Truncate): destination.Create()){
                    Encode(s, d);
                }
            }
        }

        public void Decode(Stream source, Stream destination) {
            int encodeLength = source.ReadByte();
            int write = 0;
            do {
                write = source.ReadByte();
                if(write < 0) break;

                long writeLength = 0;
                byte[] count = new byte[encodeLength];
                source.Read(count, 0, count.Length);
                if(BitConverter.IsLittleEndian) {
                    Array.Reverse(count);
                }
                switch(encodeLength) {
                    case 1:
                        writeLength = count.First();
                        break;
                    case 2:
                        writeLength = BitConverter.ToInt16(count);
                        break;
                    case 4:
                        writeLength = BitConverter.ToInt32(count);
                        break;
                    case 8:
                        writeLength = BitConverter.ToInt64(count);
                        break;
                    default:
                        throw new Exception("number parsing error");
                }

                for(long i = 0; i < writeLength; i++){
                    destination.WriteByte((byte)write);
                }
            } while(write >= 0);
        }
        public void Encode(Stream source, Stream destination) {
            List<(byte val, long count)> valueCount = new List<(byte val, long count)>();
            byte[] buffer = new byte[512];
            int read = 0;
            (byte val, long count) cur = (0, 0);
            while((read = source.Read(buffer, 0, buffer.Length)) > 0) {
                for(int i = 0; i < read; i++) {
                    if(buffer[i] == cur.val) {
                        cur.count++;
                    } else {
                        if(cur.val > 0) {
                            valueCount.Add(cur);
                        }
                        cur = (buffer[i], 1);
                    }
                }
            }

            long maxCount = valueCount.Select(x => x.count).Max();

            byte encodeLength;
            if(maxCount == (byte)maxCount) {
                encodeLength = 1;
            } else if(maxCount == (short)maxCount) {
                encodeLength = 2;
            } else if(maxCount == (int)maxCount) {
                encodeLength = 4;
            } else {
                encodeLength = 8;
            }

            destination.WriteByte(encodeLength);

            foreach(var v in valueCount){
                destination.WriteByte(v.val);
                byte[] count;
                switch(encodeLength) {
                    case 1:
                        count = new byte[] { (byte)v.count };
                        break;
                    case 2:
                        count = BitConverter.GetBytes((short)v.count);
                        break;
                    case 4:
                        count = BitConverter.GetBytes((int)v.count);
                        break;
                    case 8:
                        count = BitConverter.GetBytes((long)v.count);
                        break;
                    default:
                        throw new Exception("number parsing error");
                }
                if(BitConverter.IsLittleEndian){
                    Array.Reverse(count);
                }
                destination.Write(count, 0, count.Length);
            }
        }
    }
}
