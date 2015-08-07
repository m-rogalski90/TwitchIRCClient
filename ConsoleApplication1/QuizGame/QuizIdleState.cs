using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.QuizGame
{
    using ConsoleApplication1.Game;
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

        public override void ExecuteCommand(bool player, string from, string cmd, string param = "")
        {
            //unused in this state
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

        public override bool AcceptCommand(string cmd)
        {
            return false;
        }

        protected override void InitializeState()
        {

        }
    }
}
