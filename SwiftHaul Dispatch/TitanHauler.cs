using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    internal class TitanHauler : Vehicle
    {
        private int _numberOfTrailers;
        public int NumberOfTrailers
        {
            get { return _numberOfTrailers; }
            set { _numberOfTrailers = value; }
        }

        public TitanHauler(int vehicleID, string vehicleName, int vehicleMilage, int vehicleCapacity, int numberOfTrailers)
            : base(vehicleID, vehicleName, vehicleMilage, vehicleCapacity)
        {
            this.NumberOfTrailers = numberOfTrailers;
        }

        public override void DisplayAllVehicleInfo()
        {
            base.DisplayAllVehicleInfo();
            Console.WriteLine($"Number of Trailers: {NumberOfTrailers}");
        }

        public override string GetVehicleTypeDetails()
        {
            return $"Number of Trailers: {NumberOfTrailers}";
        }

        public override void PerformOperation()
        {
            Console.WriteLine($"{VehicleName} is hauling {NumberOfTrailers} trailer(s), grinding through the long haul route.");
        }
    }
}