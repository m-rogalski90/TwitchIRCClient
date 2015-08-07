using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.QuizGame
{
    using ConsoleApplication1.Game;

    internal class QuizEndState : GameState
    {
        internal QuizEndState(Game game) : base(game)
        {
            this.m_StateName = "End";
        }

        public override bool AcceptCommand(string cmd)
        {
            return false;
        }

        // i could make something like Executecommand and ExecutePlayerCommand...
        public override void ExecuteCommand(bool player, string from, string cmd, string param = "")
        {

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
