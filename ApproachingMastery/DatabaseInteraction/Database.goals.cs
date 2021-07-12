using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DatabaseInteraction.Models;

namespace DatabaseInteraction
{
    public partial class Database
    {
        public static bool GetGoalAssignments(Guid goalID, out List<Assignment> assignments)
        {
            const string PROC_NAME = "uspGetAssignment",
                         GOAL_ID_PARAM = "@guidGoalID";
            assignments = new List<Assignment>();
            SqlParameter goalIDParam;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, GOAL_ID_PARAM, goalID, SqlDbType.UniqueIdentifier, out goalIDParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                foreach (DataRow dr in dt.Rows)
                {
                    assignments.Add(new Assignment(dr));
                }
                return true;
            }
            catch (Exception ex)
            {
                assignments.Clear();
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool RemoveGoalAssignment(Guid goalID, long assignmentID)
        {
            const string PROC_NAME = "uspRemoveAssignment",
                             GOAL_ID_PARAM = "@guidGoalID",
                             ASSIGNMENT_ID_PARAM = "@lngAssignmentID";
            SqlParameter goalIDParam,
                         assignmentIDParam;


            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, GOAL_ID_PARAM, goalID, SqlDbType.UniqueIdentifier, out goalIDParam);
                SetParameter(ref cmd, ASSIGNMENT_ID_PARAM, assignmentID, SqlDbType.BigInt, out assignmentIDParam);
                return ExecuteProcedure(ref cmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool UdpateAccommodationdetail(AccomodationDetail d)
        {
            const string PROC_NAME = "uspEditStudentAccommodationDetail",
                    STUDENT_ID_PARAM = "@guidStudentID",
                    ACCOMMODATIONID_PARAM = "@shtAccomodationID",
                    DETAIL_TYPE_PARAM = "@shtDetailTypeID",
                    DETAIL_VALUE_PARAM = "@strDetailValue";

            SqlParameter studentIDParam,
                         accommodationIDParam,
                         detailTypeParam,
                         detailValueParam,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, d.StudentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, ACCOMMODATIONID_PARAM, d.AccomodationID, SqlDbType.SmallInt, out accommodationIDParam);
                SetParameter(ref cmd, DETAIL_TYPE_PARAM, d.DetailType, SqlDbType.SmallInt, out detailTypeParam);
                SetParameter(ref cmd, DETAIL_VALUE_PARAM, d.Value, SqlDbType.VarChar, out detailValueParam, 50);
                AddResultParam(ref cmd, out resultParam);
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)resultParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddAccommodationDetail(Accomodation accomodation, AccomodationDetail detail)
        {
            const string PROC_NAME = "uspAddStudentAccommodationDetail",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ACCOMMODATIONID_PARAM = "@shtAccomodationID",
                         DETAIL_TYPE_PARAM = "@shtDetailTypeID",
                         DETAIL_VALUE_PARAM = "@strDetailValue";

            SqlParameter studentIDParam,
                         accommodationIDParam,
                         detailTypeParam,
                         detailValueParam,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, accomodation.StudentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, ACCOMMODATIONID_PARAM, accomodation.AccomodationID, SqlDbType.SmallInt, out accommodationIDParam);
                SetParameter(ref cmd, DETAIL_TYPE_PARAM, detail.DetailType, SqlDbType.SmallInt, out detailTypeParam);
                SetParameter(ref cmd, DETAIL_VALUE_PARAM, detail.Value, SqlDbType.VarChar, out detailValueParam, 50);
                AddResultParam(ref cmd, out resultParam);
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)resultParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }


        }

        internal static bool RemoveAccomodationDetail(Guid studentID, short accomodationID, DetailType detailType)
        {
            const string PROC_NAME = "uspRemoveStudentAccommodationDetail",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ACCOMMODATIONID_PARAM = "@shtAccomodationID",
                         DETAIL_TYPE_PARAM = "@shtDetailTypeID";
            SqlParameter studentIDParam,
                         accommodationIDParam,
                         detailTypeParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, ACCOMMODATIONID_PARAM, accomodationID, SqlDbType.SmallInt, out accommodationIDParam);
                SetParameter(ref cmd, DETAIL_TYPE_PARAM,detailType, SqlDbType.SmallInt, out detailTypeParam);
                return ExecuteProcedure(ref cmd);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
