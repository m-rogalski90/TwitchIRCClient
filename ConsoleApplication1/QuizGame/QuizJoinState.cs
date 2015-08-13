using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatGame.QuizGame
{
    using TwitchIRC.Core;
    using TwitchIRC.Game;
    using System.Threading;

    internal class QuizJoinState : GameState
    {
        private int m_MaxPlayersCount;

        private Thread m_WaitingTimer;

        internal QuizJoinState(Game game) : base(game)
        {
            m_MaxPlayersCount = 10;
            m_StateName = "Join";
        }
        public override void Start()
        {
            m_Parent.Client.AddCommand(new Command("!join", OnJoinCommand));
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
            m_Parent.Client.RemoveCommand("!join");
        }

        protected override void InitializeState()
        {
        }

        private void OnJoinCommand(string from, string[] param)
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
