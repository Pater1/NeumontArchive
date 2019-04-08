using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileIn {
    public class FileIn: IDisposable {
        StreamReader ins = null;
        FileIn chain = null;
        public FileIn(string read) : this(new FileStream(read, FileMode.Open)) { }
        public FileIn(Stream read) : this(new StreamReader(read)) { }
        public FileIn(StreamReader read) {
            ins = read;
        }
        public FileIn(FileIn read) {
            chain = read;
        }

        public virtual string Decorate(string str) => str;
        public string Read() {
            string str = "";
            if(chain != null) {
                str = chain.Read();
            } else {
                str = ins.ReadToEnd();
            }
            return Decorate(str);
        }
        public void Dispose() {
            ins.Dispose();
        }
    }
}
