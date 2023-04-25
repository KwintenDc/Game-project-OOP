using System;
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
        Canvas clickedCanvas;
        int clickedCanvasNumber;
        GamePhase currentPhase;
        bool isFirstRound = true;
        Game game = new Game();
        City city = new City();
        Mine mine = new Mine();
        Field field = new Field();
        House house = new House();
        double maxWheat, maxBread, maxMaterials, maxCitizens, MAX_HEALTH, MAX_DEFENSE, MAX_HAPPINESS;
        Resource? bread, wheat, materials;
        string gameOutput, userInput, userChoice;
        int WHEAT_TO_BREAD_RATIO = 3;
        Random random = new Random();
        int randNum;
        public MainWindow()
        {
            InitializeComponent();

            // Link all click events to 1 method.
            canvas1.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas2.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas3.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas4.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas5.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            canvas6.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas7.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas8.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas9.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas10.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            canvas11.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas12.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas13.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas14.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas15.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            canvas16.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas17.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas18.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas19.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas20.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            canvas21.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas22.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas23.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas24.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas25.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;

            btnSubmit.Click += BtnSubmit_Click;

            Init_Game();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            userInput = txbxUserInput.Text;
            txbxUserInput.Text = "";
            GameMain();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedCanvas = (Canvas)sender;

            if (gameOutput.Contains("Pick an empty spot"))
            {
                // Extract the number from the canvas name.
                string numberPart = clickedCanvas.Name.Replace("canvas", "");
                clickedCanvasNumber = Int32.Parse(numberPart);
            }

            GameMain();
        }

        public void Init_Game()
        {
            // Setting max values.
            MAX_HAPPINESS = 100;
            MAX_HEALTH = 100;
            MAX_DEFENSE = 100;

            maxMaterials = 500;
            maxWheat = 500;
            maxBread = 500;
            maxCitizens = 100;

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
            city.AddResource("Bread",100);
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

            // Drawing city layout
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
                        RepairingPhase();
                        break;
                    case 2:
                        BuildingPhase();
                        CraftingPhase();
                        UpgradingPhase();
                        DefendingPhase();
                        RepairingPhase();
                        break;
                    case 3:
                        BuildingPhase();
                        CraftingPhase();
                        TradingPhase();
                        UpgradingPhase();
                        DefendingPhase();
                        RepairingPhase();
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
                if (game.TotalRounds!= game.Round)
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
                                if((clickedCanvasNumber != 0) && (city.CityLayout[clickedCanvasNumber - 1] == "E")) 
                                {
                                    if (city.MaxFields != city.TotalFields)
                                    {
                                        city.CityLayout[clickedCanvasNumber - 1] = "F";
                                        city.RemoveResource("Materials", field.MaterialsRequired);
                                        city.TotalFields++;
                                        if(isFirstRound)
                                            isFirstRound= false;
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
                                clickedCanvasNumber = 0;
                                break;
                            case "2":
                                gameOutput += "Pick an empty spot for your new mine.\n\r";
                                if ((clickedCanvasNumber != 0) && (city.CityLayout[clickedCanvasNumber - 1] == "E"))
                                {
                                    if (city.MaxMines != city.TotalMines)
                                    {
                                        city.CityLayout[clickedCanvasNumber - 1] = "M";
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
                                clickedCanvasNumber = 0;
                                break;
                            case "3":
                                gameOutput += "Pick an empty spot for your new house.\n\r";
                                if ((clickedCanvasNumber != 0) && (city.CityLayout[clickedCanvasNumber - 1] == "E"))
                                {
                                    if (city.MaxHouses != city.TotalHouses)
                                    {
                                        city.CityLayout[clickedCanvasNumber - 1] = "H";
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
                                clickedCanvasNumber = 0;
                                break;
                            default:
                                gameOutput += "Pick a valid option (1 , 2 , 3)";
                                break;
                        }
                    }
                }
                if(game.Round > 3)
                {
                    game.Round = 1;
                    currentPhase = GamePhase.Crafting;
                    city.AddResource("Materials", mine.ProductionsPerRound * city.TotalMines);
                    city.AddResource("Wheat", field.ProductionsPerRound * city.TotalFields);
                }
            }
        }
        private void CraftingPhase()
        {
            // Code for crafting phase.
            if (currentPhase == GamePhase.Crafting)
            {
                game.TotalRounds = 1;
                gameOutput = "";
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
                            city.AddResource("Materials", mine.ProductionsPerRound * city.TotalMines);
                            city.AddResource("Wheat", field.ProductionsPerRound * city.TotalFields);
                            userInput = null;
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
                                city.AddResource("Materials", mine.ProductionsPerRound * city.TotalMines);
                                city.AddResource("Wheat", field.ProductionsPerRound * city.TotalFields);
                                userChoice = "0";
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
                                city.AddResource("Materials", mine.ProductionsPerRound * city.TotalMines);
                                city.AddResource("Wheat", field.ProductionsPerRound * city.TotalFields);
                                currentPhase = GamePhase.Defending;
                                return;
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
            }
        }
        private void DefendingPhase()
        {
            // Code for defending phase.
            if (currentPhase == GamePhase.Defending)
            {
                gameOutput = "<<< DEFENDING PHASE >>>\n\r";
                randNum = random.Next(1,11);
                if(randNum <= 6) 
                {
                    gameOutput += "A village is attacking you! It's a pretty small village so they don't do much damage.\n\r" +
                        "The mayor retreats his troops as you are still much to strong for his village.\n\r";
                }
                if (6 < randNum && randNum <= 9)
                {
                    gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                        "The mayor retreats his troops after a great battle.\n\r";
                }
                if (10 <= randNum)
                {
                    gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                            "The mayor retreats his troops after a great battle.\n\r";
                }
            }
        }

        private void RepairingPhase()
        {
            // Code for rebuilding phase.
            if (currentPhase == GamePhase.Repairing)
            {
            }

        }
        private void TradingPhase()
        {
            // Code for trading phase.
            if (currentPhase == GamePhase.Trading)
            {
            }
        }

        private void UpdateUI()
        {
            prgBrCitizens.Value = city.Citizens;
            prgBrHappiness.Value = city.Happiness;
            prgBrHealth.Value = city.Health;
            prgBrDefense.Value = city.Defense;

            prgBrMaterials.Value = materials.Amount;
            prgBrWheat.Value = wheat.Amount;
            prgBrBread.Value = bread.Amount;

            prgBrMaterialsPerHour.Value = mine.ProductionsPerRound * city.TotalMines;
            prgBrWheatPerHour.Value = field.ProductionsPerRound * city.TotalFields;

            prgBrHappiness.Maximum = MAX_HAPPINESS;
            prgBrHealth.Maximum = MAX_HEALTH;
            prgBrDefense.Maximum = MAX_DEFENSE;
            prgBrMaterials.Maximum = maxMaterials;
            prgBrWheat.Maximum = maxWheat;
            prgBrBread.Maximum = maxBread;
            prgBrCitizens.Maximum = maxCitizens;

            lblRound.Content = $"Round : {game.Round}/{game.TotalRounds}";
        }

        private void UpdateGameText(string text)
        {
            txbxGameInput.Text = text;
        }

        private void DrawGameBoard()
        {
            for (int i = 0; i < 25; i++)
            {
                // Find the canvas control by its name
                Canvas canvas = (Canvas)FindName($"canvas{i + 1}");

                switch (city.CityLayout[i]) 
                {
                    case "0":
                        canvas.Background = Brushes.White;
                        break;
                    case "E":
                        canvas.Background = Brushes.RosyBrown;
                        break;
                    case "F":
                        canvas.Background = Brushes.DarkOliveGreen;
                        break;
                    case "M":
                        canvas.Background = Brushes.DimGray;
                        break;
                    case "T":
                        canvas.Background = Brushes.Goldenrod;
                        break;
                    case "D":
                        canvas.Background = Brushes.OrangeRed;
                        break;
                    case "H":
                        canvas.Background = Brushes.SlateBlue;
                        break;
                }
            }
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
