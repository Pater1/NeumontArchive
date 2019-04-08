using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace BattleCruiser {
    //Each 4-bit byte of the int is used to encode different parts of the state
    //bits 0-3 encode game state
    //bits 4-7 encode overlay status (used for mouse feedback)

    public enum TileState: int {
        Null = 0,

        Open = 1,
        Miss = 2,
        Hit = 3,
        Ship = 4,

        NoHover = (0 << 4), //pruposeful collision with Null state
        ShootHover = (1 << 4),
        ShipHover = (2 << 4),
        ShootClick = (3 << 4),
        ShipClick = (4 << 4),
    }
    public static class TileStateExtentions {
        private static readonly Dictionary<TileState, Color> colorMap = new Dictionary<TileState, Color>() {
            {TileState.Null, Colors.Transparent},

            {TileState.Open, Color.FromScRgb(1f,0.25f,0.25f,1)},
            {TileState.Miss, Colors.White},
            {TileState.Hit, Colors.Red},
            {TileState.Ship, Colors.Gray},

            {TileState.ShootHover, Color.FromScRgb(0.5f,0.75f,0.25f,0.25f) },
            {TileState.ShipHover, Color.FromScRgb(0.5f,0.5f,0.5f,0.5f)},
            {TileState.ShootClick, Color.FromScRgb(0.75f,0.75f,0.5f,0.5f) },
            {TileState.ShipClick, Color.FromScRgb(0.75f,0.75f,0.75f,0.75f)},
        };
        public static Color GetColor(this TileState state) {
            int i = (int)state;
            i = i & 7;
            state = (TileState)(i);
            return colorMap[state];
        }
        public static Color GetOverlay(this TileState state) {
            int i = (int)state;
            i = i >> 4;
            i = i & 0xF;
            i = i << 4;
            state = (TileState)i;
            return colorMap[state];
        }
    }
}
