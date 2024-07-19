using ff14bot.AClasses;
using ff14bot.Managers;
using System;
using System.Media;
using System.Text.RegularExpressions;
using System.Windows.Forms;
#pragma warning disable CA1416

namespace AlertMe
{
    public class AlertMe : BotPlugin
    {

        public override string Author => "Parrot";
        public override string Description => "A chat monitor";
        public override Version Version => new(1, 5, 0);
        public override string Name => "AlertMe";


        public override void OnEnabled()
        {
            Settings.Load();
            Log.Chat.Print($"[Date] {DateTime.Now.ToString("dd/MM-yy hh:mm")}");

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

        public override bool WantButton => true;

        public override string ButtonText => "Settings/Log";

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

                SndPlayer.Play("pm.wav");
                Log.Bot.Info("[PM] message received");
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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Shout, msg);
                    if (match)
                    {
                        SndPlayer.Play("chat.wav");
                        Log.Bot.Info("[Shout] Message received");
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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Party, msg);
                    if (match)
                    {
                        SndPlayer.Play("chat.wav");
                        Log.Bot.Info("[Party] Message received");
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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Linkshell, msg);
                    if (match)
                    {
                        SndPlayer.Play("chat.wav");
                        Log.Bot.Info("[LS] Message received");
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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Say, msg);
                    if (match)
                    {
                        SndPlayer.Play("chat.wav");
                        Log.Bot.Info("[Say] Message received");
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
                SndPlayer.Play("gm.wav");
                Log.Bot.Info("ATTENTION! A Game Master is contacting you.");
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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.FC, msg);
                    if (match)
                    {
                        SndPlayer.Play("chat.wav");
                        Log.Bot.Info("[FC] Message received");

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
                if (AuthorCheck(author))
                {
                    match = msgMatchesCrit(Settings.Current.Emote, msg);
                    if (match)
                    {
                        SndPlayer.Play("emote.wav");
                        Log.Bot.Info("Emote Received;");
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
                if (MessageContainsKeywords(msg, cc.Keywords))
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



        private bool MessageContainsKeywords(string msg, string[] keywords)
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


        private bool AuthorCheck(string author)
        {
            if (Settings.Current.ignoreSelf)
            {
                Log.Bot.Debug("Ignore self is enabled, performing name check on author.");
                return !AuthorIsLocalPlayer(author);
            }

            return true;
        }

        private bool AuthorIsLocalPlayer(string author)
        {
            bool isLocalPlayer = author == GameObjectManager.LocalPlayer.Name;
            Log.Bot.Debug($"Author is local player: {isLocalPlayer}");
            return isLocalPlayer;
        }

        private static class SndPlayer
        {

            public static void Play(string fileName)
            {
                if (!Settings.Current.sound)
                    return;

                var fullPath = Application.StartupPath + @"\Plugins\AlertMe\Sounds\" + fileName;
                try
                {
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = fullPath;
                    sp.Play();

                }
                catch (Exception e)
                {
                    Log.Bot.Info("Error: " + e.Message);
                    Beep();
                }
            }

            private static void Beep()
            {
                if (!Settings.Current.sound)
                    return;
                try
                {
                    SystemSounds.Beep.Play();
                }
                catch (Exception ee)
                {
                    Log.Bot.Info("Error: Could not play system sound \"beep\"\n" + ee.Message);
                }
            }
        }
    }
}
