using System.Windows;

namespace Chess
{
    public partial class MainWindow
    {
        public static bool Remote;
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var cb = new Chessboard();
            cb.Show();
            Close();
        }

        private void button_ClickRemote(object sender, RoutedEventArgs e)
        {
            Remote = true;
            var cb = new Chessboard();
            cb.Show();
            Close();
        }
    }
}
