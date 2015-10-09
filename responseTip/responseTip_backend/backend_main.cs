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

namespace responseTip_backend
{
    public class backend_main
    {
        private static responseTipContext responseTipDatabase = new responseTipContext();
        
        static void Main(string[] args)
        {
            while (true)//infinite loop
            {
//                TwitterHandling.TwitterHandlingClass.PublishTweet("connected");
                IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
                foreach (ResponseTipTask task in enumerator)
                {
                    if (task != null)
                    {
                        if (task.twitterUserNameSelected == null)
                        {
                            responseTipDatabase.ResponseTipTasks.Remove(task);
                        }
                        else
                        {
                            Console.WriteLine("Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected);
                        }
                        
                    }
                }
                Console.ReadKey();
            }
        }

        private void CleanTaskDatabase()
        {
            IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
            foreach (ResponseTipTask task in enumerator)
            {
                if (task != null)
                {
                    if (task.twitterUserNameSelected == null)
                    {
                        responseTipDatabase.ResponseTipTasks.Remove(task);
                        Console.WriteLine("Deleted Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected);
                    }
                    else
                    {
                        Console.WriteLine("Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected);
                    }

                }
            }
        }


    }
}
