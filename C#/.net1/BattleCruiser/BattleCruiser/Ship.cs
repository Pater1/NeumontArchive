using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BattleCruiser {
    public class Ship: IXmlSerializable {
        public static Ship Carrier() {
            return new Ship(5, "Carrier");
        }
        public static Ship Battleship() {
            return new Ship(4, "Battleship");
        }
        public static Ship Cruiser() {
            return new Ship(3, "Cruiser");
        }
        public static Ship Submarine() {
            return new Ship(3, "Submarine");
        }
        public static Ship Destroyer() {
            return new Ship(2, "Destroyer");
        }

        private Ship(int length, string name) {
            Length = length;
            Name = name;
        }
        private Ship() {}

        private int Length {
            get;
            set;
        }
        private string Name {
            get;
            set;
        } = "";
        public Rotation Rot {
            get;
            set;
        }

        public bool Placed {
            get {
                if(myTileIndexes != null && myTileIndexes.GetLength(1) == Length) {
                    foreach (BoardTile b in MyTiles) {
                        if (b == null) return false;
                    }
                    return true;
                }
                return false;
            }
        }

        private int[,] myTileIndexes = new int[0,0];
        public int[,] MyTileIndexes {
            get {
                return myTileIndexes;
            }
        }
        public BoardTile[] MyTiles {
            get {
                if(myTileIndexes.Length <= 0) {
                    return new BoardTile[0];
                }

                BoardTile[] myOldTiles = new BoardTile[Length];
                for (int i = 0; i < myTileIndexes.GetLength(1); i++) {
                    myOldTiles[i] = ImOn.GameBoard[MyTileIndexes[0, i], MyTileIndexes[1, i]];
                }
                return myOldTiles;
            }
        }
        public void ClearTiles() {
            if (onLooseTiles != null)onLooseTiles(MyTiles);
            if (myTileIndexes != null) {
                foreach (BoardTile tile in MyTiles) {
                    tile.Top = 2.0;
                    tile.Bottom = 2.0;
                    tile.Left = 2.0;
                    tile.Right = 2.0;
                }
            }
            myTileIndexes = null;
        }
        public event OnTilesetAction onLooseTiles;
        public event OnTilesetAction onGainTiles;

        private int X { get; set; }
        private int Y { get; set; }

        public SeaBoard ImOn { get; set; }
        public bool IsSunk {
            get {
                foreach(BoardTile b in MyTiles) {
                    if (((int)b.State & 15) != (int)TileState.Hit) return false;
                }
                return true;
            }
        }

        public bool TryPlaceShip(out BoardTile[] tiles, SeaBoard on, int x, int y, bool preveiw = true) {
            tiles = new BoardTile[Length];
            ImOn = on;
            switch (Rot) {
                case Rotation.Down:
                    if (x + Length > 10) {
                        return false;
                    }
                    for (int i = 0; i < Length; i++) {
                        tiles[i] = on.GameBoard[x+i, y];

                        int state = ((int)tiles[i].State & 15);
                        if (state == (int)TileState.Ship && (myTileIndexes == null || !MyTiles.Contains(tiles[i]))) {
                            return false;
                        }
                    }
                    break;
                case Rotation.Up:
                    if (x - Length < -1) {
                        return false;
                    }
                    for (int i = 0; i < Length; i++) {
                        tiles[i] = on.GameBoard[x-i, y];

                        int state = ((int)tiles[i].State & 15);
                        if (state == (int)TileState.Ship && (myTileIndexes == null || !MyTiles.Contains(tiles[i]))) {
                            return false;
                        }
                    }
                    break;
                case Rotation.Left:
                    if (y + Length > 10) {
                        return false;
                    }
                    for (int i = 0; i < Length; i++) {
                        tiles[i] = on.GameBoard[x, y+i];

                        int state = ((int)tiles[i].State & 15);
                        if (state == (int)TileState.Ship && (myTileIndexes == null || !MyTiles.Contains(tiles[i]))) {
                            return false;
                        }
                    }
                    break;
                case Rotation.Right:
                    if (y - Length < -1) {
                        return false;
                    }
                    for (int i = 0; i < Length; i++) {
                        tiles[i] = on.GameBoard[x, y-i];

                        int state = ((int)tiles[i].State & 15);
                        if (state == (int)TileState.Ship && (myTileIndexes == null || !MyTiles.Contains(tiles[i]))) {
                            return false;
                        }
                    }
                    break;
            }

            if (!preveiw) {
                if(myTileIndexes != null && onLooseTiles != null) {
                    onLooseTiles(MyTiles);
                    foreach(BoardTile tile in MyTiles) {
                        tile.Top = 2.0;
                        tile.Bottom = 2.0;
                        tile.Left = 2.0;
                        tile.Right = 2.0;
                    }
                }
                myTileIndexes = new int[2, Length];
                for(int i = 0; i < Length; i++) {
                    myTileIndexes[0, i] = tiles[i].X;
                    myTileIndexes[1, i] = tiles[i].Y;
                }
                if (onGainTiles != null) {
                    onGainTiles(tiles);
                    switch (Rot) {
                        case Rotation.Down:
                            for (int i = 0; i < Length; i++) {
                                if (i != 0) {
                                    tiles[i].Top = 0;
                                }
                                if(i != Length - 1) {
                                    tiles[i].Bottom = 0;
                                }
                            }
                            break;
                        case Rotation.Up:
                            for (int i = 0; i < Length; i++) {
                                if (i != 0) {
                                    tiles[i].Bottom = 0;
                                }
                                if (i != Length - 1) {
                                    tiles[i].Top = 0;
                                }
                            }
                            break;
                        case Rotation.Left:
                            for (int i = 0; i < Length; i++) {
                                if (i != 0) {
                                    tiles[i].Left = 0;
                                }
                                if (i != Length - 1) {
                                    tiles[i].Right = 0;
                                }
                            }
                            break;
                        case Rotation.Right:
                            for (int i = 0; i < Length; i++) {
                                if (i != 0) {
                                    tiles[i].Right = 0;
                                }
                                if (i != Length - 1) {
                                    tiles[i].Left = 0;
                                }
                            }
                            break;
                    }
                }
                X = x;
                Y = y;
            }

            return true;
        }
        private void RecalcTileIndexes() {
            myTileIndexes = new int[2, Length];
            switch (Rot) {
                case Rotation.Down:
                    for (int i = 0; i < Length; i++) {
                        myTileIndexes[0, i] = X + i;
                        myTileIndexes[1, i] = Y;
                    }
                    break;
                case Rotation.Up:
                    for (int i = 0; i < Length; i++) {
                        myTileIndexes[0, i] = X - i;
                        myTileIndexes[1, i] = Y;
                    }
                    break;
                case Rotation.Left:
                    for (int i = 0; i < Length; i++) {
                        myTileIndexes[0, i] = X;
                        myTileIndexes[1, i] = Y + i;
                    }
                    break;
                case Rotation.Right:
                    for (int i = 0; i < Length; i++) {
                        myTileIndexes[0, i] = X;
                        myTileIndexes[1, i] = Y - i;
                    }
                    break;
            }
        }
        
        public XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(XmlReader reader) {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == GetType().ToString()) {
                Length = int.Parse(reader["Length"]);
                Name = reader["Name"];
                Rot = (Rotation)Enum.Parse(typeof(Rotation), reader["Rot"]);
                X = int.Parse(reader["X"]);
                Y = int.Parse(reader["Y"]);

                if (ImOn != null) {
                    BoardTile[] tiles;
                    TryPlaceShip(out tiles, ImOn, X, Y, false);
                } else {
                    RecalcTileIndexes();
                }
            }
        }

        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement(GetType().ToString());
            writer.WriteAttributeString("Length", Length.ToString());
            writer.WriteAttributeString("Name", Name);
            writer.WriteAttributeString("Rot", Enum.GetName(typeof(Rotation), Rot));
            writer.WriteAttributeString("X", X.ToString());
            writer.WriteAttributeString("Y", Y.ToString());

            //writer.WriteStartElement("Tiles");
            //    for(int i = 0; i < myTileIndexes.GetLength(1); i++) {
            //        writer.WriteStartElement("Tile");
            //            writer.WriteAttributeString("X", myTileIndexes[0,i].ToString());
            //            writer.WriteAttributeString("Y", myTileIndexes[1,i].ToString());
            //        writer.WriteEndElement();
            //}
            //writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
