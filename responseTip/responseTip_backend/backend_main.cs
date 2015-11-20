using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArbiterTown.Models;
using System.Data.Entity;
using BtcHandling;
using TwitterHandling;
using responseTip.Helpers;
using System.IO;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace responseTip_backend
{
    public class backend_main
    {
        private static Logger backendLogger=new Logger();
        private static ApplicationDbContext dbContext=new ApplicationDbContext();
//        private static responseTipTaskContext responseTipDatabase = new responseTipTaskContext();

        private const double taskNotPaidExpirationTime = 2; //time from creation of task (in days) after which task expires
        private const double taskQuestionAskedExpirationTime = 15; //time from asking the question of task (in days) after which task expires and money is returned



        static void Main(string[] args)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            backendLogger.SetPath(directoryPath);

            Startup.Configuration();

            backendLogger.LogLine("Backend_main configured...continuing",Logger.log_types.MESSAGE_LOG);

            CleanTaskDatabase();

            //            decimal dollarPrice=externalAPIs.CallForBitcoinAverageDollarPrice();

            StateUpdateManager responseTipStateUpdateManager=new StateUpdateManager(typeof(TaskStatusesEnum));
            int responseTipStatesToUpdate = 0;

            StateUpdateManager textAnswerValidationUpdateManager = new StateUpdateManager(typeof(TextAnswerValidation_ArbiterAnswerEnum));
            int textAnswerValidationStatesToUpdate = 0;

            while (true)//infinite loop
            {
                if(BtcHandlingClass.IsNextBlock())
                {
                    backendLogger.LogLine("New Block",Logger.log_types.MESSAGE_LOG);
                    responseTipStateUpdateManager.NewBlock();
                }

                BtcHandlingClass.UpdateKeyPool(ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);

                responseTipStatesToUpdate= responseTipStateUpdateManager.StatesToUpdateNow();
                responseTipStatePusherCycle(responseTipStatesToUpdate);

                textAnswerValidationStatesToUpdate = textAnswerValidationUpdateManager.StatesToUpdateNow();
                
                //                System.Threading.Thread.Sleep(5000);
            }
        }

        private static void CleanTaskDatabase()
        {
            IEnumerable<ResponseTipTask> enumerator = dbContext.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    if (task.twitterUserNameSelected == null)
                    {
                        dbContext.ResponseTipTasks.Remove(task);
                        string line = "Deleted Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected;
                        backendLogger.LogLine(line,Logger.log_types.MESSAGE_LOG);
                    }
                    else if(!BtcHandlingClass.IsAddressValidAndMine(task.BitcoinPublicAdress))
                    {
                        dbContext.ResponseTipTasks.Remove(task);
                        backendLogger.LogLine("Deleted Task" + ": " + task.ResponseTipTaskID + " because bitcoin address "+task.BitcoinPublicAdress+" is not valid or mine", Logger.log_types.WARNING_LOG);
                    }
                    else
                    {
//                        string line = "Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected;
//                        backendLogger.LogLine(line, Logger.log_types.MESSAGE_LOG);
                    }

                }
            }
            dbContext.SaveChanges();
        }

        private static void textAnswerValidationStatePusherCycle(int statesToUpdate)
        {
            IEnumerable<TextAnswerValidationTask> enumerator = dbContext.TextAnswerValidationTasks.AsEnumerable();
            foreach (TextAnswerValidationTask task in enumerator)
            {
                if(task!= null)
                {
                    switch(task.taskStatus)
                    {
                        case ArbiterTaskStatusesEnum.textAnswerValidation_created:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_expired:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_answered:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInAgreement:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInDisagreement:
                            break;
                        default:
                            throw new responseTip.Exceptions.InvalidTaskStatus();
                    }
                }
            }
            dbContext.SaveChanges();
        }

        private static void responseTipStatePusherCycle(int statesToUpdate)
        {
            IEnumerable<ResponseTipTask> enumerator = dbContext.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    switch (task.taskStatus)
                    {
                        case TaskStatusesEnum.responseTip_created:
                            if (Convert.ToBoolean( ( (statesToUpdate >> (int)task.taskStatus) % 2))) //reads from mask if this state should be updated
                            {
                                TaskCreated(task);

                            }
                            break;

                        case TaskStatusesEnum.responseTip_notPaid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskNotPaid(task);
                            }
                            break;

                        case TaskStatusesEnum.responseTip_notPaid_expired:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.responseTip_paid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskPaid(task);
                            }

                            break;

                        case TaskStatusesEnum.responseTip_questionAsked:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskQuestionAsked(task);
                            }

                            break;

                        case TaskStatusesEnum.responseTip_questionAsked_expired:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskQuestionAskedExpired(task);
                            }
                            break;

                        case TaskStatusesEnum.responseTip_questionAnswered:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.responseTip_answerValid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.responseTip_allPaymentsSettled:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.responseTip_completed:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        default:
                            
                            throw new responseTip.Exceptions.InvalidTaskStatus();
                    }

                }
            }
            dbContext.SaveChanges();
        }

        private static void TaskCreated(ResponseTipTask task)
        {
            task.taskStatus = TaskStatusesEnum.responseTip_notPaid;
            
        }

        private static void TaskNotPaid(ResponseTipTask task)
        {
//            backendLogger.LogLine(""+task.ResponseTipTaskID, Logger.log_types.MESSAGE_LOG);
            decimal addressBalance=BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);

            if (addressBalance == task.BitcoinPrice)
            {
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance > task.BitcoinPrice + BtcHandlingClass.UpdateEstimatedTxFee()) //if amount higher than neccessary and difference higher than txfee --return it to return address
            {
                BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress,task.BitcoinReturnPublicAddress, addressBalance - task.BitcoinPrice,ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance > task.BitcoinPrice) //if amount higher than neccessary but difference lower than txfee --let it be
            {
                task.taskStatus = TaskStatusesEnum.responseTip_paid;
            }
            else if (addressBalance< task.BitcoinPrice) // if its lower then check if not expired
            {
                TimeSpan timeElapsedFromCreation = DateTime.Now.Subtract(task.timeCreated);
                if (timeElapsedFromCreation.TotalDays > taskNotPaidExpirationTime)
                {
                    task.taskStatus = TaskStatusesEnum.responseTip_notPaid_expired;
                }
            }

        }

        private static void TaskPaid(ResponseTipTask task)
        {
            task.questionTweetId=TwitterHandlingClass.PostATweetOnAWall(task.twitterUserNameSelected, task.question);
            task.taskStatus = TaskStatusesEnum.responseTip_questionAsked;
            task.timeQuestionAsked = DateTime.Now;
        }

        private static void TaskQuestionAsked(ResponseTipTask task)
        {
            long answerTweetId=TwitterHandlingClass.CheckAnswerToQuestion((long)task.questionTweetId, task.twitterUserNameSelected);
            if(answerTweetId>0)
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

        private static void TaskQuestionAskedExpired(ResponseTipTask task)
        {
            decimal addressBalance = BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);
            BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress,task.BitcoinReturnPublicAddress, addressBalance, ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
            task.taskStatus = TaskStatusesEnum.responseTip_closed;
        }

        private static void TaskQuestionAnswered(ResponseTipTask task)
        {

        }
        private static void TaskAnswerValid(ResponseTipTask task)
        {

        }
        private static void TaskAllPaymentsSettled(ResponseTipTask task)
        {

        }


       




    }
}
