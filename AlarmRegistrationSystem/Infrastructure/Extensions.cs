using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlarmRegistrationSystem.Infrastructure
{
    public static class Extensions
    {

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        /// <summary>
        /// Checks whether the element contains text regardless of letter size and spaces
        /// </summary>
        public static bool IsStringContains(this string element, string text)
        {
            if(element.ToLower().Contains(text.ToLower()) || element.ToLower().Replace(" ", "").Contains(text.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
