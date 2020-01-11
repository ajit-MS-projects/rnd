using System;
using System.Windows.Forms;

namespace ProductImportAdmin
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Service());
            //Application.Run(new TestImageImportSsisManager());
            //Application.Run(new TestImageDeleteManager());
            //Application.Run(new TestErrorIndexingManager());

        }
    }
}
