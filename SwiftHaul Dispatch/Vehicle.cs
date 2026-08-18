using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public abstract class Vehicle : IOperative
    {
        // Sets the private fields for the Vehicle class (only accessible within the class)
        private int _VehicleID;
        private string _VehicleName;
        private int _VehicleMilage;
        private int _VehicleCapacity;

        // Constructor for the Vehicle class, initializes the private fields with the provided parameters
        public Vehicle(int vehicleID, string vehicleName, int vehicleMilage, int vehicleCapacity)
        {
            this._VehicleID = vehicleID;
            this._VehicleName = vehicleName;
            this._VehicleMilage = vehicleMilage;
            this._VehicleCapacity = vehicleCapacity;
        }

        // Public properties to get and set the private fields, allowing controlled access from outside the class
        public int VehicleID { get => _VehicleID; set => _VehicleID = value; }
        public string VehicleName { get => _VehicleName; set => _VehicleName = value; }
        public int VehicleMilage { get => _VehicleMilage; set => _VehicleMilage = value; }
        public int VehicleCapacity { get => _VehicleCapacity; set => _VehicleCapacity = value; }

        // Method to display all vehicle information
        // Using virtual allows derived classes to override this method if they want to provide additional information specific to their type
        public virtual void DisplayAllVehicleInfo()
        {
            Console.WriteLine($"     ---- Vehicle ID: {VehicleID} ----");
            Console.WriteLine($"Vehicle Name: {VehicleName}");
            Console.WriteLine($"Vehicle Milage: {VehicleMilage}");
            Console.WriteLine($"Vehicle Capacity: {VehicleCapacity}");
        }

        // Abstract method to perform an operation specific to the vehicle type
        public abstract void PerformOperation();

        // each derived type overrides this to describe its unique property in one line
        public virtual string GetVehicleTypeDetails()
        {
            return "N/A";
        }

        // to track its own assigned cargo
        private List<Cargo> assignedCargo = new List<Cargo>();
        public List<Cargo> AssignedCargo
        {
            get { return assignedCargo; }
        }

        public double GetCurrentLoadWeight()
        {
            return assignedCargo.Sum(c => c.Weight);
        }
    }
}