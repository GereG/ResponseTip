using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArbiterTown.Models
{
    public class OddNumber : ValidationAttribute
    {
        public OddNumber()
            :base("{0} needs to be odd number.")
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int valueInt = (int)value;
            if (valueInt % 2 == 1)
            {
                return ValidationResult.Success;
            }
            else
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }
        }

    }
}