using System;

namespace TwitchIRC.Core
{
    /// <summary>
    /// Object will return the error message
    /// </summary>
    public class ErrorMessage : MessageBase
    {
        /// <summary>
        /// Returns the error message
        /// </summary>
        public String Message
        {
            get { return m_Message; }
            internal set { m_Message = value; }
        }

        /// <summary>
        /// Creates a new instance of ErrorMessage object
        /// </summary>
        public ErrorMessage() { m_Type = Meebey.SmartIrc4net.ReceiveType.ErrorMessage; }
    }
}
