using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO.FileIn {
    public sealed class CharDownShiftFileIn: FileIn {
        public CharDownShiftFileIn(string write) : base(write) { }
        public CharDownShiftFileIn(Stream write) : base(write) { }
        public CharDownShiftFileIn(StreamReader write) : base(write) { }
        public CharDownShiftFileIn(FileIn write) : base(write) { }

        public override string Decorate(string str) {
            return new string(
                str.Select(x => {
                    if(char.IsLetter(x)) {
                        switch(x) {
                            case 'a':
                                return 'z';
                            case 'A':
                                return 'Z';
                            default:
                                return (char)(x - 1);
                        }
                    } else {
                        return x;
                    }
                }).ToArray()
            );
        }
    }
}
