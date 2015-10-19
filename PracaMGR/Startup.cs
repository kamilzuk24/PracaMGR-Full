using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PracaMGR.Startup))]
namespace PracaMGR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
