using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;

namespace GmailNotifierClone
{
    public partial class PopupForm : Form
    {
        public PopupForm(PopupData data)
        {
            InitializeComponent();
            lblCountAdndDate.Text = data.CountAndDate;
            lblHeader.Text = data.Header;
            lblText.Text = data.Text;
            lblFrom.Text = data.From;

            ScheduleSuicide();
            Animate();
        }

        private void Animate()
        {
            Screen scn = Screen.FromPoint(this.Location);
            this.Left = scn.WorkingArea.Right - this.Width;
            this.Top = scn.WorkingArea.Bottom - this.Height;
            Application.DoEvents();
        }

        public void ScheduleSuicide()
        {
            System.Timers.Timer timerSuicide = new System.Timers.Timer();
            timerSuicide.Interval = 5000;
            timerSuicide.Elapsed += new ElapsedEventHandler(timerSuicide_Tick);
            timerSuicide.SynchronizingObject = this;
            timerSuicide.AutoReset = false;
            timerSuicide.Start();
        }

        private void timerSuicide_Tick(object sender, EventArgs e)
        {
            Log.Add("Popup suicide!");
            this.BeginInvoke(new MethodInvoker(Close));
            Application.DoEvents();
        }
    }
}
