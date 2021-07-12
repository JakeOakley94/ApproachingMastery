using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class HasNumberAttribute : RegularExpressionAttribute
    {
        const string HAS_NUMBER = @".*\d.*";
        public HasNumberAttribute() : base(HAS_NUMBER) { }
    }
}
