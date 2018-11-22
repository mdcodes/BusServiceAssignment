using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MDClassLibrary
{
    public class MDValidations
    {
        /// <summary>
        /// Accepts a string, capitalises the first letter, and sends it back
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Capitalise(string input)
        {
            if(input == null)
            {
                return input;
            }
            else
            {
                input.ToLower();
                input.Trim();
                input.Substring(0).ToUpper();
                return input;
            }
        }
    }
}
