using DatabaseInteraction.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace DatabaseInteraction.Models
{
    public enum LoginResult
    {
        Success = 0,
        InvalidUserNameOrPassword = 1,
        AccountLocked = 2,
        AccountDisabled = 3,
        Email_Not_Verified = 4,
        DatabaseConnection = 5,
        UnknownError = 6
    }

    public partial class UserLogin
    {

        private const string LOGIN_ID_COLUMN = "guidLoginID",
                             EMAIL_ADDRESS_COLUMN = "strEmailAddress",
                             PASSWORD_ID_COLUMN = "lngPasswordID",
                             SALT_GUID_COLUMN = "guidSalt",
                             EMAIL_VALIDATED_COLUMN = "blnEmailValidated",
                             USER_ROLE_ID_COLUMN = "shtUserRoleID",
                             BOOL_ACTIVE_COLUMN = "blnActive",
                             EMAIL_VALIDATION_ID_COLUMN = "guidEmailValidationID";

        #region Public Properties

        public bool IsEditing { get; set; } = false;

        [Display(Name="Current Password")]
        [Required]
        public string ExistingPassword { get; set; }
        public Guid UserID { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [NewEmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password message is required")]
        [HasSpecialCharacter(ErrorMessage = "Password must contain at least one special character")]
        [StartsWithCharacter(ErrorMessage = "Password must start with a charater")]
        [HasUpperCase(ErrorMessage = "Password must have 1 upper case character")]
        [HasNumber(ErrorMessage = "Password must contain 1 number")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirm password must match!")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsEmailVerified { get; set; }
        public short RoleID { get; set; }
        public Guid EmailValidationID { get; set; }
        public List<User> OtherTeachers { get; set; }
        public List<Student> Students { get; set; }
        public List<School> Schools { get; set; }

        public List<Student> AvailableStudents { get; set; }

        #endregion

        #region Public Methods

        public LoginResult Login(string ip)
        {
            return Database.Login(this, ip);
        }

        public UserLogin() { }

        public bool GetAvailableStudents()
        {
            List<Student> availableStudents;
            if (!Database.GetAvailableStudents(UserID, out availableStudents)) return false;
            AvailableStudents = availableStudents;
            return true;
        }


        public bool GetSchools()
        {
            List<School> schools;
            if (!Database.GetTeacherSchools(UserID, out schools)) return false;
            Schools = schools;
            return true;
        }

        public bool ConfirmEmail()
        {
            Guid? resultID = null;
            if (!Database.ConfirmEmail(EmailValidationID, out resultID)) return false;
            UserID = (Guid)resultID;
            return GetLoginInfo();
        }

        public bool GetLoginInfo()
        {
            DataRow dr;
            if (!Database.GetLoginInfo(UserID, out dr)) return false;
            return SetUserInfo(dr);
        }


        public PasswordResetResult UpdatePassword(string ipAddress)
        {
            return Database.UpdatePassword(UserID, Password, ipAddress);
        }

        public bool CreateNewAccount(string ipAddress, User userInfo)
        {
            const string PROCEDURE_PARAM = "uspCreateNewLogin";


            bool result = false;
            SqlConnection conn = null;
            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter emailParam = new SqlParameter(Constants.EMAIL_PARAM, SqlDbType.NVarChar, 255)
                {
                    Value = Email
                };
                SqlParameter passwordParam = new SqlParameter(Constants.PASSWORD_PARAM, SqlDbType.NVarChar, 255)
                {
                    Value = Password
                };

                SqlParameter ipParam = new SqlParameter(Constants.IP_ADDRESS_PARAM, SqlDbType.VarChar, 50)
                {
                    Value = ipAddress
                };

                SqlParameter firstNameParam = new SqlParameter(Constants.FIRST_NAME_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = userInfo.FirstName
                };

                SqlParameter middleNameParam = new SqlParameter(Constants.MIDDLE_NAME_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = userInfo.MiddleName
                };

                SqlParameter lastNameParam = new SqlParameter(Constants.LAST_NAME_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = userInfo.LastName
                };

                SqlParameter phoneNumberParam = new SqlParameter(Constants.PHONE_NUMBER_PARAM, SqlDbType.VarChar, 50)
                {
                    Value = userInfo.PhoneNumber
                };

                cmd.Parameters.Add(emailParam);
                cmd.Parameters.Add(passwordParam);
                cmd.Parameters.Add(ipParam);
                cmd.Parameters.Add(firstNameParam);
                cmd.Parameters.Add(middleNameParam);
                cmd.Parameters.Add(lastNameParam);
                cmd.Parameters.Add(phoneNumberParam);
                SqlParameter resultParam;
                Database.AddResultParam(ref cmd, out resultParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    int procResult = Convert.ToInt16(dt.Rows[0]["intResult"]);
                    UserID = Guid.Parse(dt.Rows[0]["guidLoginID"].ToString());
                    EmailValidationID = Guid.Parse(dt.Rows[0]["guidEmailValidationID"].ToString());
                    result = procResult == 1;
                }
                else
                {
                    result = false;
                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return result;
        }

        #endregion

        #region Jake's Additions(Needs Approval)

        public bool AddStudent(Student student, Guid schoolID)
        {

            const string PROCEDURE_PARAM = "uspAddTeacherStudent",
                         TEACHER_ID_PARAM = "@guidUserID",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         SCHOOL_ID_PARAM = "@guidSchoolID",
                         FIRST_NAME_PARAM = "@strFirstName",
                         MIDDLE_NAME_PARAM = "@strMiddleName",
                         LAST_NAME_PARAM = "@strLastName",
                         DOB_PARAM = "@dteBirthday",
                         GRADE_PARAM = "@strGrade",
                         IEP_DUE_DATE_PARAM = "@dteIEPDueDate",
                         ETR_DUE_DATE_PARAM = "@dteETRDueDate";



            SqlParameter userLogin,
                         studentID,
                         schoolIDParam,
                         firstName,
                         middleName,
                         lastName,
                         dob,
                         grade,
                         iep,
                         etr,
                         resultParam;

            try
            {
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM);
                Database.SetParameter(ref cmd, TEACHER_ID_PARAM, UserID, SqlDbType.UniqueIdentifier, out userLogin);
                if(schoolID!=Guid.Empty)
                    Database.SetParameter(ref cmd, SCHOOL_ID_PARAM, schoolID, SqlDbType.UniqueIdentifier, out schoolIDParam);
                else
                    Database.SetParameter(ref cmd, SCHOOL_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out schoolIDParam);
                if (student.StudentID == Guid.Empty)
                {
                    Database.SetParameter(ref cmd, STUDENT_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out studentID);
                }
                else
                {
                    Database.SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                }
                Database.SetParameter(ref cmd, FIRST_NAME_PARAM, student.FirstName, SqlDbType.NVarChar, out firstName, 50);
                Database.SetParameter(ref cmd, MIDDLE_NAME_PARAM, student.MiddleName, SqlDbType.NVarChar, out middleName, 50);
                Database.SetParameter(ref cmd, LAST_NAME_PARAM, student.LastName, SqlDbType.NVarChar, out lastName, 50);
                Database.SetParameter(ref cmd, GRADE_PARAM, student.GradeLevel, SqlDbType.NVarChar, out grade, 50);
                Database.SetParameter(ref cmd, DOB_PARAM, student.Birthday, SqlDbType.Date, out dob);
                Database.SetParameter(ref cmd, IEP_DUE_DATE_PARAM, student.IEPDueDate, SqlDbType.Date, out iep);
                Database.SetParameter(ref cmd, ETR_DUE_DATE_PARAM, student.ETRDueDate, SqlDbType.Date, out etr);
                Database.AddResultParam(ref cmd, out resultParam);
                if (!Database.ExecuteProcedure(ref cmd)) return false;
                if ((int)resultParam.Value != 0) return false;
                return true;

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public bool DisableUser()
        {

            const string PROCEDURE_PARAM = "uspDisableUser",
                   LOGIN_ID_PARAM = "@guidLoginID",
                   BOOL_ACTIVE_PARAM = "@blnActive";

            bool result = false;
            SqlConnection conn = null;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlParameter loginIDParam = new SqlParameter(LOGIN_ID_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = UserID
                };

                SqlParameter boolActiveParam = new SqlParameter(BOOL_ACTIVE_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = false
                };

                cmd.Parameters.Add(loginIDParam);
                cmd.Parameters.Add(boolActiveParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return result;

        }

        public bool EnableUser()
        {

            const string PROCEDURE_PARAM = "uspEnableUser",
                   LOGIN_ID_PARAM = "@guidLoginID",
                   BOOL_ACTIVE_PARAM = "@blnActive";

            bool result = false;
            SqlConnection conn = null;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlParameter loginIDParam = new SqlParameter(LOGIN_ID_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = UserID
                };

                SqlParameter boolActiveParam = new SqlParameter(BOOL_ACTIVE_PARAM, SqlDbType.NVarChar, 50)
                {
                    Value = true
                };

                cmd.Parameters.Add(loginIDParam);
                cmd.Parameters.Add(boolActiveParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return result;

        }

        public bool GetStudents()
        {

            const string PROCEDURE_PARAM = "uspGetStudents",
                             LOGIN_ID_PARAM = "@guidUserID",
                             BOOL_ACTIVE_PARAM = "@blnIsActive";


            bool result = false;

            SqlConnection conn = null;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlParameter loginIDParam = new SqlParameter(LOGIN_ID_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = UserID
                };

                SqlParameter boolActiveParam = new SqlParameter(BOOL_ACTIVE_PARAM, SqlDbType.Bit)
                {
                    Value = 1
                };

                cmd.Parameters.Add(loginIDParam);
                cmd.Parameters.Add(boolActiveParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                Students = new List<Student>();
                foreach (DataRow dr in dt.Rows)
                {
                    Students.Add(new Student(dr));
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return result;

        }

        public bool GetTeachers()
        {

            const string PROCEDURE_PARAM = "uspGetTeachers",
                 LOGIN_ID_PARAM = "@guidLoginID";


            bool result = false;

            SqlConnection conn = null;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlParameter loginIDParam = new SqlParameter(LOGIN_ID_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = UserID
                };

                cmd.Parameters.Add(loginIDParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                OtherTeachers = new List<User>();
                foreach (DataRow dr in dt.Rows)
                {
                    OtherTeachers.Add(new User(dr));
                }

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return result;

        }

        public static bool GetUserLogin(Guid LoginID, out UserLogin userLogin)
        {


            userLogin = null;
            bool result = false;
            SqlConnection sqlConn = null;
            try
            {
                // try to connect to the database
                result = Database.ConnectToDatabase(ref sqlConn);
                if (result) // did it work?
                {
                    // yes, create the command and fill the table

                    SqlCommand cmd = new SqlCommand($"EXEC uspGetUserLogin '{LoginID}'", sqlConn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    DataRow dr = dt.Rows[0];

                    // make a new student
                    userLogin = new UserLogin(dr);

                    result = true;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                // connect if the connection is open
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed && sqlConn.State != ConnectionState.Broken)
                {
                    Database.CloseDatabaseConnection(ref sqlConn);
                }
            }

            // return the result
            return result;

        }

        public bool RemoveStudent(Guid studentID)
        {
            return Database.RemoveStudentFromTeacher(UserID, studentID);
        }


        public UserLogin(Guid LoginID, string emailAddress, bool isEmailValidated, bool isActive, short userRoleID)
        {
            UserID = LoginID;
            Email = emailAddress;
            IsEmailVerified = isEmailValidated;
            IsActive = isActive;
            RoleID = userRoleID;

        }

        public UserLogin(Guid userID)
        {
            UserID = userID;
        }

        public UserLogin(DataRow dr)
        {
            SetUserInfo(dr);
        }


        public int GetGoalCounts(int Count, bool blnIsCompleted)
        {

            const string PROCEDURE_PARAM = "uspGetGoalCount",
                         COUNT_PARAM = "@Num_Goals",
                         BOOLEAN_COMPLETED_PARAM = "@blnIsCompleted";

            SqlConnection conn = null;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) throw new Exception("Could not connect to database");
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };


                SqlParameter CountParam = new SqlParameter(COUNT_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = Count
                };

                SqlParameter CompletedParam = new SqlParameter(BOOLEAN_COMPLETED_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = blnIsCompleted
                };


                cmd.Parameters.Add(CountParam);
                cmd.Parameters.Add(CompletedParam);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                Count = Convert.ToInt32(dt.Rows.Count);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    Database.CloseDatabaseConnection(ref conn);
                }
            }
            return Count;

        }



        #endregion


        #region Private Methods

        private bool SetUserInfo(DataRow dr)
        {
            try
            {
                UserID = (Guid)dr[LOGIN_ID_COLUMN];
                Email = dr[EMAIL_ADDRESS_COLUMN].ToString();
                IsEmailVerified = (bool)dr[EMAIL_VALIDATED_COLUMN];
                RoleID = Convert.ToInt16(dr[USER_ROLE_ID_COLUMN]);
                IsActive = (bool)dr[BOOL_ACTIVE_COLUMN];
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        #endregion

    }
}
