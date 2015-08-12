using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchChatGame.QuizGame
{
    using TwitchIRC.Game;

    internal class QuizEndState : GameState
    {
        internal QuizEndState(Game game) : base(game)
        {
            this.m_StateName = "End";
        }

        public override void Start()
        {
            m_Parent.SendMessage("Current round is over. New round will start soon!");
            m_Parent.StateEnded();
        }

        public override void Stop()
        {
            
        }

        protected override void InitializeState()
        {

        }
    }
}
