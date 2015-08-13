using System;

namespace TwitchIRC.Core
{
    /// <summary>
    /// Object will return the channel message
    /// </summary>
    public class ChannelMessage : MessageBase
    {
        private String m_From;

        /// <summary>
        /// Returns the contents of the message
        /// </summary>
        public String Message
        {
            get { return m_Message; }
            internal set { m_Message = value; }
        }
        /// <summary>
        /// Returns the nickname of a person that
        /// sent this message
        /// </summary>
        public String From
        {
            get { return m_From; }
            internal set { m_From = value; }
        }

        /// <summary>
        /// Creates a new instance of the ChannelMessage object
        /// </summary>
        public ChannelMessage() { m_Type = Meebey.SmartIrc4net.ReceiveType.ChannelMessage; }
    }
}
