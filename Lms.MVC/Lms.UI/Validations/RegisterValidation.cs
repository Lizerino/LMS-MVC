using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Validations
{
    public class RegisterValidation : ValidationAttribute
    {
        public RegisterValidation()
        {

        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
    }
}
