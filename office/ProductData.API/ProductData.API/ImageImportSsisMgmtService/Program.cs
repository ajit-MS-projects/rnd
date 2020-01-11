using System.ServiceProcess;

namespace ImageImportSsisMgmtService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new AffiliImageImportSsisMgmtService()
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
