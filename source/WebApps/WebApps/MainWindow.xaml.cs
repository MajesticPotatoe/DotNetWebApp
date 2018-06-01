using System.IO;
using System.Windows;

namespace WebApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // gets current view
            string view = (Application.Current as App).WebView;

            string path = view.Replace(' ', '_').ToLower();

            // call browser class 
            var browser = new Browser();
            browserWin.Navigate(browser.CreateURL(Path.Combine(@"file://{0}", @"../../../../Web/apps/" + path + "/index.html")));
            
            //relative pathing v.1
            //browserWin.Navigate(browser.CreateURL(@"file://{0}/"+path+"/index.html"));
            
            this.Icon = browser.GetIcon(view);
            this.Title = view;
        }
    }
}
