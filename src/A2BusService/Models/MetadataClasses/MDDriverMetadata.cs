/*Name: Michal Drahorat
 * Description: The metadata class for the Driver
 * 
 * Rev. History
 * October 28th, 2016 - Created by Michal Drahorat
 */

using MDClassLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace A2BusService.Models
{
    [ModelMetadataType(typeof(MDDriverMetadata))]
    public partial class Driver : IValidatableObject
    {
        BusServiceContext _context = BusServiceContext_Singleton.Context();
        /// <summary>
        /// Capitalises the first name and last name
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            FirstName = MDValidations.Capitalise(FirstName);
            LastName = MDValidations.Capitalise(LastName);

            if(LastName == null || LastName.Trim() == "")
            {
                yield return new ValidationResult(
                    "Last name cannot be null or spaces",
                   new[] { "LastName" });
            }

            yield return ValidationResult.Success;
        }
    }
    public class MDDriverMetadata
    {
        public int DriverId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName { get; set; }
        [Required]
        [Remote("HomePhoneValidation","Remotes")]
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        [Required]
        [PostalCodeValidation]
        [Remote("UpCasePostCode","Remotes")]
        public string PostalCode { get; set; }
        [Remote("CheckProvinceCode", "Remotes")]
        public string ProvinceCode { get; set; }
        [Required]
        [DateNotInFuture]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateHired { get; set; }
    }
}
