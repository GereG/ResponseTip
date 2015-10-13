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


        
        static void Main(string[] args)
        {
            string directoryPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            backendLogger.SetPath(directoryPath);

            Startup.Configuration();

            CleanTaskDatabase();

           
            while (true)//infinite loop
            {
//                TwitterHandling.TwitterHandlingClass.PublishTweet("connected");
                Console.ReadKey();
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
                    else
                    {
                        string line = "Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected;
 //                       Console.WriteLine(line);
                        backendLogger.LogLine(line, Logger.log_types.MESSAGE_LOG);
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
                            TaskCreated(task);
                            ismodified = true;
                            break;

                        default:
                            
                            throw new responseTip.Exceptions.InvalidTaskStatus();
                    }

                }
                responseTipDatabase.Entry(task).State = EntityState.Modified;
                responseTipDatabase.SaveChanges();
            }
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
            task.taskStatus = TaskStatusesEnum.notPaid;
        }


    }
}
