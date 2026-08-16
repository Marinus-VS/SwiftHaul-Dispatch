using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class RefrigeratedCargo : Cargo
    {
        private int _requiredTemperatureCelsius;
        public int RequiredTemperatureCelsius
        {
            get { return _requiredTemperatureCelsius; }
            set { _requiredTemperatureCelsius = value; }
        }

        public RefrigeratedCargo(int cargoID, string description, double weight, int requiredTemperatureCelsius)
            : base(cargoID, weight, description)
        {
            this.RequiredTemperatureCelsius = requiredTemperatureCelsius;
        }

        public override void DisplayAllCargoInfo()
        {
            base.DisplayAllCargoInfo();
            Console.WriteLine($"Required Temperature: {RequiredTemperatureCelsius}°C");
        }

        public override string GetHandlingInstructions()
        {
            return $"Keep sealed and maintain {RequiredTemperatureCelsius}°C at all times.";
        }

        public override string GetCargoTypeDetails()
        {
            return $"Required Temperature: {RequiredTemperatureCelsius}°C";
        }

        public override double CalculateShippingCost(double ratePerKg)
        {
            double baseCost = base.CalculateShippingCost(ratePerKg);
            double refrigeration = baseCost * 0.25; // 25% surcharge for refrigeration
            return baseCost + refrigeration;
        }
    }
}