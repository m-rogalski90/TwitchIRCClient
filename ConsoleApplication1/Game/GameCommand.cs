using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatGame.Game
{
    public delegate void GameCommandActionHandler(String from, String param);

    public class GameCommand : Command
    {
        protected String m_From;
        protected String m_Param;
        protected GameCommandActionHandler m_CommandAction;

        public String From
        {
            get { return m_From; }
        }
        public String Param
        {
            get { return m_Param; }
        }

        public GameCommand(GameCommandActionHandler cmdAction, String cmd, String param = "") : base(cmd)
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
