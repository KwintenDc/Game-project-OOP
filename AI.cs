using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    internal class AI
    {
        public static string AIAttack(City city, int strength)
        {
            Random rand = new Random();

            // Calculate the damage based on the strength and a random factor.
            int damage = strength * (rand.Next(50, 100) / 10);
            if (city.Defense > 0)
                damage -= (city.Defense / 10);

            // Reduce the city's happiness and number of citizens based on the damage.
            city.Happiness -= damage * 2;
            city.Health-= damage;
            city.Citizens -= damage;

            // Reduce the materials based on the damage.
            city.RemoveResource("Wheat", damage);
            city.RemoveResource("Bread", damage);
            city.RemoveResource("Materials", damage);

            return $"Citizens lost: {damage}, Happiness lost: {damage *2}, Recourses lost: {damage}";
        }
    }
}
