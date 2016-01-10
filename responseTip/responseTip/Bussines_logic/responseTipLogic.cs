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
        private static TimeSpan taskForArbiterExpirationTime = TimeSpan.FromMinutes(60);
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

        public static TextAnswerValidationTask[] TaskQuestionAnswered_CreateArbiterTasks(ResponseTipTask task,string[] arbiterIDs)
        {
            //if parent task is answered then dont add new arbiter tasks
            if(task.answerValidation!=AnswerValidationEnum.responseTip_notValidated)
            {
                return new TextAnswerValidationTask[0];
            }


            TextAnswerValidationTask[] newArbiterTasks= new TextAnswerValidationTask[arbiterIDs.Count()];
            for (int i = 0; i < arbiterIDs.Count(); i++)
            {
                newArbiterTasks[i] = new TextAnswerValidationTask(arbiterIDs[i], task.ResponseTipTaskID, taskForArbiterExpirationTime, taskForArbiterPriceInDollar);
            }

            return newArbiterTasks;
        }

        public static void TaskQuestionAnswered_CheckArbiterTasksStatus(ResponseTipTask task)
        {
            int yesVotes = 0;
            int noVotes = 0;
            int skipVotes = 0;

            IEnumerable<TextAnswerValidationTask> textAnswerValidationTaskEnumerator = task.TextAnswerValidationTasks.AsEnumerable();
            foreach (TextAnswerValidationTask arbiterTask in textAnswerValidationTaskEnumerator)
            {
                switch (arbiterTask.arbiterAnswer)
                {
                    case TextAnswerValidation_ArbiterAnswerEnum.notValid:
                        noVotes++;
                        break;
                    case TextAnswerValidation_ArbiterAnswerEnum.Valid:
                        yesVotes++;
                        break;
                    case TextAnswerValidation_ArbiterAnswerEnum.skip:
                        skipVotes++;
                        break;
                }
                if (yesVotes + noVotes == task.ArbiterCount)
                {
                    if (yesVotes > noVotes)
                    {
                        task.answerValidation = AnswerValidationEnum.responseTip_AnswerIsValid;
                        task.taskStatus = TaskStatusesEnum.responseTip_answerAfterEvalidation;
                    }
                    else if (yesVotes < noVotes)
                    {
                        task.answerValidation = AnswerValidationEnum.responseTip_AnswerIsNotValid;
                        task.taskStatus = TaskStatusesEnum.responseTip_answerAfterEvalidation;
                    }
                    else // if Arbiter answer is 50:50 than answer is not Valid
                    {
                        task.answerValidation = AnswerValidationEnum.responseTip_AnswerIsNotValid;
                        task.taskStatus = TaskStatusesEnum.responseTip_answerAfterEvalidation;
                    }

                    CloseAllPendingArbiterTasks_ParentTaskEvalidated(task);
                    //foreach(TextAnswerValidationTask arbiterTask2 in textAnswerValidationTaskEnumerator)
                    //{
                    //    if(task.taskStatus==TaskStatusesEnum.responseTip_answerValid)
                    //    {
                    //        if(arbiterTask2.arbiterAnswer==TextAnswerValidation_ArbiterAnswerEnum.Valid)
                    //        {
                    //            dbContext.Users.Find(arbiterTask2.ApplicationUserId).IncrementNumOfPuzzlesSuccesfull();
                    //        }else if(arbiterTask2.arbiterAnswer == TextAnswerValidation_ArbiterAnswerEnum.notValid)
                    //        {

                    //        }
                    //    }

                    //}

                    break;

                }
            }

            return;
        }

        public static void CloseAllPendingArbiterTasks_ParentTaskEvalidated(ResponseTipTask task)
        {
            foreach(TextAnswerValidationTask arbiterTask in task.TextAnswerValidationTasks)
            {
                arbiterTask.CloseThisTask_ParentTaskIsEvalidated();
            }
        }

        /// <summary>
        /// will settle all payments
        /// </summary>
        /// <param name="task"></param>
        public static void TaskAnswerAfterEvalidation(ResponseTipTask task)
        {
            //close all pending Arbiter Tasks 
        }
        public static void TaskAllPaymentsSettled(ResponseTipTask task)
        {
            //TODO
        }


        //Arbiter methods

        public static void TextAnswerValidationCreated(TextAnswerValidationTask task)
        {
            task.assignedArbiter.IncrementNumOfPuzzlesWaiting();
            task.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_waitingForAnswer;
        }

        /// <summary>
        /// checks if task is answered. if expiration time is passed "true" is returned and new arbiter will be added for the task..
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static bool TextAnswerValidation_WaitingForAnswer(TextAnswerValidationTask task)
        {
            
            switch (task.arbiterAnswer)
            {
                case TextAnswerValidation_ArbiterAnswerEnum.notValid:
                case TextAnswerValidation_ArbiterAnswerEnum.Valid:
                    task.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_answered;
                    task.assignedArbiter.DecrementNumOfPuzzlesWaiting();
                    return false;

                case TextAnswerValidation_ArbiterAnswerEnum.skip:
                    task.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsSkipped;
                    task.assignedArbiter.IncrementNumOfPuzzlesSkipped();
                    task.assignedArbiter.DecrementNumOfPuzzlesWaiting();
                    return false;
                default:
                    break;
            }
            if (DateTime.Now.CompareTo(task.timeAssigned.AddSeconds(task.expirationTime.TotalSeconds)) > 0)
            {
                task.expirationTime.Add(taskForArbiterExpirationTime);
                //                task.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_expired;
                //                task.assignedArbiter.IncrementNumOfPuzzlesExpired();
                //                task.assignedArbiter.DecrementNumOfPuzzlesWaiting();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}