using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SummerQuestion4.Startup))]
namespace SummerQuestion4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
