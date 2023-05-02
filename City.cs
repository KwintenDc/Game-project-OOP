using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_project_OOP
{
    class City
    {
        public int Citizens { get; set; }
        public int Happiness { get; set; }
        public int Health { get; set; }
        public int Defense { get; set; }

        public int MaxMines { get; set; }
        public int MaxFields { get; set; }
        public int MaxHouses { get; set; }
        public int MaxDefenses { get; set; }

        public int TotalMines { get; set; }
        public int TotalFields { get; set; }
        public int TotalHouses { get; set; }
        public int TotalDefenses { get; set; }

        public int TotalUpgradedMines { get; set; }
        public int TotalUpgradedFields { get; set; }
        public int TotalUpgradedDefenses { get; set; }

        private string[] cityLayout = new string[25];

        public string[] CityLayout
        {
            get { return cityLayout; }
            set { cityLayout = value; }
        }

        private List<Resource> resources = new List<Resource>();

        public List<Resource> Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        public void AddResource(string name, int amount)
        {
            // Check if resource already exists
            Resource existingResource = resources.FirstOrDefault(r => r.Name == name);

            if (existingResource != null)
            {
                // Resource already exists, so add to existing amount
                existingResource.Amount += amount;
            }
            else
            {
                // Resource doesn't exist, so create new resource
                Resource newResource = new Resource { Name = name, Amount = amount };
                resources.Add(newResource);
            }
        }

        public void RemoveResource(string name, int amount)
        {
            // Check if resource exists
            Resource existingResource = resources.FirstOrDefault(r => r.Name == name);

            if (existingResource != null)
            {
                // Resource exists, so subtract from amount
                existingResource.Amount -= amount;

                // Check if amount is zero or negative, and remove resource if necessary
                if (existingResource.Amount <= 0)
                {
                    resources.Remove(existingResource);
                }
            }
            else
            {
                // Resource doesn't exist, so do nothing
            }
        }
    }
}
