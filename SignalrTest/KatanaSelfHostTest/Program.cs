using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace KatanaSelfHostTest
{
    ///http://www.asp.net/aspnet/overview/owin-and-katana/an-overview-of-project-katana old implmentation
    ///http://aspnet.codeplex.com/sourcecontrol/latest#Samples/Katana/CustomServer/MyApp/Program.cs

    class Program
    {
        static void Main(string[] args)
        {
            const string baseUrl = "http://localhost:5000/";

            using (WebApp.Start<Startup>(new StartOptions(baseUrl)))
            {
                Console.WriteLine("Press Enter to quit.");
                Console.ReadKey();
            }

        }
    }

    public class Startup
    {
        // Invoked once at startup to configure your application.
        public void Configuration(IAppBuilder app)
        {
            app.Run(context =>
            {
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Hello World!");
            });
        }
    }

}

