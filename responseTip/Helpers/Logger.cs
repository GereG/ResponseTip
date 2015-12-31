using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responseTip.Helpers
{
    public class Logger
    {
        static string directoryPath= Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));

        public void SetPath(string path)
        {
            directoryPath = path;
        }

        public void LogLine(string lines, log_types type)
        {
            System.IO.StreamWriter log;
            Console.WriteLine(lines);
            string time = DateTime.Now.ToString();
            switch (type)
            {
                case log_types.ERROR_LOG:
                    log = new System.IO.StreamWriter(directoryPath + "\\error_log.log", true);

                    log.WriteLine(time + "\t" + type.ToString() + "\t" + lines);
                    Debug.WriteLine(time + "\t" + type.ToString() + "\t" + lines);
                    log.Close();
                    break;

                case log_types.WARNING_LOG:
                    log = new System.IO.StreamWriter(directoryPath + "\\warning_log.log", true);
                    log.WriteLine(time + "\t" + type.ToString() + "\t" + lines);
                    log.Close();
                    break;

                case log_types.MESSAGE_LOG:
                    log = new System.IO.StreamWriter(directoryPath + "\\message_log.log", true);
                    log.WriteLine(time + "\t" + type.ToString() + "\t" + lines);
                    log.Close();
                    break;

            }
            log = new System.IO.StreamWriter(directoryPath + "\\all_logs.log", true);
            log.WriteLine(time + "\t" +type.ToString()+"\t"+ lines);
            log.Close();

    
        }

        public enum log_types {ERROR_LOG=0,WARNING_LOG=1,MESSAGE_LOG=2 };
    
    }

    
}
