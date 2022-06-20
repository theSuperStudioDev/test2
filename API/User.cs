using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

/* 
       │ Author       : extatent
       │ Name         : API
       │ GitHub       : https://github.com/extatent
*/

namespace API
{
    public class User
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;

        public static void MassDM(string token, string message)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/users/@me/channels");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                        request.Content = new System.Net.Http.StringContent("{\"content\":\"" + message + "\"}", Encoding.UTF8, "application/json");
                        client.SendAsync(request);
                        Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteDMs(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/users/@me/channels");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/channels/{entry.id}");
                        Console.WriteLine($"Deleted: {entry.recipients[0].username}#{entry.recipients[0].discriminator}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void BlockRelationships(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse channelid = req.Get($"https://discord.com/api/v{apiv}/users/@me/relationships");
                    var array = JArray.Parse(channelid.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me/relationships/{entry.id}");
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Put, $"https://discord.com/api/v{apiv}/users/@me/relationships/{entry.id}");
                        request.Content = new System.Net.Http.StringContent("{\"type\":\"2\"}", Encoding.UTF8, "application/json");
                        client.SendAsync(request);
                        Console.WriteLine($"Blocked: {entry.user.username}#{entry.user.discriminator}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void UserInformation(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse userinfo = req.Get($"https://discord.com/api/v{apiv}/users/@me");
                    var id = JObject.Parse(userinfo.ToString())["id"];
                    var getbadges = JObject.Parse(userinfo.ToString())["flags"].ToString();
                    string badges = "";
                    if (getbadges == "1")
                    {
                        badges += "Staff, ";
                    }
                    if (getbadges == "2")
                    {
                        badges += "Partner, ";
                    }
                    if (getbadges == "4")
                    {
                        badges += "HypeSquad Events, ";
                    }
                    if (getbadges == "8")
                    {
                        badges += "Bug Hunter Level 1, ";
                    }
                    if (getbadges == "64")
                    {
                        badges += "HypeSquad Bravery, ";
                    }
                    if (getbadges == "128")
                    {
                        badges += "HypeSquad Brilliance, ";
                    }
                    if (getbadges == "256")
                    {
                        badges += "HypeSquad Balance, ";
                    }
                    if (getbadges == "512")
                    {
                        badges += "Early Supporter, ";
                    }
                    if (getbadges == "16384")
                    {
                        badges += "Bug Hunter Level 2, ";
                    }
                    if (getbadges == "131072")
                    {
                        badges += "Verified Bot Developer, ";
                    }
                    var email = JObject.Parse(userinfo.ToString())["email"].ToString();
                    var phone = JObject.Parse(userinfo.ToString())["phone"].ToString();
                    var bio = JObject.Parse(userinfo.ToString())["bio"].ToString();
                    var locale = JObject.Parse(userinfo.ToString())["locale"].ToString();
                    var nsfw = JObject.Parse(userinfo.ToString())["nsfw_allowed"].ToString();
                    var mfa = JObject.Parse(userinfo.ToString())["mfa_enabled"].ToString();
                    var avatarid = JObject.Parse(userinfo.ToString())["avatar"].ToString();
                    string avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                    req.Close();
                    req.AddHeader("Authorization", token);
                    HttpResponse userinfo2 = req.Get($"https://discord.com/api/v10/users/@me/settings");
                    var theme = JObject.Parse(userinfo2.ToString())["theme"].ToString();
                    var devmode = JObject.Parse(userinfo2.ToString())["developer_mode"].ToString();
                    var status = JObject.Parse(userinfo2.ToString())["status"].ToString();
                    req.Close();

                    Console.WriteLine("User Information:\n");
                    Console.WriteLine($"ID: {id}\nEmail: {email}\nPhone Number: {phone}\nBiography: {bio}\nLocale: {locale}\nNSFW Allowed: {nsfw}\n2FA Enabled: {mfa}\nBadges: {badges}\nTheme: {theme}\nDeveloper Mode: {devmode}\nStatus: {status}\nAvatar: {avatar}");
                    Console.WriteLine("\nSubscription Information:\n");
                    Console.WriteLine("SOON");
                    Console.WriteLine("\nBilling Information:\n");
                    Console.WriteLine("SOON");
                    Console.WriteLine("\nPress any key to go back.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch { }
        }

        public static void ConfuseMode(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v10/users/@me/settings");
                    var array = JObject.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri($"https://discord.com/api/v10/users/@me/settings");
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request = new HttpRequestMessage(new System.Net.Http.HttpMethod("PATCH"), $"https://discord.com/api/v10/users/@me/settings");
                        request.Content = new System.Net.Http.StringContent("{\"locale\": \"zh-CN\",\"theme\": \"light\", \"developer_mode\": \"false\", \"message_display_compact\": \"true\", \"explicit_content_filter\": \"0\"}", Encoding.UTF8, "application/json");
                        client.SendAsync(request);
                    }
                }
            }
            catch { }
        }

        public static void ChangeTheme(string token, string theme)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v10/users/@me/settings");
                    var array = JObject.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri($"https://discord.com/api/v10/users/@me/settings");
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request = new HttpRequestMessage(new System.Net.Http.HttpMethod("PATCH"), $"https://discord.com/api/v10/users/@me/settings");
                        request.Content = new System.Net.Http.StringContent("{\"theme\": \"" + theme + "\"}", Encoding.UTF8, "application/json");
                        client.SendAsync(request);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void CreateGuild(string token, string name)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds");
                request.Content = new System.Net.Http.StringContent("{\"name\": \"" + name + "\"}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
                Thread.Sleep(WaitTimeShort);
            }
            catch { }
        }

        public static void RemoveConnections(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/users/@me/connections");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/users/@me/connections/{entry.type}/{entry.id}");
                        Console.WriteLine($"Removed: {entry.type}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeauthorizeApps(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/oauth2/tokens");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/oauth2/tokens/{entry.id}");
                        Console.WriteLine("Removed: " + entry.application["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void LeaveHypeSquad(string token)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/hypesquad/online");
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/hypesquad/online");
                client.SendAsync(request);
            }
            catch { }
        }

        public static void ClearRelationships(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/users/@me/relationships");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/users/@me/relationships/{entry.id}");
                        Console.WriteLine($"Removed: {entry.user.username}#{entry.user.discriminator}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void LeaveDeleteGuilds(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse guildid = req.Get($"https://discord.com/api/v{apiv}/users/@me/guilds");
                    var array = JArray.Parse(guildid.ToString());
                    req.Close();

                    foreach (dynamic guild in array)
                    {
                        if (guild.owner == true)
                        {
                            req.AddHeader("Authorization", token);
                            req.Delete($"https://discord.com/api/v{apiv}/guilds/{guild.id}");
                            Console.WriteLine("Deleted: " + guild.name);
                            Thread.Sleep(WaitTimeShort);
                        }
                        else
                        {
                            req.AddHeader("Authorization", token);
                            req.Delete($"https://discord.com/api/v{apiv}/users/@me/guilds/{guild.id}");
                            Console.WriteLine("Left: " + guild.name);
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                }
            }
            catch { }
        }

        public static void EditProfile(string token, string hypesquad = null, string bio = null, string status = null)
        {
            try
            {
                if (hypesquad == "0")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/hypesquad/online");
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/hypesquad/online");
                    client.SendAsync(request);
                }
                else if (hypesquad == "1")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/hypesquad/online");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/hypesquad/online");
                    request.Content = new System.Net.Http.StringContent("{\"house_id\": 1}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                }
                else if (hypesquad == "2")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/hypesquad/online");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/hypesquad/online");
                    request.Content = new System.Net.Http.StringContent("{\"house_id\": 2}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                }
                else if (hypesquad == "3")
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/hypesquad/online");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/hypesquad/online");
                    request.Content = new System.Net.Http.StringContent("{\"house_id\": 3}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                }

                if (!string.IsNullOrEmpty(bio))
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(new System.Net.Http.HttpMethod("PATCH"), $"https://discord.com/api/v{apiv}/users/@me");
                    request.Content = new System.Net.Http.StringContent("{\"bio\": \"" + bio + "\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                }

                if (!string.IsNullOrEmpty(status))
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me/settings");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(new System.Net.Http.HttpMethod("PATCH"), $"https://discord.com/api/v{apiv}/users/@me/settings");
                    request.Content = new System.Net.Http.StringContent("{\"custom_status\": {\"text\": \"" + status + "\"}}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                }
            }
            catch { }
        }

        public static string GetUsername(string token)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse username = req.Get($"https://discord.com/api/v{apiv}/users/@me");
                    var usernames = JObject.Parse(username.ToString())["username"];
                    var discriminator = JObject.Parse(username.ToString())["discriminator"];
                    req.Close();
                    return usernames + "#" + discriminator;
                }
            }
            catch { return "N/A"; }
        }
    }
}
