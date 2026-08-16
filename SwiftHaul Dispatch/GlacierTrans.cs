using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    internal class GlacierTrans : Vehicle
    {
        private int _targetTemperatureCelsius;
        public int TargetTemperatureCelsius
        {
            get { return _targetTemperatureCelsius; }
            set { _targetTemperatureCelsius = value; }
        }

        public GlacierTrans(int vehicleID, string vehicleName, int vehicleMilage, int vehicleCapacity, int targetTemperatureCelsius)
            : base(vehicleID, vehicleName, vehicleMilage, vehicleCapacity)
        {
            this.TargetTemperatureCelsius = targetTemperatureCelsius;
        }

        public override void DisplayAllVehicleInfo()
        {
            base.DisplayAllVehicleInfo();
            Console.WriteLine($"Target Temperature: {TargetTemperatureCelsius}°C");
        }

        public override string GetVehicleTypeDetails()
        {
            return $"Target Temperature: {TargetTemperatureCelsius}°C";
        }

        public override void PerformOperation()
        {
            Console.WriteLine($"{VehicleName} is holding steady at {TargetTemperatureCelsius}°C, keeping the cold chain unbroken.");
        }
    }
}