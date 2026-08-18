using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class LargeCargo : Cargo
    {
        private bool _requiresForklift;
        public bool RequiresForklift
        {
            get { return _requiresForklift; }
            set { _requiresForklift = value; }
        }

        public LargeCargo(int cargoID, string description, double weight, bool requiresForklift)
            : base(cargoID, description, weight)
        {
            this.RequiresForklift = requiresForklift;
        }
        public override void DisplayAllCargoInfo()
        {
            base.DisplayAllCargoInfo();
            Console.WriteLine($"Requires Forklift: {RequiresForklift}");
        }

        public override string GetHandlingInstructions()
        {
            return RequiresForklift ? "This item requires a forklift." : "This item does not require a forklift.";
        }

        public override string GetCargoTypeDetails()
        {
            return RequiresForklift ? "This item requires a forklift." : "This item does not require a forklift.";
        }

        public override double CalculateShippingCost(double ratePerKg)
        {
            double baseCost = base.CalculateShippingCost(ratePerKg);
            double extraForklift = (RequiresForklift ? baseCost * 0.15 : 0); // 15% surcharge for items requiring a forklift
            return baseCost + extraForklift;
        }
    }
}
