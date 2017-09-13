using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SocietyManagement.Startup))]
namespace SocietyManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
