using System;
using ff14bot.Managers;
using System.Media;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ff14bot.AClasses;

namespace AlertMe
{
    public class AlertMe : BotPlugin
    {

        public override string Author { get { return "Parrot"; } }
        public override string Description { get { return "A chat monitor"; } }
        public override Version Version { get { return new Version(1, 4, 3); } }
        public override string Name { get { return "AlertMe"; } }


        public override void OnEnabled()
        {
            Settings.Load();
            Log.Chat.Print(string.Format("[Date] {0}", DateTime.Now.ToString("dd/MM-yy hh:mm")));

            GamelogManager.TellRecevied += TellReceived;
            GamelogManager.ShoutRecevied += ShoutReceived;
            GamelogManager.GameMasterMessageRecevied += GameMaster;
            GamelogManager.SayRecevied += SayReceived;
            GamelogManager.LinkShellMessageRecevied += LinkShellReceived;
            GamelogManager.FreeCompanyMessageRecevied += FCReceived;
            GamelogManager.PartyMessageRecevied += PartyReceived;
            GamelogManager.EmoteRecevied += EmoteReceived;
        }

        public override void OnDisabled()
        {

            GamelogManager.TellRecevied -= TellReceived;
            GamelogManager.ShoutRecevied -= ShoutReceived;
            GamelogManager.GameMasterMessageRecevied -= GameMaster;
            GamelogManager.SayRecevied -= SayReceived;
            GamelogManager.LinkShellMessageRecevied -= LinkShellReceived;
            GamelogManager.FreeCompanyMessageRecevied -= FCReceived;
            GamelogManager.PartyMessageRecevied -= PartyReceived;
            GamelogManager.EmoteRecevied -= EmoteReceived;
        }

        public override bool WantButton
        {
            get { return true; }
        }
        public override string ButtonText
        {
            get { return "Settings/Log"; }
        }
        public override void OnButtonPress()
        {
            Settings.Load();
            try
            {
                var sf = new Form1();
                sf.Show();
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private async void TellReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.Current.PM.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;

                SndPlayer.play("pm.wav");
                Log.Bot.Print("[PM] message received");
                Log.Chat.PrintMsg(Log.Chat.Channels.Pm, msg, author);
                if (Settings.Current.PM.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                {
                    await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Pm.ToString() + ']', msg)).Push();
                }                   
             }
        }

        private async void ShoutReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.Current.Shout.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Shout, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.Print("[Shout] Message received");
                        if (Settings.Current.Shout.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Shout.ToString() + ']', msg)).Push();
                        }    
                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Shout, msg, author);
                }
            }
        }

        private async void PartyReceived(object sender, ff14bot.Managers.ChatEventArgs e)
        {
            if (Settings.Current.Party.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Party, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.Print("[Party] Message received");
                        if (Settings.Current.Party.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Party.ToString() + ']', msg)).Push();
                        }  
                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Party, msg, author);
                }
            }
        }

        private async void LinkShellReceived(object sender, ChatEventArgs e)
        {
            if (Settings.Current.Linkshell.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Linkshell, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.Print("[LS] Message received");
                        if (Settings.Current.Linkshell.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Ls.ToString() + ']', msg)).Push();
                        }  
                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Ls, msg, author);
                }
            }
        }

        private async void SayReceived(object sender, ChatEventArgs e)
        {
            if (Settings.Current.Say.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Say, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.Print("[Say] Message received");
                        if (Settings.Current.Say.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Say.ToString() + ']', msg)).Push();
                        }  
                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Say, msg, author);
                }
            }
        }

        private async void GameMaster(object sender, ChatEventArgs e)
        {
            if (Settings.Current.GM.Enabled)
            {
                SndPlayer.play("gm.wav");
                Log.Bot.Print("ATTENTION! A Game Master is contacting you.");
                Log.Chat.PrintMsg(Log.Chat.Channels.Gm, e.ChatLogEntry.Contents, e.ChatLogEntry.SenderDisplayName);
                
                if (Settings.Current.GM.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                {
                    await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", e.ChatLogEntry.SenderDisplayName, '[' + Log.Chat.Channels.Gm.ToString() + ']', e.ChatLogEntry.Contents)).Push();
                }  
            }
        }
        private async void FCReceived(object sender, ChatEventArgs e)
        {            
            if (Settings.Current.FC.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.FC, msg);
                    if (match)
                    {
                        SndPlayer.play("chat.wav");
                        Log.Bot.Print("[FC] Message received");

                        if (Settings.Current.FC.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Fc.ToString() + ']', msg)).Push();
                        }  

                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Fc, msg, author);
                }
            }
        }

        private async void EmoteReceived(object sender, ChatEventArgs e)
        {

            if (Settings.Current.Emote.Enabled)
            {
                var msg = e.ChatLogEntry.Contents;
                var author = e.ChatLogEntry.SenderDisplayName;
                bool match = false;
                if (authorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Emote, msg);
                    if (match)
                    {
                        SndPlayer.play("emote.wav");
                        Log.Bot.Print("Emote Received;");
                        if (Settings.Current.Emote.PushBulletEnabled && Settings.Current.pushBullet.Enabled)
                        {
                            await new PushBullet.Note("AlertMe", String.Format("From: {0}\r\n{1} {2}", author, '[' + Log.Chat.Channels.Emote.ToString() + ']', msg)).Push();
                        }  
                    }
                }
                if (match || Settings.Current.chatLog.LogAll)
                {
                    Log.Chat.PrintMsg(Log.Chat.Channels.Emote, e.ChatLogEntry.Contents, e.ChatLogEntry.SenderDisplayName);
                }

            }
        }

        private bool msgMatchesCrit(Settings.ChatChannel cc, string msg)
        {
            if (!cc.UseKeywords && !cc.UseRegex)
                return true;


            if (cc.UseKeywords)
            {
                if (stringContainsKeywords(msg, cc.Keywords))
                    return true;
            }

            if (cc.UseRegex)
            {
                Regex r = new Regex(cc.Regex, RegexOptions.IgnoreCase);
                if (r.Match(msg).Success)
                {
                    return true;
                }
            }
            return false;
        }



        private bool stringContainsKeywords(string msg, string[] keywords)
        {
            if (keywords == null || keywords.Length == 0)
                return false;
            for (int i = 0; i < keywords.Length; ++i)
            {
                if (msg.ToLower().Contains(keywords[i].ToLower()))
                    return true;
            }
            return false;
        }


        private bool authorCheck(string author)
        {
            if (Settings.Current.ignoreSelf)
            {
                return !isAuthorMe(author);
            }

            return true;
        }

        private bool isAuthorMe(string author)
        {
            return author == GameObjectManager.LocalPlayer.Name;
        }

        private static class SndPlayer
        {

            public static void play(string fileName)
            {
                if (!Settings.Current.sound)
                    return;

                var fullPath = System.Windows.Forms.Application.StartupPath + @"\Plugins\AlertMe\Sounds\" + fileName;
                try
                {
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = fullPath;
                    sp.Play();

                }
                catch (Exception e)
                {
                    Log.Bot.Print("Error: " + e.Message);
                    beep();
                }
            }

            public static void beep()
            {
                if (!Settings.Current.sound)
                    return;
                try
                {
                    SystemSounds.Beep.Play();
                }
                catch (Exception ee)
                {
                    Log.Bot.Print("Error: Could not play system sound \"beep\"\n" + ee.Message);
                }
            }
        }
    }
}
