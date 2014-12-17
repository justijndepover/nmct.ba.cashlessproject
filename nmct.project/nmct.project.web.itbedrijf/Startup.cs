using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nmct.project.web.itbedrijf.Startup))]
namespace nmct.project.web.itbedrijf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
