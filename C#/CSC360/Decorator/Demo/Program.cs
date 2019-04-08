using Decorator;
using Decorator.FileIO;
using Decorator.FileIO.FileIn;
using Decorator.FileIO.FileOut;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo {
    class Program {
        static void Main(string[] args) {
            FileIn fileIn = new CharDownShiftFileIn(@"C:\Users\Patrick Conner\Desktop\usdeclar.txt");
            FileOut fileOut = new SignedFileOut("Patrick Conner",
                                new WindowsToUnixLineEndingFileOut(
                                    new CharUpShiftFileOut(@"C:\Users\Patrick Conner\Desktop\usdeclar_conv.txt")
                                )
                            );
            FileIOHelper.FileConvert(fileIn , fileOut);
        }
    }
}
