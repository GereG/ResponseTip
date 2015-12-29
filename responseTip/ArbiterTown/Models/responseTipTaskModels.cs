using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtcHandling;

namespace ArbiterTown.Models
{
    [RequireHttps]
    public class ResponseTipTask
    {
        public ResponseTipTask()
        {
            BitcoinPrice = 0.0005m;
            BitcoinReturnPublicAddress = "17xm46Mm8ZFGWKdqknF5QF3HLtFb2zd6fb";
            ArbiterCount = 5;
        }
        public int ResponseTipTaskID { get; set; }
        [StringLength(128)]
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
        [Required]
        [OddNumber]
        public int ArbiterCount { get; set; }

        [Required]
        [BtcAddress]
        public string BitcoinPublicAdress { get; set; }

        [Required]
        [BtcAddress]
        public string BitcoinReturnPublicAddress { get; set; }

        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal BitcoinPrice { get; set; }
        [Range(1, 20)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal DollarPrice { get; set; }
        public bool isQuestionPublic { get; set; }

        public long? questionTweetId { get; set; }
        public long? answerTweetId { get; set; }

        public DateTime timeCreated { get; set; }
        public DateTime timeQuestionAsked { get; set; }

        public TaskStatusesEnum taskStatus { get; set; }

        public virtual ICollection<TextAnswerValidationTask> TextAnswerValidationTasks { get; set; }


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
            currentStatus = TaskStatusesEnum.responseTip_created;

        }
    }

    public enum TaskStatusesEnum
    {
        responseTip_created = 0, responseTip_notPaid = 1, responseTip_notPaid_expired = 2, responseTip_paid = 3,
        responseTip_questionAsked = 4, responseTip_questionAsked_expired = 5, responseTip_questionAnswered = 6,
        responseTip_answerValid = 7, responseTip_allPaymentsSettled = 8, responseTip_completed = 9, responseTip_closed = 10
    }


}