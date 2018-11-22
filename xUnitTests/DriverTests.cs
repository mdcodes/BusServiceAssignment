using A2BusService.Models;
using A2BusService.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.ComponentModel.DataAnnotations;

namespace xUnitTests
{
    public class DriverTests
    {
        #region data connection and global variables

        BusServiceContext _context = BusServiceContext_Singleton.Context();

        Boolean wasAccepted = true;
        string errorMessages = "";

        Driver driver;
        
        #endregion

        #region Driver object and validation test

        public void initialize()
        {
            wasAccepted = true;
            errorMessages = "";

            driver = new Driver();
            driver.City = "Kitchener";
            driver.DateHired = DateTime.Now;
            driver.FirstName = "Michal";
            driver.FullName = "Drahorat, Michal";
            driver.HomePhone = "123-456-7890";
            driver.LastName = "Drahorat";
            driver.PostalCode = "A1B 2C3";
            driver.ProvinceCode = "ON";
            driver.Street = "299 Doon Valley Drive";
            driver.WorkPhone = "123-456-7890";
        }

        [Fact]
        public void BenchmarkDriver_shouldBeAccepted()
        {
            //arrange
            initialize();
            //act
            wasAccepted = RunValidate(out errorMessages);
            //assert
            Assert.True(wasAccepted, errorMessages);
        }

        #endregion

        #region utilities: RunValidate, Cleanup, etc
        
        public bool RunValidate(out string errors)
        {
            bool accepted = true;
            errors = "";
            var results = driver.Validate(new ValidationContext(driver));

            foreach (ValidationResult item in results)
            {
                if(item != ValidationResult.Success)
                {
                    accepted = false;
                    errorMessages += item.ErrorMessage + ">>>...";

                }
            }
            return accepted;
        }
        
        #endregion
    }
}
