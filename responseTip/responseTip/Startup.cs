using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(responseTip.Startup))]
namespace responseTip
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
