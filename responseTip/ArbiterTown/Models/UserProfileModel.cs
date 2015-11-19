using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public decimal bitcoinsEarned { get; set; }
        public ICollection<TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }

        public UserProfileModel(ApplicationUser user)
        {
            username = user.UserName;
            tasksCompleted = user.numOfPuzzlesAttemted;
            tasksSuccesfull = user.numOfPuzzlesSuccesfull;
            tasksskipped = user.numOfPuzzlesSkipped;
            if(tasksCompleted==0)
            {
                successRate = 1;
            }else
            {
                successRate = tasksSuccesfull / tasksCompleted;
            }
            bitcoinsEarned = user.bitcoinEarned;
            TextAnswerValidationTasks= Extensions.Clone(user.TextAnswerValidationTasks);
        }
    }

}