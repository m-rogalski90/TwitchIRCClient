using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC.Game
{
    public abstract class Command
    {
        protected String m_Content;
        
        public String Content
        {
            get { return m_Content; }
        }

        public Command(String cmd)
        {
            this.m_Content = cmd;
        }
    }
}
