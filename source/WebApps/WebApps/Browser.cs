using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace WebApps
{
    public partial class Browser : Form
    {
        public Browser(string view)
        {
            InitializeComponent();
            // Start the browser after initialize global component
            
            InitializeChromium(view);
            Text = view;
            Icon = GetIcon(view);
            WindowState = FormWindowState.Maximized;
        }

        public ChromiumWebBrowser chromeBrowser;

        public void InitializeChromium(string view)
        {
            chromeBrowser = new ChromiumWebBrowser(CreateURL(view));
            chromeBrowser.MenuHandler = new MyCustomMenuHandler();

            // Add it to the form and fill it to the form window.
            Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.LoadingStateChanged += OnLoadingStateChanged;
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            // do stuff like execute js
        }
        
        private void Browser_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        public Icon GetIcon(string view)
        {
            return Icon.ExtractAssociatedIcon("Images/Icons/" + view + ".ico");
        }

        public string CreateURL(string view)
        {
            // variable defaults for url
            string host = System.Environment.MachineName;
            string url = @"http://www.fcp.biz";
            string param = string.Empty;
            string curDir = Directory.GetCurrentDirectory();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            User user = GetCurrUser();
            bool local = false;

            //will point to hosted server Live/Dev Based on Computer Name (Namely RDP Farm)
            switch (view)
            {
                case "EPMS Portal":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web:3010" : @"http://EPMS-Dev:3010";
                    param = "?origin=epms&username=" + user.UserName;
                    if (local) { url = @"http://localhost:3010"; }
                    // url = @"http://google.com";
                    break;
                case "Ship Store":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web" : @"http://EPMS-Dev";
                    break;
            }

            string newUri = String.Format(url + param, baseDir);
            return newUri;
        }

        public User GetCurrUser()
        {
            //stored in case need user from the application level
            //string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            string username = Environment.UserName;
            string fullname = UserPrincipal.Current.DisplayName;

            User currUser = new User(username, fullname);
            return currUser;
        }
    }
}