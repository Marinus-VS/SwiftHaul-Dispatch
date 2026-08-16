using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    // exception for when a vehicle is not found
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException(string message) : base(message)
        {
        }
    }

    // exception for when a vehicle's capacity does not match teh vehicle type
    public class IncorrectVehicleForType : Exception
    {
        public IncorrectVehicleForType(string message) : base(message)
        {
        }
    }

    // exception for when a cargo item is not found
    public class CargoNotFoundException : Exception
    {
        public CargoNotFoundException(string message) : base(message)
        {
        }
    }

    // exeption for when cargo is too large / small for the cargo type
    public class IncorrectCargoForType : Exception
    {
        public IncorrectCargoForType(string message) : base(message)
        {
        }
    }

    // exception for when a vehicle's configuration (trailers, temperature, etc) is invalid
    public class InvalidVehicleConfigurationException : Exception
    {
        public InvalidVehicleConfigurationException(string message) : base(message)
        {
        }
    }

    // exeption for when cargo is already assigned
    public class CargoAlreadyAssignedException : Exception
    {
        public CargoAlreadyAssignedException(string message) : base(message)
        {
        }
    }

    // exeption for when a vheclie is overloaded (unable to load anothor package)
    public class VehicleOverloadException : Exception
    {
        public VehicleOverloadException(string message) : base(message) 
        { 
        }
    }

    // exeption for when a file is not found
    public class SaveFileNotFoundException : Exception
    {
        public SaveFileNotFoundException(string message) : base(message)
        {
        }
    }
}
