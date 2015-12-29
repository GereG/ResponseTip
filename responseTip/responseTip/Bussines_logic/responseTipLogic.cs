using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArbiterTown.Models;
using BtcHandling;
using System.Configuration;
using TwitterHandling;
using responseTip.Helpers;


namespace responseTip.Bussines_logic
{
	public class responseTipLogic
	{
        private const double taskNotPaidExpirationTime = 2; //time from creation of task (in days) after which task expires
        private const double taskQuestionAskedExpirationTime = 15; //time from asking the question of task (in days) after which task expires and money is returned
        private static TimeSpan taskForArbiterExpirationTimeInMinutes = TimeSpan.FromMinutes(60);
        private const decimal taskForArbiterPriceInDollar = (decimal)0.01;

        public static void TaskCreated(ResponseTipTask task)
        {
            task.taskStatus = TaskStatusesEnum.responseTip_notPaid;

        }

        public static void TaskNotPaid(ResponseTipTask task)
        {
            //            backendLogger.LogLine(""+task.ResponseTipTaskID, Logger.log_types.MESSAGE_LOG);
            decimal addressBalance = BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);

            if (addressBalance == task.BitcoinPrice)
            {
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance > task.BitcoinPrice + BtcHandlingClass.UpdateEstimatedTxFee()) //if amount higher than neccessary and difference higher than txfee --return it to return address
            {
                BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress, task.BitcoinReturnPublicAddress, addressBalance - task.BitcoinPrice, ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance > task.BitcoinPrice) //if amount higher than neccessary but difference lower than txfee --let it be
            {
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance < task.BitcoinPrice) // if its lower then check if not expired
            {
                TimeSpan timeElapsedFromCreation = DateTime.Now.Subtract(task.timeCreated);
                if (timeElapsedFromCreation.TotalDays > taskNotPaidExpirationTime)
                {
                    task.taskStatus = TaskStatusesEnum.responseTip_notPaid_expired;
                }
            }

        }

        public static void TaskPaid(ResponseTipTask task)
        {
            task.questionTweetId = TwitterHandlingClass.PostATweetOnAWall(task.twitterUserNameSelected, task.question);
            task.taskStatus = TaskStatusesEnum.responseTip_questionAsked;
            task.timeQuestionAsked = DateTime.Now;
        }

        public static void TaskQuestionAsked(ResponseTipTask task)
        {
            long answerTweetId = TwitterHandlingClass.CheckAnswerToQuestion((long)task.questionTweetId, task.twitterUserNameSelected);
            if (answerTweetId > 0)
            {
                task.answerTweetId = answerTweetId;
                task.answer = TwitterHandlingClass.GetTweetAsString((long)task.answerTweetId);
                task.taskStatus = TaskStatusesEnum.responseTip_questionAnswered;
                return;
            }

            TimeSpan timeElapsedFromQuestionAsked = DateTime.Now.Subtract(task.timeQuestionAsked);
            if (timeElapsedFromQuestionAsked.TotalDays > taskQuestionAskedExpirationTime)
            {
                task.taskStatus = TaskStatusesEnum.responseTip_questionAsked_expired;
            }
        }

        public static void TaskQuestionAskedExpired(ResponseTipTask task)
        {
            decimal addressBalance = BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);
            BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress, task.BitcoinReturnPublicAddress, addressBalance, ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
            task.taskStatus = TaskStatusesEnum.responseTip_closed;
        }

        public static TextAnswerValidationTask TaskQuestionAnswered(ResponseTipTask task)
        {
            TextAnswerValidationTask validationTask = new TextAnswerValidationTask();
            validationTask.ResponseTipTaskID = task.ResponseTipTaskID;
            validationTask.timeAssigned = DateTime.Now;
            validationTask.expirationTime = taskForArbiterExpirationTimeInMinutes;
            validationTask.taskPriceInDollars = taskForArbiterPriceInDollar;
            validationTask.taskPriceInBitcoin = validationTask.taskPriceInDollars / externalAPIs.UpdateBitcoinAverageDollarPrice();
            validationTask.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_created;
            validationTask.arbiterAnswer = TextAnswerValidation_ArbiterAnswerEnum.notAnswered;
            //TODO find Arbiters for the job ApplicationUserID

            return validationTask;


        }
        public static void TaskAnswerValid(ResponseTipTask task)
        {
            //TODO 
        }
        public static void TaskAllPaymentsSettled(ResponseTipTask task)
        {
            //TODO
        }
    }
}