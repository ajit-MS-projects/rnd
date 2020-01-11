using System.ServiceProcess;

namespace ProductExportFileMgmtService
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
				new AffilinetExportFileMgmtService() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
