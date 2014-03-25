using System;
using System.Windows.Forms;
using GmailNotifierClone.Forms;


namespace GmailNotifierClone
{
    public partial class MainForm : NoAltTabForm
    {
        public System.Windows.Forms.NotifyIcon trayIcon;

        private static MainForm m_instance;

        public static MainForm Instance
        {
            get { return m_instance ?? (m_instance = new MainForm()); }
        }

        private MainForm()
        {
            InitializeComponent();
            this.Visible = false;
            this.Hide();
            //tas = false;
            this.WindowState = FormWindowState.Minimized;
            
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_4")));
            this.trayIcon.Text = "No unread mail";


            MailManager.Instance.CheckAndNotify();
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private void button1_Click(object sender2, EventArgs e2)
        {
            //http://stackoverflow.com/questions/12657792/how-to-securely-save-username-password-local

            //MailManager.Instance.MarkAsRead();

        }

        private void trayIcon_DoubleClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://mail.google.com/mail/");
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var pd = new PopupData();
            pd.Header = " asdf asdf asd fasd";
            pd.Text = " asdf asdf asd fasd";
            PopupForm pf = new PopupForm(pd);
            pf.Show();
        }

        private void timerCheckMail_Tick(object sender, EventArgs e)
        {
            MailManager.Instance.CheckAndNotify();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }

        private void checkMailNowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailManager.Instance.CheckAndNotify();
        }

        private void tellAgainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailManager.Instance.TellMeAgain();
        }

        private void markAllAsReadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailManager.Instance.MarkAsRead(false);
        }

        private void allToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MailManager.Instance.MarkAsRead(true);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionsForm.Inctance.Show();
        }
    }
}
