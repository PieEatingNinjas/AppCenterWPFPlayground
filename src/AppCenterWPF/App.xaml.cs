using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;

namespace AppCenterWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SetupAppCenter();
            base.OnStartup(e);          
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            Application.Current.MainWindow.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Loaded -= MainWindow_Loaded;
            HandleAppCrash();
        }

        private bool ShouldProcessErrorReport(ErrorReport report)
        {
            if (report.Exception is ExecutionEngineException)
            {
                //As we return false, no reports for unhandled SystemExcpetions will be send to server
                return false;
            }
            //For other unhandled exceptions, we want to generate a report
            return true;
        }

        private void SetupAppCenter()
        {
            Crashes.ShouldProcessErrorReport = ShouldProcessErrorReport;

            Crashes.ShouldAwaitUserConfirmation = () => {
                // Build your own UI to ask for user consent here. SDK does not provide one by default.

                // Return true if you just built a UI for user consent and are waiting for user input on that custom UI, otherwise false.
                return true;
            };

            //Initializing AppCenter, getting the key from App.config
            AppCenter.Start(ConfigurationManager.AppSettings.Get("AppCenterKey"),
                   typeof(Analytics), typeof(Crashes));
        }

        private async Task HandleAppCrash()
        {
            //Check if the app previously crashed
            if(await Crashes.HasCrashedInLastSessionAsync())
            {
                ErrorReport crashReport = await Crashes.GetLastSessionCrashReportAsync();
                if (ShouldProcessErrorReport(crashReport))
                {
                    CrashReportWindow w = new CrashReportWindow();
                    w.ShowDialog();
                    Application.Current.MainWindow.Focus();
                }
            }
        }
    }
}
