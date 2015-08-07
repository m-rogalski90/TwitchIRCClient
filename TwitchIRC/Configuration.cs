using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TwitchIRC
{
    public class Configuration
    {
        #region MEMBER FIELDS
        private String m_FilePath;
        private Connection m_Connection;
        #endregion
        #region PROPERTIES
        public Connection IrcConnection
        {
            get
            {
                return m_Connection;
            }
        }
        #endregion
        #region CONSTRUCTOR
        public Configuration(String path = "")
        {
            if (path == "")
                m_FilePath = String.Format("{0}{1}cfg{2}main.cfg",
            Environment.CurrentDirectory, Path.DirectorySeparatorChar, Path.DirectorySeparatorChar);
            else
                m_FilePath = path;
            m_Connection = new Connection();
        }
        #endregion
        #region PARSING THE CONFIGURATION FILE
        public void TryLoad()
        {
            if (File.Exists(m_FilePath))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(m_FilePath)))
                {
                    String line;
                    while((line = reader.ReadLine()) != null)
                        CheckLine(line);
                }
                m_Connection.Address = "irc.twitch.tv";
                m_Connection.Port = 6667;
            }
        }

        private void CheckLine(String line)
        {
            if (Regex.IsMatch(line, @"oauth_key=oauth:[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetValueFor(out m_Connection.OAuthToken, line);
            else if (Regex.IsMatch(line, @"user_name=[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetValueFor(out m_Connection.Nickname, line);
            else if (Regex.IsMatch(line, @"channel_name=#[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetValueFor(out m_Connection.ChannelName, line);
            //else if (Regex.IsMatch(line, @"server_address=([a-zA-Z0-9]{1,10}).([a-zA-Z0-9){3,20}).([a-zA-Z0-9]{2,5}", RegexOptions.IgnoreCase))
            //    SetValueFor(out m_Address, line);
            //else if (Regex.IsMatch(line, @"server_port=([0-9]{1,5})", RegexOptions.IgnoreCase))
            //{
            //    String port_str = String.Empty;
            //    SetValueFor(out port_str, line);
            //    if (!Int32.TryParse(port_str, out m_Port))
            //        m_Port = 6667;
            //}
        }
        private void SetValueFor(out String _for, String line)
        {
            int idx = line.IndexOf('=') + 1;
            _for = line.Substring(idx, line.Length - idx);
        }
        #endregion
    }
}
