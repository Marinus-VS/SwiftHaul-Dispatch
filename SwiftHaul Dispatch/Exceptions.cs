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
}
