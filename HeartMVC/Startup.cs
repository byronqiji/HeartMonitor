using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HeartMVC.Startup))]
namespace HeartMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
