using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DatabaseInteraction.ValidationAttributes;

namespace DatabaseInteraction.Models
{
    public enum AccomodationType
    {
        [Display(Name = "Extended Time")]
        ExtendedTime = 1,
        [Display(Name = "Frequent Breaks")]
        FrequentBreakes = 2,
        [Display(Name = "Small Group")]
        smallGroup = 3,
        [Display(Name = "Math Tools")]
        MathTools = 4,
        [Display(Name = "Read Aloud")]
        ReadAloud = 5,
        [Display(Name = "Scribe")]
        scibe = 6
    }

    public class Accomodation
    {
        #region Properties 
        public short AccomodationID { get; set; }

        public string strAccomodation { get; set; }

        public Guid StudentID { get; set; }
        public Guid AddedBy { get; set; }
        public AccomodationType AccomodationType { get; set; }
        public List<AccomodationDetail> AccomodationDetails { get; set; }

        #endregion

        #region Constructors

        public Accomodation() { }
        public Accomodation(Guid studentID, Guid addedby, AccomodationType accomodationID)
        {
            StudentID = studentID;
            AddedBy = addedby;
            AccomodationType = accomodationID;
        }

        public Accomodation(Guid studentID, Guid addedby)
        {
            StudentID = studentID;
            AddedBy = addedby;

        }

        public Accomodation(DataRow dr)
        {
            const string ACCOMODATION_ID_COLUMN = "shtAccomodationID",
                         STR_ACCOMODATION_COLUMN = "strAccomodation";
            StudentID = (Guid)dr["guidStudentID"];
            AccomodationID = Convert.ToInt16(dr[ACCOMODATION_ID_COLUMN]);
            strAccomodation = Convert.ToString(dr[STR_ACCOMODATION_COLUMN]);
            GetAccomodationDetails();
        }

        #endregion

        #region Methods

        public bool GetAccomodationDetails()
        {
            bool result = false;


            const string PROCEDURE_PARAM = "uspGetStudentAccommodationDetails",
                        GUID_Student_ID_PARAM = "@guidStudentID",
                        ACCOMMODATION_ID_PARAM = @"shtAccomodationID";

            SqlParameter studentIDParam,
                         accommodationIDParam;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM);
                Database.SetParameter(ref cmd, GUID_Student_ID_PARAM, StudentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                Database.SetParameter(ref cmd, ACCOMMODATION_ID_PARAM, AccomodationID, SqlDbType.SmallInt, out accommodationIDParam);
                if (Database.ExecuteProcedure(ref cmd, out dt))
                {
                    AccomodationDetails = new List<AccomodationDetail>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        AccomodationDetails.Add(new AccomodationDetail(dr));
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }

            return result;


        }

        public bool AddAccommodationDetail(AccomodationDetail detail)
        {
            return Database.AddAccommodationDetail(this, detail);
        }

        public bool RemoveAccommodationDetail(DetailType detailType)
        {
            return Database.RemoveAccomodationDetail(StudentID, AccomodationID, detailType);
        }

        #endregion
    }
}
