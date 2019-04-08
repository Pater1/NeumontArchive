using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets {
    [Flags]
    public enum GrammarComponent: int {
        Unknown = ~0,

        City = 1 << 0,
        Number = 1 << 1,
        Unit = 1 << 2,

        Distance = Number | Unit,

        Connect = 1 << 3,
        Path = 1 << 4,
        
        PathCommand = City | Path,

        PathFindCommand = 1 << 5,
        ConnectionCommand = Connect | City | Distance
    }
}
