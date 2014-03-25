using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GmailNotifierClone.Forms
{
    public partial class NoAltTabForm : Form
    {
        protected override CreateParams CreateParams
        {

            get
            {

                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x00000080; // WS_EX_TOOLWINDOW (hide from Alt + Tab)

                cp.ExStyle |= 0x00080000; // WS_EX_LAYERED (create layered window)

                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT - mouse events go through

                //cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE

                return cp;

            }

        }
    }
}
