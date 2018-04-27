using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TelemarketingManagement.Startup))]
namespace TelemarketingManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
