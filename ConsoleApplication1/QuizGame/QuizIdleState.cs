using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatGame.QuizGame
{
    using TwitchIRC.Game;
    using System.Threading;

    internal class QuizIdleState : GameState
    {
        // in this state we are waiting for the new round so this should contain
        //  only the timer thread or some simple timing op to wait for start

        private Thread m_WaitThread;

        internal QuizIdleState(Game game) : base(game)
        {
            this.m_StateName = "Idle";
        }

        public override void Start()
        {
            m_Parent.SendMessage(String.Format("Started {0} state.", m_StateName));
            m_WaitThread = new Thread(() =>
            {
                int interval = 30 * 1000;/*5 * 60 * 1000;*/ 
                Thread.Sleep(interval);
                m_Parent.StateEnded();
            });
            m_WaitThread.Start();
        }

        public override void Stop()
        {

        }

        protected override void InitializeState()
        {

        }
    }
}
