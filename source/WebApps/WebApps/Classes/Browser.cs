using System;
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

            string param = "?username=" + user.UserName + "&fullname=" + user.FullName + "&view=" + view;

            Uri newUri = new Uri(String.Format(file + param, baseDir));
            return newUri;
        }

        public BitmapFrame GetIcon( string view) {
            
            Uri iconUri = new Uri("pack://application:,,,/Images/Icons/"+view+".ico");
            return BitmapFrame.Create(iconUri);
        }

    }
}
