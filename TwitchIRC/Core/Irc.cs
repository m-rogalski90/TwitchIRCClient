using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TwitchIRC.Core
{
    public delegate void MessageHandler(ChannelMessage message);
    public delegate void ErrorHandler(String error);

    public class Irc
    {
        #region MEMBER FIELDS
        private IrcClient m_IrcClient;
        private MessageHandler m_MessageHandler;
        private ErrorHandler m_ErrorHandler;
        private ConnectionDetails m_ConnectionDetails;
        private List<Command> m_Commands;
        private Thread m_ConnectionThread;
        #endregion

        /// <summary>
        /// Creates a new instance of Irc client and
        /// sets up the default values
        /// </summary>
        public Irc()
        {
            m_Commands = new List<Command>();
            m_IrcClient = new IrcClient();
            m_IrcClient.UseSsl = false;
            m_IrcClient.ActiveChannelSyncing = true;
            m_IrcClient.OnError += OnError;
            m_IrcClient.OnRawMessage += OnMessage;
        }

        /// <summary>
        /// Sets up the connection details used
        /// that will be then used to connect
        /// to the IRC server
        /// </summary>
        /// <param name="p1">Connection details</param>
        public void SetConnectionDetails(ConnectionDetails p1)
        {
            m_ConnectionDetails = p1;
        }

        /// <summary>
        /// Sets up the message handler, whenever the
        /// message income this handler will be invoked
        /// </summary>
        /// <param name="p1">Method that will parse messages</param>
        public void SetMessageHandler(MessageHandler p1)
        {
            m_MessageHandler = p1;
        }

        /// <summary>
        /// Sets up the error handler, whenever 
        /// IRC server send up an error message
        /// this handler will be invoked
        /// </summary>
        /// <param name="p1">Method that will handle the error</param>
        public void SetErrorHandler(ErrorHandler p1)
        {
            m_ErrorHandler = p1;
        }
        
        /// <summary>
        /// Method used to connect to the IRC server
        /// You have to specify the ConnectionDetails
        /// at first because it will throw an exception
        /// </summary>
        public void Connect()
        {
            //if (m_ConnectionDetails == null)
            //    throw new Exception("No connection details was set.");
            //m_ConnectionThread = new Thread(() =>
            //{
                m_IrcClient.Connect("", 0);
                m_IrcClient.Login("nick", "realname", 0, "username", "password");
                m_IrcClient.RfcJoin("channel");
                m_IrcClient.Listen(false);
            //});
            //m_ConnectionThread.Start();
        }
        
        /// <summary>
        /// Used to disconnect from current IRC server
        /// </summary>
        public void Disconnect()
        {
            if (m_ConnectionThread == null)
                return;

            if(m_IrcClient.IsConnected)
                m_IrcClient.Disconnect();

            if (!m_ConnectionThread.Join(5 * 1000))
                m_ConnectionThread.Abort();
        }

        /// <summary>
        /// Adds a new command to the commands list
        /// </summary>
        /// <param name="p1">Command that you want to add</param>
        /// <returns>TRUE if added, FALSE if it contains that command</returns>
        public bool AddCommand(Command p1)
        {
            if (m_Commands == null)
                throw new Exception("Commands list was not initialized... Report this bug.");

            bool _ = m_Commands.Any(cmd => cmd.Cmd.ToLower() == p1.Cmd.ToLower());
            if(!_)
            {
                m_Commands.Add(p1);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Executes the command's action
        /// </summary>
        /// <param name="p1">Command [!join, !stats etc...]</param>
        /// <param name="p2">User that sent this command</param>
        /// <param name="p3">Parameters used by the user</param>
        /// <returns></returns>
        public bool ExecuteCommand(string p1, string p2, string[] p3)
        {
            if (m_Commands == null)
                throw new Exception("Commands list was not initialized... Report this bug.");

            bool _ = m_Commands.Any(cmd => cmd.Cmd.ToLower() == p1.ToLower());
            if (_)
            {
                m_Commands.First(cmd => cmd.Cmd.ToLower() == p1.ToLower()).Execute(p2, p3);
                return true;
            }
            return false;
        }
        
        public void SendMessage(string p1)
        {
            m_IrcClient.SendMessage(SendType.Message, m_IrcClient.JoinedChannels[0], p2);
        }

        public void SendMessage(int p1, string p2)
        {
            m_IrcClient.SendMessage(SendType.Message, m_IrcClient.JoinedChannels[p1], p2);
        }

        public void SendMessageTo(string p1, string p2)
        {
            m_IrcClient.SendMessage(SendType.Message, p1, p2);
        }

        #region PRIVATE HANDLERS
        private void OnMessage(object sender, IrcEventArgs e)
        {
            MessageBase message = null;
            //check if it's a command...
            switch(e.Data.Type)
            {
                case ReceiveType.ChannelMessage:
                    message = new ChannelMessage();
                    ((ChannelMessage)message).From = e.Data.From;
                    ((ChannelMessage)message).Message = e.Data.Message;
                    m_MessageHandler((ChannelMessage)message);
                    break;

                case ReceiveType.ErrorMessage:
                    message = new ErrorMessage();
                    ((ErrorMessage)message).Message = e.Data.Message;
                    // add a few other messages like:
                    //  - private message
                    break;
                
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            m_ErrorHandler(e.ErrorMessage);
        }
        #endregion
    }
}
