using ff14bot.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            public static readonly string Filepath = Settings.PluginRootDir + @"\ChatLog.txt";
            public static void Print(String input)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Filepath, true))
                {
                    file.WriteLine(input);
                }

            }

            public static bool CreateLogFile()
            {
                if (!File.Exists(Settings.PluginRootDir + @"\ChatLog.txt"))
                {
                    Bot.Print("ChatLog.txt is missing, creating a new file", Colors.White);
                    try
                    {
                        Clear();
                    }
                    catch (Exception e)
                    {
                        Bot.Print("Could not create ChatLog.txt, make sure the plugin is installed in plugins/AlertMe/");
                        return false;
                    }
                }
                return true;
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