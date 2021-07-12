using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DatabaseInteraction.Models;

namespace ApproachingMastery.Models
{
    public class AddEditAccomodationTestModel
    {
        public Student s { get; set; }

        public Guid ExistingAccomodationTest { get; set; }

        public bool AddNewAccomodationTest { get; set; } = false;

        public Dictionary<Guid, string> ExistingAccomodations { get; set; } = new Dictionary<Guid, string>();

        public AccomodationTest Test { get; set; } = new AccomodationTest();

        public bool IsEditing { get; set; } = false;

        public AddEditAccomodationTestModel(AccomodationTest test, Guid StudentID)
        {

            Student s = new Student(StudentID);
            
            s.GetAccomodations();
            
            Test = test;
        }

        public AddEditAccomodationTestModel(AccomodationTest test)
        {

            Test = test;

        }

        public AddEditAccomodationTestModel()
        {


        }



    }



}