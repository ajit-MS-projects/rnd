using Owin;

namespace KatanaSelfHostOnTopIis
{
    
    ////Startup class is searched by owin automatically in assembly or by startup attribute 
    /// on F5 is hosted on IIS with katana as middle ware
    /// with Host.exe is hosted as self host  one can use host.exe to self host or can also host in process as in KatanaSelfHostTest project
    /// http://www.asp.net/aspnet/overview/owin-and-katana/an-overview-of-project-katana
    public class Startup
    {
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