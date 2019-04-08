using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BattleCruiser {
    [System.Serializable]
    class GameplayManager : ISeaBoardController {
        protected SeaBoard dock = null;
        public UIElement GetDock() {
            if(dock == null) {
                dock = new SeaBoard();
                dock.tileClicked += DockTileClick;
                dock.tileReleased += DockTileRelease;
                dock.tileExit += DockTileExit;
                dock.tileEnter += DockTileEnter;
                dock.dock.Child = null;
            }
            return dock;
        }

        protected SeaBoard controlled;
        public virtual void SetSeaBoard(SeaBoard setTo) {
            controlled = setTo;
        }

        private bool MyTurn { get; set; }
        public Action onTurnEnd;
        public virtual void StartTurn() {
            MyTurn = true;
        }
        public virtual void EndTurn() {
            MyTurn = false;
            if (onTurnEnd != null) onTurnEnd();
        }

        public virtual GameplayManager Opponent { get; set; }
        public bool TryAcceptShot(out TileState hitResponce, int x, int y, bool preveiw = true) {
            BoardTile tile = controlled.GameBoard[x, y];
            int state = (int)tile.State & 0xF;
            bool ret = false;
            hitResponce = TileState.Null;
            switch (state) {
                case (int)TileState.Ship:
                    hitResponce = TileState.Hit;
                    ret = true;
                    break;
                case (int)TileState.Open:
                    hitResponce = TileState.Miss;
                    ret = true;
                    break;
            }

            if (!preveiw && ret) {
                tile.State = hitResponce;
            } else {
                //Obscure shot results when previewing
                //Does nothing if shot fails 
                    //(hitResponce is already TileState.Null if invalid shot)
                hitResponce = TileState.Null;
            }

            return ret;
        }
        //Agregator for Opponent.TryAcceptShot. Used to inforce turn-taking
        private bool TryTakeShot(out TileState hitResponce, int x, int y, bool preveiw = true) {
            if (!MyTurn) {
                hitResponce = TileState.Null;
                return false;
            }
            bool valid = Opponent.TryAcceptShot(out hitResponce, x, y, preveiw);
            return valid;
        }

        internal void InitializeDock(SeaBoard dockerize) {
            foreach(BoardTile tile in dockerize.GameBoard) {
                dock.GameBoard[tile.X, tile.Y].State = ((int)tile.State & 0xF) == (int)TileState.Ship ? TileState.Open : tile.State;
            }
        }

        #region on docked board
        int clickX, clickY;
        public virtual void DockTileClick(int x, int y) {
            TileState hitResponce;
            bool valid = TryTakeShot(out hitResponce, x, y);
            if (valid) {
                int i = (int)dock.GameBoard[x, y].State;
                i &= ~(0xF << 4);
                i |= (int)TileState.ShootClick;
                dock.GameBoard[x, y].State = (TileState)i;
            }
            clickX = x;
            clickY = y;
        }

        public virtual void DockTileRelease(int x, int y) {
            if(clickX == x && clickY == y) {
                TileState hitResponce;
                bool valid = TryTakeShot(out hitResponce, x, y, false);
                if (valid) {
                    dock.GameBoard[x, y].State = hitResponce;
                    EndTurn();
                }
            }
        }

        public virtual void DockTileEnter(int x, int y) {
            if (!MyTurn) return;
            if (((int)dock.GameBoard[x, y].State & 0xF) == (int)TileState.Open) {
                int i = (int)dock.GameBoard[x, y].State;
                i &= ~(0xF << 4);
                i |= (int)TileState.ShootHover;
                dock.GameBoard[x, y].State = (TileState)i;
            }
        }

        public virtual void DockTileExit(int x, int y) {
            dock.GameBoard[x, y].State = (TileState)((int)dock.GameBoard[x, y].State & 0xF);
        }
        #endregion

        #region on controlled board
        //controlled board should not accept input
        public void TileClick(int x, int y) {}
        public void TileRelease(int x, int y) {}
        public void TileEnter(int x, int y) {}
        public void TileExit(int x, int y) {}
        #endregion
        public void OnConnect() {}
        public void OnDisconnect() {}
    }
}
