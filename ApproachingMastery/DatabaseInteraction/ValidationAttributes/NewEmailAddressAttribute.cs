using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class NewEmailAddressAttribute:RegularExpressionAttribute
    {
        const string EMAIL_REGEX = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
        public NewEmailAddressAttribute() : base(EMAIL_REGEX) { ErrorMessage = "Invalid Email Address"; }
    }
}
