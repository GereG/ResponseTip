﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using responseTip.Models;
using System.Data.Entity;
using BtcHandling;
using TwitterHandling;
using responseTip.Helpers;
using System.IO;
using System.Configuration;

namespace responseTip_backend
{
    public class backend_main
    {
        private static Logger backendLogger=new Logger();
        private static responseTipContext responseTipDatabase = new responseTipContext();

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

            StateUpdateManager stateUpdateManager=new StateUpdateManager(typeof(TaskStatusesEnum));
           
            while (true)//infinite loop
            {
                if(BtcHandlingClass.IsNextBlock())
                {
                    backendLogger.LogLine("New Block",Logger.log_types.MESSAGE_LOG);
                    stateUpdateManager.NewBlock();
                }

                BtcHandlingClass.UpdateKeyPool(ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);

                int statesToUpdate= stateUpdateManager.StatesToUpdateNow();
                taskStatePusherCycle(statesToUpdate);
//                System.Threading.Thread.Sleep(5000);
            }
        }

        private static void CleanTaskDatabase()
        {
            IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    if (task.twitterUserNameSelected == null)
                    {
                        responseTipDatabase.ResponseTipTasks.Remove(task);
                        string line = "Deleted Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected;
                        backendLogger.LogLine(line,Logger.log_types.MESSAGE_LOG);
                    }
                    else if(!BtcHandlingClass.IsAddressValidAndMine(task.BitcoinPublicAdress))
                    {
                        responseTipDatabase.ResponseTipTasks.Remove(task);
                        backendLogger.LogLine("Deleted Task" + ": " + task.ResponseTipTaskID + " because bitcoin address "+task.BitcoinPublicAdress+" is not valid or mine", Logger.log_types.WARNING_LOG);
                    }
                    else
                    {
//                        string line = "Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected;
//                        backendLogger.LogLine(line, Logger.log_types.MESSAGE_LOG);
                    }

                }
            }
            responseTipDatabase.SaveChanges();
        }

        private static void taskStatePusherCycle(int statesToUpdate)
        {
            IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    switch (task.taskStatus)
                    {
                        case TaskStatusesEnum.created:
                            if (Convert.ToBoolean( ( (statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskCreated(task);

                            }
                            break;

                        case TaskStatusesEnum.notPaid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskNotPaid(task);
                            }
                            break;

                        case TaskStatusesEnum.notPaid_expired:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.paid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskPaid(task);
                            }

                            break;

                        case TaskStatusesEnum.questionAsked:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskQuestionAsked(task);
                            }

                            break;

                        case TaskStatusesEnum.questionAsked_expired:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                TaskQuestionAskedExpired(task);
                            }
                            break;

                        case TaskStatusesEnum.questionAnswered:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.answerValid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.allPaymentsSettled:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        case TaskStatusesEnum.completed:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {

                            }
                            break;

                        default:
                            
                            throw new responseTip.Exceptions.InvalidTaskStatus();
                    }

                }
            }
            responseTipDatabase.SaveChanges();
        }

        private static void TaskCreated(ResponseTipTask task)
        {
            task.taskStatus = TaskStatusesEnum.notPaid;
            
        }

        private static void TaskNotPaid(ResponseTipTask task)
        {
//            backendLogger.LogLine(""+task.ResponseTipTaskID, Logger.log_types.MESSAGE_LOG);
            decimal addressBalance=BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);

            if (addressBalance == task.BitcoinPrice)
            {
                task.taskStatus = TaskStatusesEnum.paid;
            }
            else if (addressBalance > task.BitcoinPrice + BtcHandlingClass.UpdateEstimatedTxFee()) //if amount higher than neccessary and difference higher than txfee --return it to return address
            {
                BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress,task.BitcoinReturnPublicAddress, addressBalance - task.BitcoinPrice,ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
                task.taskStatus = TaskStatusesEnum.paid;
            }
            else if (addressBalance > task.BitcoinPrice) //if amount higher than neccessary but difference lower than txfee --let it be
            {
                task.taskStatus = TaskStatusesEnum.paid;
            }
            else if (addressBalance< task.BitcoinPrice) // if its lower then check if not expired
            {
                TimeSpan timeElapsedFromCreation = DateTime.Now.Subtract(task.timeCreated);
                if (timeElapsedFromCreation.TotalDays > taskNotPaidExpirationTime)
                {
                    task.taskStatus = TaskStatusesEnum.notPaid_expired;
                }
            }

        }

        private static void TaskPaid(ResponseTipTask task)
        {
            task.questionTweetId=TwitterHandlingClass.PostATweetOnAWall(task.twitterUserNameSelected, task.question);
            task.taskStatus = TaskStatusesEnum.questionAsked;
            task.timeQuestionAsked = DateTime.Now;
        }

        private static void TaskQuestionAsked(ResponseTipTask task)
        {
            long answerTweetId=TwitterHandlingClass.CheckAnswerToQuestion((long)task.questionTweetId, task.twitterUserNameSelected);
            if(answerTweetId>0)
            {
                task.answerTweetId = answerTweetId;
                task.answer = TwitterHandlingClass.GetTweetAsString((long)task.answerTweetId);
                task.taskStatus = TaskStatusesEnum.questionAnswered;
                return;
            }

            TimeSpan timeElapsedFromQuestionAsked = DateTime.Now.Subtract(task.timeQuestionAsked);
            if (timeElapsedFromQuestionAsked.TotalDays > taskQuestionAskedExpirationTime)
            {
                task.taskStatus = TaskStatusesEnum.questionAsked_expired;
            }
        }

        private static void TaskQuestionAskedExpired(ResponseTipTask task)
        {
            decimal addressBalance = BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);
            BtcHandlingClass.SendFromAndToAddress(task.BitcoinPublicAdress,task.BitcoinReturnPublicAddress, addressBalance, ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
            task.taskStatus = TaskStatusesEnum.closed;
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
