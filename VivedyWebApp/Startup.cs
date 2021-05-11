using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VivedyWebApp.Startup))]
namespace VivedyWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
