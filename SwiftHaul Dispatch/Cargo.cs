using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public abstract class Cargo : ICargo
    {
        private int _cargoID;
        private string _description;
        private double _weight;

        public Cargo(int cargoID, string description, double weight)
        {
            this.CargoID = cargoID;
            this.Description = description;
            this.Weight = weight;
        }

        public int CargoID { get => _cargoID; set => _cargoID = value; }
        public string Description { get => _description; set => _description = value; }
        public double Weight { get => _weight; set => _weight = value; }
        public double Cost { get; set; } = 0;
        // if the cargo is assigned
        public bool IsAssigned { get; set; } = false;

        public virtual void DisplayAllCargoInfo()
        {
            Console.WriteLine($"     ---- Cargo ID: {CargoID} ----");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Weight: {Weight} kg");
        }

        public virtual string GetCargoTypeDetails()
        {
            return "N/A";
        }

        public abstract string GetHandlingInstructions();

        // Calcuate the cost to ship the cargo
        public virtual double CalculateShippingCost(double ratePerKg)
        {
            return Weight * ratePerKg;
        }

    }
}
