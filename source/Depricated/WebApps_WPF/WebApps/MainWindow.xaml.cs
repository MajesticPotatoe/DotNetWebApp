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
            
            // init browser
            Browser browser = new Browser();
            
        }
    }
}
