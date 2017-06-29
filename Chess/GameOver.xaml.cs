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
            
        }

        private void getWinPlayer(string msg)
        {
            playerWins.Content = "Player " + msg + " has won";
            this.KeyDown += Exit;
            

        }

        private void Exit(object sender, KeyEventArgs e)
        {
           this.Close();
           chessboardInstance.Close();
        }
    }
}
