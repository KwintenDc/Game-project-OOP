using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    internal class AI
    {
        public static string AIAttack(City city, int strength, Resource wheat, Resource materials, Resource bread)
        {
            Random rand = new Random();

            // Define the minimum and maximum values.
            int maxDamage = 30;
            int minDamage = 2;
            int minHappinessReduction = 1;
            int maxHappinessReduction = 2;
            int minCitizenReduction = 1;
            int maxCitizenReduction = 3;
            int happinessReduction, citizenReduction;
            int wheatReduction, breadReduction, materialsReduction;

            // Generate a random amount of damage.
            int damage = strength * (rand.Next(50, 100) / 10);

            if (city.Defense > 0)
            {
                int defenseFactor = city.Defense / 10;
                damage -= defenseFactor;
                if (damage < minDamage)
                {
                    damage = minDamage;
                }
            }

            if (damage > maxDamage)
            {
                damage = maxDamage;
            }

            // Reduce the city's happiness and number of citizens based on the damage.
            happinessReduction = rand.Next(minHappinessReduction, maxHappinessReduction + 1) * damage;
            citizenReduction = rand.Next(minCitizenReduction, maxCitizenReduction + 1) * (damage/2);

            city.Happiness -= happinessReduction;
            city.Health -= damage;
            city.Citizens -= citizenReduction;

            // Reduce the materials based on the damage.
            wheatReduction = (rand.Next(10,150)/10)*(damage/4);
            breadReduction = (rand.Next(10, 80) / 10) * (damage / 4);
            materialsReduction = (rand.Next(10, 100) / 10) * (damage / 4);

            if(wheatReduction < wheat.Amount)
                city.RemoveResource("Wheat", wheatReduction);
            else
                city.RemoveResource("Wheat", wheat.Amount);

            if (breadReduction < bread.Amount)
                city.RemoveResource("Bread", breadReduction);
            else
                city.RemoveResource("Bread", bread.Amount);

            if(materialsReduction< materials.Amount)
                city.RemoveResource("Materials", materialsReduction);
            else
                city.RemoveResource("Materials", materials.Amount);

            return $"Citizens lost: {citizenReduction}, Happiness lost: {happinessReduction}." +
                $"\rHealth lost: {damage}, Wheat lost : {wheatReduction}" + 
                $"\rBread lost : {breadReduction}, Materials lost : {materialsReduction}";
        }
    }
}
