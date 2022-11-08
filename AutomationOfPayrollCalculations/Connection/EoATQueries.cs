using AutomationOfPayrollCalculations.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace AutomationOfPayrollCalculations.Connection
{
    public class EoATQueries
    {
        DataBase db = new DataBase();

        public User GetExecutorByTask(Task task)
        {
            User executor = new User();

            db.openConnection();

            string query = "select * from [User], [Task] where [Task].ExecutorID = [User].ID and [Task].ID = '" + task.ID + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    executor = new User { ID = Convert.ToInt32(reader["ID"]), Role = "executor", Login = reader["Login"].ToString(), Password = reader["Password"].ToString(), FirstName = reader["FirstName"].ToString(), MiddleName = reader["MiddleName"].ToString(), LastName = reader["LastName"].ToString() };
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return executor;
        }

        public void EditTask(Task oldTask, Task newTask)
        {
            db.openConnection();

            string query = "update [Task] set ExecutorID = '" + newTask.ExecutorID + "', Title = '" + newTask.Title + "', Status = '" + newTask.Status + "', Difficulty = '" + newTask.Difficulty + "' where [Task].ID = '" + oldTask.ID + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();
        }

        public void AddTask(Task newTask)
        {
            db.openConnection();

            string query = "insert into [Task] values ('" + newTask.ExecutorID + "', '" + newTask.Title + "', 'Description', '01/01/2022', '01/01/2022', '" + newTask.Difficulty + "', 0, '" + newTask.Status + "', 'WorkType', '01/01/2022', 0)";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();
        }

        public void DeleteTask(Task task)
        {
            db.openConnection();

            string query = "delete from [Task] where [Task].ID = '" + task.ID + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();
        }
    }
}
