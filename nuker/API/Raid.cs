using System.Drawing;
using Console = Colorful.Console;

namespace nuker
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
                Request.Send($"https://discord.com/api/v{apiv}/invites/{code}", "POST", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGuild(string token, ulong? id)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/users/@me/guilds/{id}", "DELETE", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddFriend(string token, string username, uint discriminator)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/users/@me/relationships", "POST", token, $"{{\"username\":\"{username}\",\"discriminator\":{discriminator}}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void SendMessage(string token, ulong? cid, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/channels/{cid}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void AddReaction(string token, ulong? cid, ulong? mid, string emoji)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/channels/{cid}/messages/{mid}/reactions/{emoji}/@me", "PUT", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void BlockUser(string token, ulong? uid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/users/@me/relationships/{uid}", "PUT", token, $"{{\"type\":\"2\"}}");
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void LeaveGroup(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/channels/{gid}", "DELETE", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }

        public static void TriggerTyping(string token, ulong? cid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/channels/{cid}/typing", "POST", token);
                Console.WriteLine("Succeed: " + token, Color.Lime);
            }
            catch { Console.WriteLine("Failed: " + token, Color.Red); }
        }
    }
}
