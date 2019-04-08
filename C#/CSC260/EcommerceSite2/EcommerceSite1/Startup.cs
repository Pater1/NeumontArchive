using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EcommerceSite1.Startup))]
namespace EcommerceSite1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
