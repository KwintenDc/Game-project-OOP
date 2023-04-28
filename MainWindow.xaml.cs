﻿using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Game_project_OOP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Path clickedPath;
        int clickedPathNumber;
        GamePhase currentPhase;
        bool isFirstRound = true;
        Game game = new Game();
        City city = new City();
        Mine mine = new Mine();
        Field field = new Field();
        House house = new House();
        double maxWheat, maxBread, maxMaterials, MAX_CITIZENS_START, MAX_HEALTH, MAX_DEFENSE, MAX_HAPPINESS;
        Resource? bread, wheat, materials;
        string gameOutput, userInput, userChoice;
        int WHEAT_TO_BREAD_RATIO = 3;
        Random random = new Random();
        int randNum;

        public string TltpCitizens { get; set; }
        public string TltpHappiness { get; set; }
        public string TltpHealth { get; set; }
        public string TltpDefense { get; set; }
        public string TltpMaterials { get; set; }
        public string TltpWheat { get; set; }
        public string TltpBread { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            TltpCitizens = "This is a tooltip test.";
            // Set the DataContext of the window to this instance
            DataContext = this;

            path1.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path2.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path3.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path3.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path4.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path5.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path6.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path7.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path8.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path9.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path10.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path11.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path12.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path13.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path14.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path15.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path16.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path17.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path18.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path19.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path20.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path21.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path22.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path23.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path24.MouseLeftButtonDown += Paths_MouseLeftButtonDown;
            path25.MouseLeftButtonDown += Paths_MouseLeftButtonDown;

            btnSubmit.Click += BtnSubmit_Click;
            txbxUserInput.KeyDown += txBxUserInput_KeyDown;

            Init_Game();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            userInput = txbxUserInput.Text;
            txbxUserInput.Text = "";
            GameMain();
        }
        private void txBxUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // trigger the click event of the button
                // Trigger the click event of the button
                BtnSubmit_Click(sender, e);
            }
        }

        private void Paths_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedPath = (Path)sender;
            if (gameOutput.Contains("Pick an empty spot"))
            {
                // Extract the number from the canvas name.
                string numberPart = clickedPath.Name.Replace("path", "");
                clickedPathNumber = Int32.Parse(numberPart);

                GameMain();
            }
        }

        public void Init_Game()
        {
            // Setting max values.
            MAX_HAPPINESS = 100;
            MAX_HEALTH = 100;
            MAX_DEFENSE = 100;
            MAX_CITIZENS_START = 100;

            maxMaterials = 200;
            maxWheat = 200;
            maxBread = 200;

            city.MaxFields = 4;
            city.MaxMines = 4;
            city.MaxHouses = 4;
            city.MaxDefenses = 0;

            // Settings starting stats.
            city.Citizens = 50;
            city.Happiness = 100;
            city.Health = 100;
            city.Defense = 0;

            // Setting starting resources.
            city.AddResource("Bread",10);
            bread = city.Resources.FirstOrDefault(r => r.Name == "Bread");
            
            city.AddResource("Wheat", 100);
            wheat = city.Resources.FirstOrDefault(r => r.Name == "Wheat");

            city.AddResource("Materials", 100);
            materials = city.Resources.FirstOrDefault(r => r.Name == "Materials");

            // Setting Citylayout.
            string[] layout = new string[] 
                                {"0", "0", "0", "0", "0",
                                 "0", "F", "E", "M", "0",
                                 "0", "E", "T", "E", "0",
                                 "0", "M", "E", "F", "0",
                                 "0", "0", "0", "0", "0"};
            city.CityLayout = layout;

            // Drawing city layout.
            DrawGameBoard();

            // Set GamePart to 1.
            game.Part = 1;

            // Set GamePhase to started.
            currentPhase = GamePhase.Building;

            // Set Total Rounds and Current Round.
            game.TotalRounds = 4;
            game.Round = 1;

            // Update the progressbars.
            UpdateUI();

            // Start the Game.
            GameMain();
        }
        public void GameMain()
        {
            if (currentPhase != GamePhase.GameOver)
            {
                switch (game.Part)
                {
                    case 1:
                        BuildingPhase();
                        CraftingPhase();
                        DefendingPhase();
                        TransitionPhase();
                        break;
                    case 2:
                        BuildingPhase();
                        CraftingPhase();
                        UpgradingPhase();
                        DefendingPhase();
                        TransitionPhase();
                        break;
                    case 3:
                        BuildingPhase();
                        CraftingPhase();
                        TradingPhase();
                        UpgradingPhase();
                        DefendingPhase();
                        TransitionPhase();
                        break;
                    default:
                        currentPhase = GamePhase.GameOver;
                        break;
                }
                UpdateGameText(gameOutput);
                DrawGameBoard();
                UpdateUI();
            }
        }

        private void BuildingPhase()
        {
            // Code for building phase.
            if(currentPhase == GamePhase.Building) 
            {
                if (game.Part == 1)
                {
                    if (game.TotalRounds != game.Round)
                    {
                        gameOutput = "<<< BUILDING PHASE >>>\n\r";
                        gameOutput += "What building would you like to build?\n\r";
                        gameOutput += $"1. Field (Cost: {field.MaterialsRequired}), 2. Mine (Cost: {mine.MaterialsRequired}), " +
                            $"3. House (Cost: {house.MaterialsRequired})\n\r";
                        if (userInput != null)
                        {
                            switch (userInput)
                            {
                                case "1":
                                    gameOutput += "Pick an empty spot for your new field.\n\r";
                                    if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                    {
                                        if (city.MaxFields != city.TotalFields)
                                        {
                                            city.CityLayout[clickedPathNumber - 1] = "F";
                                            city.RemoveResource("Materials", field.MaterialsRequired);
                                            city.TotalFields++;
                                            if (isFirstRound)
                                                isFirstRound = false;
                                            else
                                                game.Round++;

                                            // Reset vars for following round.
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new field.\n\r", "");
                                            userInput = null;
                                        }
                                        else
                                        {
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new field.\n\r", "");
                                            gameOutput += "Max Fields reached.";
                                            userInput = null;
                                        }
                                    }
                                    clickedPathNumber = 0;
                                    break;
                                case "2":
                                    gameOutput += "Pick an empty spot for your new mine.\n\r";
                                    if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                    {
                                        if (city.MaxMines != city.TotalMines)
                                        {
                                            city.CityLayout[clickedPathNumber - 1] = "M";
                                            city.RemoveResource("Materials", field.MaterialsRequired);
                                            city.TotalMines++;
                                            if (isFirstRound)
                                                isFirstRound = false;
                                            else
                                                game.Round++;

                                            // Reset vars for following round.
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new mine.\n\r", "");
                                            userInput = null;
                                        }
                                        else
                                        {
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new mine.\n\r", "");
                                            gameOutput += "Max mines reached.";
                                            userInput = null;
                                        }
                                    }
                                    clickedPathNumber = 0;
                                    break;
                                case "3":
                                    gameOutput += "Pick an empty spot for your new house.\n\r";
                                    if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                    {
                                        if (city.MaxHouses != city.TotalHouses)
                                        {
                                            city.CityLayout[clickedPathNumber - 1] = "H";
                                            city.RemoveResource("Materials", field.MaterialsRequired);
                                            city.TotalHouses++;
                                            if (isFirstRound)
                                                isFirstRound = false;
                                            else
                                                game.Round++;

                                            // Reset vars for following round.
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new house.\n\r", "");
                                            userInput = null;
                                        }
                                        else
                                        {
                                            gameOutput = gameOutput.Replace("Pick an empty spot for your new house.\n\r", "");
                                            gameOutput += "Max houses reached.";
                                            userInput = null;
                                        }
                                    }
                                    clickedPathNumber = 0;
                                    break;
                                default:
                                    gameOutput += "Pick a valid option (1 , 2 , 3)";
                                    break;
                            }
                        }
                    }
                    if (game.Round > 3)
                    {
                        game.Round = 1;
                        currentPhase = GamePhase.Crafting;
                        AddRecoursesAfterRound();
                    }
                }
            }
        }
        private void CraftingPhase()
        {
            // Code for crafting phase.
            if (currentPhase == GamePhase.Crafting)
            {
                if (game.Part == 1)
                {
                    game.TotalRounds = 1;
                    gameOutput = "<<< CRAFTING PHASE >>>\n\r";
                    gameOutput += "Do you want to craft bread from wheat? (3 -> 1)\n\r";
                    gameOutput += "1. Yes, 2. No\n\r";
                    if (userInput != null)
                    {
                        switch (userInput)
                        {
                            case "1":
                                userChoice = "1";
                                break;
                            case "2":
                                gameOutput = "<<< CRAFTING PHASE >>>\n\r";
                                gameOutput += "Okey, the wheat will be saved for later.\n\r";
                                currentPhase = GamePhase.Defending;
                                userInput = null;
                                AddRecoursesAfterRound();
                                return;
                            default:
                                gameOutput += "Pick a valid option (1 , 2)";
                                break;

                        }
                        if (userChoice == "1")
                        {
                            gameOutput = "<<< CRAFTING PHASE >>>\n\r";
                            gameOutput += "How much bread do you want to craft?\n\r";

                            if (wheat.Amount > (Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO))
                            {
                                if (userInput != "1")
                                {
                                    city.AddResource("Bread", Convert.ToInt32(userInput));
                                    city.RemoveResource("Wheat", Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO);
                                    userInput = null;
                                    userChoice = "0";
                                    AddRecoursesAfterRound();
                                    currentPhase = GamePhase.Defending;
                                    return;
                                }
                            }
                            else
                            {
                                gameOutput = gameOutput.Replace("How much bread do you want to craft?\n\r", "");
                                gameOutput += $"You don't have enough wheat, the maximum amount of bread you can craft is {wheat.Amount / WHEAT_TO_BREAD_RATIO}," +
                                    $" please enter a valid number\r\n";
                                if (wheat.Amount > (Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO))
                                {
                                    city.AddResource("Bread", Convert.ToInt32(userInput));
                                    city.RemoveResource("Wheat", Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO);
                                    userInput = null;
                                    userChoice = "0";
                                    AddRecoursesAfterRound();
                                    currentPhase = GamePhase.Defending;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void UpgradingPhase() 
        { 
            // Code for upgrading phase.
            if(currentPhase == GamePhase.Upgrading) 
            {
                if (game.Part == 1)
                {
                }
            }
        }
        private void DefendingPhase()
        {
            // Code for defending phase.
            if (currentPhase == GamePhase.Defending)
            {
                if (game.Part == 1)
                {
                    if (userInput == null)
                    {
                        gameOutput = "<<< DEFENDING PHASE >>>\n\r";
                        randNum = random.Next(1, 11);
                        if (randNum <= 6)
                        {
                            gameOutput += "A village is attacking you! It's a pretty small village so they don't do much damage.\n\r" +
                                "The mayor retreats his troops as you are still much to strong for his village.\n\r";
                            gameOutput += AI.AIAttack(city, 1);
                            gameOutput += "\r\n\nPress Enter to go to next phase";
                        }
                        if (6 < randNum && randNum <= 9)
                        {
                            gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                                "The mayor calls his troops back a minor loss for your village..\n\r";
                            gameOutput += AI.AIAttack(city, 2);
                            gameOutput += "\r\n\nPress Enter to go to next phase";
                        }
                        if (10 <= randNum)
                        {
                            gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                                    "The mayor calls his troops back a great loss for your village.\n\r";
                            gameOutput += AI.AIAttack(city, 3);
                            gameOutput += "\r\n\nPress Enter to go to next phase";
                        }
                    }
                    else
                    {
                        currentPhase = GamePhase.Transition;
                        userInput = null;
                        game.Part++;
                        return;
                    }
                }
            }
        }

        private void TransitionPhase()
        {
            // Code for rebuilding phase.
            if (currentPhase == GamePhase.Transition)
            {
                gameOutput = $"<<< PART {game.Part} >>>\n\r";
                if (game.Part == 2)
                {
                    gameOutput += "New feature available : Upgrading\n\r";
                    gameOutput += $"Upgrade cost per building : {house.MaterialsRequiredToUpgrade} materials.";
                }
                if (game.Part == 3)
                {
                    gameOutput += "New feature available : Trading\n\r";
                    gameOutput += $"Trade recourses with allied villages.";
                }
            }

        }
        private void TradingPhase()
        {
            // Code for trading phase.
            if (currentPhase == GamePhase.Trading)
            {
                if (game.Part == 1)
                {

                }
            }
        }

        private void AddRecoursesAfterRound() 
        {
            city.AddResource("Materials", mine.ProductionsPerRound * city.TotalMines);
            city.AddResource("Wheat", field.ProductionsPerRound * city.TotalFields);
            city.Citizens += (bread.Amount - (bread.Amount % 4)) / 4;
            bread.Amount -= (bread.Amount - (bread.Amount % 4));
        }

        private void UpdateUI()
        {
            // Update the progressbar values.
            prgBr1.Value = city.Citizens;
            prgBr2.Value = city.Happiness;
            prgBr3.Value = city.Health;
            prgBr4.Value = city.Defense;

            prgBr5.Value = materials.Amount;
            prgBr6.Value = wheat.Amount;
            prgBr7.Value = bread.Amount;

            lblValueMaterialsPerHour.Content = $"{mine.ProductionsPerRound * city.TotalMines}";
            lblValueWheatPerHour.Content = $"{field.ProductionsPerRound * city.TotalFields}";

            // Update/Set the progressbar maximum values.
            prgBr2.Maximum = MAX_HAPPINESS;
            prgBr3.Maximum = MAX_HEALTH;
            prgBr4.Maximum = MAX_DEFENSE;
            prgBr5.Maximum = maxMaterials;
            prgBr6.Maximum = maxWheat;
            prgBr7.Maximum = maxBread;
            prgBr1.Maximum = MAX_CITIZENS_START + (house.HousingSpace * city.TotalHouses);

            // Update the label to display the current round.
            lblRound.Content = $"Round : {game.Round}/{game.TotalRounds}";

            // Update the tooltips to display the maximum values.
            TltpCitizens = $"Max : {MAX_CITIZENS_START + (house.HousingSpace * city.TotalHouses)}";
            TltpHappiness = $"Max : {MAX_HAPPINESS}";
            TltpHealth = $"Max : {MAX_HEALTH}";
            TltpDefense = $"Max : {MAX_DEFENSE}";
            TltpMaterials = $"Max : {maxMaterials}";
            TltpWheat = $"Max : {maxWheat}";
            TltpBread = $"Max : {maxBread}";

            // Change colors of progressbars based on it's value.
            for (int i = 0; i < 7; i++)
            {
                ProgressBar progressBar = (ProgressBar)FindName($"prgBr{i+1}");
                double value = (double)progressBar.Value;
                double maximum = (double)progressBar.Maximum;
                Brush color;
                if (value < (maximum*0.15))
                {
                    color = Brushes.Red;
                }
                else if (value < (maximum * 0.60))
                {
                    color = Brushes.Yellow;
                }
                else
                {
                    color = Brushes.LimeGreen;
                }
                progressBar.Foreground = color;
            }
        }

        private void UpdateGameText(string text)
        {
            txbxGameInput.Text = text;
        }

        private void DrawGameBoard()
        {
            for (int i = 0; i < 25; i++)
            {
                // Find the path control by its name
                Path path = (Path)FindName($"path{i + 1}");

                switch (city.CityLayout[i])
                {
                    case "0":
                        path.Fill = Brushes.White;
                        break;
                    case "E":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#ac7f53");
                        break;
                    case "F":
                        path.Fill = Brushes.DarkOliveGreen;
                        break;
                    case "M":
                        path.Fill = Brushes.DimGray;
                        break;
                    case "T":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#cf3030"); 
                        break;
                    case "D":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#876e91");
                        break;
                    case "H":
                        path.Fill = Brushes.Goldenrod;
                        break;
                }
            }

            // Dictionary to count total buildings of it's type.
            Dictionary<string, int> countDict = new Dictionary<string, int>();
            foreach (string item in city.CityLayout)
            {
                if (countDict.ContainsKey(item))
                {
                    countDict[item]++;
                }
                else
                {
                    countDict[item] = 1;
                }
            }
            if (countDict.ContainsKey("F"))
                city.TotalFields = countDict["F"];
            if (countDict.ContainsKey("M"))
                city.TotalMines = countDict["M"];
            if (countDict.ContainsKey("H"))
                city.TotalHouses = countDict["H"];
            if (countDict.ContainsKey("D"))
                city.TotalDefenses = countDict["D"];
        }
    }
}