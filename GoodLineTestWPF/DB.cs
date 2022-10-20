using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoodLineTestWPF
{
    public class DB
    {
        public Exception EstablishConnection(string ServerName, string DBName, string login, string pwd)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder["Server"] = ServerName;
            builder["Integrated Security"] = false;
            builder["Database"] = DBName;
            builder["User ID"] = login;
            builder["Password"] = pwd;

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    connection.Open();                   
                    Properties.Settings.Default.ConnectionString = builder.ConnectionString;
                    return null;
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
        }

        public DataSet ReturnDataSet()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.ConnectionString))
            {
                try
                {
                    connection.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Select * from Merchandise; Select * from Categories;", connection);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd.CommandText, Properties.Settings.Default.ConnectionString))
                    {
                        da.Fill(ds);
                        ds.Tables[0].TableName = "Merchandise";
                        ds.Tables[1].TableName = "Categories";
                    }
                    return ds;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
