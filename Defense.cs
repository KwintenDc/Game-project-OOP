using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    internal class Defense : Building
    {
		private int defensePerBuilding = 20;

		public int DefensePerBuilding
		{
			get { return defensePerBuilding; }
			set { defensePerBuilding = value; }
		}
		private int upgradedDefensePerBuilding = 5;

		public int UpgradedDefensePerBuilding
		{
			get { return upgradedDefensePerBuilding; }
			set { upgradedDefensePerBuilding = value; }
		}

	}
}
