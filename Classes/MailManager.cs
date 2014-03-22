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
        private static Object m_lock = new Object();

        private static bool m_isCheckInProgress;

        private static MailManager m_instance;

        private MailManager()
        {
        }

        public static MailManager Instance
        {
            get { return m_instance ?? (m_instance = new MailManager()); }
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
                AuthForm.Instance.Show();
                return;
            }

            m_isCheckInProgress = true;
            Thread t = new Thread(CheckMailThreadFunc);
            t.Start();
        }

        private static void Notify()
        {

        }

        private static void CheckMailThreadFunc()
        {

            Log.Add("CheckMailThreadFunc lock");
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

                    var resources = new System.ComponentModel.ComponentResourceManager(typeof (Assets));
                    if (messages.Any())
                    {
                        int messCount = messages.Count();
                        MainForm.Instance.trayIcon.Text = "New mail: " + messCount;

                        if (messCount > 0 && messCount <= 9)
                        {
                            string icoName = String.Format("tray_ico_{0}", messCount);
                            MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon) (resources.GetObject(icoName)));
                        }
                        else if (messCount > 9)
                        {
                            MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("blank_ico_max")));
                        }
                        else
                        {
                            MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("gnotify_2")));
                        }
                    }
                    else
                    {
                        MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon) (resources.GetObject("gnotify_4")));
                        MainForm.Instance.trayIcon.Text = "No unread mail";
                    }

                    List<string> newMailIdsList = new List<string>();

                    int cnt = 0;
                    foreach (Lazy<MailMessage> message in messages)
                    {
                        MailMessage m = message.Value;
                        newMailIdsList.Add(m.Uid);

                        if (!MailManager.Instance.m_mails.ContainsKey(m.Uid))
                        {
                            Log.Add("Schedule notification for message with id: " + m.Uid);
                            MailManager.Instance.m_mails.Add(m.Uid, m);
                            MailManager.Instance.m_notificationsList.Add(m.Uid);
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
            catch
                (Exception
                    e)
            {
                if (e.Message.Contains("[AUTHENTICATIONFAILED] Invalid credentials"))
                {
                    Settings.Password = "";
                    MainForm.Instance.Invoke((MethodInvoker) AuthForm.Instance.Show);
                }
                try
                {
                    MainForm.Instance.trayIcon.Text = e.Message.Length > 63 ? e.Message.Substring(0, 63) : e.Message;
                    var resources = new System.ComponentModel.ComponentResourceManager(typeof (Assets));
                    MainForm.Instance.trayIcon.Icon = ((System.Drawing.Icon) (resources.GetObject("gnotify_5")));
                }
                catch (Exception ee)
                {
                    Log.Add(ee);
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
            int offset = 0;
            if (MailManager.Instance.m_notificationsList.Count < MailManager.Instance.GetNewMailCount())
            {
                offset = MailManager.Instance.GetNewMailCount() - MailManager.Instance.m_notificationsList.Count;
            }
            for (int index = 0; index < MailManager.Instance.m_notificationsList.Count; index++)
            {
                var s = MailManager.Instance.m_notificationsList[index];
                Notifier tws = new Notifier(MailManager.Instance.m_mails[s], index + 1 + offset);

                // Create a thread to execute the task, and then 
                // start the thread.
                Thread t = new Thread(new ThreadStart(tws.ThreadProc));
                t.Start();
                Log.Add("Wait...");
                Application.DoEvents();
                Thread.Sleep(5000);
                Application.DoEvents();
            }
            MailManager.Instance.m_notificationsList.Clear();
            Log.Add("Done!");
        }

        public void TellMeAgain()
        {
            Log.Add("TellMeAgain lock");
            try
            {
                MailManager.Instance.m_notificationsList.Clear();
                foreach (var mailMessage in m_mails)
                {
                    MailManager.Instance.m_notificationsList.Add(mailMessage.Value.Uid);
                }
                ShowNotifications();
            }
            catch (Exception e)
            {
                Log.Add(e);
            }
        }

        public void MarkAsRead(bool all)
        {
            try
            {
                using (
                    var imap = new AE.Net.Mail.ImapClient("imap.gmail.com", Settings.Login, Settings.Password,
                        AE.Net.Mail.ImapClient.AuthMethods.Login, 993, true))
                {
                    imap.SelectMailbox("INBOX");
                    if (all)
                    {
                        foreach (var mailMessage in m_mails)
                        {
                            imap.Store("UID " + mailMessage.Value.Uid, true, "\\Seen");
                        }
                    }
                    else
                    {
                        if (m_mails.Count > 0)
                        {
                            var lastMail = m_mails.Values.OrderBy(i => i.Date).FirstOrDefault();
                            //var lastMail2 = m_mails.Values.OrderByDescending(i => i.Date).FirstOrDefault();
                            imap.Store("UID " + lastMail.Uid, true, "\\Seen");
                        }
                    }

                }
            }
            catch (System.Exception ex)
            {
                Log.Add(ex);
            }

        }
    }
}
