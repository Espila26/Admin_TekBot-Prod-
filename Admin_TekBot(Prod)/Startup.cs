using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Admin_TekBot_Prod_.Startup))]
namespace Admin_TekBot_Prod_
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
