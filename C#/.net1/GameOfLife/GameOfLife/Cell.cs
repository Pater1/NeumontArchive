using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GameOfLife
{
    public class Cell: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void FieldChanged([CallerMemberName] string field = null)
        {
            if(PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(field));
        }
        #endregion

        public delegate void CellUpdated();
        public event CellUpdated OnCellUpdate;
        
        private Button GUIRep;
        public Button _GUIRep
        {
            get { return GUIRep; }
            set { GUIRep = value; FieldChanged(); }
        }

        private bool alive;
        public bool _Alive
        {
            get { return alive; }
            set {
                if(OnCellUpdate != null) OnCellUpdate();
                alive = value;
                FieldChanged();
            }
        }

        private int xPlace;
        public int _XPlace
        {
            get { return xPlace; }
        }

        private int yPlace;
        public int _YPlace
        {
            get { return yPlace; }
        }
            
        private GameGrid parent;
        public GameGrid _Parent
        {
            get { return parent; }
            set { parent = value; FieldChanged(); }
        }

        private Cell[] neighbors;
        public void UpdateNeighbors() { neighbors = parent.GetNeighbors(this); }

        private bool? aliveNextFrame = null;

        public async Task RegisterChanges()
        {
            /*  Any live cell with fewer than two live neighbours dies, as if by needs caused by underpopulation.
                Any live cell with more than three live neighbours dies, as if by overcrowding.
                Any live cell with two or three live neighbours lives, unchanged, to the next generation.
                Any dead cell with exactly three live neighbours becomes a live cell*/

            //int c = parent.LiveSurroundingCount(this);
            int c = neighbors.Where(x => x._Alive).Count();
            aliveNextFrame = c == 3 || (_Alive && c == 2);
        }
        public async Task ApplyChanges()
        {
            if (aliveNextFrame == null) return;

            Toggle(aliveNextFrame);
            aliveNextFrame = null;
        }

        public Cell(int x, int y)
        {
            xPlace = x;
            yPlace = y;
        }

        public void Toggle(bool? to = null)
        {
          //  _Parent.LiveSurroundingCount(this).ToString();

            //int lclX = _XPlace;
            //int lclY = _YPlace;
            
            _Alive = to != null? (bool)to: !_Alive;

            //Color c = _Alive ? _OnColor : _OffColor;
            //Application.Current.Dispatcher.Invoke((Action)(() =>
            //{
            //    GUIRep.Background = new SolidColorBrush(c);
            //}));
        }
    }
}