using System;
using System.Windows.Forms;
using AE.Net.Mail;

namespace GmailNotifierClone
{
    public class Notifier
    {
        private MailMessage m_message;
        private int m_number;

        public Notifier(MailMessage message, int number)
        {
            m_message = message;
            m_number = number;
        }

        public void ThreadProc()
        {
            DateTime date = m_message.Date;

            PopupData p = new PopupData();
            p.CountAndDate = String.Format("{0} of {1} - {2:g}", m_number, MailManager.Instance.GetNewMailCount(), date);
            p.From = m_message.From.DisplayName;
            p.Header = m_message.Subject;
            p.Text = m_message.Body;

            Log.Add("Show pop for: " + p.Header);

            PopupForm pf = new PopupForm(p);
            MainForm.Instance.Invoke((MethodInvoker) pf.Show);
            Application.DoEvents();
        }
    }
}