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
            
            string host = System.Environment.MachineName;
            string url= @"http://www.fcp.biz";
            bool local = false;

            // call browser class 
            var browser = new Browser();

            //will point to hosted server Live/Dev Based on Computer Name (Namely RDP Farm)
            switch (view) {
                case "EPMS Portal":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web:3010"  : @"http://EPMS-Dev:3010" ;
                    if (local) { url = @"http://localhost:3010"; }
                    // url = @"http://google.com";
                    break;
                case "Ship Store":
                    url = (host == "EPMS-RDP2" || host == "EPMS-RDP3") ? @"http://EPMS-Web" : @"http://EPMS-Dev";
                    break;
            }

            browserWin.Navigate(browser.CreateURL(url));
            
            this.Icon = browser.GetIcon(view);
            this.Title = view;
        }
    }
}
