using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Game_project_OOP
{
    class Building
    {
        private int materialsRequired = 20;

        public int MaterialsRequired
        {
            get { return materialsRequired; }
        }

        private int materialsRequiredToUpgrade = 30;

        public int MaterialsRequiredToUpgrade
        {
            get { return materialsRequiredToUpgrade; }
            set { materialsRequiredToUpgrade = value; }
        }

    }
}
