using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInteraction
{
    public static partial class Database
    {
        const string RESULT_PARAM = "@ReturnVal";
        public static bool ConnectToDatabase(ref SqlConnection sqlConn)
        {
            try
            {
                if (sqlConn == null) sqlConn = new SqlConnection();
                if (sqlConn.State != ConnectionState.Open)
                {
                    sqlConn.ConnectionString = Properties.DBSettings.Default.ApplicationDBConnectionString;
                    sqlConn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                sqlConn = null;
                return false;
            }
        }

        internal static bool AddResultParam(ref SqlCommand cmd, out SqlParameter result)
        {
            result = null;
            try
            {
                SetParameter(ref cmd, RESULT_PARAM, DBNull.Value, SqlDbType.Int, out result,
                    direction: ParameterDirection.ReturnValue);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool CloseDatabaseConnection(ref SqlConnection sqlConn)
        {
            try
            {
                if (sqlConn.State != ConnectionState.Closed)
                {
                    sqlConn.Close();
                    sqlConn = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        internal static int SetParameter(ref SqlCommand cm, string parameterName, object value
            , SqlDbType parameterType, out SqlParameter parameter, int fieldSize = -1
            , ParameterDirection direction = ParameterDirection.Input
            , Byte precision = 0, Byte scale = 0)
        {
            parameter = null;
            try
            {
                // set the command type to stored procedure
                cm.CommandType = System.Data.CommandType.StoredProcedure;

                // create the new parameter
                parameter = new SqlParameter(parameterName, parameterType);

                // set the parameter properties
                if (fieldSize != -1) parameter.Size = fieldSize;
                if (precision > 0) parameter.Precision = precision;
                if (scale > 0) parameter.Scale = scale;

                if (value == null) parameter.Value = DBNull.Value;
                else parameter.Value = value;
                parameter.Direction = direction;

                cm.Parameters.Add(parameter);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                parameter = null;
                return 1;
            }
        }

        internal static bool ExecuteProcedure(ref SqlCommand cmd)
        {
            SqlConnection sqlConn = null;
            bool result = false;
            try
            {
                result = ConnectToDatabase(ref sqlConn);
                sqlConn.InfoMessage += SqlConn_InfoMessage;
                if (result)
                {
                    cmd.Connection = sqlConn;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                {
                    sqlConn.InfoMessage -= SqlConn_InfoMessage;
                    CloseDatabaseConnection(ref sqlConn);
                }
            }

            return result;
        }

        static List<string[]> m_errorMessages = new List<string[]>();

        private static void SqlConn_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            lock (m_errorMessages)
            {
                if (m_errorMessages.Count > 10)
                    for (int i = 0; i < 10; i++)
                        m_errorMessages.RemoveAt(0);
            }
            m_errorMessages.Add(new[] { e.Source, e.Message });
        }

        internal static bool ExecuteProcedure(ref SqlCommand cmd, out DataTable dt)
        {
            SqlConnection sqlConn = null;
            bool result = false;
            dt = null;
            try
            {

                result = ConnectToDatabase(ref sqlConn);
                if (result)
                {
                    cmd.Connection = sqlConn;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                dt = null;
                Console.WriteLine(ex.ToString());
                result = false;
            }
            finally
            {
                if (sqlConn != null && sqlConn.State != ConnectionState.Closed)
                {
                    CloseDatabaseConnection(ref sqlConn);
                }
            }

            return result;
        }

    }
}
