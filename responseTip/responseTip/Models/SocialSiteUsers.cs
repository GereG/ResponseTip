namespace responseTip.Models
{
    public class SocialSiteUsers
    {
        
        public int SocialSiteUsersID { get; set; }
        public string UserName { get; set; }
        public SupportedSocialSites socialSite;
    }
}