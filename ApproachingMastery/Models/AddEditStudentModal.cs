using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApproachingMastery.Models
{
    public class AddEditStudentModal
    {
        public UserLogin User { get; set; }
        public bool AddExistingStudent { get; set; } = true;

        public Guid ExistingStudent { get; set; }

        public bool AddNewStudent { get; set; } = false;

        public Dictionary<Guid, string> ExistingStudents { get; set; } = new Dictionary<Guid, string>();

        public Student NewStudent { get; set; }

        public Guid StudentSchool { get; set; }

        public bool IsEditing { get; set; } = false;

        public AddEditStudentModal(Guid loginID)
        {
            User = new UserLogin(loginID);
            User.GetSchools();
            User.GetAvailableStudents();
        }

        public AddEditStudentModal()
        {


        }


    }
}