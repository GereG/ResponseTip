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
    public class backend_main
    {
        private static responseTipContext responseTipDatabase = new responseTipContext();
        
        static void Main(string[] args)
        {
            while (true)//infinite loop
            {
                
                IEnumerable<ResponseTipTask> enumerator = responseTipDatabase.ResponseTipTasks.AsEnumerable();
                foreach (ResponseTipTask task in enumerator)
                {
                    if (task != null)
                    {
                        Console.WriteLine("Task" + ": " + task.ResponseTipTaskID + "    " + task.twitterUserNameSelected);
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
