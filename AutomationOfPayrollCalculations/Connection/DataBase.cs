using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace AutomationOfPayrollCalculations.Connection
{
    class DataBase
    {
        private static string connStr = @"Data Source = MSI\SQLEXPRESS; Initial Catalog = AutomationOfPayrollCalculations; Integrated Security = True;";
        SqlConnection conn = new SqlConnection(connStr);

        public void openConnection()
        {
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void closeConnection()
        {
            if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return conn;
        }
    }
}
