using System.IO;
using System.Windows;

namespace WebApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :  Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // gets current view
            string view = (Application.Current as App).WebView;

            // init browser
            Browser browser = new Browser(view);

            // sets the icon and title
            Icon = browser.GetIcon(view);
            Title = view;
            
        }
    }
}
