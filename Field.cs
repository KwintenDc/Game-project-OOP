using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class Field : Building
    {
        private int productionPerRound = 10;

        public int ProductionsPerRound
        {
            get { return productionPerRound; }
            set { productionPerRound = value; }
        }
    }
}
