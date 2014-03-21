using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GmailNotifierClone
{
    static class Utils
    {
        private static object m_lock = new object();

        private static readonly string ApplicationDir = AppDomain.CurrentDomain.BaseDirectory;

        public static Color ToColor(this Loglevel level)
        {
            Color color = Color.Black;
            if (level == Loglevel.Error) color = Color.Red;
            if (level == Loglevel.Warning) color = Color.DarkOrange;
            if (level == Loglevel.Info) color = Color.DarkMagenta;
            if (level == Loglevel.Green) color = Color.Green;

            return color;
        }

        private static TResult SafeInvoke<T, TResult>(this T isi, Func<T, TResult> call) where T : ISynchronizeInvoke
        {
            if (isi.InvokeRequired)
            {
                IAsyncResult result = isi.BeginInvoke(call, new object[] { isi });
                object endResult = isi.EndInvoke(result); return (TResult)endResult;
            }
            return call(isi);
        }

        private static void SafeInvoke<T>(this T isi, Action<T> call) where T : ISynchronizeInvoke
        {
            if (isi.InvokeRequired) isi.BeginInvoke(call, new object[] { isi });
            else
                call(isi);
        }

        public static void WriteTextToBox(RichTextBox box, string text, Color color)
        {
            try
            {
                box.SafeInvoke(b => b.SelectionColor = color);
                box.SafeInvoke(b => b.AppendText(text));
                box.SafeInvoke(b => b.ScrollToCaret());
                Application.DoEvents();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
    }
}
