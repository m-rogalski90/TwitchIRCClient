using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TwitchIRC.windows.popups
{
    /// <summary>
    /// Interaction logic for PopupMessage.xaml
    /// </summary>
    public partial class PopupMessage : Window
    {
        enum PopupState
        {
            slide,
            display,
            die
        }

        private int display_interval;
        private int die_interval;
        private int frame_delay;

        private double m_TopOffset;
        private bool is_there;
        private Timer m_Timer;
        
        private PopupState mPopupState;

        // if this shit will work ... 
        public PopupMessage(string from, string content)
        {
            InitializeComponent();

            this.m_From.Text = from;
            this.m_Content.Text = content;
            Initialize();
            Calculate();
            Start();
            this.Show();
        }

        private void Initialize()
        {
            m_TopOffset = 0;
            mPopupState = PopupState.slide;
            this.PopupMessageWindow.Left = -this.Width;
            this.PopupMessageWindow.Top = m_TopOffset;
        }

        private void Start()
        {
            m_Timer = new Timer(onNewFrame, null, 0, frame_delay);
            this.Closing += (sender, e) =>
            {
                m_Timer.Dispose();
                m_Timer = null;
            };
        }

        private void Calculate()
        {
            frame_delay = 1000 / PopupConfig.Instance.FramesPerSecond;
            die_interval = (1000 * 30) / 100;
            display_interval = 10 * 1000;
        }

        private void onNewFrame(object state)
        {
            if(this.PopupMessageWindow.Top < m_TopOffset)
            {
                this.PopupMessageWindow.Top += PopupConfig.Instance.PixelsPerFrame;
                if (this.PopupMessageWindow.Top > m_TopOffset)
                    this.PopupMessageWindow.Top = m_TopOffset;
            }
            else if(this.PopupMessageWindow.Top > m_TopOffset)
            {
                this.PopupMessageWindow.Top -= PopupConfig.Instance.PixelsPerFrame;
                if (this.PopupMessageWindow.Top < m_TopOffset)
                    this.PopupMessageWindow.Top = m_TopOffset;
            }

            switch (mPopupState)
            {
                case PopupState.slide:
                    this.PopupMessageWindow.Left += PopupConfig.Instance.PixelsPerFrame;
                    if (this.PopupMessageWindow.Left > PopupConfig.Instance.Margin)
                    {
                        this.PopupMessageWindow.Left = PopupConfig.Instance.Margin;
                        mPopupState = PopupState.display;
                    }
                    break;

                case PopupState.display:
                    display_interval -= frame_delay;
                    if (display_interval <= 0)
                        mPopupState = PopupState.die;
                    break;

                case PopupState.die:
                    this.PopupMessageWindow.Opacity -= display_interval;
                    if (display_interval <= 0)
                        this.Close();
                    break;
            }
        }
        
        private void onMouseEnterClose(object sender, MouseEventArgs e)
        {
            m_Close.Foreground = Brushes.Red;
        }

        private void onMouseLeaveClose(object sender, MouseEventArgs e)
        {
            m_Close.Foreground = Brushes.Black;
        }

        private void onMouseClickClose(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
