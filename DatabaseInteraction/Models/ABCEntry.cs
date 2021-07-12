using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseInteraction;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace DatabaseInteraction.Models
{ 

public class ABCEntry
    {
        public Guid StudentID { get; set; }
        public Guid ABCID { get; set; }
        public Guid AddedBy { get; set; }

        [DataType(DataType.MultilineText)]
        public string Antecedent { get; set; }

        [DataType(DataType.MultilineText)]
        public string Behavior { get; set; }

        [DataType(DataType.MultilineText)]
        public string Consequence { get; set; }

        [Display(Name = "Incident Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime IncidentDate { get; set; }

        public ABCEntry(DataRow dr)
        {
            const string STUDENT_ID_COLUMN = "guidStudentID",
                         ABC_ID_COLUMN = "guidABCID",
                         ADDED_BY_COLUMN = "guidAddedBy",
                         ANTECEDENT_COLUMN = "strAntecedent",
                         BEHAVIOR_COLUMN = "strBehavior",
                         CONSEQUENCE_COLUMN = "strConsequence",
                         INCIDENT_DATE_COLUMN = "dtmIncidentDate";

            ABCID = (Guid)dr[ABC_ID_COLUMN];
            AddedBy = (Guid)dr[ADDED_BY_COLUMN];
            Antecedent = dr[ANTECEDENT_COLUMN].ToString();
            Behavior = dr[BEHAVIOR_COLUMN].ToString();
            Consequence = dr[CONSEQUENCE_COLUMN].ToString();
            IncidentDate = (DateTime)dr[INCIDENT_DATE_COLUMN];
        }
        public ABCEntry() { }
    }

}