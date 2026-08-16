using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public interface ICargo
    {
        string GetHandlingInstructions();
        double CalculateShippingCost(double ratePerKg);
    }
}
