using System.IO;
using System.Net;

namespace Phoenix
{
    public class Request
    {
        public static void Send(string endpoint, string method, string auth, string json = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v{Config.APIVersion}{endpoint}");
                ServicePointManager.DefaultConnectionLimit = 5000;
                if (Config.IsBot == true)
                {
                    request.Headers.Add("Authorization", $"Bot {auth}");
                }
                else
                {
                    request.Headers.Add("Authorization", auth);
                }
                request.Method = method;
                if (!string.IsNullOrEmpty(json))
                {
                    request.ContentType = "application/json";
                    using (var stream = new StreamWriter(request.GetRequestStream()))
                    {
                        stream.Write(json);
                    }
                }
                else
                {
                    request.ContentLength = 0;
                }
                request.GetResponse();
                request.Abort();
            }
            catch { }
        }

        public static string SendGet(string endpoint, string auth, string method = null, string json = null)
        {
            string text;
            var request = (HttpWebRequest)WebRequest.Create($"https://discord.com/api/v{Config.APIVersion}{endpoint}");
            if (Config.IsBot == true)
            {
                request.Headers.Add("Authorization", $"Bot {auth}");
            }
            else
            {
                request.Headers.Add("Authorization", auth);
            }
            if (string.IsNullOrEmpty(method))
            {
                request.Method = "GET";
            }
            else
            {
                request.Method = method;
            }
            if (!string.IsNullOrEmpty(json))
            {
                request.ContentType = "application/json";
                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(json);
                }
            }
            else
            {
                request.ContentLength = 0;
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                text = stream.ReadToEnd();
                stream.Dispose();
            }
            request.Abort();
            response.Close();
            return text;
        }
    }
}
