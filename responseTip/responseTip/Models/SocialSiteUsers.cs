using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace responseTip.Models
{
    public class SocialSiteUsers
    {
        public SocialSiteUsers() { }

        public int SocialSiteUsersID { get; set; }
        public string UserName { get; set; }
 //       public SupportedSocialSites socialSite;
    }

/*    public class SupportedSocialSites
    {
        public SupportedSocialSitesEnum site;
    }

    public enum SupportedSocialSitesEnum
    {
        Twitter, Reddit
    }*/
}