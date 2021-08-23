using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigWinBigHelp.Startup))]
namespace BigWinBigHelp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
