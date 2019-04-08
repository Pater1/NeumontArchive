using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// <summary>
    /// Interaction logic for GameGrid.xaml
    /// </summary>
    public partial class GameGrid : UserControl
    {
        private int xDim = 10;
        public int _XDim
        {
            get { return xDim; }
            set {
                if (value > 0)
                {
                    xDim = value;
                }
            }
        }

        private int yDim = 10;
        public int _YDim
        {
            get { return yDim; }
            set
            {
                if (value > 0)
                {
                    yDim = value;
                }
            }
        }
        
        public static readonly DependencyProperty TickDelayProperty =
            DependencyProperty.Register("TickDelay", typeof(double), typeof(GameGrid));
        public double TickDelay
        {
            get { return (double)GetValue(TickDelayProperty); }
            set { SetValue(TickDelayProperty, value); }
        }


        public GameGrid()
        {
            InitializeComponent();
            RebuildGrid();
        }

        private Color onColor = Colors.Orange;
        public Color _OnColor
        {
            get { return onColor; }
            set { onColor = value; }
        }

        private Color offColor = Colors.DarkSlateGray;
        public Color _OffColor
        {
            get { return offColor; }
            set { offColor = value; }
        }

        private Cell[] board;
        private List<Cell> alive = new List<Cell>();
        private Dictionary<Button, Cell> boardMap = new Dictionary<Button, Cell>();
        public void RebuildGrid()
        {
            boardMap.Clear();
            board = new Cell[_XDim*_YDim];

            gridBehind.RowDefinitions.Clear();
            for (int x = 0; x < xDim; x++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(1, GridUnitType.Star);
                gridBehind.RowDefinitions.Add(rowDefinition);
            }

            gridBehind.ColumnDefinitions.Clear();
            for (int x = 0; x < xDim; x++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                columnDefinition.Width = new GridLength(1, GridUnitType.Star);
                gridBehind.ColumnDefinitions.Add(columnDefinition);
            }

            gridBehind.Children.Clear();
            for (int x = 0; x < _XDim; x++)
            {
                for (int y = 0; y < _YDim; y++)
                {
                    BuildCell(x, y);
                }
            }

            for(int i = 0; i < board.Length; i++)
            {
                board[i].UpdateNeighbors();
            }
        }
        private void BuildCell(int x, int y)
        {
            Button b = new Button();
            b.Click += new RoutedEventHandler(ToggleCell);
            b.Background = new SolidColorBrush(_OffColor);
            gridBehind.Children.Add(b);
            Grid.SetRow(b, y);
            Grid.SetColumn(b, x);

            Cell c = new Cell(x, y)
            {
                _GUIRep = b,
                _Alive = false,
                _Parent = this
            };
            
            BoolToBrushConverter b2b = new BoolToBrushConverter()
            {
                TrueBrush = new SolidColorBrush(_OnColor),
                FalseBrush = new SolidColorBrush(_OffColor)
            };
            Binding bind = new Binding("_Alive")
            {
                Converter = b2b
            };
            b.DataContext = c;
            b.SetBinding(Button.BackgroundProperty, bind);

            boardMap.Add(b, c);

            int i = x * _YDim + y;
            board[i] = c;
        }
        
        List<Task> ts = new List<Task>();
        public async Task UpdateAsync()
        {
            foreach (Cell b in board)
            {
                ts.Add(b.RegisterChanges());
            }
            foreach (Task t in ts)
            {
                t.Wait();
            }
            ts.Clear();
            foreach (Cell b in board)
            {
                ts.Add(b.ApplyChanges());
            }
            foreach (Task t in ts)
            {
                t.Wait();
            }
        }
       
        public Cell[] GetNeighbors(Cell around)
        {
            return board.Where(x =>
            {
                int xDelt = Math.Abs(x._XPlace - around._XPlace);
                int yDelt = Math.Abs(x._YPlace - around._YPlace);
                bool ret = ((xDelt == 1 && yDelt < 2) || (yDelt == 1 && xDelt < 2));
                return ret;
            }).ToArray();
        }

        public void ToggleCell(object sender, RoutedEventArgs e)
        {
            Button obj = (Button)sender;
            boardMap[obj].Toggle();
        }
        Random rand = new Random();
        public void ToggleAllCells(bool random = true)
        {
            for (int i = 0; i < board.Length; i++)
            {
                Cell b = board[i];
                b.Toggle(random? (bool?)(rand.Next()%2==0): null);
            }
        }

        private Thread runThread = null;
        public void ToggleAutoRun()
        {
            if(runThread == null)
            {
                runThread = new Thread(new ParameterizedThreadStart(AutoRun));
                runThread.Start((int)TickDelay);
            }
            else
            {
                try
                {
                    runThread.Abort();
                }
                catch { }
                runThread = null;
            }
        }
        
        private void AutoRun(object delay)
        {
            int i = (int)delay;
            while (true)
            { 
                Task t = UpdateAsync();
                t.Wait();
                Thread.Sleep(i);
            }
        }

        public void Shutdown()
        {
            try
            {
                runThread.Abort();
            }
            catch { }
            runThread = null;
        }
    }
}
