using System;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
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
            chromeBrowser = new ChromiumWebBrowser(CreateURL(view))
            {
                MenuHandler = new MyCustomMenuHandler()
            };

            // Add it to the form and fill it to the form window.
            Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
            chromeBrowser.LoadingStateChanged += OnLoadingStateChanged;
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            // do stuff like execute js
            if (args.IsLoading == false)
            {
                User currUser = GetCurrUser();
                string script = string.Format("" +
                    "var user_name = document.getElementsByName('user_name')[0];" +
                    "user_name.value = 'epms';" +
                    "user_name.dispatchEvent(new Event('input'));" +
                    "var password = document.getElementsByName('password')[0];" +
                    "password.value = '" + currUser.Password + "|" +currUser.UserName + "';" +
                    "password.dispatchEvent(new Event('input'));" +
                    "");
                chromeBrowser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }
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
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            bool local = false;

            //will point to hosted server Live/Dev Based on Computer Name (Namely RDP Farm)
            switch (view)
            {
                case "EPMS Portal":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3" || host == "EPMS-RDP-Training") ? @"http://EPMS-Web:3010" : @"http://EPMS-Dev:3010";
                    param = "";
                    if (local) { url = @"http://localhost:3010"; }
                    break;
                case "Ship Store":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web/" : @"http://EPMS-Dev/";
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
            string secretkey = "ys7we0cn";

            User currUser = new User(username, fullname, secretkey);
            return currUser;
        }
    }
}