using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("ArbiterTownConfig", typeof(ArbiterTown.Startup))]
namespace ArbiterTown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
//            ArbiterTown.Models.TaskModelsContext taskModelContext = new Models.TaskModelsContext();
//            taskModelContext.Database.CreateIfNotExists();
        }
    }
}
