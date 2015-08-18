using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TwitchIRC.Core
{
    /// <summary>
    /// Object is used as a holder for connection details
    /// that will be used by IrcClient to connect to the 
    /// IRC server
    /// </summary>
    public class ConnectionDetails
    {
        private String m_ConfigFilePath;

        private String m_Address;
        private Int32 m_Port;
        private String m_Nickname;
        private String m_Realname;
        private Int32 m_Usermode;
        private String m_Username;
        private String m_Password;
        private List<String> m_Channels;

        /// <summary>
        /// 
        /// </summary>
        public String Address { get { return m_Address; } set { m_Address = value; } }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Port { get { return m_Port; } set { m_Port = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String Nickname { get { return m_Nickname; } set { m_Nickname = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String Realname { get { return m_Realname; } set { m_Realname = value; } }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Usermode { get { return m_Usermode; }  set { m_Usermode = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String Username { get { return m_Username; } set { m_Username = value; } }
        /// <summary>
        /// 
        /// </summary>
        public String Password { get { return m_Password; } set { m_Password = value; } }
        /// <summary>
        /// 
        /// </summary>
        public List<String> Channels { get { return m_Channels; } set { m_Channels = value; } }

        /// <summary>
        /// 
        /// </summary>
        public ConnectionDetails()
        {
            m_Channels = new List<String>();
            //String.Format($"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}data{Path.DirectorySeparatorChar}main.cfg");
            m_ConfigFilePath = String.Format("{0}{1}data{1}main.cfg",
                Environment.CurrentDirectory, Path.DirectorySeparatorChar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p1"></param>
        public void SetConfigFilePath(string p1)
        {
            m_ConfigFilePath = p1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ReadConfigFile()
        {
            if (!File.Exists(m_ConfigFilePath))
                throw new Exception("Config file dos not exist.");

            using (StreamReader stream = new StreamReader(File.Open(m_ConfigFilePath, FileMode.Open))) // I'm to lazy to open and close streams...
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                    ProcessLine(line);
            }
        }

        private void ProcessLine(string line)
        {
            if (Regex.IsMatch(line, @"oauth_key=oauth:[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetStringValueFor(out m_Password, line);
            else if (Regex.IsMatch(line, @"user_name=[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetStringValueFor(out m_Username, line);
            else if (Regex.IsMatch(line, @"nickname=[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetStringValueFor(out m_Nickname, line);
            else if (Regex.IsMatch(line, @"realname=[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetStringValueFor(out m_Realname, line);
            else if (Regex.IsMatch(line, @"address=[a-zA-z0-9]", RegexOptions.IgnoreCase))
                SetStringValueFor(out m_Address, line);
            else if (Regex.IsMatch(line, @"channel=#[a-zA-z0-9]", RegexOptions.IgnoreCase))
                AddChannel(line);
            else if (Regex.IsMatch(line, @"port=[0-9]", RegexOptions.IgnoreCase))
                SetInt32ValueFor(out m_Port, line);
            else if (Regex.IsMatch(line, @"usermode=[0-9]", RegexOptions.IgnoreCase))
                SetInt32ValueFor(out m_Usermode, line);
        }

        #region MAKE THIS GENERIC!!!!!
        private void AddChannel(String line)
        {
            int idx = line.IndexOf('=') + 1;
            String tmp = line.Substring(idx, line.Length - idx);
            m_Channels.Add(tmp);
        }
        private void SetStringValueFor(out String _for, String line)
        {
            int idx = line.IndexOf('=') + 1;
            _for = line.Substring(idx, line.Length - idx);
        }

        private void SetInt32ValueFor(out Int32 _for, String line)
        {
            int idx = line.IndexOf('=') + 1;
            String tmp = line.Substring(idx, line.Length - idx);
            if (!Int32.TryParse(tmp, out _for))
                _for = -1;
        }
        #endregion
    }
}
