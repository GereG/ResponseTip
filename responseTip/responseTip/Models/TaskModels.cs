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
            BitcoinPrice = 17.3f;
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
        public string twitterUserName { get; set; }
        public int ArbiterCount { get; set; }

        public string BitcoinPublicAdress { get; set; }

        public float BitcoinPrice { get; set; }
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