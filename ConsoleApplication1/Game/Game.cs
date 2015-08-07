using System;
using System.Collections.Generic;

using TwitchIRC;

namespace ConsoleApplication1.Game
{
    public delegate void GameStateChangedHandler();
    
    public abstract class Game
    {
        public GameStateChangedHandler GameStateChanged;

        protected List<Player> m_Players;

        public List<GameState> m_GameStates;

        protected GameState m_CurrentState;
        protected String m_GameName;
        protected Client m_Client;

        public Int32 PlayersCount
        {
            get { return m_Players.Count; }
        }

        public Game(Client client)
        {
            this.m_Client = client;
            this.m_GameStates = new List<GameState>();
            InitializeGame();
            if (m_GameStates.Count > 0)
            {
                m_CurrentState = m_GameStates[0];
                m_CurrentState.Start();
            }
        }

        protected abstract void InitializeGame();
        public abstract void SendMessage(String message);
        public abstract bool SwitchToState(int idx);
        public abstract void OnCommand(String from, String command, String param = "");
        public abstract void StateEnded();
        public abstract void ResetPlayersCounter();
        public abstract void AddPlayer(String name, Int32 max = 10);
        public abstract void AddPlayerPoints(String name, Int32 ammount);
    }
}
