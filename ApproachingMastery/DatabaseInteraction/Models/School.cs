using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public class School
    {
        public Guid SchoolID { get; set; }
        public string SchoolName { get; set; }

        public School(DataRow dr)
        {

            SchoolID = (Guid)dr["guidSchoolID"];
            SchoolName = dr["strSchoolName"].ToString();
        }





    }
}
