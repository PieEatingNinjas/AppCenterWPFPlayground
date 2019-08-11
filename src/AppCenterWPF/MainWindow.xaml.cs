using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace AppCenterWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TestCrashButton_Click(object sender, RoutedEventArgs e)
        {
            Crashes.GenerateTestCrash();
        }

        private void RealCrashButton_Click(object sender, RoutedEventArgs e)
        {
            DoCalc(0);
        }

        private int DoCalc(int divisor)
        {
            return 42 / divisor;
        }

        private void TriggerExecutionEngineException()
        {
                throw new ExecutionEngineException();
        }

        private void CathingAndLoggingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoCalc(0);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    { "UserId","1234" },
                    { "Culture", Thread.CurrentThread.CurrentCulture.Name },
                });

                MessageBox.Show("Ooops! Something went wrong!");
            }
        }

        private void RealCrash2Button_Click(object sender, RoutedEventArgs e)
        {
            TriggerExecutionEngineException();
        }
    }
}
