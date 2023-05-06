using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
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
using System.IO;

namespace Game_project_OOP
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        string gameName;
        GameWindow gameWindow = new GameWindow();
        bool gameWindowShown = false;
        string fileName = "C:\\Users\\decle\\source\\repos\\Game project OOP\\data.json";
        public HomeWindow()
        {
            InitializeComponent();

            Loaded += HomeWindow_Loaded;
            gameWindow.Closed += GameWindow_Closed;
            BtnNewGame.Click += BtnNewGame_Click;
            BtnLoadGame.Click += BtnLoadGame_Click;
            BtnQuit.Click += BtnQuit_Click;
            txbxGameName.KeyDown += TxbxGameName_KeyDown;
            lstbxLoadedGames.SelectionChanged += LstbxLoadedGames_SelectionChanged;
        }

        private void LstbxLoadedGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gameName = (string)lstbxLoadedGames.SelectedItem;
            gameWindow = new GameWindow(gameName);

            gameWindow.Closed += GameWindow_Closed;

            gameWindow.Show();

            lstbxLoadedGames.Visibility = Visibility.Hidden;
            gameWindowShown = true;
            Hide();
        }

        private void TxbxGameName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                gameName = txbxGameName.Text;
                gameWindow = new GameWindow(gameName);

                gameWindow.Closed += GameWindow_Closed; 

                gameWindow.Show();

                stackPanelNewName.Visibility = Visibility.Hidden;
                gameWindowShown = true;
                Hide();
            }
        }
        private void BtnQuit_Click(object sender, RoutedEventArgs e)
        {
            gameWindow.Close();
            Close();
        }

        private void BtnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            if (!gameWindowShown && lstbxLoadedGames.Visibility == Visibility.Hidden)
            {
                // Read the JSON file into a string
                string jsonString = File.ReadAllText(fileName);

                // Parse the JSON string into a dictionary
                var data = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonString);

                // Retrieve all the keys (headers)
                IEnumerable<string> headers = data.Keys;

                // Iterate over the headers and print them
                foreach (string header in headers)
                {
                    lstbxLoadedGames.Items.Add(header);
                }
            }
            lstbxLoadedGames.Visibility = Visibility.Visible;
            stackPanelNewName.Visibility = Visibility.Hidden;
        }

        private void BtnNewGame_Click(object sender, RoutedEventArgs e)
        {
            stackPanelNewName.Visibility = Visibility.Visible;
            lstbxLoadedGames.Visibility = Visibility.Hidden;

        }

        private void GameWindow_Closed(object? sender, EventArgs e)
        {
            try
            {
                if (gameWindowShown)
                    Show();
            }
            catch(Exception ex)
            {
                // Do nothing, this exception does not need to handled.
            }
        }

        private void HomeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            gameWindow.Owner = this;
        }
    }
}
