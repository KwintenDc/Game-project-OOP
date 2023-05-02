using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class Mine : Building
    {
        private int productionPerRound = 20;

		public int ProductionsPerRound
		{
			get { return productionPerRound; }
			set { productionPerRound = value; }
		}

        private int upgradedProductionsPerRound = 10;

        public int UpgradedProductionsPerRound
        {
            get { return upgradedProductionsPerRound; }
            set { upgradedProductionsPerRound = value; }
        }
    }
}
