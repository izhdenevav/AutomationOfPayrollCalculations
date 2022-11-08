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
    class EditOrAddTaskVM : INotifyPropertyChanged
    {
        public TasksQueries tQ = new TasksQueries();
        public EoATQueries eo = new EoATQueries();

        private Task thisTask;
        private Task newTask;

        private User thisUser;
        private Window thisWindow;

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
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

        private User selectedExecutor;
        public User SelectedExecutor
        {
            get { return selectedExecutor; }
            set
            {
                selectedExecutor = value;
                OnPropertyChanged();
            }
        }

        private bool isExecutorsEnabled;
        public bool IsExecutorsEnabled
        {
            get { return isExecutorsEnabled; }
            set
            {
                isExecutorsEnabled = value;
                OnPropertyChanged();
            }
        }

        private bool isStatusesEnabled;
        public bool IsStatusesEnabled
        {
            get { return isStatusesEnabled; }
            set
            {
                isStatusesEnabled = value;
                OnPropertyChanged();
            }
        }

        private int taskDifficulty; 
        public int TaskDifficulty
        {
            get { return taskDifficulty; }
            set
            {
                taskDifficulty = value;
                OnPropertyChanged();
            }
        }

        private bool isDeleteEnabled;
        public bool IsDeleteEnabled
        {
            get { return isDeleteEnabled; }
            set
            {
                isDeleteEnabled = value;
                OnPropertyChanged();
            }
        }

        public EditOrAddTaskVM(User user, Task task, Window window)
        {
            IsExecutorsEnabled = false;
            thisTask = task;
            thisWindow = window;
            thisUser = user;
            Statuses = new List<string> { "выполнена", "отменена", "запланирована", "исполняется" };
            if (task.Status == "отменена" || task.Status == "выполнена") IsStatusesEnabled = false; else IsStatusesEnabled = true;
            if (thisTask.Executor == null) IsDeleteEnabled = false; else IsDeleteEnabled = true;
            Title = task.Title;
            Status = task.Status;
            TaskDifficulty = task.Difficulty;
            if (user.Role == "Executor")
            {
                Executors = new List<User>();
                Executors.Add(user);
                SelectedExecutor = user;
            } else if (user.Role == "Manager")
            {
                IsExecutorsEnabled = true;
                Executors = tQ.GetExecutorsByManager(user);
                foreach (User executor in Executors)
                {
                    if (executor.MiddleName == eo.GetExecutorByTask(task).MiddleName) {
                        SelectedExecutor = executor;
                    } 
                }
            }
        }

        public ICommand backCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        TasksView tasksView = new TasksView(thisUser);
                        tasksView.Show();
                        thisWindow.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }

        public ICommand plusCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        if (TaskDifficulty <= 49)
                        {
                            TaskDifficulty++;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }

        public ICommand minusCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        if (TaskDifficulty >= 2)
                        {
                            TaskDifficulty--;
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }

        public ICommand saveCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        if (thisTask.Executor == null)
                        {
                            newTask = new Task { ID = thisTask.ID, Difficulty = TaskDifficulty, Status = Status, Title = Title, ExecutorID = SelectedExecutor.ID, Executor = SelectedExecutor.FirstName + " " + SelectedExecutor.MiddleName + " " + SelectedExecutor.LastName };
                            eo.AddTask(newTask);
                        } else
                        {
                            newTask = new Task { ID = thisTask.ID, Difficulty = TaskDifficulty, Status = Status, Title = Title, ExecutorID = SelectedExecutor.ID, Executor = SelectedExecutor.FirstName + " " + SelectedExecutor.MiddleName + " " + SelectedExecutor.LastName };
                            eo.EditTask(thisTask, newTask);
                        }
                        TasksView tasks = new TasksView(thisUser);
                        tasks.Show();
                        thisWindow.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }

        public ICommand deleteCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        eo.DeleteTask(thisTask);
                        TasksView tasks = new TasksView(thisUser);
                        tasks.Show();
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
