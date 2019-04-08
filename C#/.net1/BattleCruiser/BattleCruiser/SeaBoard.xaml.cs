using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace BattleCruiser {
    /// <summary>
    /// Interaction logic for SeaBoard.xaml
    /// </summary>
    [System.Serializable]
    public partial class SeaBoard : UserControl, IXmlSerializable {
        public SeaBoard() {
            InitializeComponent();

            for(int x = 0; x < grid.RowDefinitions.Count; x++) {
                for(int y = 0; y < grid.ColumnDefinitions.Count; y++) {
                    char c = (char)(x + 'a'-1);
                    
                    Label l = new Label();
                    l.HorizontalContentAlignment = HorizontalAlignment.Center;
                    l.VerticalContentAlignment = VerticalAlignment.Center;

                    //Top-right corner. Do nothing.
                    if (x == 0 && y == 0) {
                        l.Content = "Battle\nCruiser";

                        grid.Children.Add(l);
                        Grid.SetColumn(l, y);
                        Grid.SetRow(l, x);
                        //Number row. Add Label with # content
                    } else if(x == 0) {
                        l.Content = y.ToString();

                        grid.Children.Add(l);
                        Grid.SetColumn(l, y);
                        Grid.SetRow(l, x);
                        //Letter row. Add Label with alph' content
                    } else if(y == 0) {
                        l.Content = c.ToString();

                        grid.Children.Add(l);
                        Grid.SetColumn(l, y);
                        Grid.SetRow(l, x);
                        //Game board.
                    } else {
                        l.Name = c.ToString() + x.ToString();
                        l.Background = Brushes.Green;

                        BoardTile b = new BoardTile(l, x-1, y-1);
                        b.tileClicked += ReceiveClick;
                        b.tileEntered += ReceiveEnter;
                        b.tileExited += ReceiveExit;
                        b.tileReleased += ReceiveRelease;

                        grid.Children.Add(b);
                        Grid.SetColumn(b, y);
                        Grid.SetRow(b, x);

                        gameBoard[x - 1, y - 1] = b;
                    }
                }
            }

            fleet = new Ship[] {
                Ship.Carrier(),
                Ship.Battleship(),
                Ship.Cruiser(),
                Ship.Submarine(),
                Ship.Destroyer()
            };
            foreach(Ship s in Fleet) {
                s.ImOn = this;
            }
        }

        private BoardTile[,] gameBoard = new BoardTile[10, 10];
        public BoardTile[,] GameBoard { get { return gameBoard; } }

        private Ship[] fleet;
        public Ship[] Fleet { get { return fleet; } }

        private SeaBoard opponent;
        public SeaBoard Opponent {
            get {
                return opponent;
            }
            set {
                if(value != null) {
                    opponent = value;
                }
            }
        }

        public bool IsDead {
            get {
                foreach(Ship s in Fleet) {
                    if (!s.IsSunk) return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hitState">returns state of shot</param>
        /// <param name="x">x coordinate of the shot</param>
        /// <param name="y">y coordinate of the shot</param>
        /// <returns>is shot was valid</returns>
        public bool TakeShot(out TileState hitState, int x, int y) {
            throw new NotImplementedException();
        }

        public event OnTileAction tileClicked;
        public void ReceiveClick(int x, int y) {
            if (tileClicked != null) tileClicked(x, y);
        }
        public event OnTileAction tileReleased;
        public void ReceiveRelease(int x, int y) {
            if (tileReleased != null) tileReleased(x, y);
        }
        public event OnTileAction tileEnter;
        public void ReceiveEnter(int x, int y) {
            if (tileEnter != null) tileEnter(x, y);
        }
        public event OnTileAction tileExit;
        public void ReceiveExit(int x, int y) {
            if (tileExit != null) tileExit(x, y);
        }

        public XmlSchema GetSchema() {
            return null;
        }
        public void ReadXml(XmlReader reader) {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == GetType().ToString()) {
                reader.Read();//<Fleet>
                int count = int.Parse(reader["count"]);
                for (int i = 0; i < count; i++) {
                    reader.Read();//getNext<BattleCruiser.Ship>
                    fleet[i].ImOn = this;
                    fleet[i].ReadXml(reader);
                }


                reader.Read();//</Fleet>
                reader.Read();//<Board>
                count = int.Parse(reader["count"]);
                for (int i = 0; i < count; i++) {
                    reader.Read();//getNext<BattleCruiser.BoardTile>

                    int x = int.Parse(reader["X"]);
                    int y = int.Parse(reader["Y"]);
                    
                    gameBoard[x,y].ReadXml(reader);
                }
            }
        }
        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement(GetType().ToString());

            writer.WriteStartElement("Fleet");
            writer.WriteAttributeString("count", Fleet.Length.ToString());
            foreach (Ship boat in Fleet) {
                boat.WriteXml(writer);
            }
            writer.WriteEndElement();


            writer.WriteStartElement("Board");
            writer.WriteAttributeString("count", gameBoard.Length.ToString());
            foreach (BoardTile tile in gameBoard) {
                tile.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
