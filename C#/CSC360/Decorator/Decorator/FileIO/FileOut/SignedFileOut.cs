using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileOut {
    public sealed class SignedFileOut: FileOut {
        string name;

        public SignedFileOut(string name, string write) : base(write) {
            this.name = name;
        }
        public SignedFileOut(string name, Stream write) : base(write) {
            this.name = name;
        }
        public SignedFileOut(string name, FileOut write) : base(write) {
            this.name = name;
        }

        public override string Decorate(string str) {
            return $"{str}{Environment.NewLine}{name}, {System.DateTime.Now.ToLongDateString()}";
        }
    }
}
