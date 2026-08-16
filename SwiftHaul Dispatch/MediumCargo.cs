using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwiftHaul_Dispatch
{
    public class MediumCargo : Cargo
    {
        private bool _requiresSignature;
        public bool RequiresSignature
        {
            get { return _requiresSignature; }
            set { _requiresSignature = value; }
        } 

        public MediumCargo(int cargoID, string description, double weight, bool requiresSignature)
            : base(cargoID, weight, description)
        {
            this.RequiresSignature = requiresSignature;
        }

        public override void DisplayAllCargoInfo()
        {
            base.DisplayAllCargoInfo();
            Console.WriteLine($"Requires Signature: {RequiresSignature}");
        }

        public override string GetCargoTypeDetails()
        {
            return $"Requires Signature: {RequiresSignature}";
        }

        public override string GetHandlingInstructions()
        {
            return RequiresSignature ? "This item requires a signature." : "This item does not require a signature.";
        }
    }
}