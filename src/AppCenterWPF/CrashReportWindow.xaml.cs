using Microsoft.AppCenter;
using Microsoft.AppCenter.Crashes;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AppCenterWPF
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CrashReportWindow : Window
    {
        readonly CrashReportWindowViewModel vm;

        public CrashReportWindow()
        {
            vm = new CrashReportWindowViewModel();
            vm.IsDone += Vm_IsDone;
            this.DataContext = vm;
            InitializeComponent();
        }

        private void Vm_IsDone(object sender, EventArgs e)
        {
            vm.IsDone -= Vm_IsDone;
            Dispatcher.Invoke(() =>
            {
                Close();
            });
        }
    }
}
