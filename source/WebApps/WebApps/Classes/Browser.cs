﻿using System;
using System.Collections.Generic;
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

        public Browser(string view) {
            string host = System.Environment.MachineName;
            string url = @"http://www.fcp.biz";
            bool local = false;

            //will point to hosted server Live/Dev Based on Computer Name (Namely RDP Farm)
            switch (view)
            {
                case "EPMS Portal":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web:3010" : @"http://EPMS-Dev:3010";
                    if (local) { url = @"http://localhost:3010"; }
                    // url = @"http://google.com";
                    break;
                case "Ship Store":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web" : @"http://EPMS-Dev";
                    break;
            }
            
             MainView.browserWin.Navigate(CreateURL(url));
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
        public Uri CreateURL(string file)
        {
            string curDir = Directory.GetCurrentDirectory();
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            User user = GetCurrUser();

            string view = (Application.Current as App).WebView;

            string param = "?origin=epms&username=" + user.UserName;

            Uri newUri = new Uri(String.Format(file + param, baseDir));
            return newUri;
        }

        public BitmapFrame GetIcon( string view) {
            
            Uri iconUri = new Uri("pack://application:,,,/Images/Icons/"+view+".ico");
            return BitmapFrame.Create(iconUri);
        }

    }
}
