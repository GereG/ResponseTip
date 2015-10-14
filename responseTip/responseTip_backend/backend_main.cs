using System;
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

namespace responseTip_backend
{
    public class backend_main
    {
        private static Logger backendLogger=new Logger();
        private static responseTipContext responseTipDatabase = new responseTipContext();

        private const double taskExpirationTime = 2; //time from creation of task (in days) after which task expires


        
        static void Main(string[] args)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            backendLogger.SetPath(directoryPath);

            Startup.Configuration();

            backendLogger.LogLine("Backend_main configured...continuing",Logger.log_types.MESSAGE_LOG);

            CleanTaskDatabase();

           
            while (true)//infinite loop
            {
                taskStatePusherCycle();
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
                    else if(!BtcHandlingClass.IsAdressValidAndMine(task.BitcoinPublicAdress))
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

        private static void taskStatePusherCycle()
        {
            bool ismodified = false;
            IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    switch (task.taskStatus)
                    {
                        case TaskStatusesEnum.created:
                            TaskCreated(task);
                            ismodified = true;
                            break;
                        case TaskStatusesEnum.notPaid:
                            TaskNotPaid(task);
                            ismodified = true;
                            break;
                        case TaskStatusesEnum.paid:
                            TaskPaid();

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
            decimal addressBalance=BtcHandling.BtcHandlingClass.CheckAdressBalance(task.BitcoinPublicAdress);
            if (addressBalance == task.BitcoinPrice)
            {
                task.taskStatus = TaskStatusesEnum.paid;
            }
            TimeSpan timeElapsedFromCreation= DateTime.Now.Subtract(task.timeCreated);
            if (timeElapsedFromCreation.TotalDays > taskExpirationTime)
            {
                task.taskStatus = TaskStatusesEnum.notPaid_expired;
            }
            else
            {
                task.taskStatus = TaskStatusesEnum.notPaid;
            }
        }

        private static void TaskPaid(ResponseTipTask task)
        {

        }


    }
}
