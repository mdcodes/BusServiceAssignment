/*Name: Michal Drahorat
 * Description: The validation for for the Driver's hire date
 * 
 * Rev. History
 * October 28th, 2016 - Created by Michal Drahorat
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MDClassLibrary
{
    public class DateNotInFuture : ValidationAttribute
    {
        /// <summary>
        /// Makes sure the date entered is not in the future
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return ValidationResult.Success;
            }
            else if((DateTime)value > DateTime.Now)
            {
                return new ValidationResult("Date cannot be in the future");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
