using System;

namespace TwitchIRC
{
    public delegate void CommandActionHandler();


    public class Command
    {
        private String m_Name;
        private CommandActionHandler m_CommandAction;

        public Command(String name, CommandActionHandler action)
        {
            this.m_Name = name;
            this.m_CommandAction = action;
        }

        public void RunCommand()
        {
            m_CommandAction();
        }
    }
}
