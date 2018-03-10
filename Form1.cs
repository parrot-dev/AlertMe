using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AlertMe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            fillForm();
        }

        private void fillForm()
        {
            try
            {
                if (!Log.Chat.CreateLogFile())
                {
                    throw new Exception("Could not create ChatLog.txt, make sure the plugin is installed in plugins/AlertMe/");
                }
                txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
               
                chkboxIgnoreSelf.Checked = Settings.Current.ignoreSelf;
                checkBox1.Checked = Settings.Current.sound;
                checkBox3.Checked = Settings.Current.chatLog.LogAll;
                checkBox5.Checked = Settings.Current.PM.Enabled;
                checkBox16.Checked = Settings.Current.GM.Enabled;
                chkBoxEmote.Checked = Settings.Current.Emote.Enabled;

                //pusbullet
                chkboxPushBullet.Checked = Settings.Current.pushBullet.Enabled;
                txtBoxPbToken.Text = Settings.Current.pushBullet.Token;
                //chatfc
                chkboxFcPb.Checked = Settings.Current.FC.PushBulletEnabled;
                checkBox2.Checked = Settings.Current.FC.Enabled;
                checkBox6.Checked = Settings.Current.FC.UseKeywords;
                if (Settings.Current.FC.Keywords.Count() > 0)
                    textBox1.Text = String.Join(",", Settings.Current.FC.Keywords);
                checkBox7.Checked = Settings.Current.FC.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.FC.Regex))
                    textBox8.Text = Settings.Current.FC.Regex;
                //chatsay
                chkboxSayPb.Checked = Settings.Current.Say.PushBulletEnabled;
                checkBox8.Checked = Settings.Current.Say.Enabled;
                checkBox4.Checked = Settings.Current.Say.UseKeywords;
                if (Settings.Current.Say.Keywords.Count() > 0)
                    textBox2.Text = String.Join(",", Settings.Current.Say.Keywords);
                checkBox9.Checked = Settings.Current.Say.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.Say.Regex))
                    textBox3.Text = Settings.Current.Say.Regex;
                //linkshells
                chkboxLsPb.Checked = Settings.Current.Linkshell.PushBulletEnabled;
                checkBox13.Checked = Settings.Current.Linkshell.Enabled;
                checkBox10.Checked = Settings.Current.Linkshell.UseKeywords;
                if (Settings.Current.Linkshell.Keywords.Count() > 0)
                    textBox4.Text = String.Join(",", Settings.Current.Linkshell.Keywords);
                checkBox11.Checked = Settings.Current.Linkshell.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.Linkshell.Regex))
                    textBox5.Text = Settings.Current.Linkshell.Regex;
                //shout
                chkboxShoutPb.Checked = Settings.Current.Shout.PushBulletEnabled;
                checkBox15.Checked = Settings.Current.Shout.Enabled;
                checkBox14.Checked = Settings.Current.Shout.UseKeywords;
                if (Settings.Current.Shout.Keywords.Count() > 0)
                    textBox7.Text = String.Join(",", Settings.Current.Shout.Keywords);
                checkBox12.Checked = Settings.Current.Shout.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.Shout.Regex))
                    textBox6.Text = Settings.Current.Shout.Regex;
                //party
                chkboxPartyPb.Checked = Settings.Current.Party.PushBulletEnabled;
                chkboxParty.Checked = Settings.Current.Party.Enabled;
                chkBoxKeywordParty.Checked = Settings.Current.Party.UseKeywords;
                if (Settings.Current.Party.Keywords.Count() > 0)
                    txtboxKeywordParty.Text = String.Join(",", Settings.Current.Party.Keywords);
                chkboxRegexParty.Checked = Settings.Current.Party.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.Party.Regex))
                    txtboxRegexParty.Text = Settings.Current.Party.Regex;
                //emote
                chkboxEmotePb.Checked = Settings.Current.Emote.PushBulletEnabled;
                chkBoxEmote.Checked = Settings.Current.Emote.Enabled;
                chkBoxKeywordEmote.Checked = Settings.Current.Emote.UseKeywords;
                if (Settings.Current.Emote.Keywords.Count() > 0)
                    txtBoxKeywordEmote.Text = String.Join(",", Settings.Current.Emote.Keywords);
                chkBoxRegexEmote.Checked = Settings.Current.Emote.UseRegex;
                if (!String.IsNullOrEmpty(Settings.Current.Emote.Regex))
                    txtBoxRegexEmote.Text = Settings.Current.Emote.Regex;

            }
            catch (Exception e) { MessageBox.Show(e.Message); }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            Log.Chat.Clear();
            txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox16.Checked && checkBox16.Focused)
            {
                MessageBox.Show("This option has never been tested, it may or may not work.");
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            var profile = new Settings.Profile();        
            profile.sound = checkBox1.Checked;
            profile.ignoreSelf = chkboxIgnoreSelf.Checked;
            profile.chatLog.LogAll = checkBox3.Checked;
            profile.GM.Enabled = checkBox16.Checked;
            profile.PM.Enabled = checkBox5.Checked;
            profile.pushBullet.Token = txtBoxPbToken.Text;
            profile.pushBullet.Enabled = chkboxPushBullet.Checked;
            profile.FC = buildChatChannel(checkBox2.Checked, checkBox6.Checked, checkBox7.Checked, chkboxFcPb.Checked, textBox1.Text, textBox8.Text);
            profile.Say = buildChatChannel(checkBox8.Checked, checkBox4.Checked, checkBox9.Checked, chkboxSayPb.Checked, textBox2.Text, textBox3.Text);
            profile.Linkshell = buildChatChannel(checkBox13.Checked, checkBox10.Checked, checkBox11.Checked, chkboxLsPb.Checked, textBox4.Text, textBox5.Text);
            profile.Shout = buildChatChannel(checkBox15.Checked, checkBox14.Checked, checkBox12.Checked, chkboxShoutPb.Checked, textBox7.Text, textBox6.Text);
            profile.Party = buildChatChannel(chkboxParty.Checked, chkBoxKeywordParty.Checked, chkboxRegexParty.Checked, chkboxPartyPb.Checked, txtboxKeywordParty.Text, txtboxRegexParty.Text);
            profile.Emote = buildChatChannel(chkBoxEmote.Checked, chkBoxKeywordEmote.Checked, chkBoxRegexEmote.Checked, chkboxEmotePb.Checked, txtBoxKeywordEmote.Text, txtBoxRegexEmote.Text);
            Settings.Current = profile;
            Settings.Save();
            fillForm();           
        }

        private Settings.ChatChannel buildChatChannel(bool enabled, bool useKeywords, bool useRegex, bool pushbullet, string keywords, string regex)
        {

            var cc = new Settings.ChatChannel();
            cc.Enabled = enabled;
            cc.PushBulletEnabled = pushbullet;

            if (!String.IsNullOrEmpty(keywords))
                cc.Keywords = keywords.Split(',');

            if (useKeywords)
            {
                cc.UseKeywords = true;
            }
            else
                cc.UseKeywords = false;

            if (useRegex)
            {
                var pattern = regex;
                if (Utils.isValidRegex(pattern))
                {
                    cc.UseRegex = true;
                    cc.Regex = pattern;
                }
                else
                {
                    MessageBox.Show("Regex is invalid: \r\n" + pattern);
                    cc.Regex = pattern; //Save pattern anyway.
                    cc.UseRegex = false;
                }
            }
            else
            {
                cc.Regex = regex;
                cc.UseRegex = false;
            }
            return cc;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txtLog.Text = File.ReadAllText(Log.Chat.Filepath);
        }


        private async void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtBoxPbToken.Text))
            {
              await new PushBullet.Note("AlertMe Reborn", "Test Message").Push(txtBoxPbToken.Text);
            }
        }

 

    }
}
