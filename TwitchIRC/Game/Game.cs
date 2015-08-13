using System;
using System.Collections.Generic;
using TwitchIRC.Core;

namespace TwitchIRC.Game
{
    public delegate void GameStateChangedHandler();
    
    public abstract class Game
    {
        public GameStateChangedHandler GameStateChanged;

        protected List<Player> m_Players;

        public List<GameState> m_GameStates;

        protected GameState m_CurrentState;
        protected String m_GameName;
        protected Irc m_Client;

        public Int32 PlayersCount
        {
            get { return m_Players.Count; }
        }

        public Irc Client
        {
            get { return m_Client; }
        }

        public Game(Irc client)
        {
            m_Client = client;
            m_GameStates = new List<GameState>();
            InitializeGame();
            if (m_GameStates.Count > 0)
            {
                m_CurrentState = m_GameStates[0];
                m_CurrentState.Start();
            }
        }

        protected abstract void InitializeGame();
        public abstract void SendMessage(String message, int channel = 0);
        public abstract bool SwitchToState(int idx);
        public abstract void StateEnded();
        public abstract void ResetPlayersCounter();
        public abstract void AddPlayer(String name, Int32 max = 10);
        public abstract void AddPlayerPoints(String name, Int32 ammount);
    }
}
