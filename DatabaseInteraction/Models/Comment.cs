using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public class Comment:Message
    {
        public Comment(DataRow dr) 
            : base(dr)
        {

        }
    }
}
