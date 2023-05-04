using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        GamePhase currentPhase, nextPhase;
        bool isFirstRound = true;
        bool RoundFinished, Part3Finished, UpgradeStarted;
        Game game = new Game();
        City city = new City();
        Mine mine = new Mine();
        Field field = new Field();
        House house = new House();
        Defense defense= new Defense();
        int maxWheat, maxBread, maxMaterials, MAX_CITIZENS_START, MAX_HEALTH, MAX_DEFENSE, MAX_HAPPINESS;
        Resource? bread, wheat, materials;
        string gameOutput, userInput; 
        int userChoice, randNum, tradeResult ,totalEmptyFields = 20;
        int WHEAT_TO_BREAD_RATIO = 3;
        Random random = new Random(); 
        string[] userChoices = new string[3];

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
            GameMain();
            GameMain();
            GameMain();
            GameMain();

        }
        private void txBxUserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Trigger the click event of the button
                BtnSubmit_Click(sender, e);
            }
        }

        private void Paths_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickedPath = (Path)sender;
            if (gameOutput.Contains("Pick an empty spot") || gameOutput.Contains("Pick a building to upgrade."))
            {
                // Extract the number from the canvas name.
                string numberPart = clickedPath.Name.Replace("path", "");
                clickedPathNumber = Int32.Parse(numberPart);
            }
            GameMain();
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
                        if(!Part3Finished || (totalEmptyFields != 0))
                            BuildingPhase();
                        CraftingPhase();
                        TradingPhase();
                        UpgradingPhase();
                        DefendingPhase();
                        if(!Part3Finished)
                            TransitionPhase();
                        break;
                    default:
                        currentPhase = GamePhase.GameOver;
                        break;
                }
                if(currentPhase == GamePhase.Wait)
                    AddRecoursesAfterRound();
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
                switch (game.Part) 
                {
                    case 1:
                        game.TotalRounds = 4;
                        break;
                    case 2:
                        game.TotalRounds = 7;
                        break;
                    case 3:
                        game.TotalRounds = 9;
                        break;
                }
                if (game.TotalRounds != game.Round)
                {
                    gameOutput = "<<< BUILDING PHASE >>>\n\r";
                    gameOutput += "What building would you like to build?\n\r";
                    gameOutput += $"0. Stop building phase \r1. Field (Cost: {field.MaterialsRequired}) \r2. Mine (Cost: {mine.MaterialsRequired})" +
                        $"\r3. House (Cost: {house.MaterialsRequired})\r";
                    if (game.Part != 1)
                        gameOutput += $"4. Defense (Cost: {defense.MaterialsRequired})\n\r";
                    if (userInput != null)
                    {
                        switch (userInput)
                        {
                            case "0":
                                game.Round = game.TotalRounds;
                                userInput = null;
                                break;
                            case "1":
                                gameOutput += "\nPick an empty spot for your new field.\n\r";
                                if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                {
                                    if (city.MaxFields != city.TotalFields)
                                    {
                                        city.CityLayout[clickedPathNumber - 1] = "F";
                                        materials.Amount -= field.MaterialsRequired;
                                        city.TotalFields++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;

                                        // Reset vars for following round.
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new field.\n\r", "");
                                        userInput = null;
                                        totalEmptyFields--;
                                    }
                                    else
                                    {
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new field.\n\r", "");
                                        gameOutput += "Max Fields reached.";
                                        userInput = null;
                                    }
                                }
                                clickedPathNumber = 0;
                                break;
                            case "2":
                                gameOutput += "\nPick an empty spot for your new mine.\n\r";
                                if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                {
                                    if (city.MaxMines != city.TotalMines)
                                    {
                                        city.CityLayout[clickedPathNumber - 1] = "M";
                                        materials.Amount -= field.MaterialsRequired;
                                        city.TotalMines++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;

                                        // Reset vars for following round.
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new mine.\n\r", "");
                                        userInput = null;
                                        totalEmptyFields--;
                                    }
                                    else
                                    {
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new mine.\n\r", "");
                                        gameOutput += "Max mines reached.";
                                        userInput = null;
                                    }
                                }
                                clickedPathNumber = 0;
                                break;
                            case "3":
                                gameOutput += "\nPick an empty spot for your new house.\n\r";
                                if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                {
                                    if (city.MaxHouses != city.TotalHouses)
                                    {
                                        city.CityLayout[clickedPathNumber - 1] = "H";
                                        materials.Amount -= field.MaterialsRequired;
                                        city.TotalHouses++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;

                                        // Reset vars for following round.
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new house.\n\r", "");
                                        userInput = null;
                                        totalEmptyFields--;
                                    }
                                    else
                                    {
                                        gameOutput = gameOutput.Replace("\nPick an empty spot for your new house.\n\r", "");
                                        gameOutput += "Max houses reached.";
                                        userInput = null;
                                    }
                                }
                                clickedPathNumber = 0;
                                break;
                            case "4":
                                if(game.Part != 1) 
                                {
                                    gameOutput += "\nPick an empty spot for your new defensive building.\n\r";
                                    if ((clickedPathNumber != 0) && (city.CityLayout[clickedPathNumber - 1] == "E"))
                                    {
                                        if (city.MaxDefenses != city.TotalDefenses)
                                        {
                                            city.CityLayout[clickedPathNumber - 1] = "D";
                                            materials.Amount -= defense.MaterialsRequired;
                                            city.TotalDefenses++;
                                            if (isFirstRound)
                                                isFirstRound = false;
                                            else
                                                game.Round++;

                                            // Reset vars for following round.
                                            gameOutput = gameOutput.Replace("\nPick an empty spot for your new defensive building.\n\r", "");
                                            userInput = null;
                                            totalEmptyFields--;
                                        }
                                        else
                                        {
                                            gameOutput = gameOutput.Replace("\nPick an empty spot for your new defensive building.\n\r", "");
                                            gameOutput += "Max defensive buildings reached.";
                                            userInput = null;
                                        }
                                    }
                                    clickedPathNumber = 0;
                                }
                                else
                                    gameOutput += "\nPick a valid option (0 , 1 , 2 , 3)";
                                break;
                            default:
                                if(game.Part == 1)
                                    gameOutput += "\nPick a valid option (0 , 1 , 2 , 3)";
                                else
                                    gameOutput += "\nPick a valid option (0, 1, 2, 3, 4)";
                                break;
                        }
                    }
                }
                if ((game.Round > game.TotalRounds - 1) || (materials.Amount < 20))
                {
                    isFirstRound = true;
                    nextPhase = GamePhase.Crafting;
                    RoundFinished = true;
                    game.Round = game.TotalRounds;
                    AddRecoursesAfterRound();
                }
                
            }
        }
        private void CraftingPhase()
        {
            // Code for crafting phase.
            if (currentPhase == GamePhase.Crafting)
            {  
                game.TotalRounds = 1;
                gameOutput = "<<< CRAFTING PHASE >>>\n\r";
                gameOutput += "Do you want to craft bread from wheat? (3 -> 1)\n\r";
                gameOutput += "1. Yes \r2. No\n\r";
                if (userInput != null)
                {
                    switch (userInput)
                    {
                        case "1":
                            userChoice = 1;
                            break;
                        case "2":
                            if (game.Part == 1)
                                nextPhase = GamePhase.Defending;
                            else if (game.Part == 2)
                                nextPhase = GamePhase.Upgrading;
                            else if (game.Part == 3)
                                nextPhase = GamePhase.Trading;
                            userInput = null;
                            RoundFinished = true;
                            AddRecoursesAfterRound();
                            return;
                        default:
                            gameOutput += "Pick a valid option (1 , 2)";
                            break;

                    }
                    if (userChoice == 1)
                    {
                        gameOutput = "<<< CRAFTING PHASE >>>\n\r";
                        gameOutput += "How much bread do you want to craft?\n\r";

                        if (wheat.Amount >= (Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO))
                        {
                            if (userInput != "1")
                            {
                                bread.Amount += Convert.ToInt32(userInput);
                                wheat.Amount -= Convert.ToInt32(userInput) * WHEAT_TO_BREAD_RATIO;
                                userInput = null;
                                userChoice = 0;
                                RoundFinished = true;
                                if (game.Part == 1)
                                    nextPhase = GamePhase.Defending;
                                else if (game.Part == 2)
                                    nextPhase = GamePhase.Upgrading;
                                else if (game.Part == 3)
                                    nextPhase = GamePhase.Trading;
                                AddRecoursesAfterRound();
                                return;
                            }
                        }
                        else
                        {
                            gameOutput = gameOutput.Replace("How much bread do you want to craft?\n\r", "");
                            gameOutput += $"You don't have enough wheat, the maximum amount of bread you can craft is {wheat.Amount / WHEAT_TO_BREAD_RATIO}," +
                                $" please enter a valid number\r\n";
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
                gameOutput = "<<< UPGRADING PHASE >>>\n\r";
                gameOutput += "How many buildings would you like to upgrade?";
                if (!UpgradeStarted)
                {
                    if ((materials.Amount / mine.MaterialsRequiredToUpgrade) < (city.TotalDefenses + city.TotalMines + city.TotalFields))
                        gameOutput += $" (Max : {materials.Amount / mine.MaterialsRequiredToUpgrade})\r\n";
                    else
                        gameOutput += $" (Max : {city.TotalDefenses + city.TotalMines + city.TotalFields - city.TotalUpgradedDefenses - city.TotalUpgradedFields - city.TotalUpgradedMines})\r\n";
                }
                else
                {
                    gameOutput += $" (Max : {userChoice})\r\n";
                }
                if((materials.Amount / mine.MaterialsRequiredToUpgrade) <= 0 || (city.TotalDefenses + city.TotalMines + city.TotalFields - city.TotalUpgradedDefenses - city.TotalUpgradedFields - city.TotalUpgradedMines) <= 0)
                {
                    UpgradeStarted = false;
                    userChoice = 0;
                    userInput = null;
                    nextPhase = GamePhase.Defending;
                    isFirstRound = true;
                    clickedPathNumber = 0;
                    RoundFinished = true;
                    game.TotalRounds = 1;
                    AddRecoursesAfterRound();
                }
                if(userInput != null)
                {
                    userChoice = Convert.ToInt32(userInput);
                    if (userChoice <= (materials.Amount / mine.MaterialsRequiredToUpgrade) || UpgradeStarted)
                    {
                        game.TotalRounds = userChoice;
                        if(game.Round != game.TotalRounds)
                        {
                            gameOutput += "Pick a building to upgrade.\n\r";
                            UpgradeStarted= true;
                            if (clickedPathNumber != 0)
                            {
                                switch (city.CityLayout[clickedPathNumber - 1])
                                {
                                    case "F":
                                        city.CityLayout[clickedPathNumber - 1] = "Fu";
                                        materials.Amount -= field.MaterialsRequiredToUpgrade;
                                        city.TotalUpgradedFields++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;
                                        break;
                                    case "M":
                                        city.CityLayout[clickedPathNumber - 1] = "Mu";
                                        materials.Amount -= mine.MaterialsRequiredToUpgrade;
                                        city.TotalUpgradedMines++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;
                                        break;
                                    case "D":
                                        city.CityLayout[clickedPathNumber - 1] = "Du";
                                        materials.Amount -= defense.MaterialsRequiredToUpgrade;
                                        city.TotalUpgradedDefenses++;
                                        if (isFirstRound)
                                            isFirstRound = false;
                                        else
                                            game.Round++;
                                        break;
                                    default:
                                        gameOutput.Replace("Pick building to upgrade.\n\r", "");
                                        gameOutput += "Choose a valid building (field , mine , defense)";
                                        break;
                                }
                            }
                        }
                        if (game.Round > game.TotalRounds - 1)
                        {
                            UpgradeStarted = false;
                            userChoice = 0;
                            userInput = null;
                            nextPhase = GamePhase.Defending;
                            isFirstRound = true;
                            clickedPathNumber = 0;
                            RoundFinished = true;
                            game.TotalRounds = 1;
                            AddRecoursesAfterRound();
                            return;
                        }
                    }
                    else
                    {
                        userChoice = 0;
                        userInput = null;
                        gameOutput += "Choose a valid amount";
                    }
                }
            }
        }
        private void DefendingPhase()
        {
            // Code for defending phase.
            if (currentPhase == GamePhase.Defending)
            {
                if (isFirstRound)
                {
                    gameOutput = "<<< DEFENDING PHASE >>>\n\r";
                    randNum = random.Next(1, 11);
                    if (randNum <= 6)
                    {
                        gameOutput += "A village is attacking you! It's a pretty small village so they don't do much damage.\n\r" +
                            "The mayor retreats his troops as you are still much to strong for his village.\n\r";
                        gameOutput += AI.Attack(city, 1, wheat, materials, bread);
                        gameOutput += "\r\n\n1. Go to next phase";
                    }
                    if (6 < randNum && randNum <= 9)
                    {
                        gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                            "The mayor calls his troops back a minor loss for your village..\n\r";
                        gameOutput += AI.Attack(city, 2, wheat, materials, bread);
                        gameOutput += "\r\n\n1. Go to next phase";
                    }
                    if (10 <= randNum)
                    {
                        gameOutput += "A village is attacking you! It's a big village so they do much damage.\n\r" +
                                "The mayor calls his troops back a great loss for your village.\n\r";
                        gameOutput += AI.Attack(city, 3, wheat, materials, bread);
                        gameOutput += "\r\n\n1. Go to next phase";
                    }
                    isFirstRound= false;
                }
                if(userInput != null)
                {
                    currentPhase = GamePhase.Transition;
                    userInput = null;
                    if (Part3Finished && (totalEmptyFields == 0))
                    {
                        currentPhase = GamePhase.Crafting;
                        isFirstRound = true;
                    }
                    else if(Part3Finished && (totalEmptyFields != 0))
                    {
                        currentPhase = GamePhase.Building;
                        isFirstRound = true;
                    }
                    return;
                }
            }
        }
        private void TradingPhase()
        {
            // Code for trading phase.
            if (currentPhase == GamePhase.Trading)
            {
                gameOutput = "<<< Trading phase >>>\n\r";
                gameOutput += "Do you want to trade with a friendly village?\n\r";
                gameOutput += "1. Yes \r2. No\n\r";
                if (userInput != null)
                {
                    if (userChoice == 0)
                    {
                        switch (userInput)
                        {
                            case "1":
                                userInput = null;
                                userChoice = 1;
                                break;
                            case "2":
                                nextPhase = GamePhase.Upgrading;
                                userInput = null;
                                RoundFinished = true;
                                AddRecoursesAfterRound();
                                return;
                            default:
                                gameOutput += "Pick a valid option (1 , 2)";
                                break;
                        }
                    }
                }
                if (userChoice == 1)
                {
                    gameOutput = "<<< Trading phase >>>\n\r";
                    gameOutput += "What resource do you want to trade?\n\r";
                    gameOutput += "1. Materials \r2. Wheat \r3. Bread\r\n";
                    if (userInput != null)
                    {
                        switch (userInput)
                        {
                            case "1":
                                userChoice = 2;
                                userChoices[0] = "Materials";
                                userInput = null;
                                break;
                            case "2":
                                userChoice = 2;
                                userChoices[0] = "Wheat";
                                userInput = null;
                                break;
                            case "3":
                                userChoice = 2;
                                userChoices[0] = "Bread";
                                userInput = null;
                                break;
                            default:
                                gameOutput += "Pick a valid option (1 , 2 , 3)";
                                break;
                        }
                    }
                }
                if(userChoice == 2)
                {
                    gameOutput = "<<< Trading phase >>>\n\r";
                    gameOutput += $"How much {userChoices[0]} do you want to give?\n\r";
                    if(userInput != null)
                    {
                        userChoices[1] = userInput;
                        userChoice = 3;
                        userInput = null;
                    } 
                }
                if(userChoice == 3)
                {
                    gameOutput = "<<< Trading phase >>>\n\r";
                    gameOutput += "What resource do you want to receive?\n\r";
                    gameOutput += "1. Materials \r2. Wheat \r3. Bread\r\n";
                    if (userInput != null)
                    {
                        switch (userInput)
                        {
                            case "1":
                                userChoice = 4;
                                userChoices[2] = "Materials";
                                userInput = null;
                                break;
                            case "2":
                                userChoice = 4;
                                userChoices[2] = "Wheat";
                                userInput = null;
                                break;
                            case "3":
                                userChoice = 4;
                                userChoices[2] = "Bread";
                                userInput = null;
                                break;
                            default:
                                gameOutput += "Pick a valid option (1 , 2 , 3)";
                                break;
                        }
                    }
                }
                if(userChoice == 4)
                {
                    gameOutput = "<<< Trading phase >>>\n\r";
                    tradeResult = Convert.ToInt32(AI.Trade(userChoices));
                    gameOutput += $"The village accepted the {userChoices[1]} {userChoices[0]} and gave you {tradeResult} {userChoices[2]} in return.\n\r";
                    gameOutput += "1. Go to next phase";
                    switch (userChoices[0])
                    {
                        case "Materials":
                            materials.Amount -= Convert.ToInt32(userChoices[1]);
                            break;
                        case "Wheat":
                            wheat.Amount -= Convert.ToInt32(userChoices[1]);
                            break;
                        case "Bread":
                            bread.Amount -= Convert.ToInt32(userChoices[1]);
                            break;
                    }
                    switch(userChoices[2])
                    {
                        case "Materials":
                            materials.Amount += tradeResult;
                            userChoice = 5;
                            break;
                        case "Wheat":
                            wheat.Amount += tradeResult;
                            userChoice = 5;
                            break;
                        case "Bread":
                            bread.Amount += tradeResult;
                            userChoice = 5;
                            break;
                    }
                }
                if (userChoice == 5)
                {
                    gameOutput = "<<< Trading phase >>>\n\r";
                    gameOutput += $"The village accepted the {userChoices[1]} {userChoices[0]} and gave you {tradeResult} {userChoices[2]} in return.\n\r";
                    gameOutput += "1. Go to next phase";
                    if (userInput != null)
                    {
                        nextPhase = GamePhase.Upgrading;
                        userInput = null;
                        RoundFinished = true;
                        userChoice = 0;
                        AddRecoursesAfterRound();
                    }
                }
            }
        }
        private void TransitionPhase()
        {
            // Code for rebuilding phase.
            if (currentPhase == GamePhase.Transition)
            {
                if(game.Part != 3) 
                {
                    gameOutput = $"<<< PART {game.Part + 1} >>>\n\r";
                    if (game.Part == 1)
                    {
                        gameOutput += "New feature available : Upgrading\n\r";
                        gameOutput += $"Upgrade cost per building : {house.MaterialsRequiredToUpgrade} materials.\n\r";
                    }
                    if (game.Part == 2)
                    {
                        gameOutput += "New feature available : Trading\n\r";
                        gameOutput += $"Trade recourses with allied villages.\n\r";
                    }
                    gameOutput += "1. Go to next Part";
                    if (userInput != null)
                    {
                        if (game.Part != 3)
                            game.Part++;
                        currentPhase = GamePhase.Building;
                        userInput = null;
                        isFirstRound = true;
                        if (game.Part == 2)
                        {
                            int[] indexes = { 0, 1, 2, 3, 4, 5, 9, 10, 14, 15, 19, 20, 21, 22, 23, 24 };
                            foreach (int i in indexes)
                            {
                                city.CityLayout[i] = "E";
                            }
                            city.MaxHouses = 10;
                            city.MaxFields = 10;
                            city.MaxMines = 10;
                            city.MaxDefenses = 5;
                        }
                    }
                }
                else
                {
                    gameOutput = $"<<< PART {game.Part} >>>\n\r";
                    gameOutput += "You have completed all parts of Empire Expansion, you can choose to save and exit or keep playing to reach a goal you set for yourself.";
                    gameOutput += "1. Go to next phase.";
                    if(userInput != null)
                    {
                        Part3Finished = true;
                        if(totalEmptyFields == 0)
                            currentPhase = GamePhase.Crafting;
                        else
                            currentPhase = GamePhase.Building;
                        userInput = null;
                        isFirstRound = true;
                    }
                }
            }
        }

        private void AddRecoursesAfterRound()
        {
            if (RoundFinished)
            {
                currentPhase = GamePhase.Wait;
                gameOutput = "<<< Recource Update >>>\n\r";
                gameOutput += $"New Materials : {mine.ProductionsPerRound * city.TotalMines}\r";
                gameOutput += $"New Wheat : {field.ProductionsPerRound * city.TotalFields}\r";
                if(bread.Amount > 4)
                {
                    gameOutput += $"Bread to be consumed: {(bread.Amount - (bread.Amount % 4))}\r";
                    gameOutput += $"Possible new citizens: {(bread.Amount - (bread.Amount % 4)) / 4}\n\r";
                    gameOutput += "1. Distribute bread for new citzens \r2. Don't use bread.\n\r";
                }
                else
                {
                    gameOutput += $"Not enough bread for new citizens\n\r";
                    gameOutput += "1. Go to next phase.\n\r";
                }
                switch (userInput)
                {
                    case "1":
                        materials.Amount += (mine.ProductionsPerRound * city.TotalMines) + (mine.UpgradedProductionsPerRound * city.TotalUpgradedMines);
                        wheat.Amount += (field.ProductionsPerRound * city.TotalFields) + (field.UpgradedProductionsPerRound * city.TotalUpgradedFields);
                        if (bread.Amount > 4)
                        {
                            city.Citizens += (bread.Amount - (bread.Amount % 4)) / 4;
                            bread.Amount -= (bread.Amount - (bread.Amount % 4));
                        }
                        currentPhase = nextPhase;
                        RoundFinished = false;
                        game.Round = 1;
                        userInput = null;
                        break;
                    case "2":
                        materials.Amount += (mine.ProductionsPerRound * city.TotalMines) + (mine.UpgradedProductionsPerRound * city.TotalUpgradedMines);
                        wheat.Amount += (field.ProductionsPerRound * city.TotalFields) + (field.UpgradedProductionsPerRound * city.TotalUpgradedFields);
                        currentPhase = nextPhase;
                        RoundFinished = false;
                        game.Round = 1;
                        userInput = null;
                        break;
                    case null:
                        break;
                    default:
                        gameOutput += "Pick a valid option. (1 , 2)";
                        if (gameOutput.Contains("Not enough bread for new citizens"))
                            gameOutput.Replace("Pick a valid option. (1 , 2)", "Pick a valid option. (1)");
                        break;
                }
            }
        }
        private void UpdateUI()
        {
            // Check if values are higher/lower then maximum/minimum.
            if (city.Citizens < 0)
                city.Citizens = 0;
            else if (city.Citizens > (MAX_CITIZENS_START + (house.HousingSpace * city.TotalHouses)))
                city.Citizens = MAX_CITIZENS_START + (house.HousingSpace * city.TotalHouses);

            if(city.Happiness < 0)
                city.Happiness = 0;
            else if(city.Happiness > MAX_HAPPINESS)
                city.Happiness = MAX_HAPPINESS;

            if (city.Health < 0)
                city.Health = 0;
            else if (city.Health > MAX_HEALTH)
                city.Health = MAX_HEALTH;

            if (city.Defense < 0)
                city.Defense = 0;
            else if (city.Defense > MAX_DEFENSE)
                city.Defense = MAX_DEFENSE;

            if (materials.Amount < 0)
                materials.Amount = 0;
            else if (materials.Amount > maxMaterials)
                materials.Amount = maxMaterials;

            if (wheat.Amount < 0)
                wheat.Amount = 0;
            else if (wheat.Amount > maxWheat)
                wheat.Amount = maxWheat;

            if (bread.Amount < 0)
                bread.Amount = 0;
            else if (bread.Amount > maxBread)
                bread.Amount = maxBread;

            // Update the progressbar values.
            prgBr1.Value = city.Citizens;
            prgBr2.Value = city.Happiness;
            prgBr3.Value = city.Health;

            city.Defense = (city.TotalDefenses * defense.DefensePerBuilding) + (city.TotalUpgradedDefenses * defense.UpgradedDefensePerBuilding);
            prgBr4.Value = city.Defense;

            prgBr5.Value = materials.Amount;
            prgBr6.Value = wheat.Amount;
            prgBr7.Value = bread.Amount;

            lblValueMaterialsPerHour.Content = $"{(mine.ProductionsPerRound * city.TotalMines) + (mine.UpgradedProductionsPerRound * city.TotalUpgradedMines)}";
            lblValueWheatPerHour.Content = $"{(field.ProductionsPerRound * city.TotalFields) + (field.UpgradedProductionsPerRound * city.TotalUpgradedFields)}";

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
                    case "Fu":
                        path.Fill = Brushes.DarkOliveGreen;
                        path.StrokeThickness = 3;
                        break;
                    case "M":
                        path.Fill = Brushes.DimGray;
                        break;
                    case "Mu":
                        path.Fill = Brushes.DimGray;
                        path.StrokeThickness = 3;
                        break;
                    case "T":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#cf3030"); 
                        break;
                    case "D":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#876e91");
                        break;
                    case "Du":
                        path.Fill = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFromString("#876e91");
                        path.StrokeThickness = 3;
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
            {
                city.TotalFields = countDict["F"];
                if (countDict.ContainsKey("Fu"))
                    city.TotalFields += countDict["Fu"];
            }
            if (countDict.ContainsKey("M"))
            {
                city.TotalMines = countDict["M"];
                if (countDict.ContainsKey("Mu"))
                    city.TotalMines += countDict["Mu"];
            }
            if (countDict.ContainsKey("H"))
                city.TotalHouses = countDict["H"];
            if (countDict.ContainsKey("D"))
            {
                city.TotalDefenses = countDict["D"];
                if (countDict.ContainsKey("Du"))
                    city.TotalDefenses += countDict["Du"];
            }
        }
    }
}
