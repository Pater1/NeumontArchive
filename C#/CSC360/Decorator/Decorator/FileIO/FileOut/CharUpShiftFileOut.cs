using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileOut {
    public sealed class CharUpShiftFileOut: FileOut {
        public CharUpShiftFileOut(string write) : base(write) { }
        public CharUpShiftFileOut(Stream write) : base(write) { }
        public CharUpShiftFileOut(FileOut write) : base(write) { }

        public override string Decorate(string str) {
            return new string(
                str.Select(x => {
                    if(char.IsLetter(x)) {
                        switch(x) {
                            case 'z':
                                return 'a';
                            case 'Z':
                                return 'A';
                            default:
                                return (char)(x + 1);
                        }
                    } else {
                        return x;
                    }
                }).ToArray()
            );
        }
    }
}
