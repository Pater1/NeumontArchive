using System;
using System.Collections.Generic;
using System.Linq;

namespace AddingTuringMachine {
    class Program {
        public const bool left = true;
        public const bool right = false;
        public class TuringTransition {
            public string from, to;
            public char? see, write;
            public bool moveLeft;

            public TuringTransition(string from, string to, char? see, char? write, bool moveLeft) {
                this.from = from;
                this.to = to;
                this.see = see;
                this.write = write;
                this.moveLeft = moveLeft;
            }
        }
        private static IEnumerable<TuringTransition> transtions = new TuringTransition[]{
            new TuringTransition("Start", "A", '=', '=', right),

            new TuringTransition("A", "A", '0', '0', right),
            new TuringTransition("A", "A", '1', '1', right),
            new TuringTransition("A", "A", '2', '2', right),
            new TuringTransition("A", "A", '3', '3', right),
            new TuringTransition("A", "A", '4', '4', right),
            new TuringTransition("A", "A", '5', '5', right),
            new TuringTransition("A", "A", '6', '6', right),
            new TuringTransition("A", "A", '7', '7', right),
            new TuringTransition("A", "A", '8', '8', right),
            new TuringTransition("A", "A", '9', '9', right),
            new TuringTransition("A", "B", '+', '+', right),

            new TuringTransition("B", "B", '0', '0', right),
            new TuringTransition("B", "B", '1', '1', right),
            new TuringTransition("B", "B", '2', '2', right),
            new TuringTransition("B", "B", '3', '3', right),
            new TuringTransition("B", "B", '4', '4', right),
            new TuringTransition("B", "B", '5', '5', right),
            new TuringTransition("B", "B", '6', '6', right),
            new TuringTransition("B", "B", '7', '7', right),
            new TuringTransition("B", "B", '8', '8', right),
            new TuringTransition("B", "B", '9', '9', right),
            new TuringTransition("B", "C", null, null, left),

            new TuringTransition("C", "I", '0', '0', left),
            new TuringTransition("C", "D", '1', '0', left),
            new TuringTransition("C", "D", '2', '1', left),
            new TuringTransition("C", "D", '3', '2', left),
            new TuringTransition("C", "D", '4', '3', left),
            new TuringTransition("C", "D", '5', '4', left),
            new TuringTransition("C", "D", '6', '5', left),
            new TuringTransition("C", "D", '7', '6', left),
            new TuringTransition("C", "D", '8', '7', left),
            new TuringTransition("C", "D", '9', '8', left),

            new TuringTransition("D", "D", '0', '0', left),
            new TuringTransition("D", "D", '1', '1', left),
            new TuringTransition("D", "D", '2', '2', left),
            new TuringTransition("D", "D", '3', '3', left),
            new TuringTransition("D", "D", '4', '4', left),
            new TuringTransition("D", "D", '5', '5', left),
            new TuringTransition("D", "D", '6', '6', left),
            new TuringTransition("D", "D", '7', '7', left),
            new TuringTransition("D", "D", '8', '8', left),
            new TuringTransition("D", "D", '9', '9', left),
            new TuringTransition("D", "E", '+', '+', left),

            new TuringTransition("E", "A", '0', '1', right),
            new TuringTransition("E", "A", '1', '2', right),
            new TuringTransition("E", "A", '2', '3', right),
            new TuringTransition("E", "A", '3', '4', right),
            new TuringTransition("E", "A", '4', '5', right),
            new TuringTransition("E", "A", '5', '6', right),
            new TuringTransition("E", "A", '6', '7', right),
            new TuringTransition("E", "A", '7', '8', right),
            new TuringTransition("E", "A", '8', '9', right),
            new TuringTransition("E", "E", '9', '0', left),
            new TuringTransition("E", "E", '=', '1', left),
            new TuringTransition("E", "A", null, '=', right),

            new TuringTransition("I", "I", '0', '0', left),
            new TuringTransition("I", "Cleanup", '+', null, right),
            new TuringTransition("I", "H", '1', '0', right),
            new TuringTransition("I", "H", '2', '1', right),
            new TuringTransition("I", "H", '3', '2', right),
            new TuringTransition("I", "H", '4', '3', right),
            new TuringTransition("I", "H", '5', '4', right),
            new TuringTransition("I", "H", '6', '5', right),
            new TuringTransition("I", "H", '7', '6', right),
            new TuringTransition("I", "H", '8', '7', right),
            new TuringTransition("I", "H", '9', '8', right),

            new TuringTransition("H", "H", '0', '9', right),
            new TuringTransition("H", "D", null, null, left),

            new TuringTransition("Cleanup", "Cleanup", '0', null, right),
            new TuringTransition("Cleanup", "WalkBack", null, null, left),

            new TuringTransition("WalkBack", "WalkBack", null, null, left),
            new TuringTransition("WalkBack", "WalkBack", '+', null, left),
            new TuringTransition("WalkBack", "WalkBack", '0', '0', left),
            new TuringTransition("WalkBack", "WalkBack", '1', '1', left),
            new TuringTransition("WalkBack", "WalkBack", '2', '2', left),
            new TuringTransition("WalkBack", "WalkBack", '3', '3', left),
            new TuringTransition("WalkBack", "WalkBack", '4', '4', left),
            new TuringTransition("WalkBack", "WalkBack", '5', '5', left),
            new TuringTransition("WalkBack", "WalkBack", '6', '6', left),
            new TuringTransition("WalkBack", "WalkBack", '7', '7', left),
            new TuringTransition("WalkBack", "WalkBack", '8', '8', left),
            new TuringTransition("WalkBack", "WalkBack", '9', '9', left),
            new TuringTransition("WalkBack", "Done", '=', '=', left),
        };
        public class Tape {
            char?[] writtenMem = new char?[0];
            int zeroPntr = 0;

            public Tape(IEnumerable<char?> start){
                writtenMem = start.ToArray();
            }

            public override string ToString() => new string(
                                                    writtenMem  .Select(x => x.HasValue ? x.Value : 'ɛ')
                                                                .SkipWhile(x => x == 'ɛ')
                                                                .Reverse()
                                                                .SkipWhile(x => x == 'ɛ')
                                                                .Reverse()
                                                                .ToArray());
                                                                
            public char? this[int index]{
                get{
                    index += zeroPntr;
                    if(index < 0 || index >= writtenMem.Length){
                        return null;
                    }else{
                        return writtenMem[index];
                    }
                }
                set {
                    index += zeroPntr;
                    if(index < 0) {
                        int delta = -index;
                        zeroPntr += delta;
                        char?[] newMem = new char?[writtenMem.Length + delta + 1];
                        for(int i = 0; i < writtenMem.Length; i++){
                            newMem[i + delta] = writtenMem[i];
                        }
                        newMem[0] = value;
                        writtenMem = newMem;
                    } else if(index >= writtenMem.Length) {
                        char?[] newMem = new char?[index + zeroPntr + 1];
                        for(int i = 0; i < writtenMem.Length; i++) {
                            newMem[i] = writtenMem[i];
                        }
                        newMem[newMem.Length -1] = value;
                        writtenMem = newMem;
                    } else {
                        writtenMem[index] = value;
                    }
                }
            }
        }
        static void Main(string[] args) {
            string input = ""; 
            do {
                string curState = "Start";
                int pntr = 0;
                input = Console.ReadLine();

                Tape t = new Tape(input.ToCharArray().Select(x => (char?)x));
                TuringTransition activeTransition = null;
                do {
                    activeTransition = transtions.Where(x => x.from == curState).Where(x => x.see == t[pntr]).SingleOrDefault();
                    
                    if(activeTransition != null) {
                        curState = activeTransition.to;
                        t[pntr] = activeTransition.write;
                        pntr += activeTransition.moveLeft ? -1 : 1;
                    }
                } while(activeTransition != null);

                Console.WriteLine(t.ToString());
                Console.WriteLine();
            } while(input != "exit");
        }
    }
}
