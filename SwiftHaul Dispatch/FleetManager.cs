using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        // folder where all save files live
        private static readonly string SaveFolder = "Saves";

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
        public void AddVehicle(Vehicle vehicle, string type)
        {
            Vehicle checkVehicle = vehicles.Find(v => v.VehicleID == vehicle.VehicleID);
            if (checkVehicle == null)
            {
                vehicles.Add(vehicle);
                ConsoleHelper.ShowSuccess($"{vehicle.VehicleName} ({type}) has been added to the fleet.");
            }
            else
            {
                ConsoleHelper.ShowError($"ID: {vehicle.VehicleID} already exists in the system.");
            }
        }

        // display all vehicles in the fleet, returns true if vehicles exist, false if the fleet was empty
        public bool DisplayAllVehicles(bool waitForKeyPress = true)
        {
            if (vehicles.Count == 0)
            {
                ConsoleHelper.ShowError("No vehicles currently in the fleet.");
                return false;
            }

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

            return true;
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

                ConsoleHelper.ShowWarning($"Are you sure you would like to remove {vehicleToRemove.VehicleName}? (Y/N)");
                ConsoleHelper.ChooseOptionStyling();
                bool option = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());
                if (option)
                {
                    vehicles.Remove(vehicleToRemove);
                    ConsoleHelper.ShowSuccess($"{vehicleToRemove.VehicleName} removed from the fleet.");
                }
                else
                {
                    ConsoleHelper.ShowError($"{vehicleToRemove.VehicleName} was not removed from the fleet.");
                }

            }
            catch (VehicleNotFoundException ex)
            {
                ConsoleHelper.HandleException(ex);
            }
        }


        /////////////////////////////////////////////////// ---- CARGO --- ///////////////////////////////////////////////////////////

        // add cargo to the system
        public void AddCargo(Cargo cargo)
        {
            Cargo checkCargo = cargoList.Find(v => v.CargoID == cargo.CargoID);
            if (checkCargo == null)
            {
                cargoList.Add(cargo);
                ConsoleHelper.ShowSuccess($"{cargo.CargoID} has been added to the system.");
            }
            else
            {
                ConsoleHelper.ShowError($"ID: {cargo.CargoID} already exists in the system.");
            }
        }

        // display all cargo in the system, returns true if cargo exists, false if the system was empty
        public bool DisplayAllCargo(bool waitForKeyPress = true)
        {
            if (cargoList.Count == 0)
            {
                ConsoleHelper.ShowError("No cargo currently in the system.");
                return false;
            }

            // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated
            Console.WriteLine($"\n{"ID",-5}{"Type",-18}{"Weight",-10}{"Description",-25}{"Details",-30}");
            Console.WriteLine(new string('-', 88));

            foreach (Cargo c in cargoList)
            {
                Console.WriteLine($"{c.CargoID,-5}{c.GetType().Name,-18}{c.Weight,-10}{c.Description,-25}{c.GetCargoTypeDetails(),-30}");
            }

            if (waitForKeyPress)
            {
                ConsoleHelper.PressAnyKeyToContinue();
            }

            return true;
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

                ConsoleHelper.ShowWarning($"Are you srue you want to remove cargo ID: {cargoToRemove.CargoID} (Y/N)");
                ConsoleHelper.ChooseOptionStyling();
                bool option = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());
                if (option)
                {
                    cargoList.Remove(cargoToRemove);
                    ConsoleHelper.ShowSuccess($"{cargoToRemove.CargoID} removed from the system.");
                }
                else
                {
                    ConsoleHelper.ShowError($"{cargoToRemove.CargoID} was not removed from the fleet.");
                }
            }
            catch (CargoNotFoundException ex)
            {
                ConsoleHelper.HandleException(ex);
            }
        }


        /////////////////////////////////////////////////// ---- Assign Cargo to Vehicle --- ///////////////////////////////////////////////////////////

        public void AssignCargoToVehicle(int vehicleID, int cargoID)
        {
            Vehicle vehicle = vehicles.Find(v => v.VehicleID == vehicleID);
            if (vehicle == null)
            {
                throw new VehicleNotFoundException($"Vehicle with ID {vehicleID} not found.");
            }

            Cargo cargo = cargoList.Find(c => c.CargoID == cargoID);
            if (cargo == null)
            {
                throw new CargoNotFoundException($"Cargo with ID {cargoID} not found.");
            }

            if (cargo.IsAssigned)
            {
                throw new CargoAlreadyAssignedException($"Cargo ID {cargoID} is already assigned to a vehicle.");
            }

            double projectedLoad = vehicle.GetCurrentLoadWeight() + cargo.Weight;
            if (projectedLoad > vehicle.VehicleCapacity)
            {
                throw new VehicleOverloadException(
                    $"Cannot assign cargo {cargoID} ({cargo.Weight}kg) to {vehicle.VehicleName} — " +
                    $"would exceed capacity ({projectedLoad}kg / {vehicle.VehicleCapacity}kg max).");
            }

            vehicle.AssignedCargo.Add(cargo);
            cargo.IsAssigned = true;
            ConsoleHelper.ShowSuccess($"Cargo {cargoID} assigned to {vehicle.VehicleName}.");
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

        /////////////////////////////////////////////////// ---- File Functionality --- ///////////////////////////////////////////////////////////

        //////// ---- Save Function --- //////

        // converts the real vehicle list into saveable data objects
        private List<VehicleSaveData> ConvertVehiclesToSaveData()
        {
            List<VehicleSaveData> result = new List<VehicleSaveData>();

            foreach (Vehicle v in vehicles)
            {
                // reads the data into SaveData.cs
                VehicleSaveData data = new VehicleSaveData
                {
                    VehicleID = v.VehicleID,
                    VehicleName = v.VehicleName,
                    VehicleMilage = v.VehicleMilage,
                    VehicleCapacity = v.VehicleCapacity,
                    VehicleType = v.GetType().Name
                };

                // fill in the one type specific field that applies
                switch (v)
                {
                    case WaspRunner wasp:
                        data.MaxSpeedKmh = wasp.MaxSpeedKmh;
                        data.IsWeatherRestricted = wasp.IsWeatherRestricted;
                        break;
                    case CascadeVan van:
                        data.MaxDeliveryStops = van.MaxDeliveryStops;
                        break;
                    case TitanHauler titan:
                        data.NumberOfTrailers = titan.NumberOfTrailers;
                        break;
                    case GlacierTrans glacier:
                        data.TargetTemperatureCelsius = glacier.TargetTemperatureCelsius;
                        break;
                }

                result.Add(data);
            }

            return result;
        }

        // converts the real cargo list into saveable data objects
        private List<CargoSaveData> ConvertCargoToSaveData()
        {
            List<CargoSaveData> result = new List<CargoSaveData>();

            foreach (Cargo item in cargoList)
            {
                // reads the data into SaveData.cs
                CargoSaveData data = new CargoSaveData
                {
                    CargoID = item.CargoID,
                    Description = item.Description,
                    Weight = item.Weight,
                    CargoType = item.GetType().Name
                };

                switch (item)
                {
                    case SmallCargo small:
                        data.IsFragile = small.IsFragile;
                        break;
                    case MediumCargo medium:
                        data.RequiresSignature = medium.RequiresSignature;
                        break;
                    case LargeCargo large:
                        data.RequiresForklift = large.RequiresForklift;
                        break;
                    case RefrigeratedCargo refrigerated:
                        data.RequiredTemperatureCelsius = refrigerated.RequiredTemperatureCelsius;
                        break;
                }

                result.Add(data);
            }

            return result;
        }

        // saves the current fleet / cargo state to a named file
        public void SaveCurrentLoadout(string saveName)
        {
            if (!Directory.Exists(SaveFolder))
            {
                Directory.CreateDirectory(SaveFolder);
            }

            string filePath = Path.Combine(SaveFolder, $"{saveName}.json");

            if (File.Exists(filePath))
            {
                
                ConsoleHelper.ShowWarning($"A save named '{saveName}' already exists. Overwrite? (Y/N): ");
                ConsoleHelper.ChooseOptionStyling();
                bool overwrite = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());

                if (!overwrite)
                {
                    ConsoleHelper.ShowError("Save cancelled.");
                    return;
                }
            }

            LoadoutSaveData saveData = new LoadoutSaveData
            {
                SavedAt = DateTime.Now,
                Vehicles = ConvertVehiclesToSaveData(),
                Cargo = ConvertCargoToSaveData()
            };

            string json = JsonConvert.SerializeObject(saveData, Formatting.Indented);
            File.WriteAllText(filePath, json);

            ConsoleHelper.ShowSuccess($"Loadout saved as '{saveName}'.");
        }


        //////// ---- Load Function --- //////

        // reconstructs actual Vehicle objects from save data
        private List<Vehicle> ConvertSaveDataToVehicles(List<VehicleSaveData> data)
        {
            List<Vehicle> result = new List<Vehicle>();

            foreach (VehicleSaveData v in data)
            {
                Vehicle vehicle = null;

                switch (v.VehicleType)
                {
                    case "WaspRunner":
                        vehicle = new WaspRunner(v.VehicleID, v.VehicleName, v.VehicleMilage, v.VehicleCapacity,
                            v.MaxSpeedKmh.Value, v.IsWeatherRestricted.Value);
                        break;

                    case "CascadeVan":
                        vehicle = new CascadeVan(v.VehicleID, v.VehicleName, v.VehicleMilage, v.VehicleCapacity,
                            v.MaxDeliveryStops.Value);
                        break;

                    case "TitanHauler":
                        vehicle = new TitanHauler(v.VehicleID, v.VehicleName, v.VehicleMilage, v.VehicleCapacity,
                            v.NumberOfTrailers.Value);
                        break;

                    case "GlacierTrans":
                        vehicle = new GlacierTrans(v.VehicleID, v.VehicleName, v.VehicleMilage, v.VehicleCapacity,
                            v.TargetTemperatureCelsius.Value);
                        break;
                }

                if (vehicle != null)
                {
                    result.Add(vehicle);
                }
            }

            return result;
        }

        // reconstructs actual Cargo objects from flat save data
        private List<Cargo> ConvertSaveDataToCargo(List<CargoSaveData> data)
        {
            List<Cargo> result = new List<Cargo>();

            foreach (CargoSaveData c in data)
            {
                Cargo cargo = null;

                switch (c.CargoType)
                {
                    case "SmallCargo":
                        cargo = new SmallCargo(c.CargoID, c.Description, c.Weight, c.IsFragile.Value);
                        break;

                    case "MediumCargo":
                        cargo = new MediumCargo(c.CargoID, c.Description, c.Weight, c.RequiresSignature.Value);
                        break;

                    case "LargeCargo":
                        cargo = new LargeCargo(c.CargoID, c.Description, c.Weight, c.RequiresForklift.Value);
                        break;

                    case "RefrigeratedCargo":
                        cargo = new RefrigeratedCargo(c.CargoID, c.Description, c.Weight, c.RequiredTemperatureCelsius.Value);
                        break;
                }

                if (cargo != null)
                {
                    result.Add(cargo);
                }
            }

            return result;
        }

        // loads a saved loadout by name, replacing the current fleet and cargo
        public void LoadSavedState(string saveName)
        {
            string filePath = Path.Combine(SaveFolder, $"{saveName}.json");

            if (!File.Exists(filePath))
            {
                throw new SaveFileNotFoundException($"Save file '{saveName}' not found.");
            }

            string json = File.ReadAllText(filePath);
            LoadoutSaveData saveData = JsonConvert.DeserializeObject<LoadoutSaveData>(json);

            vehicles = ConvertSaveDataToVehicles(saveData.Vehicles);
            cargoList = ConvertSaveDataToCargo(saveData.Cargo);

            ConsoleHelper.ShowSuccess($"Loadout '{saveName}' loaded successfully. ({vehicles.Count} vehicles, {cargoList.Count} cargo items)");
        }

        //////// ---- View all files function --- //////

        // lists all save files with their vehicle/cargo counts and last saved timestamp
        public bool ViewAllSaves(bool waitForKeyPress = true)
        {
            if (!Directory.Exists(SaveFolder) || Directory.GetFiles(SaveFolder, "*.json").Length == 0)
            {
                ConsoleHelper.ShowError("No saved loadouts found.");
                return false;
            }

            string[] saveFiles = Directory.GetFiles(SaveFolder, "*.json");

            Console.WriteLine($"\n{"Save Name",-25}{"Vehicles",-12}{"Cargo",-12}{"Saved At",-25}");
            Console.WriteLine(new string('-', 74));

            foreach (string filePath in saveFiles)
            {
                string saveName = Path.GetFileNameWithoutExtension(filePath);
                string json = File.ReadAllText(filePath);
                LoadoutSaveData saveData = JsonConvert.DeserializeObject<LoadoutSaveData>(json);

                Console.WriteLine($"{saveName,-25}{saveData.Vehicles.Count,-12}{saveData.Cargo.Count,-12}{saveData.SavedAt,-25}");
            }

            if (waitForKeyPress)
            {
                ConsoleHelper.PressAnyKeyToContinue();
            }

            return true;
        }

        //////// ---- View all files function --- //////

        // deletes a saved loadout file by name, with confirmation
        public void RemoveSavedState(string saveName)
        {
            string filePath = Path.Combine(SaveFolder, $"{saveName}.json");

            if (!File.Exists(filePath))
            {
                throw new SaveFileNotFoundException($"Save file '{saveName}' not found.");
            }

            ConsoleHelper.ShowWarning($"Are you sure you want to delete save '{saveName}'? (Y/N): ");
            ConsoleHelper.ChooseOptionStyling();
            bool confirmed = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());

            if (confirmed)
            {
                File.Delete(filePath);
                ConsoleHelper.ShowSuccess($"Save '{saveName}' deleted.");
            }
            else
            {
                ConsoleHelper.ShowError($"Save '{saveName}' was not deleted.");
            }
        }

        //////// ---- Clear current loadout --- //////

        // clears the current loadout fleet and cargo without touching saved files
        public void ClearCurrentLoadout()
        {
            ConsoleHelper.ShowWarning("This will permanently clear ALL vehicles and cargo from the current session. Are you sure? (Y/N): ");
            ConsoleHelper.ChooseOptionStyling();
            bool confirmed = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());

            if (confirmed)
            {
                vehicles.Clear();
                cargoList.Clear();
                ConsoleHelper.ShowSuccess("Current loadout cleared.");
            }
            else
            {
                ConsoleHelper.ShowError("Clear cancelled. Current loadout unchanged.");
            }
        }
    }
}