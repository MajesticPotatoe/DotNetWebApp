using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;

namespace WebApps
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Web View Parameter to Pass to browser logic
        /// </summary>
        public string WebView = null;

        /// <summary>
        /// Mutex unique to webview
        /// </summary>
        private static Mutex _mutex = null;

        //DLL's Needed for 
        [DllImport("user32", CharSet = CharSet.Unicode)]
        static extern IntPtr FindWindow(string cls, string win);
        [DllImport("user32")]
        static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32")]
        static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32")]
        static extern bool OpenIcon(IntPtr hWnd);

        /// <summary>
        /// Grabs first startup parameter and sets it to WebView
        /// </summary>
        /// <param name="e">Startup event arguments</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            //Set webview to first startup parameter
            WebView = e.Args[0].ToString();
            string appName = "TestApp-" + this.WebView;
            bool createdNew;

            //Creates mutex instance and checks if it exists
            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                //Brings already running app to front and exits the application
                //MessageBox.Show("Another Instance of the application is already running.","Alert",  MessageBoxButton.OK, MessageBoxImage.Exclamation);
                ActivateOtherWindow();
                Environment.Exit(-2);
                //Application.Current.Shutdown();
            }

            //base.OnStartup(e);
        }

        /// <summary>
        /// Releases mutex on exit
        /// </summary>
        /// <param name="e">Exit event arguments</param>
        protected override void OnExit(ExitEventArgs e)
        {
            //Releases mutex on exit
            if (_mutex != null)
                _mutex.ReleaseMutex();
            base.OnExit(e);
        }

        /// <summary>
        /// Activates the already open window
        /// </summary>
        private void ActivateOtherWindow()
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
