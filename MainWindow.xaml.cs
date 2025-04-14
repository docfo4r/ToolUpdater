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
            Loaded += MainWindow_Loaded;

            labelVersion.Content = Assembly.GetExecutingAssembly().GetName().Version;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await CheckForUpdate();
        }

        async Task CheckForUpdate()
        {
            Version localVersion = Assembly.GetExecutingAssembly().GetName().Version;

            try
            {
                var (remoteVersion, downloadUrl) = await GitHubReleaseChecker.GetLatestReleaseAsync();

                if (remoteVersion > localVersion)
                {
                    var updaterPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Updater.exe");

                    if (File.Exists(updaterPath))
                    {
                        var psi = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            FileName = updaterPath,
                            Arguments = $"\"{downloadUrl}\""
                        };

                        Process.Start(psi);
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        MessageBox.Show("Updater.exe not found!", "Update error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"Update check failed: {err.Message}", "Update Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}