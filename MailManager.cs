using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AE.Net.Mail;

namespace GmailNotifierClone
{
    public class Notifier
    {
        private MailMessage m_message;

        public Notifier(MailMessage message)
        {
            m_message = message;
        }

        public void ThreadProc()
        {
            PopupData p = new PopupData();
            p.CountAndDate = "1 of 1 - sep 13";
            p.From = m_message.From.DisplayName;
            p.Header = m_message.Subject;
            p.Text = m_message.Body;

            Log.Add("Show pop for: " + p.Header);

            PopupForm pf = new PopupForm(p);
            MainForm.Instance.Invoke((MethodInvoker) pf.Show);
            Application.DoEvents();
        }
    }

    public class MailManager
    {
        private static bool m_isCheckInProgress;

        private static MailManager m_instance;

        private MailManager() { }

        public static MailManager Instance
        {
           get 
           {
              if (m_instance == null)
              {
                  m_instance = new MailManager();
              }
              return m_instance;
           }
        }

        private Dictionary<string, MailMessage> m_mails = new Dictionary<string, MailMessage>();
        private List<string> m_notificationsList = new List<string>();

        public void CheckAndNotify()
        {
            if (m_isCheckInProgress)
            {
                return;
            }

            m_isCheckInProgress = true;
            Thread t = new Thread(CheckMailThreadFunc);
            t.Start();
        }

        static void Notify()
        {
            
        }

        static void CheckMailThreadFunc()
        {
            try
            {
                Log.Add("Checking mailbox...");
                using (
                    var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", "xy2041@gmail.com", "some pass",
                        AE.Net.Mail.ImapClient.AuthMethods.Login, 993, true))
                {
                    imap.SelectMailbox("INBOX");

                    bool headersOnly = false;

                    Lazy<MailMessage>[] messages = imap.SearchMessages(SearchCondition.Unseen(), headersOnly);

                    // Run through each message:
                    MailManager.Instance.m_notificationsList.Clear();

                    Log.Add("Total messages fetched: " + messages.Count());

                    if (messages.Count() > 0)
                    {
                        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                        MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_2")));
                        MainForm.Instance.trayIcon.Text = "New mail: " + messages.Count();
                    }
                    else
                    {
                        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                        MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_4")));
                        MainForm.Instance.trayIcon.Text = "No unread mail";
                    }

                    int cnt = 0;
                    foreach (Lazy<MailMessage> message in messages)
                    {
                        cnt ++;
                        if (cnt < messages.Count() - 5) continue;

                        MailMessage m = message.Value;
                        if (!MailManager.Instance.m_mails.ContainsKey(m.MessageID))
                        {
                            Log.Add("Schedule notification");
                            MailManager.Instance.m_mails.Add(m.MessageID, m);
                            MailManager.Instance.m_notificationsList.Add(m.MessageID);
                        }
                    }
                }

                
                Log.Add("Show notifications: " + MailManager.Instance.m_notificationsList.Count);
                foreach (var s in MailManager.Instance.m_notificationsList)
                {
                    Notifier tws = new Notifier(MailManager.Instance.m_mails[s]);

                    // Create a thread to execute the task, and then 
                    // start the thread.
                    Thread t = new Thread(new ThreadStart(tws.ThreadProc));
                    t.Start();
                    Log.Add("Wait...");
                    Thread.Sleep(5000);
                    Application.DoEvents();
                }
                MailManager.Instance.m_notificationsList.Clear();
                Log.Add("Done!");
            }
            catch (Exception e)
            {
                Log.Add(e);
                throw e;
            }
            finally
            {
                m_isCheckInProgress = false;
            }
        }
    }
}
