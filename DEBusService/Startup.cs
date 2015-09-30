using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DEBusService.Startup))]
namespace DEBusService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
