using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessageQueueLog.Startup))]
namespace MessageQueueLog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
