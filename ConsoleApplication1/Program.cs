using System;
using System.Threading;
using TwitchIRC.Core;

namespace TwitchChatGame
{
    /// <summary>
    /// Chat works fine, need to make some webapi to manage followers, cahnnels and some other things
    /// to create a dashboard manager i MUST HAVE webapi...
    /// </summary>
    class Program
    {
        private Irc m_Client;
        private QuizGame.QuizGame m_Game;

        [STAThread]
        static void Main(string[] args)
        {
            new Program();
        }
        
        private Program()
        {
            ConnectionDetails condetails = new ConnectionDetails();
            condetails.ReadConfigFile();

            m_Client = new Irc();
            m_Client.SetConnectionDetails(condetails);
            m_Client.SetChannelMessageHandler(onMessage);
            m_Client.Connect();
            m_Game = new QuizGame.QuizGame(m_Client);
        }

        private void onMessage(ChannelMessage message)
        {
            Console.WriteLine(String.Format("[ {0} ] {1}", message.From, message.Message));
        }
    }
}
