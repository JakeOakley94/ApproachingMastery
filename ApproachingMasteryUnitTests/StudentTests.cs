using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseInteraction.Models;
using System.Globalization;

namespace ApproachingMasteryUnitTests
{
    [TestClass]
    public class StudentTests
    {
        [TestMethod]
        public void RunAllStudentTests()
        {
            GetStudentTest();
            UpdateStudent();
            AddGoal();
            AddObjectives();
            GetAcademicGoals(1);
            GetBehavioralGoals(1);
            GetAcademicObjectives(1);
            GetBehavioralObjectives(1);
            RemoveGoal();
            RemoveObjective();
            AddStudentComment();
            GetStudentComments();
            AddStudentTest();
            GetStudentTests();
            AddStudentAirTest();
            GetStudentAirTests();
            
        }

        

        public void GetStudentTest()
        {
            Student s;
            bool result = Student.GetStudent(new Guid("80C35FA5-11D3-4532-9496-F29B1A6AE96E"), out s);
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.FirstName, "Christin");
            Assert.AreEqual(s.MiddleName, "Dove");
            Assert.AreEqual(s.LastName, "Causer");
            Assert.AreEqual(s.Birthday, DateTime.ParseExact("11/24/2014", "MM/dd/yyyy", CultureInfo.InvariantCulture));
            Assert.AreEqual(s.IsActive, true);
        }

        public void UpdateStudent()
        {
            Student s, s_updated;
            bool result = Student.GetStudent(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"), out s);
            Assert.AreEqual(result, true);
            bool originalActive = s.IsActive;
            s.IsActive = !originalActive;
            result = s.AddUpdateStudent();
            Assert.AreEqual(result, true);
            result = Student.GetStudent(s.StudentID, out s_updated);
            Assert.AreEqual(result, true);
            Assert.AreEqual(s_updated.IsActive, !originalActive);
        }

        public void AddGoal()
        {
            Student s;
            bool result = Student.GetStudent(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"), out s);
            Assert.AreEqual(result, true);

            result = s.GetAcademicGoals();
            Assert.AreEqual(result, true);

            int oldGoalCount = s.AcademicGoals.Count;

            result = s.AddGoal(new Goal(null, DateTime.Now, null, DateTime.Now + new TimeSpan(5, 0, 0, 0), GoalType.None, GoalArea.Academic, "This is a test", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));
            result = s.AddGoal(new Goal(null, DateTime.Now, null, DateTime.Now + new TimeSpan(5, 0, 0, 0), GoalType.None, GoalArea.Behavioral, "This is a test", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));

            Assert.AreEqual(result, true);

            result = s.GetAcademicGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(oldGoalCount + 1, s.AcademicGoals.Count);
        }

        private void AddObjectives()
        {
            Student s;
            bool result = Student.GetStudent(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"), out s);
            Assert.AreEqual(result, true);

            result = s.GetAcademicObjectives();
            Assert.AreEqual(result, true);

            int oldGoalCount = s.AcademicObjectives.Count;

            result = s.AddObjective(new Goal(null, DateTime.Now, null, DateTime.Now + new TimeSpan(5, 0, 0, 0), GoalType.None, GoalArea.Academic, "This is a test", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));
            result = s.AddObjective(new Goal(null, DateTime.Now, null, DateTime.Now + new TimeSpan(5, 0, 0, 0), GoalType.None, GoalArea.Behavioral, "This is a test", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));

            Assert.AreEqual(result, true);

            result = s.GetAcademicObjectives();
            Assert.AreEqual(result, true);
            Assert.AreEqual(oldGoalCount + 1, s.AcademicObjectives.Count);
        }


        public void GetAcademicGoals(int expectedValue)
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result= s.GetAcademicGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.AcademicGoals.Count, expectedValue);
        }

        public void GetBehavioralGoals(int expectedValue)
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetBehavioralGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.BehavioralGoals.Count, expectedValue);
        }

        public void GetAcademicObjectives(int expectedValue)
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetAcademicObjectives();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.AcademicObjectives.Count, expectedValue);
        }

        public void GetBehavioralObjectives(int expectedValue)
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetBehavioralObjectives();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.BehavioralObjectives.Count, expectedValue);
        }

        public void RemoveGoal()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetAcademicGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.AcademicGoals.Count, 1);

            result = s.RemoveGoal(s.AcademicGoals[0]);
            Assert.AreEqual(result, true);
            GetAcademicGoals(0);

            result = s.GetBehavioralGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.BehavioralGoals.Count, 1);
            result = s.RemoveGoal(s.BehavioralGoals[0]);
            GetBehavioralGoals(0);
        }

        private void RemoveObjective()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetAcademicObjectives();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.AcademicObjectives.Count, 1);

            result = s.RemoveObjective(s.AcademicObjectives[0]);
            Assert.AreEqual(result, true);
            GetAcademicObjectives(0);

            result = s.GetBehavioralObjectives();
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.BehavioralObjectives.Count, 1);
            result = s.RemoveObjective(s.BehavioralObjectives[0]);
            GetBehavioralObjectives(0);
        }

        private void AddStudentComment()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.AddComment(new Message("I am a test message", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));
            Assert.AreEqual(result, true);
        }

        public void GetStudentComments()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.GetComments();
            Assert.AreEqual(s.Comments.Count > 0, true);
        }

        public void AddStudentTest()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            //bool result = s.AddTest(new Test(TestType.ProCore, Semester.Fall, 2019, 90,Guid.Empty));
            //Assert.AreEqual(result, true);
        }
        private void GetStudentTests()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            //Assert.AreEqual(s.GetTests(), true);
            Assert.AreEqual(s.Tests.Count > 0, true);
        }
        public void AddStudentAirTest()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            bool result = s.AddAirTest(new AirTest(100, 75, 2019, Guid.Empty));
            Assert.AreEqual(result, true);
        }
        private void GetStudentAirTests()
        {
            Student s = new Student(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"));
            Assert.AreEqual(s.GetAirTests(), true);
            Assert.AreEqual(s.AirTests.Count > 0, true);
        }


    }
}
