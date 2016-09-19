using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cfires.Tutor.WebApp.Startup))]
namespace Cfires.Tutor.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
