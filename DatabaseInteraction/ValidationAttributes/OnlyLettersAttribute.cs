using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.ValidationAttributes
{
    public class OnlyLettersAttribute : RegularExpressionAttribute
    {
        const string ONLY_LETTERS_REGEX = ".*[a-zA-ZÇüéâäàåçêëèïîìÄÅÉôöòûùÿÖÜáíóúñÑ].*";
        public OnlyLettersAttribute() : base(ONLY_LETTERS_REGEX) { }
    }
}
