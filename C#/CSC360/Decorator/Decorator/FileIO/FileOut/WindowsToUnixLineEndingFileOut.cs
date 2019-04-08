using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileOut {
    public sealed class WindowsToUnixLineEndingFileOut: FileOut {
        public WindowsToUnixLineEndingFileOut(string write) : base(write) { }
        public WindowsToUnixLineEndingFileOut(Stream write) : base(write) { }
        public WindowsToUnixLineEndingFileOut(FileOut write) : base(write) { }

        public override string Decorate(string str) {
            return str.Replace("\r\n", "\n");
        }
    }
}
