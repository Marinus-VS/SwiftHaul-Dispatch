using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    internal class CascadeVan : Vehicle
    {
        private int _maxDeliveryStops;
        public int MaxDeliveryStops
        {
            get { return _maxDeliveryStops; }
            set { _maxDeliveryStops = value; }
        }

        public CascadeVan(int vehicleID, string vehicleName, int vehicleMilage, int vehicleCapacity, int maxDeliveryStops)
            : base(vehicleID, vehicleName, vehicleMilage, vehicleCapacity)
        {
            this.MaxDeliveryStops = maxDeliveryStops;
        }

        public override void DisplayAllVehicleInfo()
        {
            base.DisplayAllVehicleInfo();
            Console.WriteLine($"Max Delivery Stops: {MaxDeliveryStops}");
        }

        public override string GetVehicleTypeDetails()
        {
            return $"Max Delivery Stops: {MaxDeliveryStops}";
        }

        public override void PerformOperation()
        {
            Console.WriteLine($"{VehicleName} is en route, servicing up to {MaxDeliveryStops} delivery stops.");
        }
    }
}