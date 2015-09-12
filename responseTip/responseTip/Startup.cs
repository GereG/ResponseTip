using Microsoft.Owin;
using Owin;
using responseTip.Bussines_logic;
using System.Web.Configuration;

[assembly: OwinStartupAttribute(typeof(responseTip.Startup))]
namespace responseTip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //            TwitterHandling.twitterAuthentication();
            //            TwitterHandling.PublishTweet("This is new:)");
            TwitterHandling.TwitterHandlingClass.twitterAuthentication(WebConfigurationManager.AppSettings["Twitter_ConsumerKey"],
                    WebConfigurationManager.AppSettings["Twitter_ConsumerSecret"],
                    WebConfigurationManager.AppSettings["Twitter_AccessToken"],
                    WebConfigurationManager.AppSettings["Twitter_AccessTokenSecret"]);

            //            TwitterHandling.TwitterHandlingClass.PublishTweet("separate project");
//            TwitterHandling.TwitterHandlingClass.SearchUsersM("Macek");
        }
    }
}
