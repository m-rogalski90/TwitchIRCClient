using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchIRC.windows.popups
{
    public class PopupConfig
    {
        private static PopupConfig m_Instance;
        internal static PopupConfig Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new PopupConfig();

                return m_Instance;
            }
        }

        public bool Backwards { get; set; }
        public int FramesPerSecond { get; set; }
        public int PixelsPerFrame { get; set; }
        public int Margin { get; set; }
    }
}
