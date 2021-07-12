using DatabaseInteraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApproachingMastery.Models
{
    public class TestYearModal
    {
        public Test Fall { get; set; } = new Test() { Semester = Semester.Fall };
        public Test Winter { get; set; } = new Test() { Semester = Semester.Winter };
        public Test Spring { get; set; } = new Test() { Semester = Semester.Spring };
        public Test Summer { get; set; } = new Test() { Semester = Semester.Summer };
        public short Year { get; set; }
        public TestType TestType { get; set; }
        public TestYearModal(List<Test> tests, TestType type)
        {
            TestType = type;
            if (tests != null)
            {
                Fall = tests.Find(t => t.Semester == Semester.Fall);
                Winter = tests.Find(t => t.Semester == Semester.Winter);
                Spring = tests.Find(t => t.Semester == Semester.Spring);
                Summer = tests.Find(t => t.Semester == Semester.Summer);

                Test notNull = Fall ?? Winter ?? Spring ?? Summer;
                if (notNull == null)
                    Year = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
                else
                    Year = notNull.Year;

                if (Fall == null) Fall = new Test() { Semester = Semester.Fall };
                if (Winter == null) Winter = new Test() { Semester = Semester.Winter };
                if (Spring == null) Spring = new Test() { Semester = Semester.Spring };
                if (Summer == null) Summer = new Test() { Semester = Semester.Summer };

            }
        }
        public TestYearModal() { }
    }
}