using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamKit2;
using System.IO;
using Newtonsoft.Json;
using SteamAuth;
using SteamKit2.Internal;
using System.Threading;
using System.Security.Cryptography;
using SteamKit2.GC;
using SteamKit2.GC.CSGO.Internal;
using System.Diagnostics;
using Limilabs.Mail;
using Limilabs.Client.POP3;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
namespace GodnestCSGOTool
{
    class LastMatchInfo
    {
        public uint lastCheck { get; set; }
        public uint matchEnded { get; set; }
    }
    public enum RankName
    {
        NotRanked = 0,
        SilverI = 1,
        SilverII = 2,
        SilverIII = 3,
        SilverIV = 4,
        SilverElite = 5,
        SilverEliteMaster = 6,
        GoldNovaI = 7,
        GoldNovaII = 8,
        GoldNovaIII = 9,
        GoldNovaMaster = 10,
        MasterGuardianI = 11,
        MasterGuardianII = 12,
        MasterGuardianElite = 13,
        DistinguishedMasterGuardian = 14,
        LegendaryEagle = 15,
        LegendaryEagleMaster = 16,
        SupremeMasterFirstClass = 17,
        TheGlobalElite = 18
    }
    class Otlegacheck
    {
        public class MyCallback : CallbackMsg
        {
            public EResult Result { get; private set; }

            internal MyCallback(EResult res)
            {
                Result = res;
            }
        }
        static uint accountID;
        static CallbackManager CallbackManager;
        static SteamClient SteamClient;
        static SteamUser SteamUser;
        static SteamFriends SteamFriends;
        static SteamGameCoordinator SteamGameCoordinator;
        static public bool isRunning;
        static public string TwoFactorCode, AuthCode;
        static Config Config;
        static string SentryFileName, LoginKeyFileName;
        static uint SteamAccountID = 0;
        static int startedAt = 0;
        static ulong matchID = 0;
        static RankName rankID = 0;
        static int level = 0;
        static uint victimID = 0;
        static bool reported = false;
        static string action;
        static public string informationsteam;
        static bool Authcodelegit = false;
        static public bool NeedSteamGuardKey = false;
        static public bool banned = false;
        static public bool kicksession;
        static public bool isjoinlobby;
        static public ulong nows()
        {
            return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        static public bool LoadConfig(string configFileName)
        {
            try
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + configFileName));
            }
            catch (Exception e)
            {
                informationsteam = (String.Format("Не могу прочитать файл '{0}': {1}", configFileName, e.Message));
                errorlog(String.Format("Не могу прочитать файл '{0}': {1}", configFileName, e.Message));
                return false;
            }
            if (Config.SteamLogin == null || Config.SteamPassword == null)
            {
                informationsteam = ("Нет логина или пароля!");
                errorlog(Config.SteamLogin + ":Нет логина или пароля!");
                return false;
            }
            return true;
        }

        static public void checkOWBypass(uint serverTime, uint expectedLastReportTime)
        {
            bool isReady = serverTime - expectedLastReportTime > 21 * 60 * 60;
            ulong test = serverTime - expectedLastReportTime;
            ulong test2 = nows();
            account account = new account();
            ulong test3 = test2 - test;
            account.TextOTLYOGA(test3, Config.SteamLogin + ".json");
            account.cooldowntimechecked(Convert.ToUInt32(nows()), Config.SteamLogin + ".json");
            account.TextRANK(rankID.ToString(), Config.SteamLogin + ".json");
            rankID = 0;
            if (!banned)
            {
                account.TextVAC("Yes", Config.SteamLogin + ".json");
            }
            else
            {
                account.TextVAC("None", Config.SteamLogin + ".json");
            }
            isRunning = false;
            informationsteam = "Готово";
        }
        static public string now()
        {
            return "[" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "] ";
        }
        static public string accountIDFormat(uint accountID)
        {
            var id = new SteamID();
            id.AccountID = accountID;
            return id.ToString();
        }
        static public void logCommend(uint accountID, bool result)
        {
            string report;
            if (result)
            {
                report = String.Format( ": Лайк для {0} НАКРУЧЕН!", accountIDFormat(accountID));
            }
            else
            {
                report = String.Format( ": Лайк для {0} НЕ накручен!", accountIDFormat(accountID));
            }
            informationsteam = "Готово";
            account account = new account();
            account.TextCOMMEND(report, Config.SteamLogin + ".json");
            isRunning = false;
        }
        static public void logReport(uint accountID, ulong confirmationID)
        {
            string report;
            if (confirmationID > 0)
            {
                report =  ": Репорт для " + accountIDFormat(accountID) + " ОТПРАВЛЕН, report id " + confirmationID + ".";
            }
            else
            {
                report =  ": Репорт для " + accountIDFormat(accountID) + " НЕ отправлен.";
            }
            informationsteam = "Готово";
            account account = new account();
            account.TextREPORT(report, Config.SteamLogin + ".json");
            isRunning = false;
        }
        static public void InitSteamkit()
        {
            SteamDirectory.Initialize();

            SteamClient = new SteamClient();

            CallbackManager = new CallbackManager(SteamClient);

            CallbackManager.Subscribe<SteamClient.ConnectedCallback>(OnConnected);
            CallbackManager.Subscribe<SteamClient.DisconnectedCallback>(OnDisconnected);

            SteamUser = SteamClient.GetHandler<SteamUser>();
            CallbackManager.Subscribe<SteamUser.LoggedOnCallback>(OnLoggedOn);
            CallbackManager.Subscribe<SteamUser.LoggedOffCallback>(OnLoggedOff);
            CallbackManager.Subscribe<SteamUser.AccountInfoCallback>(OnAccountInfo);
            CallbackManager.Subscribe<SteamUser.LoginKeyCallback>(OnLoginKey);
            CallbackManager.Subscribe<SteamUser.UpdateMachineAuthCallback>(OnMachineAuth);

            SteamFriends = SteamClient.GetHandler<SteamFriends>();

            SteamGameCoordinator = SteamClient.GetHandler<SteamGameCoordinator>();
            CallbackManager.Subscribe<SteamGameCoordinator.MessageCallback>(OnGCMessage);
        }
        static public void OnConnected(SteamClient.ConnectedCallback callback)
        {
            if (callback == null)
            {
                return;
            }
            informationsteam = (String.Format("Покдючен к стиму!"));

            byte[] sentryHash = null;

            if (File.Exists(SentryFileName))
            {
                byte[] sentryFileContent = File.ReadAllBytes(SentryFileName);
                sentryHash = CryptoHelper.SHAHash(sentryFileContent);
            }

            string loginKey = null;

            if (File.Exists(LoginKeyFileName))
            {
                loginKey = File.ReadAllText(LoginKeyFileName);
            }
            if (TwoFactorCode == null && Config.SteamLoginToken != null)
            {
                SteamGuardAccount account = new SteamGuardAccount();
                account.SharedSecret = Config.SteamLoginToken;
                TwoFactorCode = account.GenerateSteamGuardCode();
            }


            informationsteam = (String.Format("Вхожу в аккаунт..."));
            startedAt = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            SteamUser.LogOn(new SteamUser.LogOnDetails
            {
                Username = Config.SteamLogin,
                Password = loginKey == null ? Config.SteamPassword : null,
                AuthCode = loginKey == null ? AuthCode : null,
                LoginID = MsgClientLogon.ObfuscationMask,
                LoginKey = loginKey,
                TwoFactorCode = loginKey == null ? TwoFactorCode : null,
                SentryFileHash = sentryHash,
                ShouldRememberPassword = true
            });

        }

        static public void OnDisconnected(SteamClient.DisconnectedCallback callback)
        {
            if (callback == null)
            {
                return;
            }

            informationsteam = (String.Format("Отключен от стима! Переподключаюсь..."));

            Thread.Sleep(TimeSpan.FromSeconds(5));

            SteamClient.Connect();
        }
        static public async void OnLoggedOn(SteamUser.LoggedOnCallback callback)
        {
            if (callback == null)
            {
                return;
            }
            TwoFactorCode = null;
            AuthCode = null;
            Authcodelegit = false;
            TwoFactorCode = null;

            switch (callback.Result)
            {
                case EResult.TwoFactorCodeMismatch:
                case EResult.AccountLoginDeniedNeedTwoFactor:
                    if (Config.SteamLoginToken != null)
                    {
                        SteamGuardAccount account = new SteamGuardAccount();
                        account.SharedSecret = Config.SteamLoginToken;
                        TwoFactorCode = account.GenerateSteamGuardCode();
                        return;
                    }
                    TwoFactorCode = null;
                    Authcodelegit = false;
                    while (Authcodelegit == false)
                    {
                        NeedSteamGuardKey = true;
                        informationsteam = (String.Format("Введи Steam Guard Ключ который пришел на мобильный телефон: ", callback.EmailDomain));
                        if (mainform.SteamAuthCode != null)
                        {
                            TwoFactorCode = mainform.SteamAuthCode;
                            Authcodelegit = true;
                            NeedSteamGuardKey = false;
                        }
                    }
                    return;
                case EResult.AccountLogonDenied:
                    AuthCode = null;
                    Authcodelegit = false;
                    if (Config.emaillogin != "None")
                    {
                        informationsteam = (String.Format("Получаем почту..."));
                        Thread.Sleep(10000);
                        AuthCode = getmsgemail(Config.SteamLogin);
                        if (AuthCode == null)
                        {
                            while (Authcodelegit == false)
                            {
                                NeedSteamGuardKey = true;
                                informationsteam = (String.Format("Введи Steam Guard Ключ который пришел на Email {0}: ", callback.EmailDomain));
                                if (mainform.SteamAuthCode != null)
                                {
                                    AuthCode = mainform.SteamAuthCode;
                                    Authcodelegit = true;
                                    NeedSteamGuardKey = false;
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        while (Authcodelegit == false)
                        {
                            NeedSteamGuardKey = true;
                            informationsteam = (String.Format("Введи Steam Guard Ключ который пришел на Email {0}: ", callback.EmailDomain));
                            if (mainform.SteamAuthCode != null)
                            {
                                AuthCode = mainform.SteamAuthCode;
                                Authcodelegit = true;
                                NeedSteamGuardKey = false;
                            }
                        }
                    }
                    return;

                case EResult.InvalidPassword:
                    informationsteam = (String.Format("Unable to login to Steam: " + callback.Result));
                    if (!File.Exists(SentryFileName) && !File.Exists(LoginKeyFileName))
                    {
                        informationsteam = "Неверный Пароль";
                        errorlog(Config.SteamLogin + ":Неверный Пароль");
                    }
                    if (File.Exists(SentryFileName))
                    {
                        informationsteam = (String.Format("Удаляю старый ключ..."));
                        File.Delete(SentryFileName);
                    }

                    if (File.Exists(LoginKeyFileName))
                    {
                        informationsteam = (String.Format("Удаляю старый ключ..."));
                        File.Delete(LoginKeyFileName);
                    }
                    return;
                case EResult.OK:
                    informationsteam = (String.Format("Успешно подключен!"));
                    break;
                default:
                    informationsteam = (String.Format("Немогу подключитьсья к стиму: {0} / {1}", callback.Result, callback.ExtendedResult));
                    errorlog(Config.SteamLogin + String.Format(":Немогу подключитьсья к стиму: {0} / {1}", callback.Result, callback.ExtendedResult));
                    return;
            }
            informationsteam = (String.Format("Запускаю CSGO..."));
            if (kicksession == true)
            {
                var kickSession = new ClientMsgProtobuf<CMsgClientKickPlayingSession>(EMsg.ClientKickPlayingSession);
                kickSession.Body.only_stop_game = false;
                SteamClient.Send(kickSession);
                AbandonCompGame();
                kicksession = false;
                isRunning = false;
            }


            var request = new ClientMsgProtobuf<CMsgClientGamesPlayed>(EMsg.ClientGamesPlayed);
            request.Body.games_played.Add(new CMsgClientGamesPlayed.GamePlayed
            {
                game_id = new GameID(730),
            });

            SteamClient.Send(request);

            await Task.Delay(5000).ConfigureAwait(false);

            var clientHello = new ClientGCMsgProtobuf<CMsgClientHello>((uint)EGCBaseClientMsg.k_EMsgGCClientHello);
            clientHello.Body.version = 328;
            SteamGameCoordinator.Send(clientHello, 730);
        }
        static void HandleJoinLobbyResponse(IPacketMsg packetMsg)
        {
            var JoinResp = new ClientMsgProtobuf<CMsgClientMMSJoinLobbyResponse>(packetMsg);

            EResult result = (EResult)JoinResp.Body.chat_room_enter_response;
            EResult app_id = (EResult)JoinResp.Body.app_id;
            EResult flags = (EResult)JoinResp.Body.lobby_flags;
            EResult type = (EResult)JoinResp.Body.lobby_type;
            EResult max_members = (EResult)JoinResp.Body.max_members;
            EResult id_lobby = (EResult)JoinResp.Body.steam_id_lobby;
            EResult owner = (EResult)JoinResp.Body.steam_id_owner;
            informationsteam = (String.Format("Подключен к:" + JoinResp.Body.steam_id_lobby));
        }
        static void AbandonCompGame()
        {
            var abandonMsg = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_MatchmakingStop>(
                (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_MatchmakingStop
            );
            abandonMsg.Body.abandon = 13563;
            SteamGameCoordinator.Send(abandonMsg, 730);
            isRunning = false;
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
        static public void errorlog(string text)
        {
            try
            {
                string errorlog = File.ReadAllText("error.txt");
                errorlog = errorlog + String.Format(text) + Environment.NewLine;
                File.WriteAllText("error.txt", errorlog);
            }
            catch (Exception)
            {
                File.WriteAllText("error.txt", String.Format(text) + Environment.NewLine);
            }
        }
        static public string getmsgemail(string Steamlogin)
        {
            using (Pop3 pop3 = new Pop3())
            {
                if (Config.emailssl == true)
                {
                    try
                    {
                        pop3.ConnectSSL(Config.emailserver);
                    }
                    catch (Exception e)
                    {
                        errorlog(e.Message);
                        return null;
                    }
                }
                else
                {
                    try
                    {
                        pop3.Connect(Config.emailserver);
                    }
                    catch (Exception e)
                    {
                        errorlog(e.Message);
                        return null;
                    }
                }
                try
                {
                    pop3.UseBestLogin(Config.emaillogin, Config.emailpassword);
                }
                    catch (Exception e)
                {
                    errorlog(e.Message);
                    return null;
                }
                MailBuilder builder = new MailBuilder();
                foreach (string uid in pop3.GetAll())
                {
                    IMail email = builder.CreateFromEml(
                      pop3.GetMessageByUID(uid));
                    long unixTime = (long)(email.Date.Value - new DateTime(1970, 1, 1)).TotalSeconds;
                    long epochTicks = new DateTime(1970, 1, 1).Ticks;
                    long unixfTime = ((DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerSecond) + 10800;

                    long unixffTime = unixfTime - unixTime;
                    if (unixffTime <= 300)
                    {
                        string text1 = getBetween(email.Text, Steamlogin.ToLower() + ":", "This email was generated because");
                        if (text1 != null)
                        {
                            return text1.Replace("\r\n", string.Empty)
                                        .Replace("\n", string.Empty)
                                        .Replace("\r", string.Empty);
                        }
                    }
                }
                errorlog("Не могу найти письмо!");
                return null;
            }
        }

        static void OnGCMessage(SteamGameCoordinator.MessageCallback callback)
        {

            if (callback.EMsg == 6604)
            {
                string test = null;
                var JoinResp = new ClientGCMsgProtobuf<CMsgClientMMSJoinLobbyResponse>(callback.Message);
                EResult result = (EResult)JoinResp.Body.chat_room_enter_response;
                EResult app_id = (EResult)JoinResp.Body.app_id;
                EResult flags = (EResult)JoinResp.Body.lobby_flags;
                EResult type = (EResult)JoinResp.Body.lobby_type;
                EResult max_members = (EResult)JoinResp.Body.max_members;
                //EResult id_lobby = (EResult)JoinResp.Body.steam_id_lobby; <- Geht nicht weil EResult 32bit ist und nicht 64bit
                EResult owner = (EResult)JoinResp.Body.steam_id_owner;

                var id_lobby = JoinResp.Body.steam_id_lobby; // Das geht aber :)
                test = test + ($"Join Response: {result}") + Environment.NewLine;
                test = test + ($"app_id: {app_id}") + Environment.NewLine;
                test = test + ($"Flags: {flags}") + Environment.NewLine;
                test = test + ($"Type: {type}") + Environment.NewLine;
                test = test + ($"Max Members: {max_members}") + Environment.NewLine;
                test = test + ($"id_lobby: {id_lobby}") + Environment.NewLine;
                test = test + ($"Owner: {owner}") + Environment.NewLine;
                test = test + ($"FULL BODY: {JoinResp.Body.ToString()}") + Environment.NewLine;
                File.WriteAllText("6604.txt", test);
            }
            // setup our dispatch table for messages
            // this makes the code cleaner and easier to maintain
            var messageMap = new Dictionary<uint, Action<IPacketGCMsg>>
               {
                   { ( uint )EGCBaseClientMsg.k_EMsgGCClientWelcome, OnClientWelcome },
                   { ( uint )ECsgoGCMsg.k_EMsgGCCStrike15_v2_MatchmakingGC2ClientHello, OnMatchmakingClientHello },
                   { ( uint )ECsgoGCMsg.k_EMsgGCCStrike15_v2_ClientReportResponse, OnReportResponse },
                   { ( uint )ECsgoGCMsg.k_EMsgGCCStrike15_v2_MatchList, OnRecentMatchesResponse },
        			//{ ( uint )EDOTAGCMsg.k_EMsgGCMatchDetailsResponse, OnMatchDetails },
        		};

            Action<IPacketGCMsg> func;
            if (!messageMap.TryGetValue(callback.EMsg, out func))
            {
                // this will happen when we recieve some GC messages that we're not handling
                // this is okay because we're handling every essential message, and the rest can be ignored
                return;
            }
            func(callback.Message);

        }
        static async void OnClientWelcome(IPacketGCMsg packetMsg)
        {
            // in order to get at the contents of the message, we need to create a ClientGCMsgProtobuf from the packet message we recieve
            // note here the difference between ClientGCMsgProtobuf and the ClientMsgProtobuf used when sending ClientGamesPlayed
            // this message is used for the GC, while the other is used for general steam messages
            var msg = new ClientGCMsgProtobuf<CMsgClientWelcome>(packetMsg);

            informationsteam = (String.Format("Матчмейкинг привествует нас. Локация: {0}", msg.Body.location.country));
            var ClientToGC = new ClientGCMsgProtobuf<PlayerMedalsInfo>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_SetMyMedalsInfo);
            ClientToGC.Body.medal_global = 1318;
            SteamGameCoordinator.Send(ClientToGC, 730);
            var client2GCHello = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_MatchmakingClient2GCHello>(
                (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_MatchmakingClient2GCHello
            );
            SteamGameCoordinator.Send(client2GCHello, 730);

            await Task.Delay(TimeSpan.FromSeconds(2)).ConfigureAwait(false);
            if (action.Equals("otlyoga"))
            {
                RequestRecentMatches();
            }
            else
            {
                if (action.Equals("commend"))
                {
                    CommendAccount();
                }
                else if (action.Equals("report"))
                {
                    ReportAccount();
                }
                else if (action.Equals("Joinlobby"))
                {
                    JoinLobby();
                }
            }
        }
        static void JoinLobby()
        {
            informationsteam = ("Joining the lobby...");
            var joinRequest = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
            joinRequest.ProtoHeader.routing_appid = 730;
            joinRequest.Body.app_id = 730;
            joinRequest.Body.steam_id_lobby = mainform.lobbyid;
            joinRequest.Body.persona_name = mainform.name;
            SteamClient.Send(joinRequest);
        }

        static async void OnMatchmakingClientHello(IPacketGCMsg packetMsg)
        {
            var msg = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_MatchmakingGC2ClientHello>(packetMsg);
            if (SteamAccountID != 0
                || msg == null
                || msg.Body == null
                || msg.Body.ranking == null
                || msg.Body.commendation == null)
            {
                await Task.Delay(1000).ConfigureAwait(false);
                return;
            }
            if (msg.Body.vac_banned == 0)
            {
                banned = true;
            }
            else
            {
                banned = false;
            }
            rankID = 0;
            SteamAccountID = msg.Body.ranking.account_id;
            var commendation = msg.Body.commendation;
            rankID = (RankName)msg.Body.ranking.rank_id;
            level = msg.Body.player_level;

        }
        static async void CommendAccount()
        {
            informationsteam = (String.Format("Лайкаю {0}...", accountID));

            var requestCommend = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_ClientCommendPlayer>(
                (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_ClientCommendPlayer
            );
            requestCommend.Body.account_id = accountID;
            requestCommend.Body.match_id = 0;
            requestCommend.Body.commendation = new PlayerCommendationInfo();
            requestCommend.Body.commendation.cmd_friendly = 1;
            requestCommend.Body.commendation.cmd_leader = 1;
            requestCommend.Body.commendation.cmd_teaching = 1;
            requestCommend.Body.tokens = 0;

            SteamGameCoordinator.Send(requestCommend, 730);

            for (int i = 0; i < 10 && !reported; i++)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
            logCommend(accountID, reported);
            isRunning = false;
        }

        static void RequestRecentMatches()
        {
            informationsteam = (String.Format("Запрашиваю последнии игры для аккаунта " + SteamUser.SteamID.AccountID + "..."));
            var requestGames = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_MatchListRequestRecentUserGames>(
                (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_MatchListRequestRecentUserGames
            );
            requestGames.Body.accountid = SteamUser.SteamID.AccountID;

            SteamGameCoordinator.Send(requestGames, 730);

        }
        static async void ReportAccount()
        {
            var id = new SteamID();
            id.AccountID = accountID;

            reported = false;
            informationsteam = (String.Format("Репорчю {0}...", id));
            victimID = accountID;
            var requestReport = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_ClientReportPlayer>((uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_ClientReportPlayer
            );
            requestReport.Body.account_id = accountID;
            requestReport.Body.match_id = matchID;
            requestReport.Body.rpt_aimbot = 1;
            requestReport.Body.rpt_wallhack = 1;
            requestReport.Body.rpt_speedhack = 1;
            requestReport.Body.rpt_teamharm = 1;
            requestReport.Body.rpt_textabuse = 1;
            requestReport.Body.rpt_voiceabuse = 1;

            SteamGameCoordinator.Send(requestReport, 730);
            for (int i = 0; i < 2 && !reported; i++)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
            if (!reported)
            {
                logReport(victimID, 0);
            }
            isRunning = false;
        }
        static void OnReportResponse(IPacketGCMsg packetMsg)
        {
            var msg = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_ClientReportResponse>(packetMsg);
            if (msg.Body.response_type == (uint)ECsgoGCMsg.k_EMsgGCCStrike15_v2_ClientCommendPlayer)
            {
                reported = msg.Body.response_result == 1;
            }
            else
            {
                logReport(victimID, msg.Body.confirmation_id);
                reported = true;
            }
        }
        static void OnRecentMatchesResponse(IPacketGCMsg packetMsg)
        {
            var msg = new ClientGCMsgProtobuf<CMsgGCCStrike15_v2_MatchList>(packetMsg);
            uint serverTime = msg.Body.servertime;
            uint expectedLastReportTime = 0;
            foreach (var match in msg.Body.matches)
            {
                int matchDuration = 0;
                foreach (var round in match.roundstatsall)
                {
                    matchDuration = matchDuration < round.match_duration ? round.match_duration : matchDuration;
                }
                if (expectedLastReportTime < match.matchtime + matchDuration)
                {
                    expectedLastReportTime = match.matchtime + (uint)matchDuration;
                }
            }

            LastMatchInfo matchInfo = new LastMatchInfo();
            matchInfo.lastCheck = serverTime;
            matchInfo.matchEnded = expectedLastReportTime;
            File.WriteAllText("data/" + Config.SteamLogin + ".lastmatch", JsonConvert.SerializeObject(matchInfo));

            checkOWBypass(serverTime, expectedLastReportTime);
            isRunning = false;
        }
        static void OnLoggedOff(SteamUser.LoggedOffCallback callback)
        {
            if (callback == null)
            {
                return;
            }

            informationsteam = (String.Format("Отключен от стима: {0}", callback.Result));
            if (callback.Result == EResult.LoggedInElsewhere)
            {
                isRunning = false;
            }
        }


        static public void OnAccountInfo(SteamUser.AccountInfoCallback callback)
        {
            if (callback == null)
            {
                return;
            }
        }

        static public void OnLoginKey(SteamUser.LoginKeyCallback callback)
        {
            if (callback == null)
            {
                return;
            }

            informationsteam = (String.Format("Обновляю ключ..."));
            File.WriteAllText(LoginKeyFileName, callback.LoginKey);

            SteamUser.AcceptNewLoginKey(callback);
            informationsteam = (String.Format("Обновляю ключ...готово!"));
        }
        static void OnMachineAuth(SteamUser.UpdateMachineAuthCallback callback)
        {
            if (callback == null)
            {
                return;
            }

            informationsteam = (String.Format("Обновляю ключ..."));

            int fileSize;
            byte[] sentryHash;
            using (var fs = File.Open(SentryFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
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
            SteamUser.SendMachineAuthResponse(new SteamUser.MachineAuthDetails
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

            informationsteam = (String.Format("Done!"));
        }
        static public void DOkickow(string whataccount)
        {
            kicksession = true;
            banned = false;
            action = "otlyoga";
            SteamAccountID = 0;
            string configName = whataccount;
            if (!LoadConfig(configName))
            {
                return;
            }
            InitSteamkit();
            Authcodelegit = false;

            LoginKeyFileName = "data/" + Config.SteamLogin + ".key";
            SentryFileName = "data/" + Config.SteamLogin + ".sentry";
            //SteamClient.DebugNetworkListener = new NetHookNetworkListener( "debug/" );
            informationsteam = "Подключаюсь к стиму...";

            SteamClient.Connect();
            isRunning = true;
            while (isRunning)
            {
                CallbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }
        static public void joinlobb(string whataccount)
        {
            isjoinlobby = true;
            banned = false;
            action = "Joinlobby";
            SteamAccountID = 0;
            string configName = whataccount;
            if (!LoadConfig(configName))
            {
                return;
            }
            InitSteamkit();
            Authcodelegit = false;

            LoginKeyFileName = "data/" + Config.SteamLogin + ".key";
            SentryFileName = "data/" + Config.SteamLogin + ".sentry";
            //SteamClient.DebugNetworkListener = new NetHookNetworkListener( "debug/" );
            informationsteam = "Подключаюсь к стиму...";

            SteamClient.Connect();
            isRunning = true;
            while (isRunning)
            {
                CallbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }
        public void DoCheck(string whatdoarg, string whataccount)
        {
            banned = false;
            action = whatdoarg;
            if (action.Equals("report"))
            {
                accountID = mainform.accountID;
            }
            else
            {
                if (action.Equals("commend"))
                    accountID = mainform.accountID;
            }
            SteamAccountID = 0;
            string configName = whataccount;
            if (!LoadConfig(configName))
            {
                return;
            }
            InitSteamkit();
            Authcodelegit = false;

            LoginKeyFileName = "data/" + Config.SteamLogin + ".key";
            SentryFileName = "data/" + Config.SteamLogin + ".sentry";
            //SteamClient.DebugNetworkListener = new NetHookNetworkListener( "debug/" );
            informationsteam = "Подключаюсь к стиму...";

            SteamClient.Connect();
            isRunning = true;
            while (isRunning)
            {
                CallbackManager.RunWaitCallbacks(TimeSpan.FromSeconds(1));
            }
        }
        static public void Exit()
        {
            Thread.Sleep(500);
            Environment.Exit(0);
        }
    }
}


