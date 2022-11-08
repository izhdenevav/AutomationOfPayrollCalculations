using AutomationOfPayrollCalculations.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

namespace AutomationOfPayrollCalculations.Connection
{
    public class TasksQueries
    {
        DataBase db = new DataBase();
        
        public List<Task> GetTasksByExecutor(User user)
        {
            List<Task> tasks = new List<Task>();

            string executor = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            string manager = GetManagerByExecutor(user).FirstName + " " + GetManagerByExecutor(user).MiddleName +  " " + GetManagerByExecutor(user).LastName;

            db.openConnection();

            string query = "select * from [Task] where ExecutorID = '" + user.ID + "' order by CreateDateTime desc";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new Task { ID = Convert.ToInt32(reader["ID"]), ExecutorID = user.ID, Status = reader["Status"].ToString(), Title = reader["Title"].ToString(), Executor = executor, Manager = manager, Difficulty = Convert.ToInt32(reader["Difficulty"])});
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return tasks;
        }

        public User GetManagerByExecutor(User user)
        {
            User manager = new User();

            db.openConnection();

            string query = "select * from [User], [Executor] where [Executor].ManagerID = [User].ID and [Executor].ID = '" + user.ID + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    user = new User { ID = Convert.ToInt32(reader["ID"]), Role = "Manager", Login = reader["Login"].ToString(), Password = reader["Password"].ToString(), FirstName = reader["FirstName"].ToString(), MiddleName = reader["MiddleName"].ToString(), LastName = reader["LastName"].ToString() };
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

        public List<User> GetExecutorsByManager(User user)
        {
            List<User> executors = new List<User>();

            db.openConnection();

            string query = "select * from [Executor], [User] where [User].ID = [Executor].ID and [Executor].ManagerID = '" + user.ID + "'";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    executors.Add(new User { ID = Convert.ToInt32(reader["ID"]), Role = "Executor", Login = reader["Login"].ToString(), Password = reader["Password"].ToString(), FirstName = reader["FirstName"].ToString(), MiddleName = reader["MiddleName"].ToString(), LastName = reader["LastName"].ToString() });
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return executors;
        }

        public List<Task> GetTasksByManager(User user)
        {
            List<Task> tasks = new List<Task>();
            List<User> executors = GetExecutorsByManager(user);

            foreach (User a in executors)
            {
                tasks.AddRange(GetTasksByExecutor(a));
            }

            return tasks;
        }

        public List<Task> GetExecutorTasksByStatus(User user, string status)
        {
            List<Task> tasks = new List<Task>();

            string executor = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            string manager = GetManagerByExecutor(user).FirstName + " " + GetManagerByExecutor(user).MiddleName + " " + GetManagerByExecutor(user).LastName;

            db.openConnection();

            string query = "select * from [Task], [Executor], [User] where [Task].ExecutorID = [Executor].ID and [User].ID = [Executor].ID and [Task].Status = '" + status + "' and [User].ID = '" + user.ID + "' order by CreateDateTime desc";

            try
            {
                SqlCommand command = new SqlCommand(query, db.getConnection());
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new Task { ID = Convert.ToInt32(reader["ID"]), ExecutorID = user.ID, Status = reader["Status"].ToString(), Title = reader["Title"].ToString(), Executor = executor, Manager = manager, Difficulty = Convert.ToInt32(reader["Difficulty"]) });
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            db.closeConnection();

            return tasks;
        }

        public List<Task> GetManagerTasksByExecutor(List<Task> tasks, User executor)
        {
            List<Task> tasksByExecutor = new List<Task>();

            foreach(Task task in tasks)
            {
                if (task.ExecutorID == executor.ID) tasksByExecutor.Add(task);
            }

            return tasksByExecutor;
        }

        public List<Task> GetManagerTasksByStatus(List<Task> tasks, string status)
        {
            List<Task> tasksByStatus = new List<Task>();

            foreach (Task task in tasks)
            {
                if (task.Status == status) tasksByStatus.Add(task);
            }

            return tasksByStatus;
        }

        public List<Task> GetmanagerTasksByStatusAndExecutor(List<Task> tasks, string status, User executor)
        {
            List<Task> tasksByStatusAndExecutor = new List<Task>();

            foreach (Task task in tasks)
            {
                if (task.Status == status && task.ExecutorID == executor.ID) tasksByStatusAndExecutor.Add(task);
            }

            return tasksByStatusAndExecutor;
        }


    }
}
