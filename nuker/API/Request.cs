using System.IO;
using System.Net;

namespace nuker
{
    public class Request
    {
        public static void Send(string url, string method, string token, string json = null)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            ServicePointManager.DefaultConnectionLimit = 5000;
            request.Headers.Add("Authorization", token);
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

        public static string SendGet(string url, string token)
        {
            string text;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", token);
            request.Method = "GET";
            request.ContentLength = 0;
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
