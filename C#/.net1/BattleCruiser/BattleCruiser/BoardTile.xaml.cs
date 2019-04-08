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
    /// Interaction logic for BoardTile.xaml
    /// </summary>
    [System.Serializable]
    public partial class BoardTile : UserControl, IXmlSerializable {
        private BoardTile() { }
        public BoardTile(Label child, int x, int y) {
            InitializeComponent();

            X = x;
            Y = y;

            border = new Border();
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(2,2,2,2);

            underlay = child;
            border.Child = underlay;
            grid.Children.Add(border);
            Grid.SetZIndex(border, 0);

            overlay = new Label();
            overlay.Background = Brushes.Transparent;
            grid.Children.Add(overlay);
            Grid.SetZIndex(overlay, 1);

            overlay.MouseDown += TileClicked;
            overlay.MouseUp += TileReleased;
            overlay.MouseEnter += TileEntered;
            overlay.MouseLeave += TileExited;

            State = TileState.Open;
        }
        public Label overlay, underlay;
        public Border border;

        private TileState state;
        public TileState State {
            get {
                return state;
            }
            set {
                state = value;
                overlay.Background = new SolidColorBrush(state.GetOverlay());
                underlay.Background = new SolidColorBrush(state.GetColor());
            }
        }
        public int X { get; private set; }
        public int Y { get; private set; }

        public double Top {
            get {
                return border.BorderThickness.Top;
            }
            set {
                Thickness thick = border.BorderThickness;
                thick.Top = value;
                border.BorderThickness = thick;
            }
        }
        public double Bottom {
            get {
                return border.BorderThickness.Bottom;
            }
            set {
                Thickness thick = border.BorderThickness;
                thick.Bottom = value;
                border.BorderThickness = thick;
            }
        }
        public double Left {
            get {
                return border.BorderThickness.Left;
            }
            set {
                Thickness thick = border.BorderThickness;
                thick.Left = value;
                border.BorderThickness = thick;
            }
        }
        public double Right {
            get {
                return border.BorderThickness.Right;
            }
            set {
                Thickness thick = border.BorderThickness;
                thick.Right = value;
                border.BorderThickness = thick;
            }
        }

        public event OnTileAction tileClicked;
        private void TileClicked(object sender, MouseButtonEventArgs e) {
            int x = X;
            int y = Y;

            if (tileClicked != null) tileClicked(x, y);
        }

        public event OnTileAction tileReleased;
        private void TileReleased(object sender, MouseButtonEventArgs e) {
            int x = X;
            int y = Y;

            if (tileReleased != null) tileReleased(x, y);
        }

        public event OnTileAction tileEntered;
        private void TileEntered(object sender, MouseEventArgs e) {
            int x = X;
            int y = Y;

            if (tileEntered != null) tileEntered(x, y);
        }

        public event OnTileAction tileExited;
        private void TileExited(object sender, MouseEventArgs e) {
            int x = X;
            int y = Y;

            if (tileExited != null) tileExited(x, y);
        }

        public XmlSchema GetSchema() {
            return null;
        }

        public void ReadXml(XmlReader reader) {
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == GetType().ToString()) {
                State = (TileState)Enum.Parse(typeof(TileState), reader["State"]);
                X = int.Parse(reader["X"]);
                Y = int.Parse(reader["Y"]);

                Top = double.Parse(reader["Top"]);
                Bottom = double.Parse(reader["Bottom"]);
                Left = double.Parse(reader["Left"]);
                Right = double.Parse(reader["Right"]);
            }
        }

        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement(GetType().ToString());

            writer.WriteAttributeString("State", Enum.GetName(typeof(TileState), State));
            writer.WriteAttributeString("X", X.ToString());
            writer.WriteAttributeString("Y", Y.ToString());

            writer.WriteAttributeString("Top", Top.ToString());
            writer.WriteAttributeString("Bottom", Bottom.ToString());
            writer.WriteAttributeString("Left", Left.ToString());
            writer.WriteAttributeString("Right", Right.ToString());

            writer.WriteEndElement();
        }
    }
}
