using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace responseTip.Models
{
    public class TaskModel
    {
        public int userID { get; set; }
        public string question { get; set; }
        public SocialSiteUsers socialSiteUser { get; set; }
        public string BitcoinPublicAdress { get; set; }
        public float BitcoinPrice { get; set; }
        public bool isQuestionPublic { get; set; }
        public TaskStatuses taskStatus { get; set; }


    }

    public class TaskStatuses
    {
    }
}