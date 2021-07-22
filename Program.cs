using System;
using System.Threading;
using System.Windows.Forms;

namespace EPW_Recaster
{
    internal static class Program
    {
        private static Mutex Mutex { get; set; } = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
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