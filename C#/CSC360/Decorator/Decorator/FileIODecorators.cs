using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.Runtime.Remoting;
using System.Threading;

namespace Decorator {
    
    
    public sealed class WindowsToUnixLineEndingFileIn: FileIn {
        public WindowsToUnixLineEndingFileIn(string write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(Stream write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(StreamReader write) : base(write) { }
        public WindowsToUnixLineEndingFileIn(FileIn write) : base(write) { }

        public override string Decorate(string str) {
            return str.Replace("\r\n", "\n");
        }
    }
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
