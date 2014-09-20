using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GmailNotifierClone
{
    public partial class AuthForm : Form
    {
        private static bool m_isFirstRun = true;
        private static AuthForm m_instance;

        private AuthForm()
        {
            InitializeComponent();
        }

        public static AuthForm Instance
        {
            get { return m_instance ?? (m_instance = new AuthForm()); }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Settings.Instance.Login = tbLogin.Text;
            Settings.Instance.Password = tbPassword.Text;
            Settings.Instance.IsNeedToSaveAuth = cbSaveAuth.Checked;
            Settings.Instance.Save();

            Close();
            MailManager.Instance.CheckAndNotify();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_instance = null;
        }

        private void AuthForm_Load(object sender, EventArgs e)
        {
            if (m_isFirstRun == true)
            {
                m_isFirstRun = false;
                if (Settings.Instance.Login.Length > 0 && Settings.Instance.Password.Length > 0 &&
                    Settings.Instance.IsNeedToSaveAuth == true)
                {
                    Close();
                    MailManager.Instance.CheckAndNotify(); 
                }
            }
        }
    }
}
