using InformationTheory.Huffman;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace InformationTheory {
    public class HuffmanEncoding: IEncoder {
        public void Decode(FileInfo textComp, FileInfo textDecomp) {
            byte[] compRaw = new byte[textComp.Length];
            using(Stream r = textComp.OpenRead()){
                int readLength = 0;
                while(readLength < textComp.Length) {
                    readLength += r.Read(compRaw, readLength, compRaw.Length - readLength);
                }
            }

            byte[] headerLengthBytes = compRaw.Take(4).ToArray();
            if(BitConverter.IsLittleEndian) Array.Reverse(headerLengthBytes);
            int headerLength = BitConverter.ToInt32(headerLengthBytes);

            IEnumerable<byte> header = compRaw.Skip(4).Take(headerLength).ToArray();
            IEnumerable<HuffmanLeaf> leaves = HuffmanLeaf.DecodeAll(header);

            IEnumerable<byte> dataRaw = compRaw.Skip(4 + headerLength).ToArray();
            BitList data = BitList.Decode(dataRaw);

            Dictionary<string, HuffmanLeaf> map = leaves.ToDictionary(x => {
                string s = x.EncodedValue.ToString();
                return s;
            });
            string cache = "";
            
            using(Stream s = textDecomp.Exists? textDecomp.Open(FileMode.Truncate): textDecomp.Create()){
                foreach(char c in (IEnumerable<char>)data) {
                    cache += c;
                    if(map.ContainsKey(cache)) {
                        foreach(char c2 in map[cache].value){
                            s.WriteByte((byte)c2);
                        }
                        cache = "";
                    }
                }
                s.Flush();
            }
        }

        void IEncoder.Encode(FileInfo source, FileInfo destination) => Encode(source, destination, -1);
        public void Encode(FileInfo source, FileInfo destination, int maxLength = -1) {
            StringBuilder fileBuilder = new StringBuilder();
            using(Stream s = source.OpenRead()) {
                int read = 0;
                while(read >= 0){
                    read = s.ReadByte();
                    fileBuilder.Append((char)((byte)read));
                }
            }
            string fullFile = fileBuilder.ToString();

            Dictionary<string, int> countTracker;

            if(maxLength <= 0) {
                countTracker = DeepPatternMatch(fullFile, out maxLength);
            } else {
                countTracker = FastPatternMatch(fullFile, maxLength);
            }

            IEnumerable<HuffmanLeaf> allLeaves = 
                countTracker.Select(x => new HuffmanLeaf(x.Key, x.Value))
                            .OrderByDescending(x => x.value.Length);

            RemoveRedundantLeaves(ref allLeaves, fullFile);
            BuildTreeFromLeaves(ref allLeaves);
            
            Func<IEnumerable<HuffmanLeaf>, IEnumerable<HuffmanLeaf>> DeepClone = (y) => {
                return y.Select(x => {
                    HuffmanLeaf l = (HuffmanLeaf)x.DeepClone();
                    l.encodedValue.Clear();
                    return l;
                });
            };

            (int index, MemoryStream stream) min, max, cur;
            MemoryStream stream = new MemoryStream();

            EncodeToStream(fullFile, DeepClone(allLeaves), maxLength, stream, 0);
            min = (0, stream);

            int stringLeaves = allLeaves.Where(x => x.value.Length > 1).Count();
            stream = new MemoryStream();
            EncodeToStream(fullFile, DeepClone(allLeaves), maxLength, stream, stringLeaves);
            max = (stringLeaves, stream);

            while(min.index != max.index){
                int testIndex = (min.index + max.index) / 2;

                if(testIndex == min.index || testIndex == max.index) {
                    break;
                }

                stream = new MemoryStream();
                EncodeToStream(fullFile, DeepClone(allLeaves), maxLength, stream, testIndex);
                cur = (testIndex, stream);

                if(min.stream.Length >= max.stream.Length){
                    min.stream.Dispose();
                    min = cur;
                }else{
                    max.stream.Dispose();
                    max = cur;
                }
            }

            Stream winner = new MemoryStream();
            int start = min.stream.Length > max.stream.Length ? max.index : min.index;

            EncodeToStream(fullFile, DeepClone(allLeaves), maxLength, winner, start);
            max.stream.Dispose();
            min.stream.Dispose();
            winner.Position = 0;

            Stream destinationStream = null;

            if(!destination.Exists) {
                destinationStream = destination.Create();
            } else {
                destinationStream = destination.Open(FileMode.Truncate, FileAccess.Write);
            }

            winner.CopyTo(destinationStream);

            destinationStream.Flush();
            destinationStream.Dispose();
            winner.Dispose();
        }

        private void EncodeToStream(string fullFile, IEnumerable<HuffmanLeaf> allLeaves, int maxLength, Stream writeTo, int takeCount) {
            BuildTreeFromLeaves(ref allLeaves);

            allLeaves = allLeaves.OrderByDescending(x => x.BitsSaved).Where(x => {
                if(x.value.Length == 1) return true;

                takeCount--;
                return takeCount >= 0;
            });
            RemoveRedundantLeaves(ref allLeaves, fullFile);

            IHuffmanNode assembledTree = BuildTreeFromLeaves(ref allLeaves);

            Dictionary<string, HuffmanLeaf> leavesDictionary =
                allLeaves.ToDictionary(x => x.value);
            BitList encodedFile = new BitList();

            int[] lengths = allLeaves   .Select(x => x.value.Length)
                                        .Distinct()
                                        .OrderByDescending(x => x)
                                        .ToArray();

            #region encode file contents
            for(int stringPntr = 0; stringPntr < fullFile.Length;) {
                for(int i = 0; i < lengths.Length; i++) {
                    int len = lengths[i];
                    if((stringPntr + len) > fullFile.Length) {
                        continue;
                    }

                    string sub = fullFile.Substring(stringPntr, len);
                    if(leavesDictionary.ContainsKey(sub)) {
                        BitList v = leavesDictionary[sub].EncodedValue;
                        stringPntr += sub.Length;

                        encodedFile.Push(v);

                        break;
                    }
                }
            }
            #endregion

            HuffmanLeaf[] encodingLeaves = leavesDictionary.Select(x => x.Value).ToArray();
            byte[] serializedTree = encodingLeaves.SelectMany(x => x.Encode).ToArray();
            
            byte[] bigEndianTreeLength = BitConverter.GetBytes(serializedTree.Length);
            if(BitConverter.IsLittleEndian) {
                Array.Reverse(bigEndianTreeLength);
            }

            #region write to stream
            writeTo.Write(bigEndianTreeLength, 0, bigEndianTreeLength.Length);
            writeTo.Write(serializedTree, 0, serializedTree.Length);
            byte[] encodedBitList_Raw = encodedFile.Encode;
            writeTo.Write(encodedBitList_Raw, 0, encodedBitList_Raw.Length);
            #endregion
        }

        private void RemoveRedundantLeaves(ref IEnumerable<HuffmanLeaf> allLeaves, string fileCopy) {
            StringBuilder builder = new StringBuilder(fileCopy);
            Dictionary<string, HuffmanLeaf> usefulLeaves = new Dictionary<string, HuffmanLeaf>();// usefulLeaves.ToDictionary(x => x.value);
            
            foreach(HuffmanLeaf leaf in allLeaves) {
                if(builder.Length <= 0) break;
                
                int count = Extentions.CountAndRemoveOccurences(ref builder, leaf.value);
                if(count > 1 || (count > 0 && leaf.value.Length == 1)) {
                    if(!usefulLeaves.ContainsKey(leaf.value)) {
                        usefulLeaves.Add(leaf.value, leaf);
                        usefulLeaves[leaf.value].MatchCount = count;
                    }else{
                        usefulLeaves[leaf.value].MatchCount += count;
                    }
                }
            }
            if(fileCopy.Length > 0) {
                foreach(char c in fileCopy) {
                    string s = c.ToString();
                    if(usefulLeaves.ContainsKey(s)) {
                        usefulLeaves[s].MatchCount++;
                    } else {
                        usefulLeaves.Add(s, new HuffmanLeaf(s, 1));
                    }
                    fileCopy = fileCopy.Replace(s, "");
                }
            }
            allLeaves = usefulLeaves.Select(x => x.Value);
        }

        private IHuffmanNode BuildTreeFromLeaves(ref IEnumerable<HuffmanLeaf> allLeaves) {
            foreach(var v in allLeaves){ 
                v.encodedValue.Clear();
            }
            
            List<IHuffmanNode> sortedNodes = allLeaves.OrderBy(x => x.MatchCount).Cast<IHuffmanNode>().ToList();
            List<IHuffmanNode> toAdd = new List<IHuffmanNode>();
            while(sortedNodes.Count > 1) {
                int minCombo = int.MaxValue;
                while(sortedNodes.Count > 1) {
                    IHuffmanNode a = sortedNodes[0];
                    IHuffmanNode b = sortedNodes[1];

                    if(a.MatchCount > minCombo || b.MatchCount > minCombo){
                        break;
                    }

                    HuffmanTree n = new HuffmanTree(a, b);
                    if(n.MatchCount < minCombo){
                        minCombo = n.MatchCount;
                    }
                    toAdd.Add(n);
                    
                    sortedNodes.RemoveAt(0);
                    sortedNodes.RemoveAt(0);
                }
                sortedNodes.AddRange(toAdd);
                toAdd.Clear();
                //TODO: insert over sort
                sortedNodes.Sort((x, y) => Math.Sign(x.MatchCount - y.MatchCount));
            }

            foreach(var x in allLeaves){
                x.EncodedValue.ToString();
            }
            
            return sortedNodes[0];
        }

        private Dictionary<string, int> FastPatternMatch(string fullFile, int maxLength) {
            List<Dictionary<string, int>> matches = new List<Dictionary<string, int>>();
            for(int i = 1; i <= maxLength; i++) {
                matches.Add(PatternMatch(fullFile, i));
            }

            Dictionary<string, int> ret = new Dictionary<string, int>();
            foreach(Dictionary<string, int> d in matches) {
                foreach(var v in d) {
                    if(v.Value > 1) {
                        ret.Add(v.Key, v.Value);
                    }
                }
            }

            return ret;
        }

        private Dictionary<string, int> DeepPatternMatch(string fullFile, out int maxLength, Func<string, int, bool> validate = null) {
            if(validate == null) {
                validate = (str, cnt) => {
                    return cnt > 1;
                };
            }

            List<Dictionary<string, int>> matches = new List<Dictionary<string, int>>();
            maxLength = 1;
            Dictionary<string, int> tmp = null;
            do {
                if(tmp != null) {
                    matches.Add(tmp);
                }
                tmp = PatternMatch(fullFile, maxLength);
                maxLength++;
            } while(tmp.Where(x => validate(x.Key, x.Value)).Any());
            maxLength--;

            Dictionary<string, int> ret = new Dictionary<string, int>();
            //Parallel.ForEach(matches, (d) => {
            //    Parallel.ForEach(d, (v) => {
            //        if(v.Value > 1) {
            //            ret.Add(v.Key, v.Value);
            //        }
            //    });
            //});
            foreach(Dictionary<string, int> d in matches) {
                foreach(var v in d) {
                    if(validate(v.Key, v.Value)) {
                        ret.Add(v.Key, v.Value);
                    }
                }
            }

            return ret;
        }

        private Dictionary<string, int> PatternMatch(string fullFile, int length) {
            Dictionary<string, int> ret = new Dictionary<string, int>();
            for(int i = 0; i < fullFile.Length - length; i++) {
                string sub = fullFile.Substring(i, length);
                if(ret.ContainsKey(sub)) {
                    ret[sub] += 1;
                } else {
                    ret.Add(sub, 1);
                }
            }
            return ret;
        }
    }
}
