using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DatabaseInteraction.Models;
using System.Globalization;

namespace ApproachingMasteryUnitTests
{
    [TestClass]
    public class StudentTest
    {
        [TestMethod]
        public void GetStudentTest()
        {
            Student s;
            bool result = Student.GetStudent(new Guid("80C35FA5-11D3-4532-9496-F29B1A6AE96E"), out s);
            Assert.AreEqual(result, true);
            Assert.AreEqual(s.FirstName, "Christin");
            Assert.AreEqual(s.MiddleName, "Dove");
            Assert.AreEqual(s.LastName, "Causer");
            Assert.AreEqual(s.Birthday, DateTime.ParseExact("11/24/2014", "MM/dd/yyyy", CultureInfo.InvariantCulture));
            Assert.AreEqual(s.IsActive, false);
        }

        [TestMethod]
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

        [TestMethod]
        public void AddGoal()
        {
            Student s;
            bool result = Student.GetStudent(new Guid("0959F931-2237-4C70-A574-7B6C31F4A6E0"), out s);
            Assert.AreEqual(result, true);

            result = s.GetAccademicGoals();
            Assert.AreEqual(result, true);

            int oldGoalCount = s.AccademicGoals.Count;

            result = s.AddGoal(new Goal(null, DateTime.Now, null, DateTime.Now + new TimeSpan(5, 0, 0, 0), GoalType.None, GoalArea.Accademic, "This is a test", new Guid("74E2964F-6D75-48D9-999F-A7B3A8324766")));

            Assert.AreEqual(result, true);

            result = s.GetAccademicGoals();
            Assert.AreEqual(result, true);
            Assert.AreEqual(oldGoalCount + 1, s.AccademicGoals.Count);
            

        }

    }
}
