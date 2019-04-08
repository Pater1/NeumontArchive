using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InformationTheory {
    public interface IEncoder {
        void Decode(FileInfo source, FileInfo destination);
        void Encode(FileInfo source, FileInfo destination);
    }
}
