using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AppCenterWPF
{
    public class CrashReportWindowViewModel : INotifyPropertyChanged
    { 
        ICommand _userConfirmationSelectedCommand = null;
        public ICommand UserConfirmationSelectedCommand
        {
            get => _userConfirmationSelectedCommand ?? (_userConfirmationSelectedCommand = new SimpleCommand(OnUserConfirmationSelected));
        }

        public bool IsWaitingForUserInput
        {
            get => !IsSending && !IsDoneSending;
        }

        private bool _isSending;

        public bool IsSending
        {
            get { return _isSending; }
            set { _isSending = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsWaitingForUserInput)); }
        }

        private bool _isDoneSending;

        public bool IsDoneSending
        {
            get { return _isDoneSending; }
            set { _isDoneSending = value; RaisePropertyChanged(); RaisePropertyChanged(nameof(IsWaitingForUserInput)); }
        }

        private string _additionalInput;

        public string AdditionalInput
        {
            get { return _additionalInput; }
            set { _additionalInput = value; }
        }

        public CrashReportWindowViewModel()
        {
            Crashes.SendingErrorReport += Crashes_SendingErrorReport;
            Crashes.SentErrorReport += Crashes_SentErrorReport;
            Crashes.FailedToSendErrorReport += Crashes_FailedToSendErrorReport;
            Crashes.GetErrorAttachments += Crashes_GetErrorAttachments;
        }

        private IEnumerable<ErrorAttachmentLog> Crashes_GetErrorAttachments(ErrorReport report)
        {
            if (!string.IsNullOrWhiteSpace(AdditionalInput))
            {
                ErrorAttachmentLog textLog = ErrorAttachmentLog.AttachmentWithText(AdditionalInput, "UserFeedback.txt");
                return new List<ErrorAttachmentLog> { textLog };
            }
            return null;
        }

        private void Crashes_FailedToSendErrorReport(object sender, FailedToSendErrorReportEventArgs e)
            => Done();

        private async void Crashes_SentErrorReport(object sender, SentErrorReportEventArgs e)
        {
            IsSending = false;
            IsDoneSending = true;
            await Task.Delay(2000);
            Done();
        }

        private void Crashes_SendingErrorReport(object sender, SendingErrorReportEventArgs e)
        {
            IsSending = true;
        }

        private void OnUserConfirmationSelected(object obj)
        {
            if (obj is UserConfirmation conf)
            {
                Crashes.NotifyUserConfirmation(conf);
            }
        }

        public void Done()
        {
            Crashes.SendingErrorReport -= Crashes_SendingErrorReport;
            Crashes.SentErrorReport -= Crashes_SentErrorReport;
            Crashes.FailedToSendErrorReport -= Crashes_FailedToSendErrorReport;
            Crashes.GetErrorAttachments -= Crashes_GetErrorAttachments;
            IsDone?.Invoke(this, null);
        }

        public event EventHandler IsDone;

        public void RaisePropertyChanged([CallerMemberName]string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
