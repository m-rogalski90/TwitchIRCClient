using System;

namespace TwitchIRC.Core
{
    
    public delegate void CommandActionHandler(String from, String[] params_);

    /// <summary>
    /// Object is used to add a command with an action method that
    /// will be invoked whenever user type it in the chat.
    /// </summary>
    public class Command
    {
        private String m_Command;
        private CommandActionHandler m_CommandAction;

        /// <summary>
        /// Returns the command as String example !stats, !join etc.
        /// </summary>
        public String Cmd
        {
            get { return m_Command; }
        }

        /// <summary>
        /// Creates a new instance of an Command object
        /// that will store the command expression and 
        /// command action method
        /// </summary>
        /// <param name="p1">Expression that will be parsed as a command</param>
        /// <param name="p2">Action that will be executed when command appears in the chat.</param>
        public Command(String p1, CommandActionHandler p2)
        {
            m_Command = p1;
            m_CommandAction = p2;
        }

        /// <summary>
        /// Method used to execute the action method.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public void Execute(string p1, string[] p2)
        {
            if (m_CommandAction != null)
                m_CommandAction(p1, p2);
        }

        /// <summary>
        /// Method used to check if command sent on the chat
        /// is this command.
        /// </summary>
        /// <param name="p1">Command string</param>
        /// <returns>TRUE it's this command, FALSE otherwise</returns>
        public bool IsCommand(string p1)
        {
            return p1.ToLower().Equals(m_Command.ToLower());
        }
    }
}
