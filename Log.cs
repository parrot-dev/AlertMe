using ff14bot.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Media;

#pragma warning disable CA1416

namespace AlertMe
{
    static class Log
    {
        public static class Bot
        {

            public static void Info(string msg, Exception exception = null)
            {
                Logging.Write(Colors.White, Tag(msg));
                WriteException(exception);
            }

            public static void Debug(string msg, Exception exception = null)
            {
                Logging.Write(LogLevel.Diagnostic, Colors.LimeGreen, Tag(msg));
                WriteException(exception, LogLevel.Diagnostic);
            }

            public static void Error(string msg, Exception exception = null)
            {
                Logging.Write(Colors.Red, Tag(msg));
                WriteException(exception);
            }

            public static void WriteException(Exception exception, LogLevel logLevel = LogLevel.Normal)
            {
                if (exception != null)
                {
                    Logging.WriteException(logLevel, Colors.White, exception);
                }
            }

            private static string Tag(string msg)
            {
                return $"[AlertMe]  {msg}";
            }


        }

        public static class Chat
        {
            public enum Channels { Pm, Fc, Say, Ls, Shout, Gm, Party, Emote }
            public static readonly string Filepath = Settings.PluginRootDir + @"\ChatLog.txt";
            public static void Print(string input)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                using (var streamWriter = new StreamWriter(Filepath, true))
                {
                    streamWriter.WriteLine(input);
                }

            }

            public static bool CreateLogFile()
            {
                if (!File.Exists(Settings.PluginRootDir + @"\ChatLog.txt"))
                {
                    Bot.Info("ChatLog.txt is missing, creating a new file");
                    try
                    {
                        Clear();
                    }
                    catch (Exception e)
                    {
                        Bot.Info("Could not create ChatLog.txt, make sure the plugin is installed in plugins/AlertMe/");
                        return false;
                    }
                }
                return true;
            }

            public static void PrintMsg(Channels chn, string msg, string author)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                var timestamp = DateTime.Now.ToString("hh:mm:ss");

                using (var streamWriter = new StreamWriter(Filepath, true))
                {
                    streamWriter.WriteLine($"[{timestamp,-8}]{'[' + chn.ToString() + ']',-7} From: {author}");
                    streamWriter.WriteLine($"{"Body:",18}{msg}");
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