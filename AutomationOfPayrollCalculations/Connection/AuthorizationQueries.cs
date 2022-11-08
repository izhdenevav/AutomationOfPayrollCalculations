using AutomationOfPayrollCalculations.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace AutomationOfPayrollCalculations.Connection
{
    public class AuthorizationQueries
    {
        DataBase db = new DataBase();
        md5 md5 = new md5();

        public bool IsStringNotNull(string str)
        {
            if (str == null || str == "") return false;
            else return true;
        }

        public bool IsUserExist(string login)
        {
            bool flag = false;

            db.openConnection();

            string query = "select * from [User] where Login = '" + login + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) flag = true;
                else flag = false;

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return flag;
        }

        public bool IsPasswordCorrect(string login, string password)
        {
            bool flag = false;

            db.openConnection();

            string query = "select * from [User] where Login = '" + login + "' and Password = '" + md5.hashPassword(password) +"'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) flag = true;
                else flag = false;

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return flag;
        }

        public User GetUser(string login)
        {
            User user = new User();
            string role = "";
            if (IsUserManager(login)) role = "Manager";
            else if (IsUserExecutor(login)) role = "Executor";

            db.openConnection();

            string query = "select * from [User] where Login = '" + login + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new User { ID = Convert.ToInt32(reader["ID"]), Role = role, Login = login, Password = reader["Password"].ToString(), FirstName = reader["FirstName"].ToString(), MiddleName = reader["MiddleName"].ToString(), LastName = reader["LastName"].ToString()};
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return user;
        }

        public bool IsUserManager(string login)
        {
            bool flag = false;

            db.openConnection();

            string query = "select * from [Manager], [User] where [Manager].ID = [User].ID and [User].Login = '" + login + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) flag = true;
                else flag = false;

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return flag;
        }

        public bool IsUserExecutor(string login)
        {
            bool flag = false;

            db.openConnection();

            string query = "select * from [Executor], [User] where [Executor].ID = [User].ID and [User].Login = '" + login + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) flag = true;
                else flag = false;

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return flag;
        }
    }
}
