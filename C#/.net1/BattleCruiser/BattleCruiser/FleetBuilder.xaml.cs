using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace BattleCruiser {
    /// <summary>
    /// Interaction logic for FleetBuilder.xaml
    /// </summary>
    public partial class FleetBuilder : UserControl, ISeaBoardController {
        public FleetBuilder() {
            InitializeComponent();
            Selector.Visibility = Visibility.Collapsed;
            ValidityUpdate();
        }
        
        private void ValidityUpdate() {
            toBattle.Visibility = Valid ? Visibility.Visible : Visibility.Hidden;
        }
        public bool Valid {
            get {
                bool b = controlled != null && controlled.Fleet != null;
                if(b) foreach(Ship s in controlled.Fleet) {
                    if (!s.Placed) {
                        b = false;
                    }
                }
                return b;
            }
        }

        public UIElement GetDock() {
            return this;
        }
        SeaBoard controlled = null;
        public void SetSeaBoard(SeaBoard setTo) {
            controlled = setTo;
        }

        private void GainTiles(BoardTile[] b) {
            foreach(BoardTile tile in b) {
                int i = (int)tile.State;
                i &= ~0xF;
                i |= (int)TileState.Ship;
                i &= ~(0xF << 4);
                tile.State = (TileState)i;
            }
            ValidityUpdate();
        }
        private void LooseTiles(BoardTile[] b) {
            foreach (BoardTile tile in b) {
                int i = (int)tile.State;
                i &= ~0xF;
                i |= (int)TileState.Open;
                i &= ~(0xF << 4);
                tile.State = (TileState)i;
            }
        }

        int clickX, clickY;
        public void TileClick(int x, int y) {
            if (activeShip == null) return;

            BoardTile[] tiles = null;
            bool valid = controlled.Fleet[activeShip.Value].TryPlaceShip(out tiles, controlled, x, y);

            if (valid) {
                clickX = x;
                clickY = y;

                foreach (BoardTile tile in tiles) {
                    int s = (int)tile.State;
                    s &= ~(0xF << 4);
                    s |= (int)TileState.ShipClick;
                    tile.State = (TileState)s;
                }
            }
        }
        public void TileRelease(int x, int y) {
            if(hoverX == clickX && hoverY == clickY && activeShip != null) {
                BoardTile[] b;
                Ship boat = controlled.Fleet[activeShip.Value];

                bool valid = boat.TryPlaceShip(out b, controlled, x, y, false);
            }
        }
        int hoverX, hoverY;
        public void TileEnter(int x, int y) {
            if (activeShip == null) return;

            BoardTile[] tiles = null;
            bool valid = controlled.Fleet[activeShip.Value].TryPlaceShip(out tiles, controlled, x, y);

            if (valid) {
                hoverX = x;
                hoverY = y;

                foreach (BoardTile tile in tiles) {
                    int s = (int)tile.State;
                    s &= ~(0xF << 4);
                    s |= (int)TileState.ShipHover;
                    tile.State = (TileState)s;
                }
            }
        }
        public void TileExit(int x, int y) {
            if (activeShip == null) return;

            BoardTile[] tiles = null;
            bool valid = controlled.Fleet[activeShip.Value].TryPlaceShip(out tiles, controlled, x, y);

            if (valid) {
                foreach (BoardTile tile in tiles) {
                    int s = (int)tile.State;
                    s &= ~(0xF << 4);
                    tile.State = (TileState)s;
                }
            }
        }
        
        private int? activeShip = null;

        private void CarrierClick(object sender, MouseButtonEventArgs e) {
            if(activeShip != 0) {
                Grid.SetRow(Selector, 1);
                Selector.Visibility = Visibility.Visible;
                activeShip = 0;
            } else {
                Selector.Visibility = Visibility.Collapsed;
                activeShip = null;
            }
        }
        private void BattleshipClick(object sender, MouseButtonEventArgs e) {
            if (activeShip != 1) {
                Grid.SetRow(Selector, 2);
                Selector.Visibility = Visibility.Visible;
                activeShip = 1;
            } else {
                Selector.Visibility = Visibility.Collapsed;
                activeShip = null;
            }
        }
        private void CruiserClick(object sender, MouseButtonEventArgs e) {
            if (activeShip != 2) {
                Grid.SetRow(Selector, 3);
                Selector.Visibility = Visibility.Visible;
                activeShip = 2;
            } else {
                Selector.Visibility = Visibility.Collapsed;
                activeShip = null;
            }
        }
        private void SubmarineClick(object sender, MouseButtonEventArgs e) {
            if (activeShip != 3) {
                Grid.SetRow(Selector, 4);
                Selector.Visibility = Visibility.Visible;
                activeShip = 3;
            } else {
                Selector.Visibility = Visibility.Collapsed;
                activeShip = null;
            }
        }
        private void DestroyerClick(object sender, MouseButtonEventArgs e) {
            if (activeShip != 4) {
                Grid.SetRow(Selector, 5);
                Selector.Visibility = Visibility.Visible;
                activeShip = 4;
            } else {
                Selector.Visibility = Visibility.Collapsed;
                activeShip = null;
            }
        }

        bool rotLock = false;
        private void RotateClockwise(object sender, MouseButtonEventArgs e) {
            if (activeShip == null || rotLock) return;
            rotLock = true;

            int r = (int)controlled.Fleet[activeShip.Value].Rot;
            r++;
            r %= 4;
            controlled.Fleet[activeShip.Value].Rot = (Rotation)r;
        }

        private void RandomizeLayout(object sender, MouseButtonEventArgs e) {
            RandomizeLayout();
        }
        private void RandomizeLayout() {
            //clear the board
            foreach (Ship boat in controlled.Fleet) {
                if (boat.MyTileIndexes != null && boat.MyTileIndexes.Length > 0) {
                    foreach (BoardTile tile in boat.MyTiles) {
                        tile.State = TileState.Open;
                    }
                }
                boat.ClearTiles();
            }
            //Randomly place ships until the layout is valid
            Random rand = new Random();
            foreach (Ship boat in controlled.Fleet) {
                bool valid = true;

                do {
                    BoardTile[] n;
                    int x = rand.Next(0, 10);
                    int y = rand.Next(0, 10);
                    Rotation r = (Rotation)rand.Next(0, 4);
                    boat.Rot = r;
                    valid = boat.TryPlaceShip(out n, controlled, x, y, false);
                } while (!valid);
            }
        }

        public Action onToBattle;
        private void ToBattle(object sender, MouseButtonEventArgs e) {
            ToBattle();
        }
        public void ToBattle() {
            if (onToBattle != null) onToBattle();
        }

        private void RotateCounterClockwise(object sender, MouseButtonEventArgs e) {
            if (activeShip == null || rotLock) return;
            rotLock = true;

            int r = (int)controlled.Fleet[activeShip.Value].Rot;
            r--;
            if (r < 0) r += 4;
            controlled.Fleet[activeShip.Value].Rot = (Rotation)r;
        }
        private void RotateRelease(object sender, MouseButtonEventArgs e) {
            rotLock = false;
        }

        public void AutoBattle() {
            RandomizeLayout();
            ToBattle();
        }

        public void OnConnect() {
            foreach (Ship boat in controlled.Fleet) {
                boat.onLooseTiles += LooseTiles;
                boat.onGainTiles += GainTiles;
            }
        }

        public void OnDisconnect() {
            foreach (Ship boat in controlled.Fleet) {
                boat.onLooseTiles -= LooseTiles;
                boat.onGainTiles -= GainTiles;
            }
        }
    }
}
