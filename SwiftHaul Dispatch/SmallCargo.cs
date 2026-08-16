using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class SmallCargo : Cargo
    {
        private bool _isFragile;
        public bool IsFragile
        {
            get { return _isFragile; }
            set { _isFragile = value; }
        }

        public SmallCargo(int cargoID, string description, double weight, bool isFragile)
            : base(cargoID, weight, description)
        {
            this.IsFragile = isFragile;
        }

        public override void DisplayAllCargoInfo()
        {
            base.DisplayAllCargoInfo();
            Console.WriteLine($"Is Fragile: {IsFragile}");
        }

        public override string GetCargoTypeDetails()
        {
            return $"Fragile: {IsFragile}";
        }

        public override string GetHandlingInstructions()
        {
            return IsFragile ? "This item is fragile. Handle with care." : "This item is not fragile.";
        }

        public override double CalculateShippingCost(double ratePerKg)
        {
            double baseCost = base.CalculateShippingCost(ratePerKg);
            double extraBubbleWrap = (IsFragile ? baseCost * 0.10 : 0); // 10% surcharge for extra bubble wrap and padding for fragile items
            return baseCost + extraBubbleWrap;
        }
    }
}