using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class HasUpperCaseAttribute : RegularExpressionAttribute
    {
        const string HAS_UPPER = @".*[A-Z].*";
        public HasUpperCaseAttribute() : base(HAS_UPPER) { }
    }


}
