using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.Game
{
    public delegate void CommandActionHandler(String from, String param = "");

    public class StateCommand
    {
        protected String m_Command;
        protected String m_Param;
        protected CommandActionHandler m_CommandAction;
        
        public String Command
        {
            get { return m_Command; }
        }
        public String Param
        {
            get { return m_Param; }
        }

        public StateCommand(CommandActionHandler cmdAction, String cmd, String param = "")
        {
            m_CommandAction = cmdAction;
            m_Command = cmd;
            m_Param = param;
        }

        public void ExecuteCommand(String from, String param = "")
        {
            m_CommandAction(from, param);
        }
    }
}
