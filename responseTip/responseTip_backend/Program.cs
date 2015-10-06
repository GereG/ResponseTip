using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using responseTip.Models;
using System.Data.Entity;

namespace responseTip_backend
{
    public class Program
    {
        private static responseTipContext responseTipDatabase = new responseTipContext();
        
        static void Main(string[] args)
        {
            //            await responseTipDatabase.ResponseTipTasks.ForEachAsync{
            ResponseTipTask task;

            for(int i=0;i<responseTipDatabase.ResponseTipTasks.Count();i++)
            {
                task = responseTipDatabase.ResponseTipTasks.Find(i);
//                task= responseTipDatabase.ResponseTipTasks.ForEachAsync

                if (task != null)
                {
                    Console.WriteLine("Line " + i + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameWritten);
                }
            }
            Console.ReadKey();

        }
    }
}
