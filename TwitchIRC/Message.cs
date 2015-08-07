using System;
using Meebey.SmartIrc4net;

namespace TwitchIRC
{
    public class Message
    {
        public String       Host;
        public String       Channel;
        public String       Identity;
        public DateTime     When;
        public String       Content;
    }

    // because they are passed by a refference not by value.
    public class ChannelMessage : Message
    {
        public String From;

        public override string ToString()
        {
            return String.Format("{0} -- {1}", this.From, this.Content);
        }
    }

    public class ErrorMessage : Message
    {
        public override string ToString()
        {
            return String.Format("{0} -- {1}", this.Host, this.Content);
        }
    }
}
