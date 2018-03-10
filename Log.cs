using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AlertMe
{
    static class Log
    {
        public static class Bot
        {
            public static void Print(string input, Color col)
            {
                Logging.Write(col, string.Format("[AlertMe] {0}", input));
            }

            public static void Print(string input)
            {
                Print(input, Colors.Red);
            }
        }

        public static class Chat
        {
            public enum Channels {Pm, Fc, Say, Ls, Shout, Gm, Party, Emote}
            public static readonly string Filepath = (System.Windows.Forms.Application.StartupPath + @"\plugins\AlertMe\ChatLog.txt");
            public static void Print(String input)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath, true))
                {
                    file.WriteLine(input);
                }

            }
            public static void PrintMsg(Channels chn, string msg, string author)
            {                
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                var timestamp = DateTime.Now.ToString("hh:mm:ss");

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath, true))
                {
                    file.WriteLine(string.Format("[{0,-9}]{1,-7} From: {2}", timestamp,'['+chn.ToString()+']', author));
                    file.WriteLine(string.Format("{0,27}{1}\r\n", "",msg));
                }
            }

            public static void Clear()
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath))
                {
                    file.Write("");
                }
            }
        }
    }
}