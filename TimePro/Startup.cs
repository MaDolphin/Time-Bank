using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TimePro.Startup))]
namespace TimePro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
