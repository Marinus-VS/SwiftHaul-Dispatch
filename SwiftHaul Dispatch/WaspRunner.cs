using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    internal class WaspRunner : Vehicle
    {
        private int _maxSpeedKmh;
        private bool _isWeatherRestricted;

        public int MaxSpeedKmh
        {
            get { return _maxSpeedKmh; }
            set { _maxSpeedKmh = value; }
        }

        public bool IsWeatherRestricted
        {
            get { return _isWeatherRestricted; }
            set { _isWeatherRestricted = value; }
        }

        public WaspRunner(int vehicleID, string vehicleName, int vehicleKilos, int vehicleCapacity, int maxSpeedKmh, bool isWeatherRestricted)
            : base(vehicleID, vehicleName, vehicleKilos, vehicleCapacity)
        {
            this.MaxSpeedKmh = maxSpeedKmh;
            this.IsWeatherRestricted = isWeatherRestricted;
        }

        public override void DisplayAllVehicleInfo()
        {
            base.DisplayAllVehicleInfo();
            Console.WriteLine($"Max Speed: {MaxSpeedKmh} km/h");
            Console.WriteLine($"Weather Restricted: {IsWeatherRestricted}");
        }
        public override string GetVehicleTypeDetails()
        {
            return $"Speed: {MaxSpeedKmh}km/h, Weather Restricted: {IsWeatherRestricted}";
        }

        public override void PerformOperation()
        {
            Console.WriteLine($"{VehicleName} is weaving through traffic at up to {MaxSpeedKmh} km/h, chasing a fast single parcel drop.");
        }
    }
}