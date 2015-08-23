using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace responseTip.Models
{
    public class ResponseTipTask
    {
        public int ResponseTipTaskID { get; set; }
        public string userName { get; set; }

        [Required()]
        [Display(Name = "Asked Question")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        [StringLength(30)]
        public string question { get; set; }
        [Required()]
        public SocialSiteUsers socialSiteUser { get; set; }
        [Required()]
        public string BitcoinPublicAdress { get; set; }
        [Required()]
        public float BitcoinPrice { get; set; }
        [Required()]
        public bool isQuestionPublic { get; set; }
        
        public TaskStatusesEnum taskStatus { get; set; }


    }

    public class TaskStatuses
    {
        public TaskStatusesEnum currentStatus;

        private TaskStatuses()
        {
            currentStatus = TaskStatusesEnum.created;
        }
    }

    public enum TaskStatusesEnum
    {
        created, notPaid, notPaid_expired, paid, questionAsked, questionAsked_expired, QuestionAnswered, AnswerValid, allPaymentsSettled, completed
    }
}