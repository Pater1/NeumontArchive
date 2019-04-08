using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator
{
    public abstract class Logger{
        private Logger child;
        public Logger(Logger log){
            child = log;
        }
        public virtual string Log(string str){
            if(child != null) {
                return child.Log(str);
            }else{
                return "";
            }
        }
    }
    public class LogOut: Logger {
        StreamWriter fs;
        public LogOut(string log) : base(null) {
            fs = new StreamWriter(new FileStream("log", FileMode.Append));
        }
        public override string Log(string str) {
            fs.WriteLine(str);
            return "";
        }
    }

    public class CharUpShift: Logger {
        StreamWriter fs;
        public CharUpShift(Logger log) : base(log) { }
        public override string Log(string str) {
            return base.Log(
                new string(
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
                )
            );
        }
    }
    public class CharDownShift: Logger {
        StreamWriter fs;
        public CharDownShift(Logger log) : base(log) { }
        public override string Log(string str) {
            return base.Log(
                new string(
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
                )
            );
        }
    }
}
