using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC.Core
{
    public class ConnectionDetails
    {
        public String Address { get; set; }
        public Int32 Port { get; set; }

        public String Nickname { get; set; }
        public String Realname { get; set; }
        public Int32 Usermode { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String[] Channels { get; set; }
    }
}
