using CefSharp;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace WebApps
{
    static class Program
    {
        /// <summary>
        /// Mutex for single app instance
        /// </summary>
        private static Mutex mutex = null;

        public static string WebView = null;

        //DLL's Needed for mutex
        [DllImport("user32", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32")]
        static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32")]
        static extern bool OpenIcon(IntPtr hWnd);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            //if no parameters were passed, gracefully close
            if (args.Length == 0) {
                MessageBox.Show("Invalid Startup Parameter.", "Alert", MessageBoxButtons.OK);
                Environment.Exit(-2);
            }
            WebView = args[0].ToString();
            
            //Creates mutex instance and checks if it exists
            mutex = new Mutex(true, "WebApp-" + WebView, out bool createdNew);

            //if window already exists bring it to front and close this app
            if (!createdNew)
            {
                // Brings already running app to front and exits the application
                // MessageBox.Show("Invalid Startup Parameter.","Alert",  MessageBoxButton.OK, MessageBoxImage.Exclamation);
                ActivateOtherWindow();
                Environment.Exit(-2);
            }

            //For Windows 7 and above, best to include relevant app.manifest entries as well
            Cef.EnableHighDPISupport();

            var settings = new CefSettings
            {
                CachePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            // By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
            // settings.CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache");

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            var browser = new Browser(WebView);
            Application.Run(browser);

        }

        /// <summary>
        /// Activates the already open window
        /// </summary>
        private static void ActivateOtherWindow()
        {

            var other = FindWindow(null, WebView);
            if (other != IntPtr.Zero)
            {
                SetForegroundWindow(other);
                if (IsIconic(other))
                    OpenIcon(other);
            }
        }
    }
}
