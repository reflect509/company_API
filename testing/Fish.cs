using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testing
{
    public class Fish
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public double LengthCentimeters { get; set; }
        public string Description { get; set; }

        public Fish()
        {

        }

        public Fish(string name, double weight, double lengthCentimeters, string description)
        {
            this.Name = name;
            this.Weight = weight;
            this.LengthCentimeters = lengthCentimeters;
            this.Description = description;
        }

        public string FishInfo()
        {
            return $"Название: {Name}, Вес {Weight}, Длина(См): {LengthCentimeters}, Описание: {Description}";
        }
    }
}
