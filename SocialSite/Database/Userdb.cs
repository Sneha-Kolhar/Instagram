using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SocialSite.Database
{

    public  class Userdb 
    {
       
        public static string connnectionString;
        public static SqlConnection connection;
        public Userdb(IConfiguration configuration)
        {
            connnectionString = configuration.GetConnectionString("DefaultConnection");
             connection = new SqlConnection(connnectionString);
        }

        public void saveRecord(int userID , string uname, string psw, string eid)
        {
            SqlCommand cmd = new SqlCommand("pr_user_add", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            cmd.Parameters.AddWithValue("Id", userID);
            cmd.Parameters.AddWithValue("EmailId", eid);
            cmd.Parameters.AddWithValue("Name", uname);
            cmd.Parameters.AddWithValue("Password", psw);
            cmd.ExecuteNonQuery();
            connection.Close();
        }



    }
}
