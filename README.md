# Phoenix Nuker
Discord nuker that can nuke/raid server or account with user token(s). Also has mass report bot, webhook spammer, deleter and many more.

<p align="center">
<img src="https://img.shields.io/github/languages/top/extatent/phoenix-nuker?style=flat-square" </a>
<img src="https://img.shields.io/github/last-commit/extatent/phoenix-nuker?style=flat-square" </a>
<img src="https://img.shields.io/github/license/extatent/phoenix-nuker?style=flat-square" </a>
<img src="https://img.shields.io/github/downloads/extatent/phoenix-nuker/total?label=Downloads&style=flat-square" </a>
<img src="https://img.shields.io/github/stars/extatent/phoenix-nuker?label=Stars&style=flat-square" </a>
<img src="https://img.shields.io/github/forks/extatent/phoenix-nuker?label=Forks&style=flat-square" </a>

---

**NOTE:** ⭐ If you like the project, feel free to star it ⭐
  
### If you caught any issues, please report it in [issues](https://github.com/extatent/phoenix-nuker/issues) or in our [Discord server](https://dsc.gg/extatent)

<details>
<summary>Preview</summary>
<img src="https://i.imgur.com/01DrPLV.png" alt="png">
  
<img src="https://i.imgur.com/AfyhyEn.png" alt="png">
  
<img src="https://i.imgur.com/zIibTos.png" alt="png">

<img src="https://i.imgur.com/x2THCs2.png" alt="png">

<img src="https://i.imgur.com/qycQ9P1.png" alt="png">
</details>

<h1 allign="center">Features</h1>

* ` Easy to use (read detailed features for more information)`
* ` No installing anything (just an executable file)`
* ` Always latest (updating the project everytime a new API function/version comes out)`
* ` Report bot (mass report a message)`
* ` Webhook spammer/deleter (spam/delete a webhook)`
* ` Fully destroy an account or server`
* ` MultiToken support (raid a server or user with many tokens)`
* ` Supports bot and user authentication tokens`
* ` Fast user support (GitHub/Discord)`
* ` Unique features`

<details>
<summary>Features detailed</summary>

### To find a specific feature, press CTRL+F and enter the feature name.

#### Login
If you're using the program for the first time, it'll ask you to enter the Discord authentication token, after entering, the token will be saved in config.json file. Next time you open the the program and choose the option, it'll fetch the token from config.json to auto login.

#### MultiToken Raider
Lists all the available options for MultiToken raiding. Before choosing the option, paste your authentication tokens in tokens.txt file.

#### Delete Webhook
Deletes the Discord webhook. Requires to enter the webhook URL.

#### Account Nuker
Lists all the available options for nuking an account.

#### Server Nuker
Lists all the available options for nuking a server. Requires to enter the server ID.

#### Report Bot
Mass reports a message. Requires to enter the server ID, channel ID, message ID and count.

#### Webhook Spammer
Spams your message to the entered Discord webhook. Requires to enter the webhook, message and amount of times.

#### Login To Other Account
Deletes config.json file and goes back to the login menu.

#### Join Guild/Group
Joins a guild or group. Requires to enter the guild or group invite link.

#### Leave guild
Leaves a guild. Requires to enter the server ID.

#### Add Friend
Adds a friend. Requires to enter the user ID.

#### Spam
Spams in a server. Requires to enter the channel ID, message and amount of times.

#### Add Reaction
Adds a reaction. Requires to enter the channel ID and message.

#### Block User
Blocks a user. Requires to enter the user ID.

#### DM User
Direct messages a user. Requires to enter the user ID and message.

#### Leave Group
Leaves a group with the entered group ID.

#### Trigger Typing
Shows a typing indicator in a channel. Requires to enter the channel ID.

#### Report Message
Reports a message. Requires to enter the guild ID, channel ID and message ID.

#### Edit Profile
Edits a profile (HypeSquad badge, status, biography). Requires to choose the badge, status and biography.

#### Leave Delete/Guilds
Leaves and/or deletes all guilds.

#### Clear Relationships
Clears all relationships (friends, requests, blocked users)

#### Leave HypeSquad
Removes the HypeSquad badge.

#### Remove Connections
Removes all connections (Battle.net, Epic Games, Facebook, GitHub, Playstation Network, Reddit, Spotify, Steam, Twitch, Twitter, Xbox, YouTube).

#### Deauthorize Apps
Deauthorizes all apps (authorized bots, apps, etc)

#### Mass Create Guilds
Mass creates guilds. Requires to enter the amount of guilds.

#### Seizure Mode
Switches through dark and light modes. Requires to enter the amount of times.

#### Confuse Mode
Changes the language to chinese, theme to light, turns developer mode off, enables compact mode, turns off explicit content filter.

#### Mass DM
Direct messages all friends.

#### User Info
Fetches user info from the account (email, phone number, avatar, billing, subscription information, etc).

#### Block Relationships
Blocks all relationships (friends, friend requests).

#### Delete DMs
Deletes all direct messages.

#### Delete All Roles
Deletes all roles from the server.

#### Delete All Channels
Deletes all channels from the server.

#### Delete All Emojis
Deletes all emojis from the server.

#### Delete all Invites
Deletes all invites from the server.

#### Mass Create Roles
Mass creates roles in the server. Requires to enter the name and amount of roles.

#### Mass Create Channels
Mass creates channels in the server. Requires to enter the name and amount of channels.

#### Prune Members
Prunes members in the server who haven't been active in 1 day.

#### Remove Integrations
Removes all integrations from the server (bots, apps)

#### Delete All Reactions
Deletes all reactions from the message in the server. Requires to enter the channel ID and message ID.

#### Server Info
Fetches server info from the guild (region, vanity code, server icon, banner, verification level, etc).

#### Msg In Every Channel
Messages in every channel in the server. Requires to enter the message.

#### Delete Stickers
Deletes all stickers in the server.

#### Grant Everyone Admin
Gives `Administrator` permission to `@everyone` role.

#### Delete Auto Moderation Rules
Deletes all auto moderation rules in the server.

#### Mass Create Invites
Creates a invite for each channel.

#### Delete Guild Scheduled Events
Deletes all guild scheduled events in the server.

#### Delete Guild Template
Deletes guild template in the server.

#### Delete Stage Instances
Deletes all stage instances in the server.

#### Delete Webhooks
Deletes all webhooks in the server.

#### Switch To Other Server
Goes to "Enter Server ID" menu.

#### Ban All Members
Bans all members from the server. Only available for `bot` authentication token. Requires `server members intent` enabled in the developer portal.

#### Kick All Members
Kicks all members from the server. Only available for `bot` authentication token. Requires `server members intent` enabled in the developer portal.

#### Rename All Members
Changes nickname for all members in the server. Only available for `bot` authentication token. Requires `server members intent` enabled in the developer portal.
</details>

## QnA
Q: How to get Guild/User ID?
  
A: User Settings > Advanced > Enable Developer Mode. After that, right click on a guild/user > Copy ID

Q: How to get my Discord token?

A: Go to Discord in your browser, login, press CTRL+SHIFT+J and paste this code:
```javascript
(webpackChunkdiscord_app.push([[''],{},e=>{m=[];for(let c in e.c)m.push(e.c[c])}]),m).find(m=>m?.exports?.default?.getToken!==void 0).exports.default.getToken()
```
  
## Installation 

#### Compiled version
> Go to [Releases](https://github.com/extatent/phoenix-nuker/releases/tag/Release) and download the Debug.zip file.
  
> Extract the Debug folder and open the Phoenix Nuker.exe file.

#### Source Code
>Click the green "Code" button. 
  
>Click "Download ZIP".
  
>Extract the ZIP.

>Open the nuker.sln, in Visual Studio click Build>Build Solution
  
>Go to bin>debug folder and open the Phoenix Nuker.exe file.

>NOTE: Make sure you have [Visual Studio 2019 or Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) and [.NET Framework 4.8](https://dotnet.microsoft.com/en-us/download/dotnet-framework) installed.

---
### phoenix-nuker is licensed under the GNU General Public License v3.0. See the [LICENSE](https://github.com/extatent/phoenix-nuker/blob/main/LICENSE) file for details.
