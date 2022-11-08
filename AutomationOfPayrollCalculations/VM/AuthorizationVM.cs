using AutomationOfPayrollCalculations.Connection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace AutomationOfPayrollCalculations.VM
{
    class AuthorizationVM
    {
        private string login;
        public string Login 
        {
            get { return login; }
            set 
            {
                login = value;
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }

        Window thisWindow;

        AuthorizationQueries authQ = new AuthorizationQueries();

        public AuthorizationVM(Window window)
        {
            thisWindow = window;
        }

        public ICommand authCommand
        {
            get
            {
                return new RelayCommand((obj) =>
                {
                    try
                    {
                        if (!authQ.IsStringNotNull(login) || !authQ.IsStringNotNull(password)) MessageBox.Show("Вы оставили строку пустой", "Ошибка");
                        else
                        {
                            if (!authQ.IsUserExist(login)) MessageBox.Show("Такого пользователя не существует", "Ошибка");
                            else
                            {
                                if (!authQ.IsPasswordCorrect(login, password)) MessageBox.Show("Неправильный пароль", "Ошибка");
                                else
                                {
                                    TasksView tasks = new TasksView(authQ.GetUser(login));
                                    tasks.Show();
                                    thisWindow.Close();
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                });
            }
        }
    }
}
