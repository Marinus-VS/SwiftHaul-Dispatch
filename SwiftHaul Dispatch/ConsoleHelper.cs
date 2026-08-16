using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class ConsoleHelper
    {
        // helper method for clearing the screen
        public static void ClearScreen()
        {
            Console.Clear();
        }

        // method to display an invalid menu option message
        public static void InvalidMenuOption()
        {
            ClearScreen();
            ConsoleHelper.ShowMessage("Invalid menu option. Please try again.");
            Console.Clear();
        }

        // helper method to press any key to continue
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // helper method to show a message and wait for user input
        public static void ShowMessage(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            PressAnyKeyToContinue();
        }

        // helper method to handle exceptions and show appropriate messages
        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case FormatException formatEx:
                    ShowMessage("Invalid input. Please enter a valid number.");
                    break;

                case IncorrectCargoForType cargoTypeEx:
                    ShowMessage(cargoTypeEx.Message);
                    break;

                case InvalidVehicleConfigurationException vehicleConfigEx:
                    ShowMessage(vehicleConfigEx.Message);
                    break;

                case VehicleNotFoundException vehicleNotFoundEx:
                    ShowMessage(vehicleNotFoundEx.Message);
                    break;

                case CargoNotFoundException cargoNotFoundEx:
                    ShowMessage(cargoNotFoundEx.Message);
                    break;

                case ArgumentException argEx:
                    ShowMessage(argEx.Message);
                    break;

                default:
                    ShowMessage($"An unexpected error occurred: {ex.Message}");
                    break;
            }
        }
    }
}
