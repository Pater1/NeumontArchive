using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileIn {
    public sealed class WindowsToUnixLineEndingFileIn: FileIn {
        public WindowsToUnixLineEndingFileIn(string write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(Stream write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(StreamReader write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(FileIn write) : base(write) { }

        public override string Decorate(string str) {
            return str.Replace("\r\n", "\n");
        }
    }
}
