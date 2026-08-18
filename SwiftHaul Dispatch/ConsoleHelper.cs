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

        // helper method to convert user input of "Y" or "N" to a boolean value
        public static bool ConvertAnswerToBool(string answer)
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

        // helper method to press any key to continue
        public static void PressAnyKeyToContinue()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ResetColor();
            Console.ReadKey(true);
            Console.Clear();
        }
        
        // helper to style all the 'Choose an option' texts references
        public static void ChooseOptionStyling()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Choose an option: ");
            Console.ResetColor();
        }

        // helper method to show a styled error message in red with a border
        public static void ShowSuccess(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            string border = new string('=', message.Length + 4);
            Console.WriteLine(border);
            Console.WriteLine($"  {message}");
            Console.WriteLine(border);

            // reset text colour
            Console.ResetColor();
            PressAnyKeyToContinue();
        }

        // helper method to show a styled error message in red with a border
        public static void ShowWarning(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            string border = new string('=', message.Length + 4);
            Console.WriteLine(border);
            Console.WriteLine($"  {message}");
            Console.WriteLine(border);

            // reset text colour
            Console.ResetColor();
        }

        // helper method to show a styled error message in red with a border
        public static void ShowError(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;

            string border = new string('=', message.Length + 4);
            Console.WriteLine(border);
            Console.WriteLine($"  {message}");
            Console.WriteLine(border);

            // reset text colour
            Console.ResetColor();
            PressAnyKeyToContinue();
        }

        // helper method to handle exceptions and show appropriate error messages
        public static void HandleException(Exception ex)
        {
            switch (ex)
            {
                case VehicleNotFoundException vehicleNotFoundEx:
                    ShowError(vehicleNotFoundEx.Message);
                    break;

                case IncorrectVehicleForType incorrectVehicleEx:
                    ShowError(incorrectVehicleEx.Message);
                    break;

                case CargoNotFoundException cargoNotFoundEx:
                    ShowError(cargoNotFoundEx.Message);
                    break;

                case IncorrectCargoForType cargoTypeEx:
                    ShowError(cargoTypeEx.Message);
                    break;

                case InvalidVehicleConfigurationException vehicleConfigEx:
                    ShowError(vehicleConfigEx.Message);
                    break;

                case CargoAlreadyAssignedException cargoAssignnedEx:
                    ShowError(cargoAssignnedEx.Message);
                    break;

                case VehicleOverloadException vehicleAssignnedEx:
                    ShowError(vehicleAssignnedEx.Message);
                    break;

                case SaveFileNotFoundException saveNotFoundEx:
                    ShowError(saveNotFoundEx.Message);
                    break;

                case FormatException formatEx:
                    ShowError("Invalid input. Please enter a valid number.");
                    break;

                case ArgumentException argEx:
                    ShowError(argEx.Message);
                    break;

                default:
                    // default message so that the program does not stop running
                    ShowError($"An unexpected error occurred. Please try again. Error: {ex.Message}");
                    break;
            }
        }
    }
}
