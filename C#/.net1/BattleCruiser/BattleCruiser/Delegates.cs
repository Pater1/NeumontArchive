using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCruiser {
    public delegate void OnTileAction(int x, int y);
    public delegate void OnTilesetAction(BoardTile[] tiles);
}
