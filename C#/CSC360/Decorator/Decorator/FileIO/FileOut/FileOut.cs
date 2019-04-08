using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileOut {
    public class FileOut: IDisposable {
        Stream outs = null;
        FileOut chain = null;
        public FileOut(string write) : this(new FileStream(write, FileMode.Create)) { }
        public FileOut(Stream write) {
            outs = write;
        }
        public FileOut(FileOut write) {
            chain = write;
        }

        public virtual string Decorate(string str) => str;
        public void Write(string str) {
            str = Decorate(str);
            if(chain != null) {
                chain.Write(str);
            } else {
                byte[] data = str.Select(x => (byte)x).ToArray();
                outs.Write(data, 0, data.Length);
            }
        }
        public void Dispose() {
            outs.Dispose();
        }
    }
}
