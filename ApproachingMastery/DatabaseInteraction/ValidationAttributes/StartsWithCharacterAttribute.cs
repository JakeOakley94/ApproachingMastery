using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class StartsWithCharacterAttribute : RegularExpressionAttribute
    {
        const string LETTER_AT_BEGINNING = @"^[a-zA-Z].*";
        public StartsWithCharacterAttribute() : base(LETTER_AT_BEGINNING) { }
    }


}
