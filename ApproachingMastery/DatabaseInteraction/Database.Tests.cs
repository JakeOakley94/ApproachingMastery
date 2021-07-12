using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public partial class Database
    {

        public static bool GetAirTest(Guid airTestID, out AirTest t)
        {
            const string PROC_NAME = "uspGetAirTest",
                         AIR_TEST_ID = "@guidAirTestID";

            SqlParameter airTestIDParam;
            DataTable dt;
            try
            {
                t = null;
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, AIR_TEST_ID, airTestID, System.Data.SqlDbType.UniqueIdentifier, out airTestIDParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (dt.Rows.Count == 0) return false;
                t = new AirTest(dt.Rows[0]);
                return true;
            }
            catch(Exception ex)
            {
                t = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool GetAccomodationTest(Guid AccomodationTestID, out AccomodationTest t)
        {
            const string PROC_NAME = "uspGetAccomodationTest",
                         ACCOMODATION_TEST_ID = "@guidAccomodationTestID";

            SqlParameter accomodationTestIDParam;
            DataTable dt;
            try
            {
                t = null;
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, ACCOMODATION_TEST_ID, AccomodationTestID, System.Data.SqlDbType.UniqueIdentifier, out accomodationTestIDParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (dt.Rows.Count == 0) return false;
                t = new AccomodationTest(dt.Rows[0]);
                return true;
            }
            catch (Exception ex)
            {
                t = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
