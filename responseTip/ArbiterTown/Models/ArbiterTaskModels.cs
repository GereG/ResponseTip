using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using responseTip.Helpers;


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
        public TimeSpan expirationTime { get; set; }
        [Required()]
        public TextAnswerValidation_ArbiterAnswerEnum arbiterAnswer { get; set; }
        [Required()]
        public ArbiterTaskStatusesEnum taskStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal taskPriceInDollars { get; set; }
        [DisplayFormat(DataFormatString = "{0:F8}", ApplyFormatInEditMode = true)]
        public decimal taskPriceInBitcoin { get; set; }

        public virtual ResponseTipTask parentTask { get; set; } 
        public virtual ApplicationUser assignedArbiter { get; set; }

/*        public TextAnswerValidationTask Clone(TextAnswerValidationTask task)
        {
            TextAnswerValidationTask clonedTask = new TextAnswerValidationTask();

            return clonedTask;
        }*/
        public TextAnswerValidationTask(string assignedArbiterID,int parrentResponseTipTaskId, TimeSpan expirationTimeInMinutes, decimal priceInDollars)
        {
            ApplicationUserId = assignedArbiterID;
            ResponseTipTaskID = parrentResponseTipTaskId;
            timeAssigned = DateTime.Now;
            expirationTime = expirationTimeInMinutes;
            taskPriceInDollars = priceInDollars;
            taskPriceInBitcoin = priceInDollars / externalAPIs.UpdateBitcoinAverageDollarPrice();
            taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_created;
            arbiterAnswer = TextAnswerValidation_ArbiterAnswerEnum.notAnswered;

        }

        public TextAnswerValidationTask()
        {
           
        }

        public object Clone()
        {
            TextAnswerValidationTask clonedTask = new TextAnswerValidationTask(this.ApplicationUserId,this.ResponseTipTaskID,this.expirationTime,this.taskPriceInDollars);
            clonedTask.arbiterAnswer = this.arbiterAnswer;
            clonedTask.taskPriceInBitcoin = this.taskPriceInBitcoin;
            clonedTask.taskStatus = this.taskStatus;
            clonedTask.timeAssigned = this.timeAssigned;
            clonedTask.expirationTime = this.expirationTime;

            return clonedTask;
        }
        /// <summary>
        /// close this task
        /// </summary>
        public void CloseThisTask_ParentTaskIsEvalidated()
        {
            switch(arbiterAnswer)
            {
                case TextAnswerValidation_ArbiterAnswerEnum.notAnswered:
                    assignedArbiter.DecrementNumOfPuzzlesWaiting();
                    taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsExpired;
                    break;
                case TextAnswerValidation_ArbiterAnswerEnum.skip:
                    taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsSkipped;
                    break;
                case TextAnswerValidation_ArbiterAnswerEnum.notValid:
                    switch(parentTask.answerValidation)
                    { 
                        case AnswerValidationEnum.responseTip_AnswerIsNotValid:
                            taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedInAgreement;
                            break;
                        case AnswerValidationEnum.responseTip_AnswerIsValid:
                            taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedInDisagreement;
                            break;
                    }
                    break;
                case TextAnswerValidation_ArbiterAnswerEnum.Valid:
                    switch (parentTask.answerValidation)
                    {
                        case AnswerValidationEnum.responseTip_AnswerIsNotValid:
                            taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedInDisagreement;
                            break;
                        case AnswerValidationEnum.responseTip_AnswerIsValid:
                            taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedInAgreement;
                            break;
                    }
                    break;
                default:
                    throw new responseTip.Exceptions.InvalidTaskStatus();
                    break;
            }
/*            switch(taskStatus)
            {
                //if closed already then do nothing
                case ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsExpired:
                case ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsSkipped:
                case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInAgreement:
                case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInDisagreement:
                    return;
                    break;
                case ArbiterTaskStatusesEnum.textAnswerValidation_waitingForAnswer:

                case ArbiterTaskStatusesEnum.textAnswerValidation_expired:
                case ArbiterTaskStatusesEnum.textAnswerValidation_skipped:
                case ArbiterTaskStatusesEnum.textAnswerValidation_created:
                case ArbiterTaskStatusesEnum.textAnswerValidation_answered:
                    break;
            }*/
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
        notValid=0, Valid=1, skip=2, notAnswered=3
    }

    public enum ArbiterTaskStatusesEnum
    {
        textAnswerValidation_created=0, textAnswerValidation_waitingForAnswer=1, textAnswerValidation_expired=2, textAnswerValidation_answered=3, textAnswerValidation_skipped = 4, textAnswerValidation_finishedInAgreement =5,
        textAnswerValidation_finishedInDisagreement=6, textAnswerValidation_finishedAsSkipped = 7, textAnswerValidation_finishedAsExpired = 8
    }
}