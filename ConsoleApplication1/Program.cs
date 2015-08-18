using System;
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
            ConnectionDetails cd = new ConnectionDetails();
            cd.ReadConfigFile();
            // this will break...
            TwitchIRC.WebApi.WebApiRequest.Instance.SetChannelDetails("mrgalski", cd.Password, new TwitchIRC.WebApi.Channels.Channel()
            {
                status = "test from the API",
                delay = 0,
                game = "Programming"
            });
            TwitchIRC.WebApi.WebApiRequest.Instance.GetChannelInfo(cd.Password);
        }

        private void onMessage(ChannelMessage message)
        {
            Console.WriteLine(String.Format("[ {0} ] {1}", message.From, message.Message));
        }
    }
}
