using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AE.Net.Mail;

namespace GmailNotifierClone
{
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

        public int GetNewMailCount()
        {
            return m_mails.Count;
        }

        public void CheckAndNotify()
        {
            if (m_isCheckInProgress)
            {
                return;
            }

            if (Settings.Login == "" || Settings.Password == "")
            {
                AuthForm f = new AuthForm();
                f.Show();
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
                    var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", Settings.Login, Settings.Password,
                        AE.Net.Mail.ImapClient.AuthMethods.Login, 993, true))
                {
                    imap.SelectMailbox("INBOX");
                    const bool headersOnly = false;
                    Lazy<MailMessage>[] messages = imap.SearchMessages(SearchCondition.Unseen(), headersOnly);

                    // Run through each message:
                    MailManager.Instance.m_notificationsList.Clear();

                    Log.Add("Total messages fetched: " + messages.Count());

                    var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                    if (messages.Any())
                    {
                        MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_2")));
                        MainForm.Instance.trayIcon.Text = "New mail: " + messages.Count();
                    }
                    else
                    {
                        MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_4")));
                        MainForm.Instance.trayIcon.Text = "No unread mail";
                    }

                    List<string> newMailIdsList = new List<string>();

                    int cnt = 0;
                    foreach (Lazy<MailMessage> message in messages)
                    {
                        MailMessage m = message.Value;
                        newMailIdsList.Add(m.MessageID);

                        if (!MailManager.Instance.m_mails.ContainsKey(m.MessageID))
                        {
                            Log.Add("Schedule notification");
                            MailManager.Instance.m_mails.Add(m.MessageID, m);
                            MailManager.Instance.m_notificationsList.Add(m.MessageID);
                        }
                    }

                    // Del from cache viewed messages
                    for (int i = MailManager.Instance.m_mails.Count - 1; i >= 0; --i)
                    {
                        if (!newMailIdsList.Contains(MailManager.Instance.m_mails.ElementAt(i).Key))
                        {
                            MailManager.Instance.m_mails.Remove(MailManager.Instance.m_mails.ElementAt(i).Key);
                        }
                    }
                }

                
                ShowNotifications();
            }
            catch (Exception e)
            {
                if (e.Message.Contains("[AUTHENTICATIONFAILED] Invalid credentials"))
                {
                    Settings.Password = "";
                    AuthForm f = new AuthForm();
                    MainForm.Instance.Invoke((MethodInvoker)f.Show);
                }
                
                {
                    MainForm.Instance.trayIcon.Text = e.Message.Substring(0,63);
                    var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
                    MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_5")));
                }

                Log.Add(e);
            }
            finally
            {
                m_isCheckInProgress = false;
            }
        }

        private static void ShowNotifications()
        {
            Log.Add("Show notifications: " + MailManager.Instance.m_notificationsList.Count);
            for (int index = 0; index < MailManager.Instance.m_notificationsList.Count; index++)
            {
                var s = MailManager.Instance.m_notificationsList[index];
                Notifier tws = new Notifier(MailManager.Instance.m_mails[s], index);

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

        internal void TellMeAgain()
        {
            MailManager.Instance.m_notificationsList.Clear();
            foreach (var mailMessage in m_mails)
            {
                MailManager.Instance.m_notificationsList.Add(mailMessage.Value.MessageID); 
            }
            ShowNotifications();
        }
    }
}
