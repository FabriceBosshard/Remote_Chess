using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Chess.Factory;
using Chess.Moves;
using Chess.Pieces;
using Chess.Web_Services.PlayerService;

namespace Chess
{

    public partial class Chessboard
    {
        public Formation Fr;
        private Grid _grid;
        public ChessPieceMove Move = new ChessPieceMove();
        private const string WhiteTurnMsg = "White is at turn";
        private const string BlackTurnMsg = "Black is at turn";
        private int _gameId = Int32.MaxValue;
        public static Player _activePlayer;
        private Player _player1;
        private readonly Player _player2;
        private Client _client;
        private DispatcherTimer _t;
        private GameState _state;
        public static ObservableCollection<string> stackMsg = new ObservableCollection<string>();
        public static ObservableCollection<ChessPieceViewModel> oldState = null;
        public static ObservableCollection<ChessPieceViewModel> oldwhiteDeadPieces= null;
        public static ObservableCollection<ChessPieceViewModel> oldBlackDeadPieces = null;
        public static ObservableCollection<string> oldFieldDiff = null;


        public static Chessboard Main;


        public Chessboard()
        {
            Initialize();
            Fr = new Formation();
            DataContext = Formation.Pieces;
            BlackDeadPieceControl.DataContext = Formation.BlackDeadPieces;
            WhiteDeadPieceControl.DataContext = Formation.WhiteDeadPieces;          
            listBox.DataContext = stackMsg;
            

            SetMain();
            if (MainWindow.Remote)
            {
                OpenClient();
            }
            else
            {
                Fr.Initialize();
                _player1 = new Player {Name = "White", IsBlack = false};
                _player2 = new Player {Name = "Black", IsBlack = true};
                _activePlayer = _player1;
                Label.Content = WhiteTurnMsg;
                Square.Background = Brushes.Gray;
            }
            oldState = new History().cloneGameState();
            oldFieldDiff = new History().cloneDiffs();
            oldwhiteDeadPieces = new History().cloneWhite();
            oldBlackDeadPieces = new History().cloneBlack();
        }

        public void SetMain()
        {
            Main = this;
        }

        private void Initialize()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
        }

        private void OpenClient()
        {
            _client = new Client();
            _client.Open();
            _player1 = _client.CreatePlayer(new Player());
            _player1 = _client.StartGame(_player1);
            _gameId = _player1.CurrentGame;
            Label.Content = WhiteTurnMsg;
            GameID.Content = "GameID: " + _gameId;

            if (_player1.IsBlack == false)
            {
                Fr.Initialize();
                _state = new GameState {Pieces = Formation.Pieces.Select(p => p.ToDataContract()).ToArray(), IsBlack = false};
                _client.SetGameState(_player1.CurrentGame.ToString(), _state);
                _activePlayer = _player1;                
            }
            else
            {
                var tempList = _client.ShowState(_gameId.ToString()).Pieces;
                foreach (var chessPiece in tempList)
                {
                    Formation.Pieces.Add(new ChessPieceFactory().Create(chessPiece));
                }

            }
            if (_player1.IsBlack)
            {
                Square.Background = Brushes.Black;
            }
            StartTimer();
           
        }

        private void UpdateChessPieces(IEnumerable<ChessPiece> chessPieces)
        {
            Formation.Pieces.Clear();
            foreach (var chessPiece in chessPieces)
            {
                Formation.Pieces.Add(new ChessPieceFactory().Create(chessPiece));
            }
        }

        private void StartTimer()
        {
            _t = new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0, 1000)};
            _t.Tick += CheckOnNewState;
            _t.Start();
        }

        private void CheckOnNewState(object sender, EventArgs e)
        {
            var result = GetNewStatus(_state);
            if (result != null)
            {
                _state = result;
                UpdateChessPieces(_state.Pieces.ToList());
                UpdatePlayer(_state);
                CheckLabel.Content = !new MoveSimulator(_activePlayer).IsCheck() ? "Check!" : "";
                MSGLabel.Content = "";
            }
        }

        private void UpdatePlayer(GameState gameState)
        {
            if (gameState.IsBlack == _player1.IsBlack)
            {
                _activePlayer = _player1;
                Label.Content = _player1.IsBlack ? BlackTurnMsg : WhiteTurnMsg;
            }
        }


        private GameState GetNewStatus(GameState oldstate)
        {
            var newState = _client.ShowState(_gameId.ToString());
            if (newState?.Id != oldstate?.Id)
            {
                return newState;
            }
            return null;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if ((!MainWindow.Remote || _player1 != _activePlayer) && MainWindow.Remote) return;
            _grid = sender as Grid;
            var point = Mouse.GetPosition(_grid);

            var row = 0;
            var col = 0;
            var accumulatedHeight = 0.0;
            var accumulatedWidth = 0.0;

            try
            {
                // calc row mouse was over
                if (_grid != null)
                {
                    foreach (var rowDefinition in _grid.RowDefinitions)
                    {
                        accumulatedHeight += rowDefinition.ActualHeight;
                        if (accumulatedHeight >= point.Y)
                            break;
                        row++;
                    }

                    // calc col mouse was over
                    foreach (var columnDefinition in _grid.ColumnDefinitions)
                    {
                        accumulatedWidth += columnDefinition.ActualWidth;
                        if (accumulatedWidth >= point.X)
                            break;
                        col++;
                    }
                }
            }
            catch (NullReferenceException exp)
            {
                Console.Write(@"Not the rigth Grid"+ exp.Message);                  
            }
            CheckOption(row, col);
        }

        private void CheckOption(int row, int col)
        {

            foreach (var piece in Formation.Pieces)
            {
                if ((piece.Row == row) && (piece.Column == col) && (piece.IsBlack == _activePlayer.IsBlack))
                {
                    piece.IsSelected = true;
                    break;
                }
                if (piece.IsSelected && (piece.IsBlack == _activePlayer.IsBlack))
                {
                    Field targetField = new Field(row, col);
                    
                    PrepForNextRound(piece, targetField);
                    break;
                }
            }
        }

        private void PrepForNextRound(ChessPieceViewModel piece, Field targetField)
        {           
            Move.ValidateMove(piece,targetField);
            CheckPawnLastRow(piece);

            piece.IsSelected = false;
            
            // TODO Wait till PickPiecePopup closes!
            if (ChessPieceMove.SwapPlayer)
            {
                CheckLabel.Content = "";
                ChessPieceMove.SwapPlayer = false;
                SwapPlayer();                
            }
            
        }

        private void UpdateGameState()
        {
            _state = _activePlayer == _player1 ? new GameState { Pieces = Formation.Pieces.Select(p => p.ToDataContract()).ToArray(), IsBlack = _player1.IsBlack } : new GameState {Pieces = Formation.Pieces.Select(p => p.ToDataContract()).ToArray(), IsBlack = !_player1.IsBlack };
            _client.SetGameState(_player1.CurrentGame.ToString(), _state);
        }

        public void CheckPawnLastRow(ChessPieceViewModel piece)
        {
            var type = piece.GetType();

            if (type == typeof(Pawn))
            {
                if (piece.IsBlack && piece.Row == 7)
                {
                    PickPiecePopUp pfpu = new PickPiecePopUp(true,piece);
                    pfpu.Show();
                    pfpu.TrapOccurred += (sender, e) => SwitchPiece(piece); 
                }
                else if (!piece.IsBlack && piece.Row == 0)
                {
                    PickPiecePopUp pfpu = new PickPiecePopUp(false,piece);
                    pfpu.Show();
                    pfpu.TrapOccurred += (sender, e) => SwitchPiece(piece);
                }               
            }
        }

        private void SwitchPiece(ChessPieceViewModel piece)
        {
            Formation.Pieces.Remove(piece);
            Formation.Pieces.Add(PickPiecePopUp.SelectedPiece);

            if (MainWindow.Remote)
            {
                UpdateGameState();
            }
        }

        public void SwapPlayer()
        {           
            ChangeActivePlayer();

            if (MainWindow.Remote)
            { 
                UpdateGameState();
            }

            if (IsKingInCheck())
            {                
                if (IsKingInCheckmate())
                {
                    showGameOverScreen();
                }
            }
            else if (IsKingInPatt())
            {
                showDrawOverScreen();
            }
            MSGLabel.Content = "";
        }

        private void showDrawOverScreen()
        {
            var Do = new DrawPage(this);
            Do.Show();
        }

        private bool IsKingInPatt()
        {
            if (new MoveSimulator(_activePlayer).IsCheckmate())
            {
                string msg = "Player " + (_activePlayer.IsBlack ? "Black" : "White") + " is Patt!";
                CheckLabel.Content = msg;
                new PushToStack(msg);
                return true;
            }
            return false;
        }

        private void showGameOverScreen()
        {
            if (_activePlayer.IsBlack)
            {
                var go = new GameOver("White", this);
                go.Show();
            }
            else
            {
                var go = new GameOver("Black", this);
                go.Show();
            }
        }

        private bool IsKingInCheckmate()
        {
            if (new MoveSimulator(_activePlayer).IsCheckmate())
            {
                string msg = "Player " + (_activePlayer.IsBlack ? "Black" : "White") + " is Checkmate!";
                CheckLabel.Content = msg;
                new PushToStack(msg);
                return true;
            }
            return false;
        }

        private bool IsKingInCheck()
        {
            if (new MoveSimulator(_activePlayer).IsCheck())
            {
                string msg = "Player " + (_activePlayer.IsBlack ? "Black" : "White") + " is in Check!";
                CheckLabel.Content = msg;
                new PushToStack(msg);
                return true;
            }
            return false;
        }

        private void ChangeActivePlayer()
        {
            if (_activePlayer == _player1)
            {
                _activePlayer = _player2;
                Label.Content = _player1.IsBlack ? WhiteTurnMsg : BlackTurnMsg;
                if (MainWindow.Remote)
                {
                    Square.Background = _player1.IsBlack ? Brushes.Black : Brushes.White;
                }
            }
            else
            {
                _activePlayer = _player1;
                Label.Content = _player1.IsBlack ? BlackTurnMsg : WhiteTurnMsg;
                if (MainWindow.Remote)
                {
                    Square.Background = _player1.IsBlack ? Brushes.White : Brushes.Black;
                }
            }
        }

        private void undo_Click(object sender, RoutedEventArgs e)
        {
            replacePieces();
            replaceFieldDiffs();
            replaceDeadWhite();
            replaceDeadBlack();

            SwapPlayer();

        }

        private void replaceDeadWhite()
        {
            ObservableCollection<ChessPieceViewModel> temp = History.whiteDead.Pop();
            Formation.WhiteDeadPieces.Clear();
            foreach (ChessPieceViewModel chesspiece in temp)
            {
                Formation.WhiteDeadPieces.Add(new ChessPieceFactory().Create(chesspiece.ToDataContract()));
            }
            if (History.whiteDead.Count == 0)
            {
                undo.IsEnabled = false;
                oldwhiteDeadPieces = new History().cloneWhite();
            }
        }

        private void replaceDeadBlack()
        {
            ObservableCollection<ChessPieceViewModel> temp = History.blackDead.Pop();
            Formation.BlackDeadPieces.Clear();
            foreach (ChessPieceViewModel chesspiece in temp)
            {
                Formation.BlackDeadPieces.Add(new ChessPieceFactory().Create(chesspiece.ToDataContract()));
            }
            if (History.blackDead.Count == 0)
            {
                undo.IsEnabled = false;
                oldBlackDeadPieces = new History().cloneBlack();
            }
        }

        private void replaceFieldDiffs()
        {
            ObservableCollection<string> temp = History.FieldDiffs.Pop();
            stackMsg.Clear();
            foreach (string s in temp)
            {
                stackMsg.Add(s);
            }
            if (stackMsg.Count==0)
            {
                oldFieldDiff = new History().cloneDiffs();
            }
        }

        private void replacePieces()
        {
            ObservableCollection<ChessPieceViewModel> temp = History.GameStates.Pop();
            Formation.Pieces.Clear();
            foreach (ChessPieceViewModel chesspiece in temp)
            {
                Formation.Pieces.Add(new ChessPieceFactory().Create(chesspiece.ToDataContract()));
            }
            if (History.GameStates.Count == 0)
            {
                undo.IsEnabled = false;
                oldState = new History().cloneGameState();
            }
        }
    }
}