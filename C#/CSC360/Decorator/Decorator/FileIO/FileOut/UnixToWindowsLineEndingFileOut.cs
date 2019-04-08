using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileOut {
    public sealed class UnixToWindowsLineEndingFileOut: FileOut {
        public UnixToWindowsLineEndingFileOut(string write) : base(write) { }
        public UnixToWindowsLineEndingFileOut(Stream write) : base(write) { }
        public UnixToWindowsLineEndingFileOut(FileOut write) : base(write) { }

        public override string Decorate(string str) {
            return str.Replace("\n", "\r\n");
        }
    }
}
