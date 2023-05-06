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

namespace Game_project_OOP
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        GameWindow gameWindow = new GameWindow();
        bool gameWindowShown = false;
        public HomeWindow()
        {
            InitializeComponent();

            this.Loaded += HomeWindow_Loaded;
            gameWindow.Closed += GameWindow_Closed;
            BtnNewGame.Click += BtnNewGame_Click;
            BtnLoadGame.Click += BtnLoadGame_Click;
            BtnQuit.Click += BtnQuit_Click;
            txbxGameName.KeyDown += TxbxGameName_KeyDown;
        }

        private void TxbxGameName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                gameWindow.Show();
                Hide();
                gameWindowShown = true;
            }
        }
        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            stackPanelName.Visibility = Visibility.Visible;
        }

        private void GameWindow_Closed(object? sender, EventArgs e)
        {
            if(gameWindowShown)
                Show();
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gameWindow.Owner = this;
        }
    }
}
