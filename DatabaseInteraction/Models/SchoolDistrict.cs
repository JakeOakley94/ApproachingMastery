using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction.Models
{
    public class SchoolDistrict
    {




        
 

        public static bool CheckDomainExists(string domainName)
        {
            const string DOMAIN_PARAM = "@strEmailDomain",
                         RESULT_PARAM = "@ResultVal",
                         PROCEDURE_NAME = "uspCheckDomainExists";

            bool result = false;
            SqlConnection conn = null;
            try
            {
                if (Database.ConnectToDatabase(ref conn))
                {
                    SqlCommand cmd = new SqlCommand(PROCEDURE_NAME,conn) {
                        CommandType = System.Data.CommandType.StoredProcedure
                    };
                    SqlParameter domainParam = new SqlParameter(DOMAIN_PARAM, System.Data.SqlDbType.NVarChar, 255)
                    {
                        Value = domainName
                    };
                    SqlParameter resultParam = new SqlParameter(RESULT_PARAM, System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.ReturnValue
                    };
                    cmd.Parameters.Add(domainParam);
                    cmd.Parameters.Add(resultParam);
                    cmd.ExecuteNonQuery();
                    result = Convert.ToInt16(resultParam.Value) == 0;
                }
                else
                    result = false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    Database.CloseDatabaseConnection(ref conn);
            }

            return result;
        }


    }
}
