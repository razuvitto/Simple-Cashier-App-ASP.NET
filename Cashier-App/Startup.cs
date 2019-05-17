using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cashier_App.Startup))]
namespace Cashier_App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
