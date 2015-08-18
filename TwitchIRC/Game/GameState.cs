using System;

namespace TwitchIRC.Game
{
    /// <summary>
    /// Base class for each GameState
    /// </summary>
    public abstract class GameState
    {
        /// <summary>
        /// Game in which the GameState will be "played"(?)
        /// </summary>
        protected Game m_Parent;
        /// <summary>
        /// Name of the game state
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// Returns the name of the game state
        /// </summary>
        public string Name { get { return m_Name; } }

        /// <summary>
        /// Initializes newsly created gamestate within specified Game and with the specified name
        /// </summary>
        /// <param name="parent">Game to which the GameState belongs</param>
        /// <param name="name">Name of the state</param>
        public GameState(Game parent, string name)
        {
            this.m_Parent = parent;
            this.m_Name = name;
        }

        /// <summary>
        /// This method is called always when Game wants to initialize this state
        /// </summary>
        public abstract void InitializeState();
        /// <summary>
        /// This method is called always when the Game wants to start this particular GameState
        /// </summary>
        public abstract void StartState();
        /// <summary>
        /// This method is called always when GameState has ended
        /// NOTE: You must inform the Game if this state ends.
        /// </summary>
        public abstract void CleanUp();
    }

    /// <summary>
    /// Repetetive game state is used to create state that needs to be repeated
    /// like within the quiz game you should use this state to print questions to the user
    /// and reapeat while questions list is not empty.
    /// </summary>
    public abstract class RepetetiveState : GameState
    {
        /// <summary>
        /// Creates a new instance of the repetetive game state
        /// </summary>
        /// <param name="parent">Game to which belongs this state</param>
        /// <param name="name">Name of the state</param>
        public RepetetiveState(Game parent, string name) : base(parent, name) { }

        /// <summary>
        /// Condition that will be checked after State will end
        /// Id condition returns TRUE this state will repeat itself 
        /// automatically, otherwise it will start the next state in
        /// the queue
        /// </summary>
        /// <returns>TRUE if should repeat, FALSE otherwise</returns>
        public abstract bool CheckCondition();
    }

    /// <summary>
    /// Single game state is used only for non repetetive tasks such as "join to the game" state or "idle state"
    /// where it should only be called once per GameLoop.
    /// </summary>
    public abstract class SingleState : GameState
    {
        /// <summary>
        /// Creates a new instance of the SingleState object
        /// </summary>
        /// <param name="parent">Game to which this state belongs</param>
        /// <param name="name">Name of the state</param>
        public SingleState(Game parent, string name) : base(parent, name) { }
    }
}
