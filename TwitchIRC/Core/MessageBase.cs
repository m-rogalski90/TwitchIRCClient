using Meebey.SmartIrc4net;
using System;

namespace TwitchIRC.Core
{
    /// <summary>
    /// I think it has some purpose now... :/
    /// </summary>
    public class MessageBase
    {
        /// <summary>
        /// Represents the type of the received message
        /// -- check Meebey.SmartIrc4net for the ReceiveTypes --
        /// </summary>
        protected ReceiveType m_Type;

        /// <summary>
        /// Message contents
        /// </summary>
        protected String m_Message;
    }
}
