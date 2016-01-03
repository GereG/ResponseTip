using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BtcHandling
{
    public class BtcAddressAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var address = Convert.ToString(value);

            if (String.IsNullOrEmpty(address))
                return true;

            bool isReturnAddressValid = true;
            //TODO UNCOMENT NEXT LINE FOR BTC ADRESS VERIFICATION(after database play)
 //           bool isReturnAddressValid = BtcHandling.BtcHandlingClass.IsAddressValid(address);

            return isReturnAddressValid;
        }

        public override string FormatErrorMessage(string name)
        {
            return "This is not valid bitcoin address.";
        }
    }
}
