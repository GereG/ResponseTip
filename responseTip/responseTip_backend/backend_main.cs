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
        private static ArbiterFinder arbiterFinder;
        private static Logger backendLogger = new Logger();
        private static ApplicationDbContext dbContext = new ApplicationDbContext();
        //        private static responseTipTaskContext responseTipDatabase = new responseTipTaskContext();





        static void Main(string[] args)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            backendLogger.SetPath(directoryPath);

            Startup.Configuration();

            backendLogger.LogLine("Backend_main configured...continuing", Logger.log_types.MESSAGE_LOG);

            arbiterFinder = new ArbiterFinder(dbContext);

            CleanTaskDatabase();

            //            decimal dollarPrice=externalAPIs.CallForBitcoinAverageDollarPrice();

            StateUpdateManager responseTipStateUpdateManager = new StateUpdateManager(typeof(TaskStatusesEnum));
            int responseTipStatesToUpdate = 0;

            StateUpdateManager textAnswerValidationUpdateManager = new StateUpdateManager(typeof(ArbiterTaskStatusesEnum));
            int textAnswerValidationStatesToUpdate = 0;

            while (true)//infinite loop
            {
                if (BtcHandlingClass.IsNextBlock())
                {
                    backendLogger.LogLine("New Block", Logger.log_types.MESSAGE_LOG);
                    responseTipStateUpdateManager.NewBlock();
                }

                BtcHandlingClass.UpdateKeyPool(ConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);

                responseTipStatesToUpdate = responseTipStateUpdateManager.StatesToUpdateNow();
                responseTipStatePusherCycle(responseTipStatesToUpdate);

                textAnswerValidationStatesToUpdate = textAnswerValidationUpdateManager.StatesToUpdateNow();
                textAnswerValidationStatePusherCycle(textAnswerValidationStatesToUpdate);
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
                        backendLogger.LogLine(line, Logger.log_types.MESSAGE_LOG);
                    }
                    else if (!BtcHandlingClass.IsAddressValidAndMine(task.BitcoinPublicAdress))
                    {
                        dbContext.ResponseTipTasks.Remove(task);
                        backendLogger.LogLine("Deleted Task" + ": " + task.ResponseTipTaskID + " because bitcoin address " + task.BitcoinPublicAdress + " is not valid or mine", Logger.log_types.WARNING_LOG);
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
                if (task != null)
                {
                    switch (task.taskStatus)
                    {
                        case ArbiterTaskStatusesEnum.textAnswerValidation_created:
                            responseTip.Bussines_logic.responseTipLogic.TextAnswerValidationCreated(task);

                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_waitingForAnswer:
                            bool assignNewArbiter=responseTip.Bussines_logic.responseTipLogic.TextAnswerValidation_WaitingForAnswer(task);
                            if(assignNewArbiter)
                            {
                                string[] arbiterID2 = new string[1];
                                try
                                {
                                    string[] excludedIds = task.parentTask.TextAnswerValidationTasks.Select(s => s.ApplicationUserId).ToArray();
                                    arbiterID2 = arbiterFinder.FindArbiters(1, excludedIds);

                                    TextAnswerValidationTask[] newArbiterTasks2 = responseTip.Bussines_logic.responseTipLogic.TaskQuestionAnswered_CreateArbiterTasks(task.parentTask, arbiterID);
                                    dbContext.TextAnswerValidationTasks.Add(newArbiterTasks2[0]);

                                }
                                catch (responseTip.Exceptions.NotEnoughArbitersAvailable notEnoughArbitersAvailable)
                                {
                                    //dont do anything just dont create another arbiter..we will have to wait
                                }
                            }
                            break;
//                        case ArbiterTaskStatusesEnum.textAnswerValidation_expired: //when its expired, we must create new one
                        case ArbiterTaskStatusesEnum.textAnswerValidation_skipped://and also when its skipped, we must create new one
                            string[] arbiterID=new string[1];

                            try
                            {
                                string[] excludedIds = task.parentTask.TextAnswerValidationTasks.Select(s => s.ApplicationUserId).ToArray();
                                arbiterID = arbiterFinder.FindArbiters(1, excludedIds);


                                TextAnswerValidationTask[] newArbiterTasks = responseTip.Bussines_logic.responseTipLogic.TaskQuestionAnswered_CreateArbiterTasks(task.parentTask, arbiterID);
                                dbContext.TextAnswerValidationTasks.Add(newArbiterTasks[0]);
                                           
                                task.taskStatus = ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsSkipped;
                                
                            }
                            catch (responseTip.Exceptions.NotEnoughArbitersAvailable notEnoughArbitersAvailable)
                            {
                                //TODO how to handle missing arbiters
                            }

                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_answered:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInAgreement:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_finishedInDisagreement:
                            break;
                        case ArbiterTaskStatusesEnum.textAnswerValidation_finishedAsSkipped:
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
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2))) //reads from mask if this state should be updated
                            {
                                responseTip.Bussines_logic.responseTipLogic.TaskCreated(task);

                            }
                            break;

                        case TaskStatusesEnum.responseTip_notPaid:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                responseTip.Bussines_logic.responseTipLogic.TaskNotPaid(task);
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
                                responseTip.Bussines_logic.responseTipLogic.TaskPaid(task);
                            }

                            break;

                        case TaskStatusesEnum.responseTip_questionAsked:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                responseTip.Bussines_logic.responseTipLogic.TaskQuestionAsked(task);
                            }

                            break;

                        case TaskStatusesEnum.responseTip_questionAsked_expired:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                responseTip.Bussines_logic.responseTipLogic.TaskQuestionAskedExpired(task);
                            }
                            break;

                        case TaskStatusesEnum.responseTip_questionAnswered:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                int numOfArbitersToAsk = (int)(task.ArbiterCount * 1.5f); //always ask half more people for the task. those that will be first can answer
                                if (task.TextAnswerValidationTasks.Count >= numOfArbitersToAsk)
                                {
                                    responseTip.Bussines_logic.responseTipLogic.TaskQuestionAnswered_CheckArbiterTasksStatus(task);
                                }
                                else // if count is less than needed then surely create new arbiter tasks
                                {
                                    //but if enough arbiters to resolve the parent task, then check arbiter tasks 
                                    if(task.TextAnswerValidationTasks.Count>=task.ArbiterCount)
                                    {
                                        responseTip.Bussines_logic.responseTipLogic.TaskQuestionAnswered_CheckArbiterTasksStatus(task);
                                    }
                                    
                                    string[] arbiterIDs=new string[numOfArbitersToAsk];

                                    try
                                    {
                                        arbiterIDs = arbiterFinder.FindArbiters(numOfArbitersToAsk, null);


                                        TextAnswerValidationTask[] newArbiterTasks = responseTip.Bussines_logic.responseTipLogic.TaskQuestionAnswered_CreateArbiterTasks(task, arbiterIDs);

                                        for (int i = 0; i < numOfArbitersToAsk; i++)
                                        {
                                            dbContext.TextAnswerValidationTasks.Add(newArbiterTasks[i]);
                                        }

                                    }
                                    catch (responseTip.Exceptions.NotEnoughArbitersAvailable notEnoughArbitersAvailable)
                                    {
                                        //if not enough arbiters then do nothing. wait for some to free up
                                    }
                                }
                            }
                            break;

                        case TaskStatusesEnum.responseTip_answerAfterEvalidation:
                            if (Convert.ToBoolean(((statesToUpdate >> (int)task.taskStatus) % 2)))
                            {
                                responseTip.Bussines_logic.responseTipLogic.TaskAnswerAfterEvalidation(task);
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
                        case TaskStatusesEnum.responseTip_closed:
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
    }
}
