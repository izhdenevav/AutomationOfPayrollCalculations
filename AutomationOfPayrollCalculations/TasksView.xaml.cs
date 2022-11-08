using AutomationOfPayrollCalculations.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutomationOfPayrollCalculations
{
    /// <summary>
    /// Логика взаимодействия для TasksView.xaml
    /// </summary>
    public partial class TasksView : Window
    {
        public TasksView(User user)
        {
            InitializeComponent();
            DataContext = new AutomationOfPayrollCalculations.VM.TasksViewVM(this, user);
        }
    }
}
