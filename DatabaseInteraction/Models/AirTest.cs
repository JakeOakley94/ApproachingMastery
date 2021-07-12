
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{

    public class AirTest
    {
        #region Public Properties

        public Guid AirTestID { get; set; }
        public double MathGrade { get; set; }
        public double ReadingGrade { get; set; }
        public short Year { get; set; }

        #endregion

        #region Constructors

        public AirTest()
        {

        }


        public AirTest(DataRow dr)
        {
            const string AIR_TEST_ID_COLUMN = "guidAirTestID",
                         MATH_GRADE_COLUMN = "dblMathGrade",
                         READING_GRADE_COLUMN = "dblReadingGrade",
                         YEAR_COLUMN = "shtYear";

            AirTestID = Guid.Parse(dr[AIR_TEST_ID_COLUMN].ToString());
            MathGrade = (double)dr[MATH_GRADE_COLUMN];
            ReadingGrade = (double)dr[READING_GRADE_COLUMN];
            Year = (short)dr[YEAR_COLUMN];
        }

        public AirTest(float readingGrade, float mathGrade, short year, Guid airTestID)
        {
            AirTestID = airTestID;
            MathGrade = mathGrade;
            ReadingGrade = readingGrade;
            Year = year;
        }

        #endregion

        #region Methods

        public bool RemoveTest()
        {
          return Database.RemoveAirTest(this);
                 
        }

        public bool UpdateTest()
        {

            return Database.UpdateAirTest(this);

        }

        public static bool GetAirTest(Guid airTestID, out AirTest t)
        {
            t = null;
            try
            {
                if (!Database.GetAirTest(airTestID, out t)) return false;
                return true;
            }
            catch(Exception ex)
            {
                t = null;
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool RemoveAirTest(Guid airTestID)
        {
            try
            {
                return Database.RemoveAirTest(new AirTest() { AirTestID = airTestID });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }



        #endregion

    }
}