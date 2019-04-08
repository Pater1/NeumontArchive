using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileIn {
    public sealed class UnixToWindowsLineEndingFileIn: FileIn {
        public UnixToWindowsLineEndingFileIn(string write) : base(write) { }
        public UnixToWindowsLineEndingFileIn(Stream write) : base(write) { }
        public UnixToWindowsLineEndingFileIn(StreamReader write) : base(write) { }
        public UnixToWindowsLineEndingFileIn(FileIn write) : base(write) { }

        public override string Decorate(string str) {
            return str.Replace("\n", "\r\n");
        }
    }
}
