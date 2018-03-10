using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Windows.Media;

namespace AlertMe
{
    public static class Settings
    {
        public static Profile Current = new Profile();
        public static readonly string PluginRootDir = ff14bot.Managers.PluginManager.PluginDirectory + @"\AlertMe";

        public static bool Save()
        {
            return Save(Current);
        }

        public static bool Save(Profile profile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Profile));
                TextWriter textWriter = new StreamWriter(PluginRootDir + @"\Settings.xml");
                serializer.Serialize(textWriter, profile);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Bot.Print("Error while saving :\n" + e.Message);
                return false;
            }
            Log.Bot.Print("Saved settings.", Colors.White);
            return true;
        }

        public static bool Load()
        {
            if (!CreateSettingsFile())
            {
                Log.Bot.Print("Could not create Settings.xml. Make sure the plugin installed in plugins/AlertMe/");
                Current = new Profile();
                return false;
            }
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Profile));
                TextReader textReader = new StreamReader(PluginRootDir + @"\Settings.xml");
                Current = (Profile)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch (Exception e)
            {
                Log.Bot.Print("Failed to load settings :\n" + e.Message);
                Current = new Profile();
                return false;
            }

            Log.Bot.Print("Settings loaded", Colors.White);
            return true;

        }

        public static bool CreateSettingsFile()
        {
            if (!File.Exists(PluginRootDir + @"\Settings.xml"))
            {
                Log.Bot.Print("Settings.xml is missing, creating a new file.", Colors.White);
                return Save(new Profile());
            }
            return true;
        }

        public class Profile
        {
            //pm and gm ChatChannel only use the enabled attrib atm.
            public ChatChannel PM;
            public ChatChannel FC;
            public ChatChannel Shout;
            public ChatChannel Say;
            public ChatChannel GM;
            public ChatChannel Linkshell;
            public ChatChannel Party;
            public ChatChannel Emote;
            public ChatLog chatLog;
            public PushBullet pushBullet;
            public bool sound;
            public bool ignoreSelf;
            
            public Profile()
            {

                FC = new ChatChannel();
                PM = new ChatChannel
                {
                    Enabled = true,
                    PushBulletEnabled = true
                };
                chatLog = new ChatLog();
                Shout = new ChatChannel();
                Say = new ChatChannel();
                GM = new ChatChannel {PushBulletEnabled = true};
                Party = new ChatChannel();
                Emote = new ChatChannel();
                pushBullet = new PushBullet();
                Emote.Keywords = new string[] { "you" };
                GM.Enabled = true;
                Linkshell = new ChatChannel();
                ignoreSelf = true;
                sound = true;
            }
        }

        public class ChatChannel
        {
            public ChatChannel()
            {
                Enabled = false;
                PushBulletEnabled = false;
                UseRegex = false;
                UseKeywords = false;
                Regex = string.Empty;
                Keywords = new String[0];
            }
            public bool Enabled;
            public bool PushBulletEnabled;
            public bool UseRegex;
            public bool UseKeywords;
            public string Regex = "";
            public string[] Keywords;
        }

        public class ChatLog 
        {
            public bool Enabled = true;
            public bool LogAll = true; //log all messages on monitored channels.
        }

        public class PushBullet
        {
            public PushBullet()
            {
                Enabled = false;
            }
            public bool Enabled;
            public string Token = "";            
        }
    }
}
