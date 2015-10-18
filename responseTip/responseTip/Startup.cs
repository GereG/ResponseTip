using Microsoft.Owin;
using Owin;
//using responseTip.Bussines_logic;
using System.Web.Configuration;
using System.Diagnostics;

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
            BtcHandling.BtcHandlingClass.ConnectToRpc(WebConfigurationManager.AppSettings["Bitcoin_DaemonUrl_Testnet"],
                WebConfigurationManager.AppSettings["Bitcoin_RpcUsername"],
                WebConfigurationManager.AppSettings["Bitcoin_RpcPassword"],
                WebConfigurationManager.AppSettings["Bitcoin_WalletPassword"]);
            string address=BtcHandling.BtcHandlingClass.GetNewBtcAdress();
            Debug.WriteLine("adress: "+address);
        }
    }
}
