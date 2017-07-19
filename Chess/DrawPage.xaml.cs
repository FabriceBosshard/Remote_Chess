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
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// Interaktionslogik für DrawPage.xaml
    /// </summary>
    public partial class DrawPage : Window
    {
        private readonly Chessboard chessboardInstance;

        public DrawPage(Chessboard chessboardInstance)
        {
            this.chessboardInstance = chessboardInstance;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
            setText();
            Chessboard.Main.undo.IsEnabled = false;
            Chessboard.Main.undo_Copy1.IsEnabled = false;

        }

        private void setText()
        {
            playerWins.Content = "The Partie ended in a draw!";
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
