using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public enum DetailType
    {
        //Needs values
        [Display(Name = "Number of Occurances")]
        NumberOfOccurances = 1, 
        Duration = 2

    }

    public class AccomodationDetail
    {

        #region Properties 
        public Guid StudentID { get; set; }
        public short AccomodationID { get; set; }
        public DetailType DetailType { get; set; }
        public string Value { get; set; }

        #endregion

        #region Constructors

        public AccomodationDetail() { }
        public AccomodationDetail(Guid studentID, short accomodationID, DetailType detailtype, string value)
        {
            StudentID = studentID;
            AccomodationID = AccomodationID;
            DetailType = detailtype;
            Value = value;
        }

        public AccomodationDetail(DataRow dr)
        {
            StudentID = (Guid)dr["guidStudentID"];
            AccomodationID = Convert.ToInt16(dr["shtAccomodationID"]);
            DetailType = (DetailType)Convert.ToInt16(dr["shtDetailTypeID"]);
            Value = dr["strDetailValue"].ToString();
        }

        public bool UpdateDetail()
        {
            return Database.UdpateAccommodationdetail(this);
        }
        #endregion
    }
}
