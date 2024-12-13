using System;
using System.Windows.Forms;
using PasswordPrinter.Services;

namespace PasswordPrinter.Forms
{
    public partial class MainForm : Form
    {
        private readonly PollingService _pollingService;

        public MainForm()
        {
            InitializeComponent();
            _pollingService = new PollingService(LogMessage);
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            _pollingService.Start();
            LogMessage("Polling iniciado.");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _pollingService.Stop();
            LogMessage("Polling parado.");
        }

        private void LogMessage(string message)
        {
            if (txtLog.InvokeRequired)
            {
                txtLog.Invoke(new Action(() => txtLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}")));
            }
            else
            {
                txtLog.AppendText($"{DateTime.Now}: {message}{Environment.NewLine}");
            }
        }

        private void btnStart_Click_1(object sender, EventArgs e)
        {
            _pollingService.Start();
            LogMessage("Polling iniciado.");
        }

        private void btnStop_Click_1(object sender, EventArgs e)
        {
            _pollingService.Stop();
            LogMessage("Polling parado.");
        }
    }
}
