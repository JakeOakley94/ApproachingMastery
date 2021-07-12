using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public enum TestType
    {
        SRI = 1,
        ProCore = 2,
        TenMark = 3
    }

    public enum Semester
    {
        Fall = 1,
        Winter = 2,
        Spring = 3,
        Summer = 4
    }

    public class Test
    {



        #region Public Properties

        public Guid TestID { get; set; }
        public TestType TestType { get; set; }
        public Semester Semester { get; set; }
        public DateTime DateOfTest { get; set; }
        public short Year { get; set; }
        public double? Score { get; set; }


        #endregion

        #region Constructors

        public Test() { }

        public Test(TestType testType, Semester semester, short year, double score)
        {
            TestType = testType;
            Semester = semester;
            Year = year;
            Score = score;
        }

        public Test(DataRow dr)
        {
            const string TEST_ID_COLUMN = "guidTestID",
                         TEST_TYPE_COLUMN = "shtTestNameID",
                         TEST_YEAR_COLUMN = "shtTestYear",
                         TEST_SEMESTER_COLUMN = "shtSemesterID",
                         TEST_GRADE_COLUMN = "dblGrade";

            TestType = (TestType)Convert.ToInt16(dr[TEST_TYPE_COLUMN]);
            Semester = (Semester)Convert.ToInt16(dr[TEST_SEMESTER_COLUMN]);
            Year = Convert.ToInt16(dr[TEST_YEAR_COLUMN]);
            Score = Convert.ToDouble(dr[TEST_GRADE_COLUMN]);
            TestID = (Guid)dr[TEST_ID_COLUMN];
        }



        #endregion

        #region Methods

        public bool RemoveTest(Test test)
        {
            return Database.RemoveTest(test);
        }

        #endregion

    }
}
