using Meebey.SmartIrc4net;
using System;

namespace TwitchIRC
{
    public delegate void MessageHandler(Message message);

    public class Client
    {
        private IrcClient m_Client;
        private MessageHandler m_MessageCallback;
        private Connection m_Connection;

        public Client(Connection connection, MessageHandler msgCallback)
        {
            windows.popups.PopupConfig.Instance.Margin = 60;
            windows.popups.PopupConfig.Instance.FramesPerSecond = 120; // still better than twitch or youtube ;D
            windows.popups.PopupConfig.Instance.PixelsPerFrame = 2;

            m_Client = new IrcClient();
            m_Client.UseSsl = false;
            m_Client.ActiveChannelSyncing = true;
            m_MessageCallback = msgCallback;
            m_Connection = connection;
        }

        public void Initialize()
        {
            m_Client.OnErrorMessage += ErrorMessage;
            m_Client.OnChannelMessage += ChannelMessage;
            m_Client.OnRawMessage += RawMessage;
            m_Client.OnError += Error;
        }

        /// <summary>
        /// REMEMBER TO PUT THIS ON A DIFFERENT THREAD!
        /// </summary>
        public void Connect()
        {
            m_Client.Connect(m_Connection.Address, m_Connection.Port);
            m_Client.Login(m_Connection.Nickname, m_Connection.Nickname, 0,
                m_Connection.Nickname, m_Connection.OAuthToken);
            m_Client.RfcJoin(m_Connection.ChannelName);
            m_Client.Listen();
        }

        public void Disconnect()
        {
            if(m_Client.IsConnected)
            {
                m_Client.Disconnect();
            }
        }

        public void SendMessage(String message)
        {
            m_Client.SendMessage(SendType.Message, m_Connection.ChannelName, message);
        }

        private void ErrorMessage(object sender, IrcEventArgs e)
        {
            ErrorMessage message = new ErrorMessage()
            {
                Host = e.Data.Host,
                Channel = e.Data.Channel,
                Content = e.Data.Message,
                Identity = e.Data.Ident,
                When = DateTime.Now
            };
            m_MessageCallback(message);
        }

        private void ChannelMessage(object sender, IrcEventArgs e)
        {
            ChannelMessage message = new ChannelMessage()
            {
                Host = e.Data.Host,
                Channel = e.Data.Channel,
                Content = e.Data.Message,
                Identity = e.Data.Ident,
                When = DateTime.Now,
                From = e.Data.Nick
            };
            m_MessageCallback(message);
        }

        private void RawMessage(object sender, IrcEventArgs e)
        {
            // implement this later instead of channel and error messages
        }

        private void Error(object sender, ErrorEventArgs e)
        {
            // implement this method...
        }
    }
}