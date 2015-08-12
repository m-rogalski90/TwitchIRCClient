using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC.Core
{
    // i could make some kind of abstract CommandAction class...
    public delegate void CommandActionHandler(String from, String[] params_);

    public class Command
    {
        private String m_Command;
        private CommandActionHandler m_CommandAction;

        public String Cmd
        {
            get { return m_Command; }
        }

        public Command(String p1, CommandActionHandler p2)
        {
            m_Command = p1;
            m_CommandAction = p2;
        }

        public void Execute(string p1, string[] p2)
        {
            if (m_CommandAction != null)
                m_CommandAction(p1, p2);
        }

        public bool IsCommand(string p1)
        {
            return p1.ToLower().Equals(m_Command.ToLower());
        }
    }
}
