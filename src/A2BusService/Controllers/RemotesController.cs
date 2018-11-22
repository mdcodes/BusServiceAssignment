/*Name: Michal Drahorat
 * Description: The controller for remote actions
 * 
 * Rev. History
 * October 28th, 2016 - Created by Michal Drahorat
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A2BusService.Models;
using System.Text.RegularExpressions;

namespace A1BusService.Controllers
{
    public class RemotesController : Controller
    {
        #region DB Setup
        private readonly BusServiceContext _context;

        public RemotesController(BusServiceContext context)
        {
            _context = context;
        }
        #endregion
        
        //Verify the province code is only two characters, as well as if the code actually exists
        public JsonResult CheckProvinceCode(string provinceCode)
        {
            Regex validProvCode = new Regex(@"[a-z][a-z]", RegexOptions.IgnoreCase);
            if (!validProvCode.IsMatch(provinceCode))
            {
                return Json("Country code MUST match the following format: AB");
            }
            if (provinceCode.Length != 2)
            {
                return Json("The province code MUST be two letters. No more, no less.");
            }
            try
            {
                var province = _context.Province.SingleOrDefault(a => a.ProvinceCode == provinceCode);
                if (province == null)
                {
                    return Json($"Province code '{provinceCode}' does not exist on file");
                }
            }
            catch (Exception ex)
            {
                return Json($"Exception caught while validating Province: {ex.GetBaseException().Message}");
            }
            provinceCode.ToUpper();
            return Json(true);
        }
        /// <summary>
        /// Validates the home phone number
        /// </summary>
        /// <param name="homePhone"></param>
        /// <returns></returns>
        public JsonResult HomePhoneValidation(string homePhone)
        {
            Regex validPhone = new Regex($"[0-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]");
            if (homePhone == null)
            {
                return Json(homePhone);
            }
            if(!validPhone.IsMatch(homePhone))
            {
                return Json("Phone number entered in incorrect format. Enter in format: 123-456-7890");
            }
            return Json(true);
        }
    }
}