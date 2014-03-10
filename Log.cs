using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace GmailNotifierClone
{
    static class Log
    {
        private static RichTextBox m_textBox;

        public static void SetTextBox(RichTextBox tb)
        {
            m_textBox = tb;
        }

        public static void Add(Exception e)
        {
            string message = e.Message;
            string innerMessage = e.InnerException != null ? e.InnerException.Message : String.Empty;

            string outText = String.Format("Exception: {0} inner: [{1}]", message, innerMessage);
            Add(outText, Loglevel.Error);
        }

        public static void Add(String mess, Loglevel level = Loglevel.None, bool timestamp = true)
        {
            String time = String.Format("{0:HH:mm:ss}", DateTime.Now);

            String outText = timestamp ? String.Format("{0} - {1}\n", time, mess) : String.Format("{0}\n", mess);

            Debug.Write(outText);

            if (m_textBox != null)
            {
                Utils.WriteTextToBox(m_textBox, outText, level.ToColor());
            }
        }
    }
}
