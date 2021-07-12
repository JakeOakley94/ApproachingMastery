using DatabaseInteraction.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;


namespace DatabaseInteraction.Models
{
    public class User
    {

        private const string USER_ID_COLUMN = "guidLoginID",
                     FIRST_NAME_COLUMN = "strFirstName",
                     MIDDLE_NAME_COLUMN = "strMiddleName",
                     LAST_NAME_COLUMN = "strLastName",
                     PHONE_NUMBER_COLUMN = "strPhoneNumber",
                     CLASS_COLUMN = "strClass",
                     IMAGE_ID_COLUMN = "guidImageID";

        #region Properties
        public Guid UserLoginID { get; set; }
        public Guid ImageID { get; set; }

        [Display(Name = "First Name")]
        [IsNotOnlyWhiteSpace(ErrorMessage = "First name is required and cannot be empty")]
        [Required(ErrorMessage = "First Name is Required")]
        [OnlyLetters(ErrorMessage = "First name must only have letters")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [OnlyLetters(ErrorMessage = "Middle name must only have letters")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        [IsNotOnlyWhiteSpace(ErrorMessage = "Last name is required and cannot be empty")]
        [Required(ErrorMessage = "Last Name is Required")]
        [OnlyLetters(ErrorMessage = "Last name must only have letters")]
        public string LastName { get; set; }


        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public string Class { get; set; }

        public Picture Image { get; set; }

        public bool IsEditing { get; set; } = false;

        #endregion


        #region Constructors  
        public User() { }
        /// <summary>
        /// Creates a new User. Use this when a new student is being added to the database
        /// </summary>
        public User(string firstName, string middleName, string lastname, string phoneNumber, string strClass, Picture img)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastname;
            PhoneNumber = phoneNumber;
            Class = strClass;
            Image = img;
        }


        /// <summary>
        /// Creates a new user, use this when getting a student from the database
        /// </summary>
        public User(Guid userLoginID, Guid imageID, string firstName, string middleName, string lastname, string phoneNumber, string strClass, Picture img)
            : this(firstName, middleName, lastname, phoneNumber, strClass, img)
        {
            UserLoginID = userLoginID;
            ImageID = imageID;
        }

        public User(DataRow dr)
        {
            UserLoginID = (Guid)dr[USER_ID_COLUMN];
            FirstName = dr[FIRST_NAME_COLUMN].ToString();
            MiddleName = dr[MIDDLE_NAME_COLUMN].ToString();
            LastName = dr[LAST_NAME_COLUMN].ToString();
            PhoneNumber = dr[PHONE_NUMBER_COLUMN].ToString();
            Class = dr[CLASS_COLUMN].ToString();
            if (dr[IMAGE_ID_COLUMN] != DBNull.Value)
                ImageID = (Guid)dr[IMAGE_ID_COLUMN];
        }

        #endregion

        #region Methods

        public static bool GetUser(Guid guid, out User currentUser)
        {
            currentUser = null;
            User u;
            if (!Database.GetUser(guid, out u)) return false;
            currentUser = u;
            return true;
        }


        public bool UpdateImage()
        {
            bool result = false;
            SqlConnection sqlConn = null;
            try
            {
                // try to connect to the database
                result = Database.ConnectToDatabase(ref sqlConn);
                if (result) // did it work?
                {
                    // yes, create the command and execute it
                    string sql = $"EXEC uspUpdateImage '{UserLoginID}', '{Image}' ";
                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                    int cmdResult = cmd.ExecuteNonQuery();

                    // what was the result?
                    result = cmdResult == 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed && sqlConn.State != ConnectionState.Broken)
                {
                    Database.CloseDatabaseConnection(ref sqlConn);
                }
            }
            return result;

        }
        public bool UpdateUserInformation()
        {
            return Database.AddUpdateUserInfo(this);
        }

        #endregion

    }
}
