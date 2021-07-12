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
    // this file contains all of the database interaction for a student, its in here instead of student because it will be quite large


    public partial class Database
    {

        #region Student 


        internal static bool GetStudent(Guid studentID, out Student student)
        {
            const string PROC_NAME = "uspGetStudent",
                         STUDENT_ID_PARAM = "guidStudentID";

            SqlParameter studentIDParam,
                         resultParam;
            DataTable dt = null;
            student = null;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (((int)resultParam.Value) != 0) return false;
                student = new Student(dt.Rows[0]);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddUpdateStudent(Student s)
        {
            const string PROC_NAME = "uspAddUpdateStudent",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         FIRST_NAME_PARAM = "@strFirstName",
                         MIDDLE_NAME_PARAM = "@strMiddleName",
                         LAST_NAME_PARAM = "@strLastName",
                         BIRTHDAY_PARAM = "@dteBirthday",
                         IS_ACTIVE_APRAM = "@blnActive",
                         IEP_DUEDATE_PARAM = "@dteIEPDueDate",
                         ETR_DUEDATE_PARAM = "@dteETRDueDate",
                         STR_GRADELEVEL_PARAM = "@strGradeLevel";


            SqlParameter studentID,
                         firstName,
                         middleName,
                         lastName,
                         birthday,
                         isActive,
                         iepDueDate,
                         etrDueDate,
                         strGradeLevel,
                         resultParam;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                object studentIDValue = s.StudentID;
                if (s.StudentID == null)
                    studentIDValue = DBNull.Value;
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentIDValue, SqlDbType.UniqueIdentifier, out studentID, direction: ParameterDirection.InputOutput);
                SetParameter(ref cmd, FIRST_NAME_PARAM, s.FirstName, SqlDbType.NVarChar, out firstName, 50);

                if (s.MiddleName == null)
                    s.MiddleName = "";

                SetParameter(ref cmd, MIDDLE_NAME_PARAM, s.MiddleName, SqlDbType.NVarChar, out middleName, 50);
                SetParameter(ref cmd, LAST_NAME_PARAM, s.LastName, SqlDbType.NVarChar, out lastName, 50);
                SetParameter(ref cmd, BIRTHDAY_PARAM, s.Birthday, SqlDbType.Date, out birthday);
                SetParameter(ref cmd, IS_ACTIVE_APRAM, s.IsActive, SqlDbType.Bit, out isActive);
                SetParameter(ref cmd, IEP_DUEDATE_PARAM, s.IEPDueDate, SqlDbType.Date, out iepDueDate);
                SetParameter(ref cmd, ETR_DUEDATE_PARAM, s.ETRDueDate, SqlDbType.Date, out etrDueDate);
                SetParameter(ref cmd, STR_GRADELEVEL_PARAM, s.GradeLevel, SqlDbType.NVarChar, out strGradeLevel, 50);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)resultParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        internal static bool ActivateDeactivateStudent(Guid studentID, bool deactivate = false)
        {
            const string PROC_NAME = "uspActivateDeactivateStudent",
                         STUDENT_ID_NAME = "@guidStudentID",
                         STUDENT_STATUS = "@blnStatus";

            SqlParameter studentIDParam,
                         studentStatus,
                         resultParam;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_NAME, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, STUDENT_STATUS, deactivate, SqlDbType.Bit, out studentStatus);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((short)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        #endregion


        #region Accomodations


        //internal static bool AddAccomodation(Student student, Accomodation accomodation, bool objective = false)
        //{
        //    const string ACCOMODATION_PRC_NAME = "uspAddStudentAccomodation",
        //                 STUDENT_ID_PARAM = "@guidStudentID",
        //                 ACCOMODATION_ID_PARAM = "@shtAccomodationID",
        //                 ACCOMODATION_PARAM = "@strAccomodation",
        //                 UPPERACCOMODATION_PARAM = "@strUppderAccomodation",
        //                 GOAL_TYPE_PARAM = "@shtGoalType",
        //                 GOAL_AREA_PARAM = "@shtGoalArea",
        //                 DESCRIPTION_PARAM = "@strDescription",
        //                 ASSIGNED_BY_PARAM = "@guidAssignedBy";

        //    SqlParameter studentID,
        //                 dateAssigned,
        //                 dueDate,
        //                 goalType,
        //                 goalArea,
        //                 dateCompleted,
        //                 description,
        //                 assignedByParam,
        //                 returnParam;
        //    try
        //    {
        //        // create the command and add the parameters
        //        SqlCommand cmd = new SqlCommand(GOAL_PRC_NAME);
        //        if (objective)
        //            cmd = new SqlCommand(OBJECTIVE_PRC_NAME);

        //        SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
        //        SetParameter(ref cmd, DATE_ASSIGNED_PARAM, goal.DateAssigned, SqlDbType.Date, out dateAssigned);
        //        SetParameter(ref cmd, DUE_DATE_PARAM, goal.DueDate, SqlDbType.Date, out dueDate);
        //        SetParameter(ref cmd, GOAL_TYPE_PARAM, goal.GoalType, SqlDbType.SmallInt, out goalType);
        //        SetParameter(ref cmd, GOAL_AREA_PARAM, goal.GoalArea, SqlDbType.SmallInt, out goalArea);
        //        SetParameter(ref cmd, DATE_COMPLETED_PARAM, goal.DateCompleted, SqlDbType.DateTime, out dateCompleted);
        //        SetParameter(ref cmd, DESCRIPTION_PARAM, goal.Description, SqlDbType.NVarChar, out description, 500);
        //        SetParameter(ref cmd, ASSIGNED_BY_PARAM, goal.AssignedBy, SqlDbType.UniqueIdentifier, out assignedByParam);
        //        SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out returnParam, direction: ParameterDirection.ReturnValue);

        //        // execute the command
        //        if (!ExecuteProcedure(ref cmd)) return false;
        //        return (int)returnParam.Value == 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return false;
        //    }
        //}   

        public static bool AddStudentAccommodation(Guid studentID, Accomodation ac)
        {
            const string PROCEDURE_NAME = "uspAddStudentAccommodation",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ADDED_BY_PARAM = "@guidAddedBy",
                         ACCOMMODATION_ID_PARAM = "@shtAccommodationID";

            SqlParameter studentIDParam,
                         addedByParam,
                         accomodationIDParam,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROCEDURE_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, ADDED_BY_PARAM, ac.AddedBy, SqlDbType.UniqueIdentifier, out addedByParam);
                short accommodationID = (short)ac.AccomodationType;
                SetParameter(ref cmd, ACCOMMODATION_ID_PARAM, accommodationID, SqlDbType.SmallInt, out accomodationIDParam);
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

        internal static bool RemoveAccomodation(Accomodation ac, Guid StudentID)
        {
            const string REMOVE_ACCOMODATION_SP_NAME = "uspRemoveStudentAccommodation",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ACCOMODATION_ID_PARAM = "@shtAccomodationID";

            SqlParameter studentID,
                         accomodationID,
                         returnParam;
            try
            {
                // create the command and add the parameters
                SqlCommand cmd = new SqlCommand(REMOVE_ACCOMODATION_SP_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, StudentID, SqlDbType.UniqueIdentifier, out studentID);
                SetParameter(ref cmd, ACCOMODATION_ID_PARAM, ac.AccomodationID, SqlDbType.SmallInt, out accomodationID);
                AddResultParam(ref cmd, out returnParam);

                // execute the command
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)returnParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        internal static bool GetAccomodations(Student student, out List<Accomodation> Accomodations)
        {
            const string ACCOMODATION_SP_NAME = "uspGetStudentAccomodations",
                         STUDENT_ID_PARAM = "@guidStudentID";


            SqlParameter studentID,
                         resultParam;

            DataTable dt = null;

            try
            {
                Accomodations = new List<Accomodation>();

                SqlCommand cmd = new SqlCommand(ACCOMODATION_SP_NAME);

                SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                AddResultParam(ref cmd, out resultParam);

                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if ((int)resultParam.Value != 0) return false;


                foreach (DataRow dr in dt.Rows)
                {

                    Accomodations.Add(new Accomodation(dr));

                }

                return true;
            }
            catch (Exception ex)
            {
                Accomodations = null;
                Console.WriteLine(ex.ToString());
                return false;
            }

        }


        internal static bool AddAccomodationTest(Guid studentID, AccomodationTest at)
        {
            const string PROC_NAME = "uspAddUpdateStudentAccomodationTest",
                     STUDENT_ID_PARAM = "@guidStudentID",
                     ACCOMODATION_TEST_ID_PARAM = "@guidAccomodationTestID",
                     DATE_PARAM = "@dteDate",
                       TEST_GIVEN_PARAM = "@strTestGiven",
                     PERCENTILE_PARAM = "@dblPercentile",
                     ACCOMODATION_ID_PARAM = "@shtAccomodationID",
                     TEST_TYPE_PARAM = "@shtTestTypeID";

            SqlParameter studentIDParam,
                         accomodationID,
                         date,
                         testgiven,
                         percentile,
                         testtype,
                         accomdationid,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                if (at.TestID == Guid.Empty)
                    SetParameter(ref cmd, ACCOMODATION_TEST_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out accomodationID);
                else
                    SetParameter(ref cmd, ACCOMODATION_TEST_ID_PARAM, at.TestID, SqlDbType.UniqueIdentifier, out accomodationID);
                SetParameter(ref cmd, DATE_PARAM, at.Date, SqlDbType.Date, out date);
                SetParameter(ref cmd, TEST_GIVEN_PARAM, at.TestGiven, SqlDbType.VarChar, out testgiven);
                SetParameter(ref cmd, PERCENTILE_PARAM, at.Percentile, SqlDbType.Float, out percentile);
                SetParameter(ref cmd, ACCOMODATION_ID_PARAM, at.AccomodationID, SqlDbType.SmallInt, out accomdationid);
                SetParameter(ref cmd, TEST_TYPE_PARAM, at.Type, SqlDbType.SmallInt, out testtype);
                AddResultParam(ref cmd, out resultParam);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool RemoveAccomodationTest(AccomodationTest at)
        {
            const string REMOVE_TEST_SP_NAME = "uspRemoveAccomodationTest",
                         ACCOMODATION_TEST_ID_PARAM = "@guidTestID";

            SqlParameter testID,
                         returnParam;
            try
            {
                // create the command and add the parameters
                SqlCommand cmd = new SqlCommand(REMOVE_TEST_SP_NAME);

                SetParameter(ref cmd, ACCOMODATION_TEST_ID_PARAM, at.TestID, SqlDbType.UniqueIdentifier, out testID);
                AddResultParam(ref cmd, out returnParam);

                // execute the command
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)returnParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool GetAccomodationTests(Student student, out List<AccomodationTest> tests)
        {
            const string ACCOMODATION_SP_NAME = "uspGetStudentAccomodationTests",
                         STUDENT_ID_PARAM = "@guidStudentID";

            SqlParameter studentID;

            DataTable dt = null;

            try
            {
                tests = new List<AccomodationTest>();

                SqlCommand cmd = new SqlCommand(ACCOMODATION_SP_NAME);

                SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;

                foreach (DataRow dr in dt.Rows)
                {

                    tests.Add(new AccomodationTest(dr));

                }

                return true;
            }
            catch (Exception ex)
            {
                tests = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        #endregion



        #region Goals

        /// <summary>
        /// Adds a new goal to the specified student
        /// </summary>
        /// <param name="student"></param>
        /// <param name="goal"></param>
        /// <returns></returns>
        internal static bool AddGoal(Student student, Goal goal, bool objective = false)
        {
            const string GOAL_PRC_NAME = "uspAddStudentGoal",
                         OBJECTIVE_PRC_NAME = "uspAddStudentObjective",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         DATE_ASSIGNED_PARAM = "@dteDateAssigned",
                         DATE_COMPLETED_PARAM = "@dteDateCompleted",
                         DUE_DATE_PARAM = "@dteDueDate",
                         GOAL_TYPE_PARAM = "@shtGoalType",
                         GOAL_AREA_PARAM = "@shtGoalArea",
                         DESCRIPTION_PARAM = "@strDescription",
                         ASSIGNED_BY_PARAM = "@guidAssignedBy",
                         GOAL_ID_PARAM = "@guidGoalID";

            SqlParameter studentID,
                         dateAssigned,
                         dueDate,
                         goalType,
                         goalArea,
                         dateCompleted,
                         description,
                         assignedByParam,
                         returnParam,
                         goalIDParam;
            try
            {
                // create the command and add the parameters
                SqlCommand cmd = new SqlCommand(GOAL_PRC_NAME);
                if (objective)
                    cmd = new SqlCommand(OBJECTIVE_PRC_NAME);

                SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                SetParameter(ref cmd, DATE_ASSIGNED_PARAM, goal.DateAssigned, SqlDbType.Date, out dateAssigned);
                SetParameter(ref cmd, DUE_DATE_PARAM, goal.DueDate, SqlDbType.Date, out dueDate);
                SetParameter(ref cmd, GOAL_TYPE_PARAM, goal.GoalType, SqlDbType.SmallInt, out goalType);
                SetParameter(ref cmd, GOAL_AREA_PARAM, goal.GoalArea, SqlDbType.SmallInt, out goalArea);
                SetParameter(ref cmd, DATE_COMPLETED_PARAM, goal.DateCompleted, SqlDbType.DateTime, out dateCompleted);
                SetParameter(ref cmd, DESCRIPTION_PARAM, goal.Description, SqlDbType.NVarChar, out description, 500);
                SetParameter(ref cmd, ASSIGNED_BY_PARAM, goal.AssignedBy, SqlDbType.UniqueIdentifier, out assignedByParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out returnParam, direction: ParameterDirection.ReturnValue);
                if (goal.GoalID != Guid.Empty && goal.GoalID != null)
                    SetParameter(ref cmd, GOAL_ID_PARAM, goal.GoalID, SqlDbType.UniqueIdentifier, out goalIDParam, direction: ParameterDirection.InputOutput);
                else
                    SetParameter(ref cmd, GOAL_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out goalIDParam, direction: ParameterDirection.InputOutput);

                // execute the command
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)returnParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Removes the specified goal from the database
        /// </summary>
        /// <param name="goal"></param>
        /// <returns></returns>
        internal static bool RemoveGoal(Goal goal, bool objective = false)
        {
            const string GOAL_PROC_NAME = "uspRemoveGoal",
                         OBJECTIVE_PROC_NAME = "uspRemoveObjective",
                         GOAL_ID_PARAM = "@guidGoalID";
            SqlParameter goalID,
                         returnParam;
            try
            {
                // create the command and add the parameters
                SqlCommand cmd = new SqlCommand(GOAL_PROC_NAME);
                if (objective) cmd = new SqlCommand(OBJECTIVE_PROC_NAME);

                SetParameter(ref cmd, GOAL_ID_PARAM, goal.GoalID, SqlDbType.UniqueIdentifier, out goalID);
                AddResultParam(ref cmd, out returnParam);

                // execute the command
                if (!ExecuteProcedure(ref cmd)) return false;
                return (int)returnParam.Value == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets student goals or objectives for the specified student
        /// </summary>
        /// <param name="student"></param>
        /// <param name="area"></param>
        /// <param name="goals"></param>
        /// <param name="getObjectives"></param>
        /// <returns></returns>
        internal static bool GetGoals(Student student, GoalArea area, out List<Goal> goals, bool getObjectives = false)
        {
            const string GOALS_PRC_NAME = "uspGetStudentGoals",
                         OBJECTIVES_PRC_NAME = "uspGetStudentObjectives",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         GOAL_AREA_PARAM = "@shtGoalArea";


            SqlParameter studentID,
                         goalArea,
                         resultParam;
            DataTable dt = null;

            try
            {
                goals = new List<Goal>();

                SqlCommand cmd = new SqlCommand(GOALS_PRC_NAME);
                if (getObjectives)
                    cmd = new SqlCommand(OBJECTIVES_PRC_NAME);

                SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                SetParameter(ref cmd, GOAL_AREA_PARAM, (short)area, SqlDbType.SmallInt, out goalArea);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if ((int)resultParam.Value != 0) return false;
                foreach (DataRow dr in dt.Rows)
                {
                    goals.Add(new Goal(dr));
                }
                return true;
            }
            catch (Exception ex)
            {
                goals = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        #endregion


        #region Comments

        internal static bool GetComments(Student student, out List<Message> comments)
        {
            const string PROC_NAME = "uspGetStudentComments",
                         STUDENT_ID_PARAM = "@guidStudentID";

            SqlParameter studentParam,
                         resultParam;

            DataTable dt;
            try
            {
                comments = new List<Message>();
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, student.StudentID, SqlDbType.UniqueIdentifier, out studentParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if ((int)resultParam.Value != 0) return false;
                foreach (DataRow dr in dt.Rows)
                {
                    Message msg = new Message(dr);
                    msg.GetReplies();
                    comments.Add(msg);
                }
                return true;
            }
            catch (Exception ex)
            {
                comments = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool GetMessageReplies(Guid? messageID, out List<Message> replies)
        {
            const string PROC_NAME = "uspGetMessageReplies",
                         PARENT_ID_PARAM = "@guidParentID";

            SqlParameter parentParam,
                         resultParam;

            DataTable dt;
            try
            {
                replies = new List<Message>();
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, PARENT_ID_PARAM, messageID, SqlDbType.UniqueIdentifier, out parentParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.SmallInt, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if ((short)resultParam.Value != 0) return false;
                foreach (DataRow dr in dt.Rows)
                {
                    Message msg = new Message(dr);

                    msg.GetReplies();
                    replies.Add(msg);
                }
                return true;
            }
            catch (Exception ex)
            {
                replies = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddComment(Guid studentID, Message Message)
        {
            const string PROC_NAME = "uspAddUpdateComment",
                         STUDENT_ID_PARAM = "@guidStudentID";

            SqlParameter studentIDParameter,
                         messageText,
                         senderID,
                         resultParam,
                         messageID;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParameter);
                SetParameter(ref cmd, Message.Constants.MESSAGE_PARAM, Message.MessageText, SqlDbType.NVarChar, out messageText, -1);
                SetParameter(ref cmd, Message.Constants.SENDER_PARAM, Message.SenderID, SqlDbType.UniqueIdentifier, out senderID);
                SetParameter(ref cmd, Message.Constants.MESSAGE_ID_PARAM, Message.MessageID, SqlDbType.UniqueIdentifier, out messageID,
                    direction: ParameterDirection.InputOutput);
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

        #endregion


        #region Tests

        internal static bool GetTests(Guid studentID, TestType type, out List<Test> tests)
        {
            const string PROC_NAME = "uspGetStudentTests",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         TEST_TYPE_PARAM = "@shtTestNameID";


            SqlParameter studentIDParam,
                         testTypeParam,
                         resultParam;

            DataTable dt;

            try
            {
                tests = new List<Test>();
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, TEST_TYPE_PARAM, (short)type, SqlDbType.SmallInt, out testTypeParam);
                AddResultParam(ref cmd, out resultParam);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if ((int)resultParam.Value != 0) return false;

                foreach (DataRow dr in dt.Rows)
                {
                    tests.Add(new Test(dr));
                }
                return true;
            }
            catch (Exception ex)
            {
                tests = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddTest(Guid studentID, Test test)
        {
            const string PROC_NAME = "uspAddUpdateTest",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         TEST_ID_PARAM = "@guidTestID",
                         TEST_TYPE_PARAM = "@shtTestNameID",
                         TEST_YEAR_PARAM = "@shtTestYear",
                         TEST_SEMESTER_PARAM = "@shtSemesterID",
                         TEST_GRADE_PARAM = "@dblGrade";

            SqlParameter studentIDParam,
                         testIDParam,
                         testType,
                         testYear,
                         testSemester,
                         testGrade,
                         resultParam;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                if (test.TestID == Guid.Empty || test.TestID == null)
                    SetParameter(ref cmd, TEST_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out testIDParam);
                else
                    SetParameter(ref cmd, TEST_ID_PARAM, test.TestID, SqlDbType.UniqueIdentifier, out testIDParam);
                SetParameter(ref cmd, TEST_TYPE_PARAM, (short)test.TestType, SqlDbType.SmallInt, out testType);
                SetParameter(ref cmd, TEST_YEAR_PARAM, test.Year, SqlDbType.SmallInt, out testYear);
                SetParameter(ref cmd, TEST_SEMESTER_PARAM, (short)test.Semester, SqlDbType.SmallInt, out testSemester);
                SetParameter(ref cmd, TEST_GRADE_PARAM, test.Score, SqlDbType.Float, out testGrade);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool RemoveTest(Test test)
        {
            const string PROC_NAME = "uspAddTest",
                         TEST_ID_PARAM = "@guidTestID";

            SqlParameter testID, resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, TEST_ID_PARAM, test.TestID, SqlDbType.UniqueIdentifier, out testID);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((short)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        #region AirTests

        internal static bool GetAirTests(Guid studentID, out List<AirTest> airTests)
        {
            const string PROC_NAME = "uspGetStudentAirTests",
                         STUDENT_ID_PARAM = "@guidStudentID";

            SqlParameter studentIDParam,
                         resultParam;
            airTests = null;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                airTests = new List<AirTest>();
                foreach (DataRow dr in dt.Rows)
                {
                    airTests.Add(new AirTest(dr));
                }
                return true;
            }
            catch (Exception ex)
            {
                airTests = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddAirTest(Guid studentID, AirTest test)
        {
            const string PROC_NAME = "uspAddStudentAirTest",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         MATH_GRADE_PARAM = "@dblMathGrade",
                         READING_GRAD_PARAM = "@dblReadingGrade",
                         YEAR_PARAM = "@shtAirTestYear";

            SqlParameter studentIDParam,
                         mathGrade,
                         readingGrade,
                         year,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, MATH_GRADE_PARAM, test.MathGrade, SqlDbType.Float, out mathGrade);
                SetParameter(ref cmd, READING_GRAD_PARAM, test.ReadingGrade, SqlDbType.Float, out readingGrade);
                SetParameter(ref cmd, YEAR_PARAM, test.Year, SqlDbType.SmallInt, out year);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Bit, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool RemoveAirTest(AirTest test)
        {
            const string PROC_NAME = "uspRemoveAirTest",
                         TEST_ID_PARAM = "@guidAirTestID",
                         RESULT_PARAM = "@blnResult";

            SqlParameter testID, resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, TEST_ID_PARAM, test.AirTestID, SqlDbType.UniqueIdentifier, out testID);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Bit, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }


        internal static bool UpdateAirTest(AirTest test)
        {
            const string PROC_NAME = "uspAddUpdateAirTest",
                        TEST_ID_PARAM = "@guidAirTestID",
                        MATHGRADE_PARAM = "@dblMathGrade",
                        READINGGRADE_PARAM = "@dblReadingGrade",
                        YEAR_PARAM = "@shtAirTestYear";


            SqlParameter testID, mathgrade, readinggrade, year, resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, TEST_ID_PARAM, test.AirTestID, SqlDbType.UniqueIdentifier, out testID);
                SetParameter(ref cmd, MATHGRADE_PARAM, test.MathGrade, SqlDbType.Float, out mathgrade);
                SetParameter(ref cmd, READINGGRADE_PARAM, test.ReadingGrade, SqlDbType.Float, out readinggrade);
                SetParameter(ref cmd, YEAR_PARAM, test.Year, SqlDbType.SmallInt, out year);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        #endregion

        #endregion


        #region Behavior (ABC/ENTRY)

        internal static bool GetABCEntries(Guid studentID, out List<ABCEntry> abcEntries)
        {
            const string PROC_NAME = "uspGetStudentABCCharts",
                         STUDENT_ID_PARAM = "@guidStudentID";

            SqlParameter studentIDParam,
                         resultParam;

            abcEntries = null;
            DataTable dt;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd, out dt)) return false;
                if (((int)resultParam.Value) != 0) return false;

                abcEntries = new List<ABCEntry>();
                foreach (DataRow dr in dt.Rows)
                {
                    abcEntries.Add(new ABCEntry(dr));
                }

                return true;
            }
            catch (Exception ex)
            {
                abcEntries = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool AddABCEntry(ABCEntry entry)
        {
            const string PROC_NAME = "uspAddStudentABCEntry",
                         ABC_ID_PARAM = "@guidABCID",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ADDED_BY_PARAM = "@guidAddedBy",
                         ANTECEDENT_PARAM = "@strAntecedent",
                         BEHAVIOR_PARAM = "@strBehavior",
                         CONSEQUENCE_PARAM = "@strConsequence",
                         INCIDENT_DATE_PARAM = "@dtmIncidentDate";
            const int FIELD_SIZE = 1000;

            SqlParameter studentID,
                         abcID,
                         addedBy,
                         antecedent,
                         behavior,
                         consequence,
                         incidentDate,
                         resultParam;
            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, entry.StudentID, SqlDbType.UniqueIdentifier, out studentID);
                if (entry.ABCID == null || entry.ABCID == Guid.Empty)
                    SetParameter(ref cmd, ABC_ID_PARAM, DBNull.Value, SqlDbType.UniqueIdentifier, out abcID);
                else
                    SetParameter(ref cmd, ABC_ID_PARAM, entry.ABCID, SqlDbType.UniqueIdentifier, out abcID);
                SetParameter(ref cmd, ADDED_BY_PARAM, entry.AddedBy, SqlDbType.UniqueIdentifier, out addedBy);
                SetParameter(ref cmd, ANTECEDENT_PARAM, entry.Antecedent, SqlDbType.NVarChar, out antecedent, FIELD_SIZE);
                SetParameter(ref cmd, BEHAVIOR_PARAM, entry.Behavior, SqlDbType.NVarChar, out behavior, FIELD_SIZE);
                SetParameter(ref cmd, CONSEQUENCE_PARAM, entry.Consequence, SqlDbType.NVarChar, out consequence, FIELD_SIZE);
                SetParameter(ref cmd, INCIDENT_DATE_PARAM, entry.IncidentDate, SqlDbType.DateTime, out incidentDate);
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out resultParam, direction: ParameterDirection.ReturnValue);
                if (!ExecuteProcedure(ref cmd)) return false;
                return ((int)resultParam.Value) == 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static bool DeleteABCEntry(Guid studentID, Guid abcID)
        {
            const string PROC_NAME = "uspRemoveStudentABCEntry",
                         STUDENT_ID_PARAM = "@guidStudentID",
                         ABC_ID_PARAM = "@guidABCID";

            SqlParameter abcIDParam,
                         studentIDParam,
                         resultParam;

            try
            {
                SqlCommand cmd = new SqlCommand(PROC_NAME);
                SetParameter(ref cmd, STUDENT_ID_PARAM, studentID, SqlDbType.UniqueIdentifier, out studentIDParam);
                SetParameter(ref cmd, ABC_ID_PARAM, abcID, SqlDbType.UniqueIdentifier, out abcIDParam);
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

        #endregion






    }
}
