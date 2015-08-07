using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.QuizGame
{
    internal class Question
    {
        private String m_Content;
        private Int32 m_Points;
        private Int32 m_AnswerTime;
        private String m_Correct;
        private String[] m_Answers;

        public String Content
        {
            get { return m_Content; }
            internal set { m_Content = value; }
        }
        public Int32 Points
        {
            get { return m_Points; }
            internal set { m_Points = value; }
        }
        public Int32 AnswerTime
        {
            get { return m_AnswerTime; }
            internal set { m_AnswerTime = value; }
        }
        public String Correct
        {
            get { return m_Correct; }
            internal set { m_Correct = value; }
        }
        public String[] Answers
        {
            get { return m_Answers; }
            internal set { m_Answers = value; }
        }
    }
}
