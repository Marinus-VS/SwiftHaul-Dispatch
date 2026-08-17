using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    internal class Program
    {
        // create a static instance of FleetManager to manage vehicles and cargo
        static FleetManager fleetManager = new FleetManager();

        public enum Menu
        {
            ManageVehicles = 1,
            ManageCargo,
            AssignCargoToVehicle,
            ViewDispatchLog,
            ManageSaveOperations,
            Exit
        }

        public enum VehicleMenu
        {
            AddVehicle = 1,
            DisplayAllVehicles,
            RemoveVehicle,
            BackToMainMenu
        }

        public enum VehicleType
        {
            WaspRunner = 1, // motorbike
            CascadeVan, // standard delivery van 
            TitanHauler, // heavy truck with / without trailers
            GlacierTrans, // refrigerated truck
        }

        public enum CargoMenu
        {
            AddCargo = 1,
            DisplayAllCargo,
            RemoveCargo,
            BackToMainMenu
        }

        public enum CargoType
        {
            SmallCargo = 1, // gets a 10% chare if fragile
            MediumCargo,
            LargeCargo, // gets a 15% charge if a forklift is required
            RefrigeratedCargo // gets a 25% charge for refrigeration
        }

        public enum FileMenu
        {
            SaveCurrentLoadout = 1,
            LoadSavedState,
            ViewAllSaves,
            RemoveSavedState,
            ClearCurrentLoadout,
            BackToMainMenu
        }

        /////////////////////////////////////////////////// ---- MAIN METHOD --- ///////////////////////////////////////////////////////////

        static void Main(string[] args)
        {

            /////////////// TEMP CODE BLOCK -> FOR TESTING ///////////////////////

            fleetManager.LoadSavedState("TestData");

            //////////////////////////////////////////////////////////////////////
            
            bool running = true;
            ConsoleHelper.ClearScreen();
            while (running)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("================================");
                Console.WriteLine("     Swift-Haul Dispatch");
                Console.WriteLine("================================");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Manage Vehicles");
                Console.WriteLine("2. Manage Cargo");
                Console.WriteLine("3. Assign Cargo to Vehicle");
                Console.WriteLine("4. View Dispatch Log");
                Console.WriteLine("5. Manage Save Operations");
                Console.WriteLine("6. Exit");
                Console.WriteLine();
                ConsoleHelper.ChooseOptionStyling();

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());

                    switch ((Menu)option)
                    {
                        case Menu.ManageVehicles:
                            ManageVehicles();
                            break;
                        case Menu.ManageCargo:
                            ManageCargo();
                            break;
                        case Menu.AssignCargoToVehicle:
                            AssignCargoToVehicle();
                            break;
                        case Menu.ViewDispatchLog:
                            ConsoleHelper.ShowError("This is still under development.");
                            break;
                        case Menu.ManageSaveOperations:
                            ManageSaveOperations();
                            break;
                        case Menu.Exit:
                            running = false;
                            break;
                        default:
                            ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.HandleException(ex);
                }
            }
        }

        /////////////////////////////////////////////////////////// ---- Vehicle --- ///////////////////////////////////////////////////////////////////

        static void ManageVehicles()
        {
            bool managingVehicles = true;
            ConsoleHelper.ClearScreen();
            while (managingVehicles)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("     Manage Vehicles");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Display All Vehicles");
                Console.WriteLine("3. Remove Vehicle");
                Console.WriteLine("4. Back to Main Menu");
                Console.WriteLine();
                ConsoleHelper.ChooseOptionStyling();
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    switch ((VehicleMenu)option)
                    {
                        case VehicleMenu.AddVehicle:
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("\nSelect Vehicle Type:");
                            Console.ResetColor();
                            Console.WriteLine("1. Wasp Runner (Capacity: 1 - 30)kg");
                            Console.WriteLine("2. Cascade Van (Capacity: 30 - 1700)kg");
                            Console.WriteLine("3. Titan Hauler (Capacity: 1000 - 36000)kg");
                            Console.WriteLine("4. Glacier Trans (Capacity: 30 - 7000)kg");
                            ConsoleHelper.ChooseOptionStyling();

                            int type = Convert.ToInt32(Console.ReadLine());

                            // shared fields of all vehicles
                            Console.Write("Enter Vehicle ID: ");
                            int newVehicleID = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Vehicle Name: ");
                            string newVehicleName = Console.ReadLine();

                            Console.Write("Enter Vehicle Milage: ");
                            int newVehicleMileage = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Vehicle Capacity:");
                            int newVehicleCapacity = Convert.ToInt32(Console.ReadLine());

                            // validate vehicle capacity based on vehicle type
                            if (!(newVehicleCapacity < 1 || newVehicleCapacity > 36000))
                            {
                                if ((VehicleType)type == VehicleType.WaspRunner && newVehicleCapacity > 30)
                                {
                                    throw new IncorrectVehicleForType("Vehicle capacity too high for Wasp Runner.");
                                }
                                else if ((VehicleType)type == VehicleType.CascadeVan && (newVehicleCapacity > 1700 || newVehicleCapacity < 30))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 1700 ? "Vehicle capacity too high for Cascade Van." : "Vehicle capacity too low for Cascade Van.");
                                }
                                else if ((VehicleType)type == VehicleType.TitanHauler && (newVehicleCapacity > 36000 || newVehicleCapacity < 1000))
                                {
                                    throw new IncorrectVehicleForType("Vehicle capacity too low for Titan Hauler.");
                                }
                                else if ((VehicleType)type == VehicleType.GlacierTrans && (newVehicleCapacity > 7000 || newVehicleCapacity < 30))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 7000 ? "Vehicle capacity too high for Glacier Trans." : "Vehicle capacity too low for Glacier Trans.");
                                }
                            }
                            else 
                            {
                                throw new ArgumentException(newVehicleCapacity > 36000 ? "Vehicle capacity is too high for our transport options." : "Vehicle capacity is too low for our transport options.");
                            }

                            switch ((VehicleType)type)
                            {
                                case VehicleType.WaspRunner:
                                    Console.Write("Enter Max Speed (km/h): ");
                                    int maxSpeed = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Is Weather Restricted (Y/N): "); // unable to drive in bad weather conditions
                                    bool isWeatherRestricted = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());
                                    WaspRunner newWaspRunner = new WaspRunner(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, maxSpeed, isWeatherRestricted);
                                    fleetManager.AddVehicle(newWaspRunner, "Wasp Runner");
                                    break;

                                case VehicleType.CascadeVan:
                                    Console.Write("Enter max delivery stops: ");
                                    int maxDeliveryStops = Convert.ToInt32(Console.ReadLine());
                                    CascadeVan newCascadeVan = new CascadeVan(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, maxDeliveryStops);
                                    fleetManager.AddVehicle(newCascadeVan, "Cascade van");
                                    break;

                                case VehicleType.TitanHauler:
                                    Console.Write("Enter number of trailers (Range 0 - 2): ");
                                    int numberOfTrailers = Convert.ToInt32(Console.ReadLine());
                                        if (numberOfTrailers > 2 || numberOfTrailers < 0)
                                        {
                                            throw new InvalidVehicleConfigurationException(numberOfTrailers > 2 ? "Too many trailers added." : "Unable to add negative trailers.");
                                        }

                                    TitanHauler newTitanHauler = new TitanHauler(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, numberOfTrailers);
                                    fleetManager.AddVehicle(newTitanHauler, "Titan Hauler");
                                    break;

                                case VehicleType.GlacierTrans:
                                    Console.Write("Enter target temperature (must be between -20°C and 5°C): ");
                                    int targetTemperatureCelsius = Convert.ToInt32(Console.ReadLine());
                                        if (targetTemperatureCelsius < -20 || targetTemperatureCelsius > 5)
                                        {
                                             throw new InvalidVehicleConfigurationException("Invalid input. Please enter a temperature between -20°C and 5°C.");
                                        }

                                    GlacierTrans newGlacierTrans = new GlacierTrans(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, targetTemperatureCelsius);
                                    fleetManager.AddVehicle(newGlacierTrans, "Glacier Trans");
                                    break;

                                default:
                                    ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                                    break;
                            }
                            break;
                        case VehicleMenu.DisplayAllVehicles:
                            fleetManager.DisplayAllVehicles();
                            break;
                        case VehicleMenu.RemoveVehicle:
                            if (fleetManager.DisplayAllVehicles(false)) // only continue if there were vehicles to show
                            {
                                Console.Write("\nEnter Vehicle ID: ");
                                int removeVehicleID = Convert.ToInt32(Console.ReadLine());
                                fleetManager.RemoveVehicle(removeVehicleID);
                            }
                            break;
                        case VehicleMenu.BackToMainMenu:
                            ConsoleHelper.ClearScreen();
                            managingVehicles = false;
                            break;
                        default:
                            ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.HandleException(ex);
                }
            }
        }

        //////////////////////////////////////////////////////////// ---- CARGO --- ////////////////////////////////////////////////////////////////////

        static void ManageCargo()
        {
            bool managingCargo = true;
            ConsoleHelper.ClearScreen();
            while (managingCargo)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("     Manage Cargo");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Add Cargo");
                Console.WriteLine("2. Display All Cargo");
                Console.WriteLine("3. Remove Cargo");
                Console.WriteLine("4. Back to Main Menu");
                Console.WriteLine();
                ConsoleHelper.ChooseOptionStyling();

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    switch ((CargoMenu)option)
                    {
                        case CargoMenu.AddCargo:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\nSelect a cargo type:");
                            Console.ResetColor();
                            Console.WriteLine("1. Small Cargo (Weight: 1 - 30)kg");
                            Console.WriteLine("2. Medium Cargo (Weight: 30 - 1000)kg");
                            Console.WriteLine("3. Large Cargo (Weight: 1000 - 36000)kg");
                            Console.WriteLine("4. Refrigerated Cargo (Weight: 30 - 7000)kg");
                            ConsoleHelper.ChooseOptionStyling();
                            int type = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Cargo ID: ");
                            int newCargoID = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter a short description of what the cargo is:");
                            string newCargoDescription = Console.ReadLine();

                            Console.Write("Enter Cargo Weight: ");
                            double newCargoWeight = Convert.ToDouble(Console.ReadLine());

                            // validate cargo capacity based on cargo type
                            if (!(newCargoWeight <= 0 || newCargoWeight > 36000))
                            {
                                if ((CargoType)type == CargoType.SmallCargo && newCargoWeight > 30)
                                {
                                    throw new IncorrectCargoForType("Cargo too heavy for small cargo type.");
                                }
                                else if ((CargoType)type == CargoType.MediumCargo && (newCargoWeight > 1000 || newCargoWeight < 30))
                                {
                                    throw new IncorrectCargoForType(newCargoWeight > 1000 ? "Cargo too heavy for medium cargo type." : "Cargo too light for medium cargo type.");
                                }
                                else if ((CargoType)type == CargoType.LargeCargo && (newCargoWeight > 36000 || newCargoWeight < 1000))
                                {
                                    throw new IncorrectCargoForType("Cargo too light for large cargo type.");
                                }
                                else if ((CargoType)type == CargoType.RefrigeratedCargo && (newCargoWeight > 7000 || newCargoWeight < 30))
                                {
                                    throw new IncorrectCargoForType(newCargoWeight > 7000 ? "Cargo too heavy for refrigerated cargo type." : "Cargo too light for refrigerated cargo type.");
                                }
                            }
                            else
                            {
                                throw new ArgumentException(newCargoWeight > 36000 ? "Cargo capacity is too high for our transport options." : "Cargo capacity is too low for our transport options.");
                            }

                            switch ((CargoType)type)
                            {
                                case CargoType.SmallCargo:
                                    Console.Write("Is the cargo fragile? (Y/N): ");
                                    bool isFragile = ConsoleHelper. ConvertAnswerToBool(Console.ReadLine());
                                    SmallCargo newSmallCargo = new SmallCargo(newCargoID, newCargoDescription, newCargoWeight, isFragile);
                                    fleetManager.AddCargo(newSmallCargo);
                                    break;

                                case CargoType.MediumCargo:
                                    Console.Write("Does the cargo require a signature? (Y/N): ");
                                    bool requiresSignature = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());
                                    MediumCargo newMediumCargo = new MediumCargo(newCargoID, newCargoDescription, newCargoWeight, requiresSignature);
                                    fleetManager.AddCargo(newMediumCargo);
                                    break;

                                case CargoType.LargeCargo:
                                    Console.Write("Does the cargo require a forklift? (Y/N): ");
                                    bool requiresForklift = ConsoleHelper.ConvertAnswerToBool(Console.ReadLine());
                                    LargeCargo newLargeCargo = new LargeCargo(newCargoID, newCargoDescription, newCargoWeight, requiresForklift);
                                    fleetManager.AddCargo(newLargeCargo);
                                    break;

                                case CargoType.RefrigeratedCargo:
                                    Console.Write("Enter the required temperature (must be between -20°C and 5°C): ");
                                    int requiredTemperatureCelsius = Convert.ToInt32(Console.ReadLine());
                                        if (requiredTemperatureCelsius < -20 || requiredTemperatureCelsius > 5)
                                        {
                                            throw new ArgumentException("Invalid input. Please enter a temperature between -20°C and 5°C.");
                                        }
                                    RefrigeratedCargo newRefrigeratedCargo = new RefrigeratedCargo(newCargoID, newCargoDescription, newCargoWeight, requiredTemperatureCelsius);
                                    fleetManager.AddCargo(newRefrigeratedCargo);
                                    break;

                                default:
                                    ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                                    break;
                            }
                            break;

                        case CargoMenu.DisplayAllCargo:
                            fleetManager.DisplayAllCargo();
                            break;

                        case CargoMenu.RemoveCargo:
                            if (fleetManager.DisplayAllCargo(false)) // only continue if there were vehicles to show
                            {
                                Console.Write("\nEnter Cargo ID: ");
                                int removeCargoID = Convert.ToInt32(Console.ReadLine());
                                fleetManager.RemoveCargo(removeCargoID);
                            }
                            break;

                        case CargoMenu.BackToMainMenu:
                            ConsoleHelper.ClearScreen();
                            managingCargo = false;
                            break;

                        default:
                            ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.HandleException(ex);
                }
            }
        }

        /////////////////////////////////////////////////// ---- ASSIGN CARGO TO VEHICLE --- ///////////////////////////////////////////////////////////

        static void AssignCargoToVehicle()
        {
            ConsoleHelper.ClearScreen();
            // Check if there are vehicles / cargo in thge system
            if (fleetManager.DisplayAllVehicles(false))
            {
                Console.Write("\nEnter Vehicle ID: ");
                int vehicleID = Convert.ToInt32(Console.ReadLine());

                if (fleetManager.DisplayAllCargo(false))
                {
                    Console.Write("\nEnter Cargo ID: ");
                    int cargoID = Convert.ToInt32(Console.ReadLine());

                    fleetManager.AssignCargoToVehicle(vehicleID, cargoID);
                }
            }
        }

        /////////////////////////////////////////////////// ---- File Functionality --- ///////////////////////////////////////////////////////////

        static void ManageSaveOperations()
        {
            bool manageSaveOperations = true;
            ConsoleHelper.ClearScreen();
            while (manageSaveOperations)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("     Manage Save Operations");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. Save Current Loadout");
                Console.WriteLine("2. Load Saved State");
                Console.WriteLine("3. View All Saves");
                Console.WriteLine("4. Remove Saved State");
                Console.WriteLine("5. Clear Current Loadout");
                Console.WriteLine("6. Back To Main Menu");
                Console.WriteLine();
                ConsoleHelper.ChooseOptionStyling();

                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());

                    switch ((FileMenu)option)
                    {
                        case FileMenu.SaveCurrentLoadout:
                            Console.Write("Enter a name for this save: ");
                            string saveName = Console.ReadLine();
                            fleetManager.SaveCurrentLoadout(saveName);
                            break;
                        case FileMenu.LoadSavedState:
                            Console.Write("Enter the name of the save to load: ");
                            string loadName = Console.ReadLine();
                            fleetManager.LoadSavedState(loadName);
                            break;
                        case FileMenu.ViewAllSaves:
                            fleetManager.ViewAllSaves();
                            break;
                        case FileMenu.RemoveSavedState:
                            if (fleetManager.ViewAllSaves(false))
                            {
                                Console.Write("\nEnter the name of the save to remove: ");
                                string removeSaveName = Console.ReadLine();
                                fleetManager.RemoveSavedState(removeSaveName);
                            }
                            break;
                        case FileMenu.ClearCurrentLoadout:
                            fleetManager.ClearCurrentLoadout();
                            break;
                        case FileMenu.BackToMainMenu:
                            ConsoleHelper.ClearScreen();
                            manageSaveOperations = false;
                            break;
                        default:
                            ConsoleHelper.ShowError("Invalid menu option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.HandleException(ex);
                }
            }
        }
    }
}