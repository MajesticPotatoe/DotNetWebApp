using CefSharp;
using CefSharp.WinForms;
using System;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WebApps
{
    public class Browser
    {
        /// <summary>
        /// Gets main window to be able to access controls. This will only work on 1 Window app
        /// </summary>
        MainWindow MainView = Application.Current.Windows[0] as MainWindow;

        
        public Browser() {
            // gets current view
            string view = (Application.Current as App).WebView;

            // sets icon and title
            MainView.Icon = GetIcon(view);
            MainView.Title = view;

            // initializes cefsharp
            CefSettings settings = new CefSettings();
            settings.CachePath = "";
            Cef.Initialize(settings);

            // creates chrome browser and loads page
            ChromiumWebBrowser chromeBrowser = new ChromiumWebBrowser(CreateURL(view));
            chromeBrowser.LoadingStateChanged += OnLoadingStateChanged;

            //attaches browser to view
            MainView.browserWin.Child = chromeBrowser;
            
        }

        /// <summary>
        /// listens for page to be loaded
        /// </summary>
        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            if (!args.IsLoading)
            {
                // Page has finished loading, do whatever you want here
                /*MainView.mainGrid.Dispatcher.Invoke(() =>
                {
                    MainView.mainGrid.RowDefinitions[0].Height = new GridLength(0);
                    MainView.mainGrid.Children.RemoveAt(0);
                    MainView.mainGrid.RowDefinitions[1].Height = new GridLength(1.0, GridUnitType.Star);
                });*/
            }

        }

        /// <summary>
        /// get current user and constuct a user object
        /// </summary>
        /// <returns>New user object</returns>
        public User GetCurrUser()
        {

            //stored in case need user from the application level
            //string username = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
            string username = Environment.UserName;
            string fullname = UserPrincipal.Current.DisplayName;

            User currUser = new User(username, fullname);
            return currUser;
        }

        /// <summary>
        /// Creates a Uri for browser to navigate
        /// </summary>
        /// <param name="file">File targeted for URL</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates icon image for specific view
        /// </summary>
        /// <param name="view">View to retrieve icon</param>
        /// <returns></returns>
        public BitmapFrame GetIcon( string view) {
            Uri iconUri = new Uri("pack://application:,,,/Images/Icons/"+view+".ico");
            return BitmapFrame.Create(iconUri);
        }

    }
}
