using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(DoAn_LapTrinhWeb.App_Start.Startup))]

namespace DoAn_LapTrinhWeb.App_Start
{
    public class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // need to add UserManager into owin, because this is used in cookie invalidation
           
        }
    }
}
