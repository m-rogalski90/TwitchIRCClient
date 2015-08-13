using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace TwitchChatGame.QuizGame
{
    using System.Threading.Tasks;
    using TwitchIRC.Core;
    using TwitchIRC.Game;

    internal class QuizGame : Game
    {
        private String m_DatabaseFile = String.Format("{0}{1}data{1}database.xml",
            Environment.CurrentDirectory, Path.DirectorySeparatorChar);

        private List<Question> m_Questions;
        protected Database m_Database;

        public QuizGame(Irc client) : base(client)
        {
            this.m_GameName = "Quiz";
        }

        protected override void InitializeGame()
        {
            // create new game states here...
            var idle_state = new QuizIdleState(this);
            this.m_GameStates.Add(idle_state);
            var join_state = new QuizJoinState(this);
            this.m_GameStates.Add(join_state);
            var question_state = new QuizQuestionState(this);
            this.m_GameStates.Add(question_state);
            var end_state = new QuizEndState(this);
            this.m_GameStates.Add(end_state);

            this.m_Client.AddCommand(new Command("!stats", onStatsCommand));
            this.m_Client.AddCommand(new Command("!reset", onResetCommand));

            m_Database = new Database();
            if (File.Exists(m_DatabaseFile))
                m_Database.ReadXml(m_DatabaseFile, System.Data.XmlReadMode.Auto /*which is the default :oo*/);

        }

        public void onResetCommand(string from, string[] param)
        {
            SendMessage(String.Format("@{0}, NO!", from));
        }

        public void onStatsCommand(string from, string[] param)
        {
            Task t = new Task(() =>
            {
                if (!m_Database.Players.Any(p => p.PlayerName == from))
                {
                    SendMessage(String.Format("There is no player with @{0} nickname in our database.", from));
                    return;
                }
                var all = m_Database.Players.OrderByDescending(p => p.PlayerPoints).ToList();
                int place = all.IndexOf(m_Database.Players.First(p => p.PlayerName == from)) + 1;
                int points = m_Database.Players.First(p => p.PlayerName == from).PlayerPoints;
                SendMessage(String.Format("@{0} -> You have {1} points what gives you {2} place in the ranking.",
                    from, points, place));
            });
            t.Start();
        }

        public override void StateEnded()
        {
            int idx = m_GameStates.FindIndex(s => s.StateName == m_CurrentState.StateName);
            idx++;
            if (m_CurrentState.StateName == "Join")
            {
                if (m_Players.Count <= 1)
                    idx = 0;

                // make this read from the file...
                m_Questions = new List<Question>();
                m_Questions.AddRange(new Question[3]
                {
                    new Question()
                    {
                        Points = 5,
                        Answers = new string[]
                        {
                            "correct answer",
                            "bad answer",
                            "another bad answer",
                            "yet aother bad answer"
                        },
                        Correct = "correct answer",
                        AnswerTime = 60,
                        Content = "pick the correct answer!"
                    },
                    new Question()
                    {
                        Points = 5,
                        Answers = new string[]
                        {
                            "correct answer",
                            "bad answer",
                            "another bad answer",
                            "yet aother bad answer"
                        },
                        Correct = "correct answer",
                        AnswerTime = 60,
                        Content = "pick the correct answer 2!"
                    },new Question()
                    {
                        Points = 5,
                        Answers = new string[]
                        {
                            "correct answer",
                            "bad answer",
                            "another bad answer",
                            "yet aother bad answer"
                        },
                        Correct = "correct answer",
                        AnswerTime = 60,
                        Content = "pick the correct answer 3!"
                    }
                });
            }
            if(m_CurrentState.StateName == "Question")
            {
                if(m_Questions.Count > 0)
                {
                    int index = new Random().Next(0, m_Questions.Count - 1);
                    Question q = m_Questions[index];
                    m_Questions.RemoveAt(index);
                    (m_CurrentState as QuizQuestionState).SetCurrentQuestion(q);
                    m_CurrentState.Start();
                    return;
                }
            }
            if(m_CurrentState.StateName == "End")
            {
                SaveDatabase();
            }
            if (!SwitchToState(idx))
                SwitchToState(0);
        }

        public override bool SwitchToState(int next)
        {
            if(next < m_GameStates.Count)
            {
                m_CurrentState.Stop();
                m_CurrentState = m_GameStates[next];
                if (m_GameStates[next].StateName == "Question")
                {
                    if (m_Questions.Count > 0)
                    {
                        int index = new Random().Next(0, m_Questions.Count - 1);
                        Question q = m_Questions[index];
                        m_Questions.RemoveAt(index);
                        (m_CurrentState as QuizQuestionState).SetCurrentQuestion(q);
                    }
                    else
                    {
                        next++;
                        m_CurrentState = m_GameStates[next];
                    }
                }
                m_CurrentState.Start();
                return true;
            }
            return false;
        }

        public override void SendMessage(string message, int channel = 0)
        {
            m_Client.SendMessage(message);
        }

        public override void ResetPlayersCounter()
        {
            m_Players = new List<Player>();
        }

        public override void AddPlayer(string name, int max)
        {
            if (m_Players.Any(p => p.Name == name))
                SendMessage(String.Format("@{0} you're currently participating in the game.", name));
            else
            {
                m_Players.Add(new Player()
                {
                    Name = name,
                    Points = 0
                });
                SendMessage(String.Format("@{0} has just signed in for the round! There are only {1} spots left!", name, max - PlayersCount));
            }
        }

        public override void AddPlayerPoints(string name, int ammount)
        {
            m_Players.First(p => p.Name == name).Points += ammount;
        }

        public void SaveDatabase()
        {
            Task t = new Task(() =>
            {
                foreach (Player p in m_Players)
                {
                    if (!m_Database.Players.Any(pl => pl.PlayerName == p.Name))
                        m_Database.Players.AddPlayersRow(p.Name, p.Points);
                    else
                        m_Database.Players.First(pl => pl.PlayerName == p.Name).PlayerPoints += p.Points;
                }
                m_Database.AcceptChanges();
                m_Database.WriteXml(m_DatabaseFile);
            });
            t.Start();
        }
    }
}
