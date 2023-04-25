using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class Mine : Building
    {
        private int productionPerRound = 10;

		public int ProductionsPerRound
		{
			get { return productionPerRound; }
			set { productionPerRound = value; }
		}
	}
}
