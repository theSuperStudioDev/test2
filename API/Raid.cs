using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Console = Colorful.Console;
using Newtonsoft.Json.Linq;

/* 
       │ Author       : extatent
       │ Name         : API
       │ GitHub       : https://github.com/extatent
*/

namespace API
{
    public class Raid
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;

        public static void JoinGuild(string token, string code)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Post($"https://discord.com/api/v{apiv}/invites/{code}");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGuild(string token, ulong id)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Delete($"https://discord.com/api/v{apiv}/users/@me/guilds/{id}");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddFriend(string token, string username, uint discriminator)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me/relationships");
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/users/@me/relationships");
                request.Content = new System.Net.Http.StringContent($"{{\"username\":\"{username}\",\"discriminator\":{discriminator}}}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
                Console.WriteLine("Success: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void SendMessage(string token, ulong cid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{cid}/messages");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/channels/{cid}/messages");
                request.Content = new System.Net.Http.StringContent("{\"content\":\"" + message + "\"}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
                Console.WriteLine("Success: " + token, Color.Lime);
                Thread.Sleep(WaitTimeShort);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddReaction(string token, ulong cid, ulong mid, string emoji)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Put($"https://discord.com/api/v{apiv}/channels/{cid}/messages/{mid}/reactions/{emoji}/@me");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void JoinGroup(string token, string code)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Post($"https://discord.com/api/v{apiv}/invites/{code}");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void BlockUser(string token, ulong uid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me/relationships/{uid}");
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, $"https://discord.com/api/v{apiv}/users/@me/relationships/{uid}");
                request.Content = new System.Net.Http.StringContent("{\"type\":\"2\"}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
                Console.WriteLine("Success: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGroup(string token, ulong gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Delete($"https://discord.com/api/v{apiv}/channels/{gid}");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void TriggerTyping(string token, ulong cid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Post($"https://discord.com/api/v{apiv}/channels/{cid}/typing");
                    Console.WriteLine("Success: " + token, Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }
    }
}
