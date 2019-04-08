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
    /// Interaction logic for Game.xaml
    /// </summary>
    [System.Serializable]
    public partial class Game : UserControl, IXmlSerializable {
        public Game() {
            InitializeComponent();
        }
        public void InitializeGame() {
            enemyFleetBuilder = new FleetBuilder();
            enemyFleetBuilder.Attach(enemyBoard);
            enemyFleetBuilder.onToBattle += EnemyToBattleCallback;
            enemyFleetBuilder.AutoBattle();

            playerFleetBuilder = new FleetBuilder();
            playerFleetBuilder.Attach(playerBoard);
            playerFleetBuilder.onToBattle += PlayerToBattleCallback;
        }

        FleetBuilder playerFleetBuilder;
        bool playerReady = false;
        private void PlayerToBattleCallback() {
            playerFleetBuilder.Detach(playerBoard);
            playerReady = true;
            if (enemyReady && playerReady) {
                StartGame();
            }
        }
        FleetBuilder enemyFleetBuilder;
        bool enemyReady = false;
        private void EnemyToBattleCallback() {
            enemyFleetBuilder.Detach(enemyBoard);
            enemyReady = true;
            if(enemyReady && playerReady) {
                StartGame();
            }
        }

        GameplayManager playerGameplayManager = new GameplayManager();
        GameplayManager enemyGameplayManager = new AIGameplayManager();
        private void StartGame() {
            playerGameplayManager.Opponent = enemyGameplayManager;
            enemyGameplayManager.Opponent = playerGameplayManager;

            playerGameplayManager.Attach(playerBoard);
            enemyGameplayManager.Attach(enemyBoard);

            playerGameplayManager.onTurnEnd += PlayerEndTurn;
            enemyGameplayManager.onTurnEnd += EnemyEndTurn;

            playerGameplayManager.InitializeDock(enemyBoard);
            enemyGameplayManager.InitializeDock(playerBoard);

            playerGameplayManager.StartTurn();
        }
        private void PlayerEndTurn() {
            CheckIfWin();
            if (!someoneWon) enemyGameplayManager.StartTurn();
        }
        private void EnemyEndTurn() {
            CheckIfWin();
            if(!someoneWon) playerGameplayManager.StartTurn();
        }

        bool someoneWon = false;
        private void CheckIfWin() {
            bool enemyDead = enemyBoard.IsDead;
            bool playerDead = playerBoard.IsDead;

            if(enemyDead || playerDead) {
                someoneWon = true;
                if (!playerDead) {
                    WinPic.Visibility = Visibility.Visible;
                } else {
                    LoosePic.Visibility = Visibility.Visible;
                }
                NewButton.Visibility = Visibility.Visible;
                LoadButton.Visibility = Visibility.Visible;
            }
        }

        public XmlSchema GetSchema() {
            return null;
        }
        public void ReadXml(XmlReader reader) {
            reader.Read();//Skip<Game>
            if (reader.MoveToContent() == XmlNodeType.Element && reader.LocalName == GetType().ToString()) {
                playerReady = reader["playerReady"] == "True";
                enemyReady = reader["enemyReady"] == "True";

                reader.Read();
                grid.Children.Remove(playerBoard);
                playerBoard = new SeaBoard();
                playerBoard.ReadXml(reader);
                grid.Children.Add(playerBoard);
                Grid.SetRow(playerBoard, 0);


                reader.Read();
                reader.Read();
                reader.Read();

                grid.Children.Remove(enemyBoard);
                enemyBoard = new SeaBoard();
                enemyBoard.ReadXml(reader);
                grid.Children.Add(enemyBoard);
                Grid.SetRow(enemyBoard, 1);

                if(enemyReady && playerReady) {
                    CheckIfWin();
                    if(!someoneWon) StartGame();
                } else {
                    InitializeGame();
                }
            }
        }
        public void WriteXml(XmlWriter writer) {
            writer.WriteStartElement(GetType().ToString());
            writer.WriteAttributeString("playerReady", playerReady.ToString());
            writer.WriteAttributeString("enemyReady", enemyReady.ToString());

            //XmlSerializer playerBoardSerilizer = new XmlSerializer(playerBoard.GetType());
            //playerBoardSerilizer.Serialize(writer, playerBoard);
            playerBoard.WriteXml(writer);

            //XmlSerializer enemyBoardSerilizer = new XmlSerializer(enemyBoard.GetType());
            //enemyBoardSerilizer.Serialize(writer, enemyBoard);
            enemyBoard.WriteXml(writer);
            writer.WriteEndElement();
        }

        private void NewGame(object sender, MouseButtonEventArgs e) {
            MainWindow.Instance.NewGame(sender, e);
        }
        private void LoadGame(object sender, MouseButtonEventArgs e) {
            MainWindow.Instance.OpenGame(sender, e);
        }

        public bool InCheatMode {
            get {
                return enemyBoard.Visibility == Visibility.Visible;
            }
            set {
                enemyBoard.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                Grid.SetRowSpan(playerBoard, value? 1: 2);
            }
        }
    }
}
