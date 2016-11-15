using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChoixRestau.Startup))]
namespace ChoixRestau
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
