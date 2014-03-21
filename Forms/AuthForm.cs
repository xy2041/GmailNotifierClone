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
            Settings.Login = tbLogin.Text;
            Settings.Password = tbPassword.Text;
            Close();
            MailManager.Instance.CheckAndNotify();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_instance = null;
        }
    }
}
