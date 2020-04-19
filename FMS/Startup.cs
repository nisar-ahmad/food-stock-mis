using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FMS.Startup))]
namespace FMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
