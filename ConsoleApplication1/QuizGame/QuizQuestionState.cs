﻿using System;
using System.Linq;
using System.Threading;

namespace ConsoleApplication1.QuizGame
{
    using ConsoleApplication1.Game;

    internal class QuizQuestionState : GameState
    {
        private Question m_CurrentQuestion;
        private Thread m_QuestionTimerThread;

        internal QuizQuestionState(Game game) : base(game)
        {
            this.m_StateName = "Question";
        }

        public void SetCurrentQuestion(Question q)
        {
            this.m_CurrentQuestion = q;
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
            if (player)
                m_StateCommands.First(c => c.Command == cmd).ExecuteCommand(from, param);
        }

        public override void Start()
        {
            /// have to make it a little bit different
            m_Parent.SendMessage("Q: " + m_CurrentQuestion.Content);
            int rand = new Random().Next(0, 3);
            m_CurrentQuestion.Answers[rand] = m_CurrentQuestion.Correct;
            m_Parent.SendMessage("a) " + m_CurrentQuestion.Answers[0]);
            m_Parent.SendMessage("b) " + m_CurrentQuestion.Answers[1]);
            m_Parent.SendMessage("c) " + m_CurrentQuestion.Answers[2]);
            m_Parent.SendMessage("d) " + m_CurrentQuestion.Answers[3]);
            m_QuestionTimerThread = new Thread(() =>
            {
                Thread.Sleep(1000 * m_CurrentQuestion.AnswerTime);
                m_Parent.StateEnded(); // this may be executed even when thread is terminated?
            });
            m_QuestionTimerThread.Start();
        }

        public override void Stop()
        {

        }

        protected override void InitializeState()
        {
            m_StateCommands = new StateCommand[]
            {
                new StateCommand(OnAnswerCommand, "!answer")
            };
        }

        private void OnAnswerCommand(string from, string param)
        {
            lock(new object())
            {
                if (string.IsNullOrEmpty(param))
                {
                    m_Parent.SendMessage("@{0} you have to specify the answer. example: !answer a");
                    return;
                }
                else
                {
                    int answer_id = param.ToLower()[0] - 97;
                    if(answer_id < 0 || answer_id >= m_CurrentQuestion.Answers.Length)
                    {
                        m_Parent.SendMessage(String.Format("Wrong @{0}! That was not the correct answer. Please try again.",
                            from));
                        return;
                    }
                    else if (m_CurrentQuestion.Answers[answer_id] == m_CurrentQuestion.Correct)
                    {
                        m_QuestionTimerThread.Abort();
                        m_Parent.SendMessage(String.Format("Congratulations @{0}! That was the correct answer. You have been rewarded with {1} points.",
                            from, m_CurrentQuestion.Points));
                        m_Parent.StateEnded();
                        m_Parent.AddPlayerPoints(from, m_CurrentQuestion.Points);
                    }
                    else
                    {
                        m_Parent.SendMessage(String.Format("Wrong @{0}! That was not the correct answer. Please try again.",
                            from));
                    }
                }
            }
        }
    }
}
