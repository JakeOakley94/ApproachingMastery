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
        public static bool GetUser(Guid guid, out User user)
        {
            const string PROC_NAME = "uspGetUser",
                         USER_ID = "guidLoginID";

            user = null;
            bool result = false;
            SqlParameter userID;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, USER_ID, guid, SqlDbType.UniqueIdentifier, out userID);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (dt.Rows.Count == 0) return false;
                user = new User(dt.Rows[0]);
                return true;
            }
            catch (Exception ex)
            {
                user = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool AddUpdateUserInfo(User u)
        {
            const string PROC_NAME = "uspAddUpdateUserInfo",
                         LOGIN_ID_PARAM = "@guidLoginID",
                         FIRST_NAME_PARAM = "@strFirstName",
                         MIDDLE_NAME_PARAM = "@strMiddleName",
                         LAST_NAME_PARAM = "@strLastName",
                         PHONE_NUMBER_PARAM = "@strPhoneNumber",
                         IMAGE_ID_PARAM = "@guidImageID",
                         CLASS_PARAM = "@strClass";

            SqlParameter loginID,
                         firstName,
                         middleName,
                         lastName,
                         phoneNumber,
                         imageID,
                         classParam,
                         result;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, LOGIN_ID_PARAM, u.UserLoginID, SqlDbType.UniqueIdentifier, out loginID);
                SetParameter(ref cmd, FIRST_NAME_PARAM, u.FirstName, SqlDbType.NVarChar, out firstName, 50);
                if (u.MiddleName == null) u.MiddleName = "";
                SetParameter(ref cmd, MIDDLE_NAME_PARAM, u.MiddleName, SqlDbType.NVarChar, out middleName, 50);
                SetParameter(ref cmd, LAST_NAME_PARAM, u.LastName, SqlDbType.NVarChar, out lastName, 50);
                SetParameter(ref cmd, PHONE_NUMBER_PARAM, u.PhoneNumber, SqlDbType.VarChar, out phoneNumber, 50);
                if (u.ImageID == Guid.Empty)
                    SetParameter(ref cmd, IMAGE_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out imageID, 50);
                else
                    SetParameter(ref cmd, IMAGE_ID_PARAM, u.ImageID, SqlDbType.UniqueIdentifier, out imageID, 50);
                if (u.Class == null)
                    SetParameter(ref cmd, CLASS_PARAM, DBNull.Value, SqlDbType.NVarChar, out classParam, 50);
                else
                    SetParameter(ref cmd, CLASS_PARAM, u.Class, SqlDbType.NVarChar, out classParam, 50);

                AddResultParam(ref cmd, out result);
                if (!ExecuteProcedure(ref cmd)) return false;
                if ((int)result.Value != 0) return false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

    }
}
