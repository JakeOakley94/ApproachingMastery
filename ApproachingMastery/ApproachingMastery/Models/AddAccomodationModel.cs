using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseInteraction.Models;

namespace ApproachingMastery.Models
{
    public class AddAccomodationModel
    {
        public Accomodation AccomodationModel { get; set; }

        public AccomodationDetail AccomodationDetailModel { get; set; }

        public Student Student { get; set; }

        public  AddAccomodationModel(Guid StudentID)
        {
            
            Student = new Student(StudentID);

            Student s;

            Student.GetStudent(StudentID, out s);

            Student.FirstName = s.FirstName;

        }


    }
}