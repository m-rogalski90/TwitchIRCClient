using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tst
{
    class Program
    {
        static void Main(string[] args)
        {
            TwitchIRC.TwitchIrc.Instance.Start();
            TwitchIRC.TwitchIrc.Instance.SetMessageCallback(OnMessage);


            while (true) ;
        }

        static void OnMessage(TwitchIRC.Message message)
        {
            
        }
    }
}
