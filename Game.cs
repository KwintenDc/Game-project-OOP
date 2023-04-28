using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class Game
    {
		private string saveGame;
        
		public string SaveGame
		{
			get { return saveGame; }
			set { saveGame = value; }
		}

        public int Round { get; set; }
        public int TotalRounds { get; set; }
        public int Part { get; set; }
    }
    enum GamePhase
    {
       Building,
       Crafting,
       Defending,
       Trading,
       Upgrading,
       Transition,
       GameOver
    }
}
