using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArbiterTown.Models
{
    public class UserProfileModel
    {
        public string username { get; set; }
        public int tasksCompleted { get; set; }
        public int tasksSuccesfull { get; set; }
        public int tasksskipped { get; set; }
        public float successRate { get; set; }
        public decimal bitcoinsEarned { get; set; }
    }
}