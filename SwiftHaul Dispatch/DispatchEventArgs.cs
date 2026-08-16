using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class DispatchEventArgs : EventArgs
    {
        // properties to hold event data
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }

        // constructor to initialize the event data
        public DispatchEventArgs(int vehicleID, string vehicleName, string message)
        {
            VehicleID = vehicleID;
            VehicleName = vehicleName;
            Message = message;
            Timestamp = DateTime.Now;
        }
    }
    // delegate for the event handler
    public delegate void DispatchEventHandler(object sender, DispatchEventArgs e);
}
