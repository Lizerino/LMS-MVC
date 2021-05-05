using Lms.MVC.UI.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Lms.MVC.UI.Areas.Identity.Pages.Account.RegisterModel;

namespace Lms.MVC.UI.Validations
{
    public class RegisterAttribute : ValidationAttribute
    {
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            const string errorMessage = "A student must be enrolled to 1 course";

            var model = (InputModel)validationContext.ObjectInstance;
            if (value is List<int> input)
            {
            if (model.Role != "Teacher" && model.Role != "Admin")
            {
                if (input.Count == 1)
                {
                    return ValidationResult.Success;
                }
            }
                else if (model.Role == "Teacher" || model.Role == "Admin")
                {
                    if (input.Count >= 0)
                    {
                        return ValidationResult.Success;
                    }
                }
                else
                {
                    return new ValidationResult(errorMessage);
                }
            }
            else
            {
                    
                    return new ValidationResult(errorMessage);
            }
            return new ValidationResult(errorMessage);
        }
    }
    
}
