using System;
using System.Collections.Generic;
using System.Linq;
using TwitchIRC.Core;

namespace TwitchIRC.Game
{
    /// <summary>
    /// Object used as a basic and generic
    ///  chat/text based game engine
    /// </summary>
    public class Game
    {
        private Irc m_IrcClient;
        private List<GameState> m_States;
        private GameState m_CurrentState;
        
        /// <summary>
        /// Returns the Irc object
        /// </summary>
        public Irc IrcClient { get { return m_IrcClient; } }

        /// <summary>
        /// Initializes a new instance of the game object
        /// </summary>
        public Game()
        {
            m_States = new List<GameState>();
        }

        /// <summary>
        /// Used to set the Irc client that will be used
        /// to send messages to the Irc
        /// </summary>
        /// <param name="client">Irc that you want to use to send messages with</param>
        public void SetIrcClient(Irc client)
        {
            this.m_IrcClient = client;
        }

        /// <summary>
        /// Used to add a GameState object to the game
        /// </summary>
        /// <param name="state">GameState you want to add</param>
        public void AddState(GameState state)
        {
            m_States.Add(state);
        }

        /// <summary>
        /// Used to remove GameState from the game by the name
        /// </summary>
        /// <param name="state_name">Name of the state you want to remove</param>
        public void RemoveState(string state_name)
        {
            RemoveState(m_States.First(s => s.Name == state_name));
        }

        /// <summary>
        /// Used to remove the GameState from the game
        /// </summary>
        /// <param name="state">GameState object that you want to remove</param>
        public void RemoveState(GameState state)
        {
            m_States.Remove(state);
        }

        /// <summary>
        /// Used to start the game. Game starts with the GameState added first
        /// and when this state ends it jumps to another in the list.
        /// </summary>
        public void StartGame()
        {
            if (m_States == null)
                throw new Exception("States wasn't initialized.");

            if (m_States.Count == 0)
                throw new Exception("States list is empty");

            m_CurrentState = m_States[0];
            StartState();
        }

        /// <summary>
        /// Must be called by the GameState to inform the game that 
        /// the new state should be started.
        /// </summary>
        public void StateEnded()
        {
            m_CurrentState.CleanUp();
            if (m_CurrentState is RepetetiveState)
            {
                if (((RepetetiveState)m_CurrentState).CheckCondition())
                {
                    ((RepetetiveState)m_CurrentState).StartState();
                    return;
                }
            }
            m_CurrentState = NextState();
            StartState();
        }

        private GameState NextState()
        {
            int idx = m_States.IndexOf(m_CurrentState);
            if (idx < m_States.Count)
                idx++;
            else
                idx = 0;

            return m_States[idx];
        }

        private void StartState()
        {
            m_CurrentState.InitializeState();
            m_CurrentState.StartState();
        }
    }
}
