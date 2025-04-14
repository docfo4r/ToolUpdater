using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            CheckForUpdate();

            labelVersion.Content = Assembly.GetExecutingAssembly().GetName().Version;
        }

        async Task CheckForUpdate()
        {
            Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            var (remoteVersion, downloadUrl) = await GitHubReleaseChecker.GetLatestReleaseAsync();

            if (remoteVersion > localVersion)
            {
                Process process = new Process();
                ProcessStartInfo psi = new ProcessStartInfo();

                psi.UseShellExecute = true;
                psi.FileName = System.IO.Path.Combine(AppContext.BaseDirectory, "Updater.exe");
                process.StartInfo = psi;

                process.Start();
                Environment.Exit(0);
            }
        }
    }
}