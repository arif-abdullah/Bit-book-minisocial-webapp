using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BitBookWebApplication.Startup))]
namespace BitBookWebApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
