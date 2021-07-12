using DatabaseInteraction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {

            DatabaseInteraction.Emailer.SendEmail(Emailer.Senders.DoNotReply, "bcapuana@approaching-mastery.com", "test", "test from c#", false);



            /*SqlConnection test = null;
            bool connected = Database.ConnectToDatabase(ref test);

            string strSqlCommand = $"SELECT * FROM TUserRoles";
            SqlCommand cmd = new SqlCommand(strSqlCommand, test);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach(DataRow dr in dt.Rows)
            {
                Console.WriteLine($"{dr[0].ToString()}, {dr[1].ToString()}");
            }

            Database.CloseDatabaseConnection(ref test);

            Console.ReadLine();*/

            string messageText = File.ReadAllText("HTMLPage1.html");

            
            
            Emailer.SendEmail(Emailer.Senders.DoNotReply, "capuanbm@gmail.com","HTML Test", messageText, false);
            Console.ReadKey();
        }
    }
}
