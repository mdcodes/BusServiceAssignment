/*Name: Michal Drahorat
 * Description: The postal code validation class for the Driver
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
    /// <summary>
    /// Checks for a valid postal code entry
    /// </summary>
    public class PostalCodeValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Accepts the postal code entry, matches it against the valid entry, and returns either a success or a fail
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex validPostCode = new Regex(@"[a-z]\d[a-z]\d[a-z]\d", RegexOptions.IgnoreCase);
            if (validPostCode.IsMatch(value.ToString()))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"The {validationContext.DisplayName} '{value.ToString()}' does not match the correct pattern: A1B 2C3");
            } 
        }
    }
}
