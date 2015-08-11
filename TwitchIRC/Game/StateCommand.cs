using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC.Game
{
    public delegate void CommandActionHandler(String from, String param = "");

    public class StateCommand : Command
    {
        protected String m_From;
        protected String m_Param;
        protected CommandActionHandler m_CommandAction;
        
        public String From
        {
            get { return m_From; }
        }
        public String Param
        {
            get { return m_Param; }
        }

        public StateCommand(CommandActionHandler cmdAction, String cmd, String param = "") : base(cmd)
        {
            m_CommandAction = cmdAction;
            m_Param = param;
        }

        public void ExecuteCommand(String from, String param = "")
        {
            m_CommandAction(from, param);
        }
    }
}
