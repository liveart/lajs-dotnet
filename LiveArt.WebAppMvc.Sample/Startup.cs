using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LiveArt.WebAppMvc.Sample.Startup))]
namespace LiveArt.WebAppMvc.Sample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
