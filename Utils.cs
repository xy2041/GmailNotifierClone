using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace GmailNotifierClone
{
    static class Utils
    {
        private static object m_lock = new object();

        private static readonly string ApplicationDir = AppDomain.CurrentDomain.BaseDirectory;

        public static string GetSqlQueryText(string fileName)
        {
            string fullPath = Path.Combine(ApplicationDir, "sql", fileName);
            return File.ReadAllText(fullPath);
        }

        public static int? ToNullableInt32(this string s)
        {
            int i;
            if (Int32.TryParse(s, out i)) return i;

            Log.Add(String.Format("Format exception: ToNullableInt32({0})", s), Loglevel.Warning, false);
            return null;
        }

        public static bool? ToNullableBool(this string s)
        {
            bool i;
            if (Boolean.TryParse(s, out i)) return i;
            Log.Add(String.Format("Format exception: ToNullableBool({0})", s), Loglevel.Warning, false);
            return null;
        }

        public static string ToGroupedDigits(this int? s)
        {
            if (s == null) return String.Empty;

            Decimal d = (int)s;
            return d.ToString("#,##0");
        }

        public static string ToLocalizedBool(this bool? s)
        {
            if (s == null) return "?";
            return (bool) s ? "Да" : "Нет";
        }

        public static decimal? ToNullableDecimal(this string s)
        {
            decimal i;
            if (Decimal.TryParse(s, out i)) return i;
            Log.Add(String.Format("Format exception: ToNullableDecimal({0})", s), Loglevel.Warning, false);
            return null;
        }

        public static String ToNullableString(this object s)
        {
            return s == null ? null : s.ToString();
        }

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
