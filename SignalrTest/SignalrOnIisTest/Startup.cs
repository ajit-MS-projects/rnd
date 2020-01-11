using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;

namespace SignalrOnIisTest
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}