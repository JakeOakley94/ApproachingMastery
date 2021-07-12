using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class IsNotOnlyWhiteSpaceAttribute:RegularExpressionAttribute
    {
        const string NOT_WHITESPACE_REGEX = @"^(?!\s*$).+";
        public IsNotOnlyWhiteSpaceAttribute() : base(NOT_WHITESPACE_REGEX) { }
    }
}
