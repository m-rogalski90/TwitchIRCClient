﻿using System;
using TwitchIRC.Core;

namespace TwitchIRC.Game
{
    public abstract class GameState
    {
        protected Command[] m_StateCommands;
        protected String m_StateName;
        protected Game m_Parent;

        public String StateName
        {
            get { return m_StateName; }
        }

        public GameState(Game game)
        {
            m_Parent = game;
            InitializeState();
        }

        protected abstract void InitializeState();
        public abstract void Start();
        public abstract void Stop();
        public abstract bool AcceptCommand(String cmd);
        public abstract void ExecuteCommand(Boolean player, String from, String cmd, String param = "");
    }
}
