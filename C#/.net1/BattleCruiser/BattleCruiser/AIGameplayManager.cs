using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BattleCruiser {
    [System.Serializable]
    class AIGameplayManager : GameplayManager {
        public AIGameplayManager(): base(){}

        public override void SetSeaBoard(SeaBoard setTo) {
            controlled = setTo;

            foreach(BoardTile tile in controlled.GameBoard) {
                uncalculatedTiles.Add(
                    new TileSpaceWrapper() {
                        x = tile.X,
                        y = tile.Y,
                        gm = this,
                        dispatch = Dispatcher.CurrentDispatcher
                    }
                );
            }
        }

        private double[,] kernel = new double[,] {
            {-0.5,  1.0,    -0.5},
            {1.0,   0.0,    1.0 },
            {-0.5,  1.0,    -0.5},
        };

        private class TileSpaceWrapper {
            public int x, y;
            public AIGameplayManager gm;

            public bool valid;
            public double score;

            public Dispatcher dispatch;

            public void CalculateScore(Object thing) {
                BoardTile tile = gm.dock.GameBoard[x, y];

                //Do Scoring Logic
                score = 0;

                TileState hitOut;
                valid = gm.Opponent.TryAcceptShot(out hitOut, x, y);

                if (valid) {
                    int width = gm.kernel.GetLength(0);
                    int height = gm.kernel.GetLength(1);
                    for (int lx = -(width / 2); lx <= (width / 2); lx++) {
                        for (int ly = -(height / 2); ly < (height / 2); ly++) {
                            int gx = lx + x, gy = ly + y;
                            if (gx >= 0 && gx < 10 && gy >= 0 && gy < 10) {
                                double weight = gm.kernel[lx + (width / 2), ly + (height / 2)];
                                int state = ((int)gm.dock.GameBoard[gx, gy].State) & 0xF;
                                double dir = state == (int)TileState.Hit ? 2 : state == (int)TileState.Miss ? -0.5 : 0;

                                score += weight * dir;
                            }
                        }
                    }
                } else {
                    score = -99999.9;
                }

                lock (gm.calculatedTiles) {
                    gm.calculatedTiles.Add(this);
                }
                lock (gm.uncalculatedTiles) {
                    gm.uncalculatedTiles.Remove(this);
                }
            }
        }
        private List<TileSpaceWrapper> uncalculatedTiles = new List<TileSpaceWrapper>();
        private List<TileSpaceWrapper> calculatedTiles = new List<TileSpaceWrapper>();

        public override void StartTurn() {
            for (int i = uncalculatedTiles.Count-1; i >= 0; i--) {
                uncalculatedTiles[i].CalculateScore(this);
                //ThreadPool.QueueUserWorkItem(uncalculatedTiles[i].CalculateScore, this);
            }
            while (uncalculatedTiles.Count != 0) { }
            for (int i = calculatedTiles.Count - 1; i >= 0; i--) {
                string score = calculatedTiles[i].score.ToString();
                BoardTile tile = calculatedTiles[i].gm.dock.GameBoard[calculatedTiles[i].x, calculatedTiles[i].y];
                tile.underlay.Content = score;
            }

            EndTurn();
        }
        Random rand = new Random();
        public override void EndTurn() {
            calculatedTiles = calculatedTiles
                                .Where(x => x.valid)
                                .OrderByDescending(x => x.score)
                                .ThenBy(x => rand.Next())   //randomize order of all tiles with same score
                                .ToList();
            uncalculatedTiles = calculatedTiles.ToList();

            int select = 0;
            TileSpaceWrapper selected = calculatedTiles[select];
            calculatedTiles.Clear();

            TileState hitResponce;
            bool valid = true;

            do {
                valid = Opponent.TryAcceptShot(out hitResponce, selected.x, selected.y, false);

                dock.GameBoard[selected.x, selected.y].State = hitResponce;

                uncalculatedTiles.Remove(selected);

                if (!valid) {
                    select++;
                    selected = uncalculatedTiles[select];
                }
            } while (!valid);

            if (base.onTurnEnd != null) base.onTurnEnd();
        }

        #region on docked board
        //for AI docked board should accept no input
        public override void DockTileClick(int x, int y) {}
        public override void DockTileRelease(int x, int y) {}
        public override void DockTileEnter(int x, int y) {}
        public override void DockTileExit(int x, int y) {}
        #endregion
    }
}
