using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class FleetManager
    {
        private List<Vehicle> vehicles;
        private List<Cargo> cargoList;

        public event DispatchEventHandler DeliveryCompleted;
        public event DispatchEventHandler AlertTriggered;

        public List<Vehicle> Vehicles
        {
            get { return vehicles; }
        }

        public List<Cargo> CargoList
        {
            get { return cargoList; }
        }

        public FleetManager()
        {
            vehicles = new List<Vehicle>();
            cargoList = new List<Cargo>();
        }

        /////////////////////////////////////////////////// ---- Vehicle --- ///////////////////////////////////////////////////////////

        // add a vehicle to the fleet
        public void AddVehicle(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
        }

        // remove a vehicle from the fleet by its ID
        public void RemoveVehicle(int vehicleID)
        {
            try
            {
                Vehicle vehicleToRemove = vehicles.Find(v => v.VehicleID == vehicleID);

                if (vehicleToRemove == null)
                {
                    throw new VehicleNotFoundException($"Vehicle with ID {vehicleID} not found.");
                }

                vehicles.Remove(vehicleToRemove);
                ConsoleHelper.ShowMessage($"{vehicleToRemove.VehicleName} removed from the fleet.");
            }
            catch (VehicleNotFoundException ex)
            {
                ConsoleHelper.HandleException(ex);
            }
        }

        // display all vehicles in the fleet
        public void DisplayAllVehicles(bool waitForKeyPress = true)
        {
            if (vehicles.Count == 0)
            {
                ConsoleHelper.ShowMessage("No vehicles currently in the fleet.");
                return;
            }

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"\n{"ID",-5}{"Name",-15}{"Type",-15}{"Mileage",-10}{"Capacity",-10}{"Details",-40}");
            Console.WriteLine(new string('-', 95));

            foreach (Vehicle v in vehicles)
            {
                Console.WriteLine($"{v.VehicleID,-5}{v.VehicleName,-15}{v.GetType().Name,-15}{v.VehicleMilage,-10}{v.VehicleCapacity,-10}{v.GetVehicleTypeDetails(),-40}");
            }

            if (waitForKeyPress)
            {
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }

        /////////////////////////////////////////////////// ---- CARGO --- ///////////////////////////////////////////////////////////

        // add cargo to the system
        public void AddCargo(Cargo cargo)
        {
            cargoList.Add(cargo);
        }

        // remove cargo from the system by its ID
        public void RemoveCargo(int cargoID)
        {
            try
            {
                Cargo cargoToRemove = cargoList.Find(c => c.CargoID == cargoID);

                if (cargoToRemove == null)
                {
                    throw new CargoNotFoundException($"Cargo with ID {cargoID} not found.");
                }

                cargoList.Remove(cargoToRemove);
                ConsoleHelper.ShowMessage($"Cargo ID {cargoToRemove.CargoID} removed from the system.");
            }
            catch (CargoNotFoundException ex)
            {
                ConsoleHelper.HandleException(ex);
            }
        }

        // display all cargo in the system
        public void DisplayAllCargo(bool waitForKeyPress = true)
        {
            if (cargoList.Count == 0)
            {
                ConsoleHelper.ShowMessage("No cargo currently in the system.");
                return;
            }

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"{"ID",-5}{"Type",-18}{"Weight",-10}{"Description",-25}{"Details",-30}");
            Console.WriteLine(new string('-', 88));

            foreach (Cargo c in cargoList)
            {
                Console.WriteLine($"{c.CargoID,-5}{c.GetType().Name,-18}{c.Weight,-10}{c.Description,-25}{c.GetCargoTypeDetails(),-30}");
            }

            if (waitForKeyPress)
            {
                ConsoleHelper.PressAnyKeyToContinue();
            }
        }

        /////////////////////////////////////////////////// ---- Events --- ///////////////////////////////////////////////////////////

        protected virtual void OnDeliveryCompleted(DispatchEventArgs e)
        {
            DeliveryCompleted?.Invoke(this, e);
        }

        protected virtual void OnAlertTriggered(DispatchEventArgs e)
        {
            AlertTriggered?.Invoke(this, e);
        }

        private Random random = new Random();
        private bool isMonitoring = false;
        private Thread monitorThread;

        // a thread to monitor the fleet and trigger events based on random outcomes
        public void StartDispatchMonitor()
        {
            //creates a new thread
            isMonitoring = true;

            // point the thread to the method
            monitorThread = new Thread(MonitorFleet); 

            // starts the thread
            monitorThread.Start(); 
        }

        // this is the method the thread executes independently of the program
        private void MonitorFleet()
        {
            while (isMonitoring)
            {
                // pauses for 3 seconds then check again
                Thread.Sleep(3000);


                // prevents program thread and this thread from touching the list at the same time
                lock (vehicles) 
                {
                    if (vehicles.Count == 0)
                        continue;

                    Vehicle vehicle = vehicles[random.Next(vehicles.Count)];
                    int outcome = random.Next(1, 4);

                    if (outcome == 1)
                    {
                        OnDeliveryCompleted(new DispatchEventArgs(vehicle.VehicleID, vehicle.VehicleName,
                            $"{vehicle.VehicleName} has completed its delivery."));
                    }
                    else if (outcome == 2)
                    {
                        OnAlertTriggered(new DispatchEventArgs(vehicle.VehicleID, vehicle.VehicleName,
                            $"ALERT: {vehicle.VehicleName} reported a mechanical issue."));
                    }
                }
            }
        }

        // lets the while loop exit naturally on its next check
        public void StopDispatchMonitor()
        {
            isMonitoring = false; 
        }
    }
}