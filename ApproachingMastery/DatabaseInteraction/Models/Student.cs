using DatabaseInteraction.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public class Student
    {
        #region constants
        private const string STUDENT_ID_COLUMN = "guidStudentID",
                             FIRST_NAME_COLUMN = "strFirstName",
                             MIDDLE_NAME_COLUMN = "strMiddleName",
                             LAST_NAME_COLUMN = "strLastName",
                             BIRTHDAY_COLUMN = "dteBirthday",
                             IS_ACTIVE_COLUMN = "blnActive",
                             GRADE_LEVEL_COLUMN = "strGradeLevel",
                             IEP_DUE_DATE_COLUMN = "dteIEPDueDate",
                             ETR_DUE_DATE_COLUMN = "dteETRDueDate",
                             IMAGE_ID_COLUMN = "guidImageID";

        #endregion


        #region Public Properties

        public Guid StudentID { get; set; }
        public Guid ImageID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Grade Level")]
        public string GradeLevel { get; set; }

        [Display(Name = "Birthday")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Birthday { get; set; }

        [Display(Name = "IEP Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime IEPDueDate { get; set; }

        [Display(Name = "ETR Due Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ETRDueDate { get; set; }
        //NEED TO ADD MODEL 

        public string FullName
        {
            get
            {
                string fullName = FirstName;
                if (MiddleName != null)
                    fullName += $" {MiddleName}";
                fullName += $" {LastName}";
                return fullName;
            }
        }

        public List<Accomodation> Accomodations { get; set; }
        public List<AccomodationTest> AccomodationTests { get; set; }
        public List<Goal> AcademicGoals { get; set; }
        public List<Goal> BehavioralGoals { get; set; }
        public List<Goal> AcademicObjectives { get; set; }
        public List<Goal> BehavioralObjectives { get; set; }
        public List<Message> Comments { get; set; }

        public bool IsActive { get; set; }
        public List<Test> Tests { get; set; }
        public List<AirTest> AirTests { get; set; }
        public List<ABCEntry> ABCEntries { get; set; }


        #endregion

        #region Constructors
        public Student() { }

        public Student(Guid studentID)
        {
            StudentID = studentID;
        }

        /// <summary>
        /// Creates a new student. Use this when a new student is being added to the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastname"></param>
        /// <param name="birthday"></param>
        public Student(string firstName, string middleName, string lastname, DateTime birthday, bool isActive)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastname;
            Birthday = birthday;
            IsActive = isActive;
        }

        public Student(string firstName, string middleName, string lastname, DateTime birthday, string grade, DateTime iepDueDate, DateTime etrDueDate)
            : this(firstName, middleName, lastname, birthday, true)
        {
            GradeLevel = grade;
            IEPDueDate = iepDueDate;
            ETRDueDate = ETRDueDate;
        }

        /// <summary>
        /// Creates a new student, use this when getting a student from the database
        /// </summary>
        /// <param name="studentID"></param>
        /// <param name="imageID"></param>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastname"></param>
        /// <param name="birthday"></param>
        public Student(Guid studentID, Guid imageID, string firstName, string middleName, string lastname, DateTime birthday, bool isActive)
            : this(firstName, middleName, lastname, birthday, isActive)
        {
            StudentID = studentID;
            ImageID = imageID;
        }

        public Student(DataRow dr)
        {
            // make a new student
            StudentID = new Guid(dr[STUDENT_ID_COLUMN].ToString());
            FirstName = dr[FIRST_NAME_COLUMN].ToString();
            MiddleName = dr[MIDDLE_NAME_COLUMN].ToString();
            LastName = dr[LAST_NAME_COLUMN].ToString();
            Birthday = (DateTime)dr[BIRTHDAY_COLUMN];
            if (dr[GRADE_LEVEL_COLUMN] != null)
                GradeLevel = dr[GRADE_LEVEL_COLUMN].ToString();
            if (dr[IEP_DUE_DATE_COLUMN] != DBNull.Value)
                IEPDueDate = (DateTime)dr[IEP_DUE_DATE_COLUMN];
            if (dr[ETR_DUE_DATE_COLUMN] != DBNull.Value)
                ETRDueDate = (DateTime)dr[ETR_DUE_DATE_COLUMN];
            IsActive = Convert.ToBoolean(dr[IS_ACTIVE_COLUMN]);
            ImageID = new Guid(dr[STUDENT_ID_COLUMN].ToString());
        }


        #endregion

        #region Public Methods

        public bool AddGoal(Goal goal)
        {
            return Database.AddGoal(this, goal);
        }

        public bool RemoveGoal(Goal goal)
        {
            return Database.RemoveGoal(goal);
        }

        public bool AddObjective(Goal goal)
        {
            return Database.AddGoal(this, goal, true);
        }

        public bool RemoveObjective(Goal goal)
        {
            return Database.RemoveGoal(goal, true);
        }

        public bool GetAcademicGoals()
        {
            AcademicGoals = null;
            List<Goal> goals = null;
            if (Database.GetGoals(this, GoalArea.Academic, out goals))
            {
                AcademicGoals = goals;
                return true;
            }

            return false;
        }
        public bool GetBehavioralGoals()
        {
            BehavioralGoals = null;
            List<Goal> goals = null;
            if (Database.GetGoals(this, GoalArea.Behavioral, out goals))
            {
                BehavioralGoals = goals;
                return true;
            }

            return false;
        }

        public bool GetAcademicObjectives()
        {
            AcademicObjectives = null;
            List<Goal> goals = null;
            if (Database.GetGoals(this, GoalArea.Academic, out goals, true))
            {
                AcademicObjectives = goals;
                return true;
            }

            return false;
        }

        public bool GetBehavioralObjectives()
        {
            BehavioralGoals = null;
            List<Goal> goals = null;
            if (Database.GetGoals(this, GoalArea.Behavioral, out goals, true))
            {
                BehavioralObjectives = goals;
                return true;
            }

            return false;
        }

        public bool GetAccomodations()
        {

            List<Accomodation> accomodations;
            if (!Database.GetAccomodations(this, out accomodations)) return false;
            Accomodations = accomodations;
            return true;

        }

        public bool GetAccomodationTests()
        {
            List<AccomodationTest> tests;
            if (!Database.GetAccomodationTests(this, out tests)) return false;
            AccomodationTests = tests;
            return true;
        }

        public bool AddAccomodationTest(Guid StudentID, AccomodationTest at)
        {
            return Database.AddAccomodationTest(StudentID, at);

        }

        public bool GetComments()
        {
            Comments = null;
            List<Message> comments;
            if (!Database.GetComments(this, out comments))
                return false;
            Comments = comments;
            return true;
        }

        public bool AddComment(Message message)
        {
            return Database.AddComment(StudentID, message);
        }

        public bool GetTests(TestType type)
        {
            Tests = null;
            List<Test> tests;
            if (!Database.GetTests(StudentID, type, out tests)) return false;
            Tests = tests;
            return true;
        }

        public bool AddTest(Test test)
        {
            return Database.AddTest(StudentID, test);
        }

        public bool GetAirTests()
        {
            List<AirTest> tests;
            if (!Database.GetAirTests(StudentID, out tests)) return false;
            AirTests = tests;
            return true;
        }

        public bool AddAirTest(AirTest test)
        {
            return Database.AddAirTest(StudentID, test);
        }

        public bool GetABCEntries()
        {
            List<ABCEntry> abcEntries;
            if (!Database.GetABCEntries(StudentID, out abcEntries)) return false;
            ABCEntries = abcEntries;
            return true;
        }

        public bool AddABCEntry(ABCEntry entry)
        {
            entry.StudentID = StudentID;
            return Database.AddABCEntry(entry);
        }

        public bool DeleteABCEntry(Guid abcID)
        {
            return Database.DeleteABCEntry(StudentID, abcID);
        }


        public bool DeactivateStudent()
        {
            return Database.ActivateDeactivateStudent(StudentID, true);
        }

        public bool ActivateStudent()
        {
            return Database.ActivateDeactivateStudent(StudentID, false);
        }

        /// <summary>
        /// Gets the student with the matching student ID
        /// </summary>
        /// <param name="studentID">The student ID</param>
        /// <param name="student">The resulting student</param>
        /// <returns>True if it works, false if not</returns>
        public static bool GetStudent(Guid studentID, out Student student)
        {
            return Database.GetStudent(studentID, out student);
        }


        /// <summary>
        /// Updates The current student's information
        /// </summary>
        /// <returns>True if it worked, false otherwise</returns>
        public bool AddUpdateStudent()
        {
            return Database.AddUpdateStudent(this);
        }

        public bool AddAccomodation(Accomodation accomodation)
        {
            return Database.AddStudentAccommodation(StudentID, accomodation);
        }

        public bool RemoveAccommodation(short accommodationID)
        {
            return Database.RemoveAccomodation(new Accomodation() { AccomodationID = accommodationID }, StudentID);
        }

        public bool RemoveAccommodationTest(Guid accomodationTestID)
        {
            return Database.RemoveAccomodationTest(new AccomodationTest() { TestID = accomodationTestID });
        }

        #endregion


        #region PrivateMethods



        #endregion




    }
}
