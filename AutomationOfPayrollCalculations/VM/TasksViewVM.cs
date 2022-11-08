using AutomationOfPayrollCalculations.Connection;
using AutomationOfPayrollCalculations.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AutomationOfPayrollCalculations.VM
{
    public class TasksViewVM : INotifyPropertyChanged
    {
        private TasksQueries tasksQ = new TasksQueries();
        private User thisUser;
        private Window thisWindow;

        private List<Task> tasks;
        public List<Task> Tasks
        {
            get { return tasks; }
            set
            {
                tasks = value;
                OnPropertyChanged();
            }
        }

        private Task selectedTask;
        public Task SelectedTask
        {
            get { return selectedTask; }
            set
            {
                selectedTask = value;
                EditOrAddTask editTask = new EditOrAddTask(thisUser, SelectedTask);
                editTask.Show();
                thisWindow.Close();
                OnPropertyChanged();
            }
        }

        private List<string> statuses; 
        public List<string> Statuses
        {
            get { return statuses; }
            set
            {
                statuses = value;
                OnPropertyChanged();
            }
        }

        private string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                Filter();
                OnPropertyChanged();
            }
        }

        private List<User> executors;
        public List<User> Executors
        {
            get { return executors; }
            set
            {
                executors = value;
                OnPropertyChanged();
            }
        }

        private User executor;
        public User Executor
        {
            get { return executor; }
            set
            {
                executor = value;
                Filter();
                OnPropertyChanged();
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }

        public TasksViewVM(Window window, User user)
        {
            thisUser = user;
            thisWindow = window;
            if (user.Role == "Executor")
            {
                isEnabled = false;
                tasks = tasksQ.GetTasksByExecutor(user);
            }
            else {
                isEnabled = true;
                tasks = tasksQ.GetTasksByManager(user);
            } 

            statuses = new List<string>() { "выполнена", "отменена", "запланирована", "исполняется", ""};
            executors = tasksQ.GetExecutorsByManager(user);
            executors.Add(new User { });
        }

        private void Filter()
        {
            if (Status == null || Status == "")
            {
                if (Executor.MiddleName == null || Executor.MiddleName == "")
                {
                    tasks = tasksQ.GetTasksByManager(thisUser);
                } else
                {
                    tasks = tasksQ.GetManagerTasksByExecutor(tasksQ.GetTasksByManager(thisUser), Executor);
                }
            } else
            {
                if (Executor.MiddleName == null || Executor.MiddleName == "")
                {
                    tasks = tasksQ.GetManagerTasksByStatus(tasksQ.GetTasksByManager(thisUser), Status);
                } else
                {
                    tasks = tasksQ.GetmanagerTasksByStatusAndExecutor(tasksQ.GetTasksByManager(thisUser), Status, Executor);
                }
            }
        }

        public ICommand addCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        EditOrAddTask addTask = new EditOrAddTask(thisUser, new Task { });
                        addTask.Show();
                        thisWindow.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
