using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class FutureDate:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dateTime;

           var isValid = DateTime.TryParseExact(Convert.ToString(value),
                "d MMM yyyy",
                CultureInfo.CurrentCulture,      // culture-specific format information, and style.
                DateTimeStyles.None,
                out dateTime);

            //boolean expression to check if the date in the future
            return (isValid && dateTime > DateTime.Now);
        }
    }
}