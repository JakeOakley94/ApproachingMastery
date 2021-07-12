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


// had to add the ? to some types to make them nullable, don't undo this or shit will break

namespace DatabaseInteraction.Models
{

    public enum GoalType
    {
        //needs values
        None = 10,
        Counter = 11,
        AssignmentsCompleted = 12
    }

    public enum GoalArea
    {
        Behavioral = 1,
        Academic = 2
    }

    public class Goal
    {
        #region Properties
        public Guid? GoalID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAssigned { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateCompleted { get; set; }
        public GoalType GoalType { get; set; }
        public GoalArea GoalArea { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public List<Assignment> Assignments { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public Guid AssignedBy { get; set; }


        #endregion

        #region Constructers
        public Goal(Guid? goalID, DateTime dateAssigned, DateTime? dateCompleted, DateTime dueDate, GoalType goalType, GoalArea goalArea, string description, Guid assignedBy, List<Assignment> assignments)
            : this(goalID, dateAssigned, dateCompleted, dueDate, goalType, goalArea, description, assignedBy)
        {
            Assignments = assignments;
        }

        public Goal(Guid? goalID, DateTime dateAssigned, DateTime? dateCompleted, DateTime dueDate, GoalType goalType, GoalArea goalArea, string description, Guid assignedBy)
        {
            GoalID = goalID;
            DateAssigned = dateAssigned;
            DateCompleted = dateCompleted;
            DueDate = dueDate;
            GoalType = goalType;
            GoalArea = goalArea;
            Description = description;
            AssignedBy = assignedBy;
        }

        public Goal() { }

        public Goal(DataRow dr)
        {
            const string GOAL_ID_COLUMN = "guidGoalID",
                         DATE_ASSIGNED_COLUMN = "dteDateAssigned",
                         DATE_COMPLETED_COLUMN = "dteDateCompleted",
                         DATE_DUE_COLUMN = "dteDateDue",
                         GOAL_TYPE_COLUMN = "shtGoalType",
                         GOAL_AREA_COLUMN = "shtGoalArea",
                         GOAL_DESCRIPTION_COLUMN = "strDescription",
                         ASSIGNED_BY_COLUMN = "guidAssignedBy";

            GoalID = (Guid)dr[GOAL_ID_COLUMN];
            DateAssigned = (DateTime)dr[DATE_ASSIGNED_COLUMN];
            if (dr[DATE_COMPLETED_COLUMN] != DBNull.Value)
                DateCompleted = (DateTime)dr[DATE_COMPLETED_COLUMN];
            DueDate = (DateTime)dr[DATE_DUE_COLUMN];
            GoalType = (GoalType)Convert.ToInt16(dr[GOAL_TYPE_COLUMN]);
            GoalArea = (GoalArea)Convert.ToInt16(dr[GOAL_AREA_COLUMN]);
            Description = dr[GOAL_DESCRIPTION_COLUMN].ToString();
            AssignedBy = (Guid)dr[ASSIGNED_BY_COLUMN];

            List<Assignment> assignments;
            if (Database.GetGoalAssignments((Guid)GoalID, out assignments))
            {
                Assignments = assignments;
            }

        }


        #endregion

        #region Methods
        public bool AddGoal()
        {
            const string PROCEDURE_PARAM = "uspAddUpdateGoal",
                        DATE_ASSIGNED_PARAM = "@dteDateAssigned",
                        DATE_COMPLETED_PARAM = "@dteDateCompleted",
                        GOAL_TYPE_PARAM = "@shtGoalType",
                        GOAL_AREA_PARAM = "@shtGoalArea",
                        DESCRIPTION_PARAM = "@strDescription";


            SqlConnection conn = null;
            bool result = false;
            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter DateAssignedParam = new SqlParameter(DATE_ASSIGNED_PARAM, SqlDbType.DateTime)
                {
                    Value = DateAssigned
                };

                SqlParameter DateCompletedParam = new SqlParameter(DATE_COMPLETED_PARAM, SqlDbType.DateTime)
                {
                    Value = DateCompleted
                };

                SqlParameter GoalTypeParam = new SqlParameter(GOAL_TYPE_PARAM, SqlDbType.SmallInt)
                {
                    Value = GoalType
                };

                SqlParameter GoalAreaParam = new SqlParameter(GOAL_AREA_PARAM, SqlDbType.SmallInt)
                {
                    Value = GoalArea
                };

                SqlParameter DescriptionParam = new SqlParameter(DESCRIPTION_PARAM, SqlDbType.VarChar)
                {
                    Value = Description
                };

                cmd.Parameters.Add(DateAssignedParam);
                cmd.Parameters.Add(DateCompletedParam);
                cmd.Parameters.Add(GoalTypeParam);
                cmd.Parameters.Add(GoalAreaParam);
                cmd.Parameters.Add(DescriptionParam);

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

        public bool RemoveGoal()
        {

            const string PROCEDURE_PARAM = "uspRemoveGoal",
                         GUID_GOAL_ID_PARAM = "@guidGoalID";

            SqlConnection conn = null;
            bool result = false;
            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;
                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter GoalIDParam = new SqlParameter(GUID_GOAL_ID_PARAM, SqlDbType.UniqueIdentifier, 255)
                {
                    Value = GoalID
                };

                cmd.Parameters.Add(GoalIDParam);

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
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }


            return result;
        }

        public bool AddAssignment(Assignment assignment)
        {
            const string PROCEDURE_PARAM = "uspAddAssignment",
                         GUID_GOAL_ID_PARAM = "@guidGoalID",
                         STR_DETAILS_PARAM = "@strDetails",
                         GUID_IMAGE_ID_PARAM = "@guidImageID",
                         GUID_ADDED_BY_PARAM = "@guidAddedBy",
                         GUID_COMPLETED_BY_PARAM = "@guidCompletedBy",
                         COMPLETION_DATE_PARAM = "@guidCompletionDate",
                         ASSIGNMENT_ID_PARAM = @"@lngAssignmentID",
                         SCORE_PARAM = "@strScore";

            SqlConnection conn = null;
            bool result = false;
            SqlParameter score;
            SqlParameter assignmentID;

            try
            {
                if (!Database.ConnectToDatabase(ref conn)) return false;

                SqlCommand cmd = new SqlCommand(PROCEDURE_PARAM, conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter GoalIDParam = new SqlParameter(GUID_GOAL_ID_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = GoalID
                };
                SqlParameter DetailsParam = new SqlParameter(STR_DETAILS_PARAM, SqlDbType.VarChar, 500)
                {
                    Value = assignment.Details
                };

                SqlParameter imageParam;
                if (assignment.Image != null && assignment.Image.ImageID != null && assignment.Image.ImageID != Guid.Empty)
                {
                    imageParam = new SqlParameter(GUID_IMAGE_ID_PARAM, SqlDbType.UniqueIdentifier)
                    {
                        Value = assignment.Image.ImageID
                    };
                }
                else
                {
                    imageParam = new SqlParameter(GUID_IMAGE_ID_PARAM, SqlDbType.UniqueIdentifier)
                    {
                        Value = DBNull.Value
                    };
                }
                SqlParameter AddedByParam = new SqlParameter(GUID_ADDED_BY_PARAM, SqlDbType.UniqueIdentifier)
                {
                    Value = assignment.AddedBy.UserLoginID
                };

                SqlParameter completedBy = null;
                if (assignment.CompletedBy == null)
                {
                    completedBy = new SqlParameter(GUID_COMPLETED_BY_PARAM, SqlDbType.UniqueIdentifier)
                    {
                        Value = DBNull.Value
                    };
                }
                else
                {
                    completedBy = new SqlParameter(GUID_COMPLETED_BY_PARAM, SqlDbType.UniqueIdentifier)
                    {
                        Value = assignment.CompletedBy.UserLoginID
                    };
                }

                SqlParameter completionDate = null;
                if (assignment.CompletionDate == null)
                {
                    completionDate = new SqlParameter(COMPLETION_DATE_PARAM, SqlDbType.DateTime)
                    {
                        Value = DBNull.Value
                    };
                }
                else
                {
                    completionDate = new SqlParameter(COMPLETION_DATE_PARAM, SqlDbType.DateTime)
                    {
                        Value = assignment.CompletionDate
                    };
                }




                cmd.Parameters.Add(GoalIDParam);
                cmd.Parameters.Add(DetailsParam);
                cmd.Parameters.Add(imageParam);
                cmd.Parameters.Add(AddedByParam);
                cmd.Parameters.Add(completedBy);
                cmd.Parameters.Add(completionDate);
                Database.SetParameter(ref cmd, SCORE_PARAM, assignment.Score, SqlDbType.NVarChar, out score, 50);
                Database.SetParameter(ref cmd, ASSIGNMENT_ID_PARAM, assignment.AssignmentID, SqlDbType.BigInt, out assignmentID, direction: ParameterDirection.InputOutput);
                return Database.ExecuteProcedure(ref cmd);

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result = false;

            }

            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }

            return result;
        }
        public bool RemoveAssignment(int assignmentID)
        {
            return Database.RemoveGoalAssignment((Guid)GoalID, assignmentID);
        }

        public bool GetAssignment()
        {
            List<Assignment> assignments = new List<Assignment>();
            try
            {
                Database.GetGoalAssignments((Guid)GoalID, out assignments);
                Assignments = assignments;
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
