using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcHandling;

namespace responseTip.Models
{
    [RequireHttps]
    public class ResponseTipTask
    {
        public ResponseTipTask()
        {
            BitcoinPrice = 0.0005m;
            BitcoinReturnPublicAddress = "17xm46Mm8ZFGWKdqknF5QF3HLtFb2zd6fb";
        }
        public int ResponseTipTaskID { get; set; }
        public string userName { get; set; }

        [Required()]
        [Display(Name = "Asked Question")]
        //[RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        [StringLength(137)]
        public string question { get; set; }
        public string answer { get; set; }
        [Required()]
        [StringLength(30)]
        public string twitterUserNameWritten { get; set; }
        public int twitterUserIdSelected { get; set; }
        public string twitterUserNameSelected { get; set; }
        public int ArbiterCount { get; set; }

        [Required]
        [BtcAddress]
        public string BitcoinPublicAdress { get; set; }

        [Required]
        [BtcAddress] public string BitcoinReturnPublicAddress { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode =true)]
        public decimal BitcoinPrice { get; set; }
        [Range(1,20)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal DollarPrice { get; set; }
        public bool isQuestionPublic { get; set; }

        public DateTime timeCreated { get; set; }
        public DateTime timeQuestionAsked { get; set; }

        public TaskStatusesEnum taskStatus { get; set; }

/*        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            bool isReturnAddressValid = BtcHandling.BtcHandlingClass.IsAddressValid(this.BitcoinReturnPublicAddress);
            if (!isReturnAddressValid)
            {
                yield return new ValidationResult("This is not valid bitcoin address.", new[] { "BitcoinReturnPublicAddress" });
            }
        }*/

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
        created, notPaid, notPaid_expired, paid, questionAsked, questionAsked_expired, QuestionAnswered, AnswerValid, allPaymentsSettled, completed, closed
    }

    
}