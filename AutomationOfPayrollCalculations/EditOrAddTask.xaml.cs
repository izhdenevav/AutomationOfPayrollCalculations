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
using AutomationOfPayrollCalculations.Models;

namespace AutomationOfPayrollCalculations
{
    /// <summary>
    /// Логика взаимодействия для EditOrAddTask.xaml
    /// </summary>
    public partial class EditOrAddTask : Window
    {
        public EditOrAddTask(User user, Task task)
        {
            InitializeComponent();
            DataContext = new AutomationOfPayrollCalculations.VM.EditOrAddTaskVM(user, task, this);
        }
    }
}
