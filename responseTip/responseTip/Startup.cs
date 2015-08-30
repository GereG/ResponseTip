using Microsoft.Owin;
using Owin;
using responseTip.Bussines_logic;

[assembly: OwinStartupAttribute(typeof(responseTip.Startup))]
namespace responseTip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            TwitterHandling.twitterizerAuthentication("APItest2");
        }
    }
}
