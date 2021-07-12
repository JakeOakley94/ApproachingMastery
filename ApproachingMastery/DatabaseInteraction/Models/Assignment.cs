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
    public class Assignment
    {
        #region Properties
        public Guid GoalID { get; set; }
        public long AssignmentID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CompletionDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Details { get; set; }
        public double Score { get; set; }
        public Picture Image { get; set; }
        public User AddedBy { get; set; }
        public User CompletedBy { get; set; }
        #endregion


        #region Constructers 
        public Assignment(Guid goalID, long assignmentID, DateTime completionDate, string details, double score, Guid image, Guid addedBy, Guid completedBy)
        {
            GoalID = goalID;
            AssignmentID = assignmentID;
            CompletionDate = completionDate;
            Details = details;
            Score = score;
            Image = Picture.GetImageFromDatabase(image);
            User temp = null;
            User.GetUser(addedBy, out temp);
            AddedBy = temp;
            User.GetUser(completedBy, out temp);
            CompletedBy = temp;
        }

        public Assignment(DataRow dr)
        {
            GoalID = (Guid)dr["guidGoalID"];
            AssignmentID = (long)dr["lngAssignmentID"];
            // needs to be guidCompleteionDate becasue somebody messed up, probably me
            // -ben
            if (dr["guidCompletionDate"] != DBNull.Value)
                CompletionDate = (DateTime)dr["guidCompletionDate"];
            Details = dr["strDetails"].ToString();
            if (dr["strScore"] != DBNull.Value) Score = Convert.ToDouble(dr["strScore"].ToString());
            if (dr["guidImageID"] != DBNull.Value)
                Image = Picture.GetImageFromDatabase((Guid)dr["guidImageID"]);
            User temp = null;
            User.GetUser((Guid)dr["guidAddedBy"], out temp);
            AddedBy = temp;
            if(dr["guidCompletedBy"]!= DBNull.Value)
            User.GetUser((Guid)dr["guidCompletedBy"], out temp);
            CompletedBy = temp;
        }

        public Assignment() { }

        #endregion




    }
}
