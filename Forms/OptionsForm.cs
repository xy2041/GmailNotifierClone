using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace GmailNotifierClone
{
    public partial class OptionsForm : Form
    {
        private const string REGISTRY_AUTORUN_PATH = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static OptionsForm m_instance;
        public static OptionsForm Inctance
        {
            get { return m_instance ?? (m_instance = new OptionsForm()); }
        }

        private OptionsForm()
        {
            InitializeComponent();
            cbAutorun.Checked = IsAutorunEnabled();
        }

        private bool IsAutorunEnabled()
        {
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(REGISTRY_AUTORUN_PATH, true);
            if (rkApp == null)
            {
                return false;
            }
            return (rkApp.GetValue("GmailNotifierClone") != null);
        }

        private void SetAutorun(bool enable)
        {
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey(REGISTRY_AUTORUN_PATH, true) ??
                                Registry.CurrentUser.CreateSubKey(REGISTRY_AUTORUN_PATH);

            
            // Add the value in the registry so that the application runs at startup
            if (rkApp != null)
            {
                if (enable)
                {
                    rkApp.SetValue("GmailNotifierClone", Application.ExecutablePath); 
                }
                else
                {
                    rkApp.DeleteValue("GmailNotifierClone");
                }
                
            }
            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SetAutorun(cbAutorun.Checked);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void OptionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_instance = null;
        }
    }
}
