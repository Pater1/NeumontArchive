using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decorator.FileIO {
    public static class FileIOHelper {
        public static void FileConvert(FileIn.FileIn inp, FileOut.FileOut outp) {
            string dat = inp.Read();
            outp.Write(dat);
        }
    }
}
