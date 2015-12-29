using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using responseTip.Helpers;

namespace ArbiterTown.Models
{
    public class UserProfileModel
    {
        public string username { get; set; }
        public int tasksCompleted { get; set; }
        public int tasksSuccesfull { get; set; }
        public int tasksskipped { get; set; }
        public float successRate { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal bitcoinsEarned { get; set; }
        public ICollection<TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }

        public UserProfileModel(ApplicationUser user)
        {
            username = user.UserName;
            tasksCompleted = user.GetNumOfPuzzlesAttemted();
            tasksSuccesfull = user.GetNumOfPuzzlesSuccesfull();
            tasksskipped = user.GetNumOfPuzzlesSkipped();
            successRate = user.GetPercentageOfPuzzlesSuccesfull();
            bitcoinsEarned = user.bitcoinEarned;
            TextAnswerValidationTasks= Extensions.Clone(user.TextAnswerValidationTasks);
        }
    }


}