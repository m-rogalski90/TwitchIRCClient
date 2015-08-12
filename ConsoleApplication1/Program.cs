using System;
using System.Threading;

using TwitchIRC.Core;

namespace TwitchChatGame
{

    class Program
    {
        private Thread m_IrcThread;
        private Irc m_Client;
        private QuizGame.QuizGame m_Game;

        [STAThread]
        static void Main(string[] args)
        {
            new Program();
        }
        
        private Program()
        {
            m_Client = new Irc();
            m_Client.SetMessageHandler(onMessage);
            m_Client.Connect();
            m_Game = new QuizGame.QuizGame(m_Client);
        }

        private void onMessage(ChannelMessage message)
        {
            if (message is ChannelMessage)
            {
                ChannelMessage chmsg = (ChannelMessage)message;
                if (message.Message.StartsWith("!"))
                    ParseCommand(chmsg);

                Console.WriteLine(String.Format("[ {0} ] {1}", chmsg.From, chmsg.Message));
            }
        }

        private void ParseCommand(ChannelMessage message)
        {
            int idx = message.Message.IndexOf(' ');
            string cmd = message.Message.Substring(0, (idx == -1 ? message.Message.Length : idx));
            string param = String.Empty;
            if(idx != -1)
                param = message.Message.Substring(idx + 1, message.Message.Length - idx - 1);
        }
    }
}
