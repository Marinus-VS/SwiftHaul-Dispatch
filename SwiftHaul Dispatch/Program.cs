using System;
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


        // helper method to convert user input of "Y" or "N" to a boolean value
        static bool ConvertAnswerToBool(string answer)
        {
            if (answer.ToUpper() == "Y")
            {
                return true;
            }
            else if (answer.ToUpper() == "N")
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Invalid input. Please enter 'Y' or 'N'.");
            }
        }

        public enum Menu
        {
            ManageVehicles = 1,
            ManageCargo,
            AssignCargoToVehicle,
            ViewDispatchLog,
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

        /////////////////////////////////////////////////// ---- MAIN METHOD --- ///////////////////////////////////////////////////////////

        static void Main(string[] args)
        {
            bool running = true;
            ConsoleHelper.ClearScreen();
            while (running)
            {
                Console.WriteLine("================================");
                Console.WriteLine("     Swift-Haul Dispatch");
                Console.WriteLine("================================");
                Console.WriteLine();
                Console.WriteLine("1. Manage Vehicles");
                Console.WriteLine("2. Manage Cargo");
                Console.WriteLine("3. Assign Cargo to Vehicle");
                Console.WriteLine("4. View Dispatch Log");
                Console.WriteLine("5. Exit");
                Console.WriteLine();
                Console.Write("Choose an option: ");

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
                            break;
                        case Menu.ViewDispatchLog:
                            break;
                        case Menu.Exit:
                            running = false;
                            break;
                        default:
                            ConsoleHelper.InvalidMenuOption();
                        break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowMessage(ex.ToString());
                }
            }
        }

        /////////////////////////////////////////////////// ---- Vehicle --- ///////////////////////////////////////////////////////////

        static void ManageVehicles()
        {
            bool managingVehicles = true;
            ConsoleHelper.ClearScreen();
            while (managingVehicles)
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("     Manage Vehicles");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine();
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Display All Vehicles");
                Console.WriteLine("3. Remove Vehicle");
                Console.WriteLine("4. Back to Main Menu");
                Console.WriteLine();
                Console.Write("Choose an option: ");
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    switch ((VehicleMenu)option)
                    {
                        case VehicleMenu.AddVehicle:
                            Console.WriteLine("\nSelect Vehicle Type:");
                            Console.WriteLine("1. Wasp Runner");
                            Console.WriteLine("2. Cascade Van");
                            Console.WriteLine("3. Titan Hauler");
                            Console.WriteLine("4. Glacier Trans");
                            Console.Write("Choose an option: ");

                            int type = Convert.ToInt32(Console.ReadLine());

                            // shared fields of all vehicles
                            Console.Write("Enter Vehicle ID: ");
                            int newVehicleID = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Vehicle Name: ");
                            string newVehicleName = Console.ReadLine();

                            Console.Write("Enter Vehicle Milage: ");
                            int newVehicleMileage = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Vehicle Capacity (kg): ");
                            int newVehicleCapacity = Convert.ToInt32(Console.ReadLine());

                            // validate vehicle capacity based on vehicle type
                                if ((VehicleType)type == VehicleType.WaspRunner && (newVehicleCapacity > 30 || newVehicleCapacity < 1))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 30 ? "Vehicle capacity too high for Wasp Runner." : "Vehicle capacity too low for Wasp Runner.");
                                }
                                else if ((VehicleType)type == VehicleType.CascadeVan && (newVehicleCapacity > 1700 || newVehicleCapacity < 31))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 1700 ? "Vehicle capacity too high for Cascade Van." : "Vehicle capacity too low for Cascade Van.");
                                }
                                else if ((VehicleType)type == VehicleType.TitanHauler && (newVehicleCapacity > 36000 || newVehicleCapacity < 1001))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 36000 ? "Vehicle capacity too high for Titan Hauler." : "Vehicle capacity too low for Titan Hauler.");
                                }
                                else if ((VehicleType)type == VehicleType.GlacierTrans && (newVehicleCapacity > 7000 || newVehicleCapacity < 31))
                                {
                                    throw new IncorrectVehicleForType(newVehicleCapacity > 7000 ? "Vehicle capacity too high for Glacier Trans." : "Vehicle capacity too low for Glacier Trans.");
                                }
                                else if (newVehicleMileage < 0)
                                {
                                    throw new ArgumentException("Invalid input. Please enter a non-negative mileage.");
                                }

                            switch ((VehicleType)type)
                            {
                                case VehicleType.WaspRunner:
                                    Console.Write("Enter Max Speed (km/h): ");
                                    int maxSpeed = Convert.ToInt32(Console.ReadLine());
                                    Console.Write("Is Weather Restricted (Y/N): "); // unable to drive in bad weather conditions
                                    bool isWeatherRestricted = ConvertAnswerToBool(Console.ReadLine());
                                    WaspRunner newWaspRunner = new WaspRunner(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, maxSpeed, isWeatherRestricted);
                                    fleetManager.AddVehicle(newWaspRunner);
                                    ConsoleHelper.ShowMessage($"{newWaspRunner.VehicleName} (Wasp Runner) has been added to the fleet.");
                                    break;

                                case VehicleType.CascadeVan:
                                    Console.Write("Enter max delivery stops: ");
                                    int maxDeliveryStops = Convert.ToInt32(Console.ReadLine());
                                    CascadeVan newCascadeVan = new CascadeVan(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, maxDeliveryStops);
                                    fleetManager.AddVehicle(newCascadeVan);
                                    ConsoleHelper.ShowMessage($"{newCascadeVan.VehicleName} (Cascade Van) has been added to the fleet.");
                                    break;

                                case VehicleType.TitanHauler:
                                    Console.Write("Enter number of trailers (max = 2): ");
                                    int numberOfTrailers = Convert.ToInt32(Console.ReadLine());
                                        if (numberOfTrailers > 2)
                                        {
                                            throw new InvalidVehicleConfigurationException("Invalid input. Please enter a number between 0 and 2.");
                                        }

                                    TitanHauler newTitanHauler = new TitanHauler(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, numberOfTrailers);
                                    fleetManager.AddVehicle(newTitanHauler);
                                    ConsoleHelper.ShowMessage($"{newTitanHauler.VehicleName} (Titan Hauler) has been added to the fleet.");
                                    break;

                                case VehicleType.GlacierTrans:
                                    Console.Write("Enter target temperature (must be between -20°C and 5°C): ");
                                    int targetTemperatureCelsius = Convert.ToInt32(Console.ReadLine());
                                        if (targetTemperatureCelsius < -20 || targetTemperatureCelsius > 5)
                                        {
                                             throw new InvalidVehicleConfigurationException("Invalid input. Please enter a temperature between -20°C and 5°C.");
                                        }

                                    GlacierTrans newGlacierTrans = new GlacierTrans(newVehicleID, newVehicleName, newVehicleMileage, newVehicleCapacity, targetTemperatureCelsius);
                                    fleetManager.AddVehicle(newGlacierTrans);
                                    ConsoleHelper.ShowMessage($"{newGlacierTrans.VehicleName} (Glacier Trans) has been added to the fleet.");
                                    break;

                                default:
                                    ConsoleHelper.InvalidMenuOption();
                                    break;
                            }
                            break;
                        case VehicleMenu.DisplayAllVehicles:
                            fleetManager.DisplayAllVehicles();
                            break;
                        case VehicleMenu.RemoveVehicle:
                            fleetManager.DisplayAllVehicles(false); // false wont prompt the waiting for key message
                            Console.Write("\nEnter Vehicle ID: ");
                            int removeVehicleID = Convert.ToInt32(Console.ReadLine());
                            fleetManager.RemoveVehicle(removeVehicleID);
                            break;
                        case VehicleMenu.BackToMainMenu:
                            ConsoleHelper.ClearScreen();
                            managingVehicles = false;
                            break;
                        default:
                            ConsoleHelper.InvalidMenuOption();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowMessage(ex.ToString());
                }
            }
        }

        /////////////////////////////////////////////////// ---- CARGO --- ///////////////////////////////////////////////////////////

        static void ManageCargo()
        {
            bool managingCargo = true;
            ConsoleHelper.ClearScreen();
            while (managingCargo)
            {
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine("     Manage Cargo");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                Console.WriteLine();
                Console.WriteLine("1. Add Cargo");
                Console.WriteLine("2. Display All Cargo");
                Console.WriteLine("3. Remove Cargo");
                Console.WriteLine("4. Back to Main Menu");
                Console.WriteLine();
                Console.Write("Choose an option: ");
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    switch ((CargoMenu)option)
                    {
                        case CargoMenu.AddCargo:
                            Console.WriteLine("\nSelect a cargo type:");
                            Console.WriteLine("1. Small Cargo");
                            Console.WriteLine("2. Medium Cargo");
                            Console.WriteLine("3. Large Cargo");
                            Console.WriteLine("4. Refrigerated Cargo");
                            Console.Write("Choose an option: ");
                            int type = Convert.ToInt32(Console.ReadLine());

                            Console.Write("Enter Cargo ID: ");
                            int newCargoID = Convert.ToInt32(Console.ReadLine());

                            Console.WriteLine("Enter a short description of what the cargo is:");
                            string newCargoDescription = Console.ReadLine();

                            Console.Write("Enter Cargo Weight: ");
                            double newCargoWeight = Convert.ToDouble(Console.ReadLine());

                            if ((CargoType)type == CargoType.SmallCargo && newCargoWeight > 30)
                            {
                                throw new IncorrectCargoForType("Cargo too heavy for small cargo type.");
                            }
                            else if ((CargoType)type == CargoType.MediumCargo && (newCargoWeight > 1000 || newCargoWeight < 31))
                            {
                                throw new IncorrectCargoForType(newCargoWeight > 1000 ? "Cargo too heavy for medium cargo type." : "Cargo too light for medium cargo type.");
                            }
                            else if ((CargoType)type == CargoType.LargeCargo && (newCargoWeight > 36000 || newCargoWeight < 1001))
                            {
                                throw new IncorrectCargoForType(newCargoWeight > 36000 ? "Cargo too heavy for large cargo type." : "Cargo too light for large cargo type.");
                            }

                            switch ((CargoType)type)
                            {
                                case CargoType.SmallCargo:
                                    Console.Write("Is the cargo fragile? (Y/N)");
                                    bool isFragile = ConvertAnswerToBool(Console.ReadLine());
                                    SmallCargo newSmallCargo = new SmallCargo(newCargoID, newCargoDescription, newCargoWeight, isFragile);
                                    fleetManager.AddCargo(newSmallCargo);
                                    ConsoleHelper.ShowMessage($"{newSmallCargo.CargoID} has been added to the system.");
                                    break;

                                case CargoType.MediumCargo:
                                    Console.Write("Does the cargo require a signature? (Y/N): ");
                                    bool requiresSignature = ConvertAnswerToBool(Console.ReadLine());
                                    MediumCargo newMediumCargo = new MediumCargo(newCargoID, newCargoDescription, newCargoWeight, requiresSignature);
                                    fleetManager.AddCargo(newMediumCargo);
                                    ConsoleHelper.ShowMessage($"{newMediumCargo.CargoID} has been added to the system.");
                                    break;

                                case CargoType.LargeCargo:
                                    Console.Write("Does the cargo require a forklift? (Y/N): ");
                                    bool requiresForklift = ConvertAnswerToBool(Console.ReadLine());
                                    LargeCargo newLargeCargo = new LargeCargo(newCargoID, newCargoDescription, newCargoWeight, requiresForklift);
                                    fleetManager.AddCargo(newLargeCargo);
                                    ConsoleHelper.ShowMessage($"{newLargeCargo.CargoID} has been added to the system.");
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
                                    ConsoleHelper.ShowMessage($"{newRefrigeratedCargo.CargoID} has been added to the system.");
                                    break;

                                default:
                                    ConsoleHelper.InvalidMenuOption();
                                    break;
                            }
                            break;
                        case CargoMenu.DisplayAllCargo:
                            fleetManager.DisplayAllCargo();
                            break;
                        case CargoMenu.RemoveCargo:
                            fleetManager.DisplayAllCargo(false);
                            Console.Write("\nEnter Cargo ID: ");
                            int removeCargoID = Convert.ToInt32(Console.ReadLine());
                            fleetManager.RemoveCargo(removeCargoID);
                            break;
                        case CargoMenu.BackToMainMenu:
                            ConsoleHelper.ClearScreen();
                            managingCargo = false;
                            break;
                        default:
                            ConsoleHelper.InvalidMenuOption();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    ConsoleHelper.ShowMessage(ex.ToString());
                }
            }
        }

        /////////////////////////////////////////////////// ---- ASSIGN CARGO TO VEHICLE --- ///////////////////////////////////////////////////////////

        static void AssignCargoToVehicle()
        {

        }
    }
}