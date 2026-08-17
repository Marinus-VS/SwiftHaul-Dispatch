using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    // flat, serializable version of any vehicle
    public class VehicleSaveData
    {
        public string VehicleType { get; set; }
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public int VehicleMilage { get; set; }
        public int VehicleCapacity { get; set; }

        // type specific fields so that only the relevent fields get filed
        public int? MaxDeliveryStops { get; set; }       // CascadeVan
        public int? NumberOfTrailers { get; set; }        // TitanHauler
        public int? MaxSpeedKmh { get; set; }              // WaspRunner
        public bool? IsWeatherRestricted { get; set; }     // WaspRunner
        public int? TargetTemperatureCelsius { get; set; } // GlacierTrans
    }

    // flat, serializable version of any cargo item
    public class CargoSaveData
    {
        public string CargoType { get; set; }
        public int CargoID { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }

        public bool? IsFragile { get; set; }               // SmallCargo
        public bool? RequiresSignature { get; set; }        // MediumCargo
        public bool? RequiresForklift { get; set; }          // LargeCargo
        public int? RequiredTemperatureCelsius { get; set; } // RefrigeratedCargo
    }

    // the full save file structure
    public class LoadoutSaveData
    {
        public DateTime SavedAt { get; set; }
        public List<VehicleSaveData> Vehicles { get; set; }
        public List<CargoSaveData> Cargo { get; set; }
    }
}
