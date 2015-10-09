using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace responseTip.Models
{
    [RequireHttps]
    public class ResponseTipTask
    {
        public ResponseTipTask()
        {
            BitcoinPrice = 300000;
        }
        public int ResponseTipTaskID { get; set; }
        public string userName { get; set; }

        [Required()]
        [Display(Name = "Asked Question")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        [StringLength(30)]
        public string question { get; set; }
        public string answer { get; set; }
        [Required()]
        [StringLength(30)]
        public string twitterUserNameWritten { get; set; }
        public int twitterUserIdSelected { get; set; }
        public string twitterUserNameSelected { get; set; }
        public int ArbiterCount { get; set; }

        public string BitcoinPublicAdress { get; set; }

        public decimal BitcoinPrice { get; set; }
        public bool isQuestionPublic { get; set; }

        public DateTime timeCreated { get; set; }
        public DateTime timeQuestionAsked { get; set; }

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