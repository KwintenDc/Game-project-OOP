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
        int WORKING_SPACE = 5;

        private int productionPerRound = 20;

        public int ProductionsPerRound
        {
            get { return productionPerRound; }
            set { productionPerRound = value; }
        }
    }
}
