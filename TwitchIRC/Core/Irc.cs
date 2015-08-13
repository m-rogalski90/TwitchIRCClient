using Meebey.SmartIrc4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace TwitchIRC.Core
{
    public delegate void ChannelMessageHandler(ChannelMessage message);
    public delegate void ErrorMessagehandler(ErrorMessage message);
    public delegate void ErrorHandler(String error);

    public class Irc
    {
        #region CALLBACKS/HANDLERS
        private ChannelMessageHandler m_ChannelMessageHandler;
        private ErrorMessagehandler m_ErrorMessageHandler;
        private ErrorHandler m_ErrorHandler;
        #endregion

        #region MEMBER FIELDS
        private IrcClient m_IrcClient;
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
        /// Sets up the channel message handler, whenever the
        /// channel message income this handler will be invoked
        /// </summary>
        /// <param name="p1">Method that will be invoked whenever channel message come in</param>
        public void SetChannelMessageHandler(ChannelMessageHandler p1)
        {
            m_ChannelMessageHandler = p1;
        }

        /// <summary>
        /// Sets up the  error message handler, whenever the
        /// error message income this handler will be invoked
        /// </summary>
        /// <param name="p1">Method that will be invoked when error message occures on chat</param>
        public void SetErrorMessageHandler(ErrorMessagehandler p1)
        {
            m_ErrorMessageHandler = p1;
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
            if (m_ConnectionDetails == null)
                throw new Exception("No connection details was set.");
            m_ConnectionThread = new Thread(() =>
            {
                m_IrcClient.Connect("irc.twitch.tv", 6667);
                m_IrcClient.Login(m_ConnectionDetails.Username, m_ConnectionDetails.Username, 0, m_ConnectionDetails.Username, m_ConnectionDetails.Password);
                m_IrcClient.RfcJoin("#mrgalski");
                m_IrcClient.Listen();
            });
            m_ConnectionThread.Start();
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
        /// Used to send a message on to the default channel
        /// </summary>
        /// <param name="p1">Message that will be sent</param>
        public void SendMessage(string p1)
        {
            m_IrcClient.SendMessage(SendType.Message, "#mrgalski", p1);
        }

        /// <summary>
        /// Used to send a message on to the channel from currently
        /// joined channel.
        /// </summary>
        /// <param name="p1">Index of the channel in the currently joined channels array</param>
        /// <param name="p2">Message that will be sent</param>
        public void SendMessage(int p1, string p2)
        {
            m_IrcClient.SendMessage(SendType.Message, m_IrcClient.JoinedChannels[p1], p2);
        }

        /// <summary>
        /// Used to send a message to the user or on to the channel that will be refferenced by the name.
        /// </summary>
        /// <param name="p1">Username or channel name, depends on what you want to do.</param>
        /// <param name="p2">Message that will be sent</param>
        public void SendMessageTo(string p1, string p2)
        {
            m_IrcClient.SendMessage(SendType.Message, p1, p2);
        }



        // Dont look!
        #region PRIVATE HANDLERS
        private void ProcessCommand(IrcMessageData data)
        {
            List<string> params_ = new List<string>();
            params_.AddRange(data.Message.Split(' '));
            params_.RemoveAt(0);
            m_Commands.First(cmd => data.Message.StartsWith(cmd.Cmd)).Execute(data.From, params_.ToArray());
        }

        private void ProcessChannelMessage(IrcMessageData data)
        {
            ChannelMessage message = new ChannelMessage()
            {
                From = data.Nick,
                Message = data.Message
            };
            m_ChannelMessageHandler(message);
        }

        private void ProcessErrorMessage(IrcMessageData data)
        {
            ErrorMessage message = new ErrorMessage()
            {
                Message = data.Message
            };
            m_ErrorMessageHandler(message);
        }

        private void OnMessage(object sender, IrcEventArgs e)
        {
            if (Object.ReferenceEquals(e.Data.Message, null))
                return;

            if (m_Commands.Any(cmd => e.Data.Message.StartsWith(cmd.Cmd)))
                ProcessCommand(e.Data);
            else
            {
                switch (e.Data.Type)
                {
                    case ReceiveType.ChannelMessage:    ProcessChannelMessage(e.Data);      break;
                    case ReceiveType.ErrorMessage:      ProcessErrorMessage(e.Data);        break;
                }
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            m_ErrorHandler(e.ErrorMessage);
        }
        #endregion
    }
}
