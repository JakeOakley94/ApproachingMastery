using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public class AccomodationTest
    {

        public enum AccomodationTestType
        {
            Reading = 1,
            Writing = 2,
            Math = 3,        

        }

        public Guid TestID { get; set; }
        public AccomodationTestType Type { get; set; }

        public Int16 AccomodationID { get; set; }

        public string TestGiven { get; set; }
        public double Percentile { get; set; }
        public DateTime Date { get; set; }

        #region Public Methods

        public static bool GetAccomodationTest(Guid AccomodationTestID, out AccomodationTest t)
        {
            t = null;
            try
            {
                if (!Database.GetAccomodationTest(AccomodationTestID, out t)) return false;
                return true;
            }
            catch (Exception ex)
            {
                t = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        #endregion

        #region Constructors

        public AccomodationTest(DataRow dr)
        {
            const string TEST_ID_COLUMN = "guidTestID",
                         TEST_GIVEN_COLUMN = "strTestGiven",
                         PERCENTILE_COLUMN = "dblPercentile",
                         TEST_TYPE_COLUMN = "shtTestTypeID",
                         DATE_COLUMN = "dteDate";

            TestID = Guid.Parse(dr[TEST_ID_COLUMN].ToString());
            TestGiven = (string)dr[TEST_GIVEN_COLUMN];
            Percentile = (double)dr[PERCENTILE_COLUMN];
            Type = (AccomodationTestType)Convert.ToInt16(dr[TEST_TYPE_COLUMN]);
            Date = (DateTime)dr[DATE_COLUMN];
        }

        public AccomodationTest(string testgiven, double percentile, DateTime date)
        {
            TestGiven = testgiven;
            Percentile = percentile;
            Date = date;

        }

        public AccomodationTest()
        {


        }

        #endregion

    }
}

