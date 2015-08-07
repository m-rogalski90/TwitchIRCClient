using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.QuizGame
{
    using ConsoleApplication1.Game;
    using System.Threading;

    internal class QuizJoinState : GameState
    {
        private int m_MaxPlayersCount;

        private Thread m_WaitingTimer;

        internal QuizJoinState(Game game) : base(game)
        {
            m_MaxPlayersCount = 10;
            this.m_StateName = "Join";
        }

        public override bool AcceptCommand(string cmd)
        {
            if (m_StateCommands == null)
                return false;

            if (m_StateCommands.Any(c => c.Command == cmd))
                return true;

            return false;
        }

        public override void ExecuteCommand(bool player, string from, string cmd, string param = "")
        {
            m_StateCommands.First(c => c.Command == cmd).ExecuteCommand(from, param);
        }

        public override void Start()
        {
            m_Parent.SendMessage(String.Format("Started {0} state.", m_StateName));
            m_Parent.ResetPlayersCounter();
            m_WaitingTimer = new Thread(() =>
            {
                Thread.Sleep(60 * 1000);
                m_Parent.StateEnded();
            });
            m_WaitingTimer.Start();
        }

        public override void Stop()
        {

        }

        protected override void InitializeState()
        {
            m_StateCommands = new StateCommand[]
            {
                new StateCommand(OnJoinCommand, "!join")
            };
        }

        private void OnJoinCommand(string from, string param = "")
        {
            m_Parent.AddPlayer(from);
            if (m_Parent.PlayersCount == m_MaxPlayersCount)
            {
                m_WaitingTimer.Abort();
                m_Parent.StateEnded();
            }
        }
    }
}
