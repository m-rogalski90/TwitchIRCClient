using Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace TwitchIRC.WebApi
{
    public class WebApiRequest
    {
        private static WebApiRequest m_Instance;
        public static WebApiRequest Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new WebApiRequest();

                return m_Instance;
            }
        }

        //https://api.twitch.tv/kraken/chat/mrgalski
        //GET /chat/:channel/badges
        //public typehere GetBages(string channel){body here}
        /// <summary>
        /// https://github.com/justintv/Twitch-API/blob/master/v3_resources/channels.md#put-channelschannel
        /// PUT /channels/:channel/
        /// </summary>
        /// <param name="p1">channel name</param>
        /// <param name="p2">oauth2 token</param>
        /// <param name="p3">Channels.Channel object</param>
        /// <returns>TRUE if request succeded, FALSE otherwise</returns>
        public bool SetChannelDetails(string p1, string p2, Channels.Channel p3) // .... 
        {
            Debug.Write("from set channel");
            string json = string.Format("channel[status]={0}&channel[game]={1}&channel[delay]={2}",
                p3.status, p3.game, p3.delay);
            string response = string.Empty;
            string url = string.Format("https://api.twitch.tv/kraken/channels/{0}", p1);
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(url);
                req.Method = "POST";
                req.Headers.Set(HttpRequestHeader.Authorization, string.Format("OAuth {0}", p2));
                using (Stream stream = req.GetRequestStream())
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(json);
                    stream.Write(buffer, 0, buffer.Length);
                }
                WebResponse resp = req.GetResponse();
                using (StreamReader stream = new StreamReader(resp.GetResponseStream()))
                {
                    StringBuilder sb = new StringBuilder();
                    while (!stream.EndOfStream)
                        sb.AppendLine(stream.ReadLine());

                    response = sb.ToString();
                }
                Debug.Write(response);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// https://github.com/justintv/Twitch-API/blob/master/v3_resources/channels.md#get-channel
        /// GET /channel
        /// </summary>
        /// <returns>TRUE if request succeded, FALSE otherwise</returns>
        public bool GetChannelInfo(string p1)
        {
            Debug.Write("from get channel");
            string response = string.Empty;
            string url = "https://api.twitch.tv/kraken/channel";
            try
            {
                HttpWebRequest req = WebRequest.CreateHttp(url);
                req.Method = "GET";
                req.Headers.Set(HttpRequestHeader.Authorization, string.Format("OAuth {0}", p1));
                WebResponse resp = req.GetResponse();
                using (StreamReader stream = new StreamReader(resp.GetResponseStream()))
                {
                    StringBuilder sb = new StringBuilder();
                    while (!stream.EndOfStream)
                        sb.AppendLine(stream.ReadLine());

                    response = sb.ToString();
                }
                Debug.Write(response);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
