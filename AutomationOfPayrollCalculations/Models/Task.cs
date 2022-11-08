using System;
using System.Collections.Generic;
using System.Text;

namespace AutomationOfPayrollCalculations.Models
{
    public class Task
    {
        public int ID { get; set; }
        public int ExecutorID { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Executor { get; set; }
        public string Manager { get; set; }
        public int Difficulty { get; set; }
    }
}
