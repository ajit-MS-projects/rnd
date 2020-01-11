using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace SignalrTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:8059";
            WebApp.Start(url);
            Console.ReadLine();
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}

