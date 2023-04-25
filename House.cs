using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class House : Building
    {
		private int housingSpace = 10;

		public int HousingSpace
		{
			get { return housingSpace; }
			set { housingSpace = value; }
		}

	}
}
