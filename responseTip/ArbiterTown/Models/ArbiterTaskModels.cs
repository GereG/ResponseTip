using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ArbiterTown.Models
{
    public class ImageEvaluationTask
    {
        public int id { get; set; }
        public byte[] image1 { get; set; }
        public byte[] image2 { get; set; }
        public int chosenImage { get; set; }
    }

    public class TextAnswerValidationTask : ICloneable
    {
        public int id { get; set; }
        [Required()]
        //        [StringLength(128)]
        //        public string AspNetUsersId { get; set; }
        [StringLength(128)]
        public string ApplicationUserId { get; set; }
        [Required()]
        public int ResponseTipTaskID { get; set; }
        [Required()]
        public DateTime timeAssigned { get; set; }
        [Required()]
        public TimeSpan timeBeforeExpired { get; set; }
        [Required()]
        public TextAnswerValidation_ArbiterAnswerEnum arbiterAnswer { get; set; }
        [Required()]
        public ArbiterTaskStatusesEnum taskStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal taskPriceInDollars { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal taskPriceInBitcoin { get; set; }

/*        public TextAnswerValidationTask Clone(TextAnswerValidationTask task)
        {
            TextAnswerValidationTask clonedTask = new TextAnswerValidationTask();

            return clonedTask;
        }*/

        public object Clone()
        {
            TextAnswerValidationTask clonedTask = new TextAnswerValidationTask();
            clonedTask.ApplicationUserId = this.ApplicationUserId;
            clonedTask.arbiterAnswer = this.arbiterAnswer;
            clonedTask.id = this.id;
            clonedTask.ResponseTipTaskID = this.ResponseTipTaskID;
            clonedTask.taskPriceInBitcoin = this.taskPriceInBitcoin;
            clonedTask.taskPriceInDollars = this.taskPriceInDollars;
            clonedTask.taskStatus = this.taskStatus;
            clonedTask.timeAssigned = this.timeAssigned;
            clonedTask.timeBeforeExpired = this.timeBeforeExpired;

            return clonedTask;
        }
    }

    public class TextAnswerValidationPresented
    {
        public int? textAnswerValidationTaskId { get; set; }
            [Required()]
        public TextAnswerValidation_ArbiterAnswerEnum arbiterAnswer { get; set; }
        public string question { get; }
        public string answer { get; }
        public decimal taskPriceInBitcoin { get; }

        public TextAnswerValidationPresented(int? taskID,string newQuestion,string newAnswer,decimal newTaskpriceinBTC)
        {
            textAnswerValidationTaskId = taskID;
            question = newQuestion;
            answer = newAnswer;
            taskPriceInBitcoin = newTaskpriceinBTC;
        }

        public TextAnswerValidationPresented()
        {

        }
    }

    public enum TextAnswerValidation_ArbiterAnswerEnum
    {
        notValid=0, Valid=1, skip=2
    }

    public enum ArbiterTaskStatusesEnum
    {
        created=0, expired=1, answered=2, finishedInAgreement=3,finishedInDisagreement=4
    }
}