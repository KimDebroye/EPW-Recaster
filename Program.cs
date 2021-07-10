using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPW_Recaster
{
    static class Program
    {
        private static Mutex Mutex { get; set; } = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string appName = Application.ProductName.ToString();
            bool isNew;

            Mutex = new Mutex(true, appName, out isNew);

            if (!isNew)
            {
                // If app is already running: exit application.
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainGui());
        }
    }
}
