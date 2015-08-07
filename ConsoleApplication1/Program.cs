using System;
using System.Threading;

using TwitchIRC;

namespace ConsoleApplication1
{

    class Program
    {
        private Thread m_IrcThread;
        private Client m_Client;
        private QuizGame.QuizGame m_Game;

        static void Main(string[] args)
        {
            new Program();
        }
        
        private Program()
        {
            //FillTheDatabase();

            Configuration conf = new Configuration();
            conf.TryLoad();
            m_Client = new Client(conf.IrcConnection, onMessage);
            m_IrcThread = new Thread(() =>
            {
                m_Client.Initialize();
                m_Client.Connect();
            });
            m_IrcThread.Start();
            m_Game = new QuizGame.QuizGame(m_Client);
        }

        private void onMessage(TwitchIRC.Message message)
        {
            if (message is ChannelMessage)
            {
                ChannelMessage chmsg = (ChannelMessage)message;
                if (message.Content.StartsWith("!"))
                    ParseCommand(chmsg);

                Console.WriteLine(String.Format("[ {0} ] {1}", chmsg.From, chmsg.Content));
            }
        }

        private void ParseCommand(ChannelMessage message)
        {
            int idx = message.Content.IndexOf(' ');
            string cmd = message.Content.Substring(0, (idx == -1 ? message.Content.Length : idx));
            string param = String.Empty;
            if(idx != -1)
                param = message.Content.Substring(idx + 1, message.Content.Length - idx - 1);

            m_Game.OnCommand(message.From, cmd, param);
        }

        //private void FillTheDatabase()
        //{
        //    QuizGame.Database db = new QuizGame.Database();
        //    db.Questions.Add(new QuizGame.Question()
        //    {

        //    });
        //}
    }
}
