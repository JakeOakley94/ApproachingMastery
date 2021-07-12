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

    public enum PasswordResetResult
    {
        Success,
        PreviousPassword,
        Error
    }

    public static partial class Database
    {
        

        public static LoginResult Login(UserLogin login, string ip)
        {
            const string PROC_NAME = "uspLoginUser";
            SqlParameter email,
                         password,
                         ipAddress,
                         loginID,
                         resultParam;



            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, UserLogin.Constants.EMAIL_PARAM, login.Email, SqlDbType.NVarChar,
                    out email, UserLogin.Constants.EMAIL_FIELD_SIZE);

                SetParameter(ref cmd, UserLogin.Constants.PASSWORD_PARAM, login.Password, SqlDbType.NVarChar,
                    out password, UserLogin.Constants.PASSWORD_FIELD_SIZE);

                SetParameter(ref cmd, UserLogin.Constants.IP_ADDRESS_PARAM, ip, SqlDbType.NVarChar, out ipAddress,
                    UserLogin.Constants.IP_ADDRESS_FIELD_SIZE);

                SetParameter(ref cmd, UserLogin.Constants.LOGIN_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier,
                    out loginID, direction: ParameterDirection.Output);

                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam,
                    direction: ParameterDirection.ReturnValue);

                if (!ExecuteProcedure(ref cmd)) return LoginResult.UnknownError;
                LoginResult result = (LoginResult)Convert.ToInt32(resultParam.Value);
                if (result != LoginResult.Success) return result;
                login.UserID = (Guid)loginID.Value;
                return LoginResult.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return LoginResult.UnknownError;
            }
        }


        
        public static PasswordResetResult UpdatePassword(Guid loginID, string password, string ipAddress)
        {
            const string PROC_NAME = "uspUpdatePassword",
                         LOGIN_ID_PARAM = "@guidLoginID",
                         PASSWORD_PARAM = "@strPassword",
                         IP_ADDRESS_PARAM = "@strIPAddress";

            SqlParameter loginIDParam,
                         passwordParam,
                         ipAddressPaaram,
                         result;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, LOGIN_ID_PARAM, loginID, SqlDbType.UniqueIdentifier, out loginIDParam);
                SetParameter(ref cmd, PASSWORD_PARAM, password, SqlDbType.NVarChar, out passwordParam, 50);
                SetParameter(ref cmd, IP_ADDRESS_PARAM, ipAddress, SqlDbType.NVarChar, out ipAddressPaaram,50);
                AddResultParam(ref cmd, out result);
                if (!ExecuteProcedure(ref cmd)) return PasswordResetResult.Error;
                return (PasswordResetResult)result.Value;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return PasswordResetResult.Error;
            }

        }
        public static bool GetAvailableStudents(Guid loginID, out List<Student> students)
        {
            students = null;
            DataTable dt;
            const string PROC_NAME = "uspGetAvailableStudents";
            SqlParameter userLoginParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, "@guidLoginID", loginID, SqlDbType.UniqueIdentifier, out userLoginParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                students = new List<Student>();
                foreach(DataRow dr in dt.Rows)
                {
                    students.Add(new Student(dr));
                }
                return true;
            }
            catch(Exception ex)
            {
                students = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public static bool GetTeacherSchools(Guid loginID, out List<School> schools)
        {
            schools = null;
            DataTable dt;

            const string PROC_NAME = "uspGetTeacherSchools";
            SqlParameter loginIDParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, "@guidLoginID", loginID, SqlDbType.UniqueIdentifier, out loginIDParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                schools = new List<School>();
                foreach(DataRow dr in dt.Rows)
                {
                    schools.Add(new School(dr));
                }
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                schools = null;
                return false;
            }
        }

        public static bool ConfirmEmail(Guid emailConfirmationID, out Guid? userID)
        {
            const string PROC_NAME = "uspValidateEmail";

            SqlParameter emailConfirmation,
                         userIDParam,
                         resultParam;

            userID = null;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, UserLogin.Constants.EMAIL_CONFIRMATION_PARAM, emailConfirmationID,
                    SqlDbType.UniqueIdentifier, out emailConfirmation);
                SetParameter(ref cmd, UserLogin.Constants.LOGIN_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier,
                    out userIDParam, direction:ParameterDirection.Output);
                AddResultParam(ref cmd, out resultParam);
                if (!ExecuteProcedure(ref cmd)) return false;
                bool result = (int)resultParam.Value == 0 ? true : false;
                if (!result) return false;
                userID = (Guid?)(Guid)userIDParam.Value;
                return true;
            }
            catch (Exception ex)
            {
                userID = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool GetLoginInfo(Guid loginID, out DataRow dr)
        {
            const string PROC_NAME = "uspGetLoginInfo";

            SqlParameter resultParam,
                         loginIDParam;

            dr = null;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                AddResultParam(ref cmd, out resultParam);
                SetParameter(ref cmd, UserLogin.Constants.LOGIN_ID_PARAM, loginID, SqlDbType.UniqueIdentifier,
                    out loginIDParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (dt.Rows.Count == 0) return false;
                dr = dt.Rows[0];
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public static int GetTeachers(Guid userID, out List<User> users)
        {

            const string PROC_NAME = "uspGetTeachers",
                         LOGIN_ID_PARAM = "@guidLoginID";

            users = null;
            SqlParameter loginID,
                         resultParam;

            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, LOGIN_ID_PARAM, userID, SqlDbType.UniqueIdentifier, out loginID);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return -1;
                if ((int)resultParam.Value != 0) return (int)resultParam.Value;

                users = new List<User>();
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(new User(dr));
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -2;
            }
        }


        public static bool RemoveStudentFromTeacher(Guid loginID, Guid studentID)
        {
            const string PROC_NAME = "uspRemoveStudentFromTeacher",
                         LOGIN_ID_PARAM = "guidLoginID",
                         STUDENT_ID_PARAM = "guidStudentID";

            SqlParameter loginIDParam,
                         studentIDParam,
                         result;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, LOGIN_ID_PARAM, loginID, SqlDbType.UniqueIdentifier, out loginIDParam);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
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
