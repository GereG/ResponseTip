using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ArbiterTown.Startup))]
namespace ArbiterTown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
