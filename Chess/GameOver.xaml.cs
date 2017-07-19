using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Chess
{
    public partial class GameOver
    {
        private readonly Chessboard chessboardInstance;


        public GameOver(string msg,Chessboard chessboardInstance)
        {
            this.chessboardInstance = chessboardInstance;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            getWinPlayer(msg);
            Chessboard.Main.undo.IsEnabled = false;
            Chessboard.Main.undo_Copy1.IsEnabled = false;

        }

        private void getWinPlayer(string msg)
        {
            playerWins.Content = "Player " + msg + " has won!";
            this.KeyDown += Exit;            

        }

        private void Exit(object sender, KeyEventArgs e)
        {
            MainWindow newGame = new MainWindow();
            newGame.Show();
            this.Close(); 
            chessboardInstance.Close();           
        }
    }
}
