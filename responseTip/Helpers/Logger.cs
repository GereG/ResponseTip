using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace responseTip.Helpers
{
    public static class Logger
    {
        private static System.IO.StreamWriter[] logs;

        public static void InitiateLogs(string path)
        {
            if (logs == null) logs = new System.IO.StreamWriter[4];

            logs[0] = new System.IO.StreamWriter(path + "\\error_log.log", true);
            logs[1] = new System.IO.StreamWriter(path + "\\warning_log.log", true);
            logs[2] = new System.IO.StreamWriter(path + "\\message_log.log", true);
            logs[3] = new System.IO.StreamWriter(path + "\\all_logs.log", true);


        }

        public static void LogLine(string lines, log_types type)
        {
            switch (type)
            {
                case log_types.ERROR_LOG:
                    logs[0].WriteLine(lines);
                    break;
                case log_types.WARNING_LOG:
                    logs[1].WriteLine(lines);
                    break;
                case log_types.MESSAGE_LOG:
                    logs[2].WriteLine(lines);
                    break;
            }
            logs[3].WriteLine(lines);
        }

        public enum log_types {ERROR_LOG=0,WARNING_LOG=1,MESSAGE_LOG=2 };

        
    }

    
}
