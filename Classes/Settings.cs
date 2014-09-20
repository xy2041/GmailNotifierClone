using System;
using System.Collections.Generic;
using System.Linq;

namespace GmailNotifierClone
{
    public class Settings
    {
        public String Login            { get; set; }
        public String Password         { get; set; }
        public bool IsNeedToSaveAuth   { get; set; }

        private static Settings m_instance;
        public static Settings Instance
        {
            get { return m_instance ?? (m_instance = new Settings()); }
        }

        private Settings()
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                IsNeedToSaveAuth = Properties.Settings.Default.IsNeedToSaveAuth;
                Login = Properties.Settings.Default.Login;
                Password = StringCipher.Decrypt(Properties.Settings.Default.Password, Environment.MachineName + Environment.UserName);
            }
            catch (Exception)
            {
                IsNeedToSaveAuth = false;
                Login = "";
                Password = "";
            }
  
        }

        public void Save()
        {
            Properties.Settings.Default.IsNeedToSaveAuth = IsNeedToSaveAuth;
            Properties.Settings.Default.Login = Login;
            Properties.Settings.Default.Password = StringCipher.Encrypt(Password, Environment.MachineName + Environment.UserName);
            Properties.Settings.Default.Save();
        }

    }
}
