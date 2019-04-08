using InformationTheory.Huffman;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace InformationTheory {
    class Program {
        const string textFile = "testDoc.txt";
        const string imgFile = "4KTest.jpg";

        static void Main(string[] args) {
            bool[] waitLock = new bool[4];
            ThreadPool.QueueUserWorkItem((x) => {
                waitLock[0] = false;

                IEncoder encoder = new HuffmanEncoding();
                string file = textFile,
                        extention = file.Split('.').Last(),
                        encode = encoder.GetType().Name.Replace("Encoding", "").ToLowerInvariant();

                FileInfo textRaw = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}");
                FileInfo textComp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}");
                FileInfo textDecomp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}.decode.{extention}");

                Console.WriteLine("Huffman ecoding text file, start");
                encoder.Encode(textRaw, textComp);
                Console.WriteLine("Huffman ecoding text file, finish");

                textComp.Refresh();

                Console.WriteLine("Huffman decoding text file, start");
                encoder.Decode(textComp, textDecomp);
                Console.WriteLine("Huffman decoding text file, finish");

                waitLock[0] = true;
            });
            ThreadPool.QueueUserWorkItem((x) => {
                waitLock[1] = false;
                IEncoder encoder = new RunlengthEncoding();
                string file = textFile,
                        extention = file.Split('.').Last(),
                        encode = encoder.GetType().Name.Replace("Encoding", "").ToLowerInvariant();

                FileInfo textRaw = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}");
                FileInfo textComp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}");
                FileInfo textDecomp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}.decode.{extention}");

                Console.WriteLine("Runlength ecoding text file, start");
                encoder.Encode(textRaw, textComp);
                Console.WriteLine("Runlength ecoding text file, finish");

                textComp.Refresh();

                Console.WriteLine("Runlength decoding text file, start");
                encoder.Decode(textComp, textDecomp);
                Console.WriteLine("Runlength decoding text file, finish");

                waitLock[1] = true;
            });

            ThreadPool.QueueUserWorkItem((x) => {
                waitLock[2] = false;
                IEncoder encoder = new HuffmanEncoding();
                string file = imgFile,
                        extention = file.Split('.').Last(),
                        encode = encoder.GetType().Name.Replace("Encoding", "").ToLowerInvariant();

                FileInfo textRaw = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}");
                FileInfo textComp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}");
                FileInfo textDecomp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}.decode.{extention}");

                Console.WriteLine("Huffman ecoding image file, start");
                encoder.Encode(textRaw, textComp);
                Console.WriteLine("Huffman ecoding image file, finish");

                textComp.Refresh();

                Console.WriteLine("Huffman decoding image file, start");
                encoder.Decode(textComp, textDecomp);
                Console.WriteLine("Huffman decoding image file, finish");

                waitLock[2] = true;
            });
            ThreadPool.QueueUserWorkItem((x) => {
                waitLock[3] = false;
                IEncoder encoder = new RunlengthEncoding();
                string file = imgFile,
                        extention = file.Split('.').Last(),
                        encode = encoder.GetType().Name.Replace("Encoding", "").ToLowerInvariant();

                FileInfo textRaw = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}");
                FileInfo textComp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}");
                FileInfo textDecomp = new FileInfo($@"C:\Users\Patrick Conner\Desktop\{file}.{encode}.decode.{extention}");

                Console.WriteLine("Runlength ecoding image file, start");
                encoder.Encode(textRaw, textComp);
                Console.WriteLine("Runlength ecoding image file, finish");

                textComp.Refresh();

                Console.WriteLine("Runlength decoding image file, start");
                encoder.Decode(textComp, textDecomp);
                Console.WriteLine("Runlength decoding image file, finish");

                waitLock[3] = true;
            });

            for(int i = 0; i < waitLock.Length; i++){
                waitLock[i] = false;
            }

            do {
                Thread.Sleep(50);
            } while(!waitLock.Aggregate((x, y) => x && y));
        }
    }
}
