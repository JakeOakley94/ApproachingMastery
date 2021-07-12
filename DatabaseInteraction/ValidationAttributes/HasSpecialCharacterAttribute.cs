using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class HasSpecialCharacterAttribute : RegularExpressionAttribute
    {
        const string SPECIAL_CHAR_REGEX = ".*[ @$#_.].*";
        public HasSpecialCharacterAttribute() : base(SPECIAL_CHAR_REGEX) { }
    }
    
}
