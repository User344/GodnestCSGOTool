using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Management; //Need to manually add to the References
using MaterialSkin;
using MaterialSkin.Controls;
using Limilabs.Mail;
using Limilabs.Client.POP3;
using System.Net;
namespace GodnestCSGOTool
{
    public partial class mainform : MaterialForm
    {

        SettingsConfig SettingsConfig;
        static public int howmuchaccounts;
        Config Config;
        static public int secondstimer = 0;
        static public bool itslaunched;
        static public int selectblyat = 0;
        static public uint howmuchaccountsnotneed;
        static public string SteamAuthCode;
        static public bool isrunning;
        static public uint accountID;
        static public ulong lobbyid;
        static public ulong createdlobbyid;
        static public string name = "XD";
        static public bool isjoinlobby;
        static public string numbersofwins = "0";
        static public bool IsPrime;
        static public bool Autokick;
        static public bool Medals;
        static public bool crashlobby;
        static public int level = 1;
        static public int rankid = 0;
        static public string currentmessage;
        static public string spamessage;
        static public string chat;
        static public string fakesteamid = "123";
        static public int kicksteamid;
        static public bool autoparser;
        static public bool autokick;
        static public bool autocrash;
        static public bool autospam;
        static public bool imconnecting;
        static public bool nonkickablebots;
        static public string textwhenjoin;
        static public int steamids;
        static public bool startyowerwatch;
        static public bool startsearchlobby;
        static public bool nahlebscheck;
        static public bool nonahlebscheck;
        static public bool isparsinglocalprofile;
        static public bool LACenabled;
        static public bool scaneveything;
        static public ulong idofstart;
        static public ulong steamid64whotofind;
        static public int bannedbyow;
        static public int checkingnowbans;
        static public string spamessagetoall;
        static public bool callform;
        static public bool spamsomemessages;
        static public bool dontcrashihm;
        static public bool justjointolisten;
        static public bool joinforprotect;
        static public bool listenkech;

        Otlegacheck Otlegacheck = new Otlegacheck();

        private readonly MaterialSkinManager materialSkinManager;
        private void updatelistbox1()
        {
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            listBox11.Items.Clear();
            string[] lines = null;
            try
            {
                lines = File.ReadAllLines("steamidsforattack.txt");
            }
            catch (Exception)
            {
                File.WriteAllText("steamidsforattack.txt", null);
            }
            if (lines != null)
            {
                for (int i = 0; i < lines.Count(); i++)
                {
                    listBox11.Items.Add(lines[i].ToString());
                }
            }
        }
        public mainform()
        {

            Directory.CreateDirectory("data");
            InitializeComponent();
            updatelistbox1();
            try
            {
                File.ReadAllText("nahlebs.txt");
            }
            catch (Exception)
            {
                File.WriteAllText("nahlebs.txt", "start");
            }
            uodatelistboxcommend();
            uodatelistboxreport();
            textBox15.Visible = false;
            launchsteam.Visible = false;
            launchcsgo.Visible = false;
            materialRaisedButton8.Visible = false;
            listBox1.Enabled = true;
            listBox2.Enabled = true;
            listBox3.Enabled = true;
            materialRaisedButton20.Visible = false;
            textBox10.Visible = false;
            comboBox1.Visible = false;
            textBox12.Visible = false;
            textBox11.Visible = false;
            textBox13.Visible = false;
            button2.Enabled = false;
            materialRaisedButton18.Visible = false;
            materialRaisedButton17.Visible = false;
            textBox16.Visible = false;
            checkBox5.Visible = false;
            try
            {
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            }
            catch (Exception)
            {
                SettingsConfig _SettingsConfig = new SettingsConfig()
                {
                    Accslegit = false,
                    Steamfile = "None",
                    emaillogin = "None",
                    emailpassword = "None",
                    emailserver = "None",
                    emailssl = false,
                };
                string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
                System.IO.File.WriteAllText("cfg.json", json);
            }

            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Orange700, Primary.Red400, Primary.Blue400, Accent.Blue400, TextShade.BLACK);
            tabPage1.Text = "Отлега";
            tabPage2.Text = "Аккаунты и настройки";
            tabPage3.Text = "Репортбот";
            tabPage4.Text = "Лайкбот";
            tabPage5.Text = "Lobby Stuff";
            tabPage6.Text = "Парсер";
            updatelistbox();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            howmuchaccounts = 0;
            howmuchaccountsnotneed = 0;
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    listBox3.Items.Add(Config.SteamLogin);
                    howmuchaccounts = howmuchaccounts + 1;
                }
                else
                {
                    listBox2.Items.Add(Config.SteamLogin);
                    howmuchaccountsnotneed = howmuchaccountsnotneed + 1;
                }
                materialLabel2.Text = String.Format("Используются = " + howmuchaccounts);
                materialLabel3.Text = String.Format("Не Используются = " + howmuchaccountsnotneed);
            }

            try
            {
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            }
            catch (Exception)
            {
                SettingsConfig _SettingsConfig = new SettingsConfig()
                {
                    Accslegit = false,
                    Steamfile = "None",
                    emaillogin = "None",
                    emailpassword = "None",
                    emailserver = "None",
                    emailssl = false,
                };
                string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
                System.IO.File.WriteAllText("cfg.json", json);
            }
        }
        public static String GetHDDSerialNo()
        {
            ManagementClass mangnmt = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection mcol = mangnmt.GetInstances();
            string result = "";
            foreach (ManagementObject strt in mcol)
            {
                result += Convert.ToString(strt["VolumeSerialNumber"]);
            }
            return result;
        }
        public ulong nows()
        {
            return (ulong)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        private void updatelistbox()
        {
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            if (listBox3.SelectedItem == null)
            {
                if (listBox2.SelectedItem == null)
                {
                    listBox3.Items.Clear();
                    listBox2.Items.Clear();
                    foreach (FileInfo file in Files)
                    {
                        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                        if (Config.InUse == true)
                        {
                            listBox3.Items.Add(Config.SteamLogin);
                        }
                        else
                        {
                            listBox2.Items.Add(Config.SteamLogin);
                        }
                    }
                }
            }
            materialLabel2.Text = String.Format("Используются = " + howmuchaccounts);
            materialLabel3.Text = String.Format("Не Используются = " + howmuchaccountsnotneed);
            label1.Text = (Otlegacheck.informationsteam);
            label2.Text = (Otlegacheck.informationsteam);
            label3.Text = (Otlegacheck.informationsteam);
            label4.Text = (Otlegacheck.informationsteam);
            label5.Text = (lobby.statusteam);
            label7.Text = (lobby.statusteam);
            label6.Text = Handler.searchstatus;
            if (Handler.whoinlobby != null)
            {
                if (listBox8.SelectedItem == null)
                {
                    listBox8.Items.Clear();
                    for (int di = 0; di < Convert.ToInt32(getBetween(Handler.whoinlobby, "CS", "CE"));)
                    {
                        string who = null;
                        who = "Name:" + getBetween(Handler.whoinlobby, "SID" + di, "MID" + di);
                        who = who + " SteamID:" + getBetween(Handler.whoinlobby, "MID" + di, "EID" + di) + ".";
                        listBox8.Items.Add(who);
                        di = di + 1;
                    }
                }
            }
            int i = -1;

            if (listBox1.SelectedIndex != -1)
            {
                i = listBox1.SelectedIndex;
            }
            if (Otlegacheck.NeedSteamGuardKey == false)
            {
                textBox9.Enabled = false;
                SteamAuthCode = null;
            }
            else
            {
                textBox9.Enabled = true;
            }
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            if (textBox1.Text.Equals("Логин"))
            {
                materialRaisedButton3.Enabled = false;
            }
            else
            {
                materialRaisedButton3.Enabled = true;
            }
            if (isrunning != true)
            {
                if (checkBox1.Checked == true)
                {
                    textBox4.Enabled = true;
                    textBox5.Enabled = true;
                    textBox6.Enabled = true;
                    checkBox2.Enabled = false;
                    checkBox3.Enabled = true;
                }
                else
                {
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    textBox6.Enabled = false;
                    checkBox3.Enabled = false;
                    if (SettingsConfig.emaillogin == "None")
                    {
                        checkBox2.Enabled = false;
                    }
                    else
                    {
                        textBox4.Enabled = true;
                        textBox5.Enabled = true;
                        textBox6.Enabled = true;
                        checkBox2.Enabled = true;
                        checkBox3.Enabled = true;
                        if (checkBox2.Checked == true)
                        {
                            checkBox1.Enabled = false;
                            textBox4.Enabled = false;
                            textBox5.Enabled = false;
                            textBox6.Enabled = false;
                            checkBox3.Enabled = false;
                        }
                        else
                        {
                            checkBox1.Enabled = true;
                            textBox4.Enabled = true;
                            textBox5.Enabled = true;
                            textBox6.Enabled = true;
                            checkBox3.Enabled = true;

                        }
                    }
                }
            }
            if (listBox2.SelectedItem != null)
            {
                materialRaisedButton4.Enabled = true;
            }
            else
            {
                materialRaisedButton4.Enabled = false;
            }

            listBox1.Items.Clear();
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    TimeSpan t = TimeSpan.FromSeconds(nows() - Convert.ToUInt64(Config.TextOTLYOGA));
                    TimeSpan p = TimeSpan.FromSeconds(nows() - Convert.ToUInt64(Config.cooldowntimechecked));
                    string log;
                    log = String.Format(Config.SteamLogin + ":::" + "Последний матч закончился:{0:D1}d:{1:D1}h:{2:D1}m:{3:D1}:::Проверялась:{4:D1}d:{5:D1}h:{6:D1}m:{7:D1}", t.Days, t.Hours, t.Minutes, t.Seconds, p.Days, p.Hours, p.Minutes, p.Seconds + "s назад:::VAC-" + Config.TextVAC);
                    if (Config.TextVAC == "None")
                    {
                        log = log + ":::Rank-" + Config.TextRANK;
                        if (nows() - Convert.ToUInt64(Config.TextOTLYOGA) > 21 * 60 * 60)
                        {
                            log = log + "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++";
                        }
                    }
                    listBox1.Items.Add(log);
                }
            }
            if (i != -1)
            {
                listBox1.SetSelected(i, true);
            }
        }

        private void mainform_Load(object sender, EventArgs e)
        {
            timer1.Tick += new EventHandler(timer1sec);
            timer1.Interval = 1000; // Здесь измени интервал на 5000 (5 сек)
            timer1.Start();
            timer1.Tick += new EventHandler(timer100ms);
            timer1.Interval = 100; // Здесь измени интервал на 5000 (5 сек)
            timer1.Start();
        }
        public void listoflobby(int counts)
        {
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

        private void timer100ms(object sender, EventArgs e)
        {
            string pathsteam = null;
            try
            {
                pathsteam = File.ReadAllText("pathstatus.txt") + "\\";
            }
            catch (Exception)
            {
                pathsteam = null;
            }
            if (textBox25.Text.Length == 85)
            {
                File.WriteAllText("accestoken.txt", textBox25.Text);
            }
            if (Handler.chat1 != null)
            {
                listBox7.Items.Add(Handler.chat1);
                Handler.chat1 = null;
            }
        }

        private void updatelis6box()
        {
            listBox6.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    listBox6.Items.Add(Config.SteamLogin);
                }
            }
            listBox9.Items.Clear();
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    listBox9.Items.Add(Config.SteamLogin);
                }
            }
        }
        private void timer1sec(object sender, EventArgs e)
        {
            if (createdlobbyid != 0)
            {
                textBox22.Text = "steam://joinlobby/730/" + createdlobbyid;
                createdlobbyid = 0;
            }
            if (Handler.currentlobbyid != 0)
            {
                textBox26.Text = "steam://joinlobby/730/" + Handler.currentlobbyid;
            }
            else
            {
                textBox26.Text = "";
            }
            try
            {
                listBox10.Items.Clear();
                string[] vkpubs = File.ReadAllLines("vkpublics.txt");
                for (int i = 0; i <= vkpubs.Count() - 1; i++)
                {
                    listBox10.Items.Add(vkpubs[i].ToString());
                }
            }
            catch (Exception)
            {
                File.WriteAllText("vkpublics.txt", null);
            }
            label14.Text = "Проверено:" + checkingnowbans;
            label15.Text = "Забанено:" + bannedbyow;
            if (listBox12.SelectedItem == null)
            {
                listBox12.Items.Clear();
                try
                {
                    string whitelist = File.ReadAllText("whitelist.txt");
                    string[] whitelist1 = whitelist.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    int i = 0;
                    while (i != whitelist1.Length)
                    {
                        listBox12.Items.Add(whitelist1[i].ToString());
                        i = i + 1;
                    }
                }
                catch (Exception)
                {
                    File.WriteAllText("whitelist.txt", "");
                }
            }
            if (listBox13.SelectedItem == null)
            {
                listBox13.Items.Clear();
                try
                {
                    string blacklist = File.ReadAllText("blacklist.txt");
                    string[] blacklist1 = blacklist.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                    int i = 0;
                    while (i != blacklist1.Length)
                    {
                        listBox13.Items.Add(blacklist1[i].ToString());
                        i = i + 1;
                    }
                }
                catch (Exception)
                {
                    File.WriteAllText("blacklist.txt", "");
                }
            }

            label13.Text = "Засканил:" + Handler.checkinglobbys;
            label16.Text = "Крашнуто:" + Handler.crashedhomeless;
            label17.Text = "Отправил:" + Handler.sendjoins;
            label18.Text = Convert.ToString(Handler.firstid);

            try
            {
                steamids = File.ReadAllLines("steamids.txt").Count();
                label8.Text = "SteamID'шников записано:" + steamids;
            }
            catch (Exception)
            {

            }
            label9.Text = "Лобби крашнуто:" + lobby.howmucrashed;
            if (itslaunched != true)
            {
                button1.Text = "Проверить отлегу";
                materialTabSelector1.Enabled = true;
                materialRaisedButton5.Enabled = true;
                materialLabel1.Visible = true;
                materialRaisedButton2.Visible = true;
                materialRaisedButton1.Visible = true;
                materialRaisedButton3.Visible = true;
                materialRaisedButton4.Visible = true;
                materialRaisedButton14.Visible = true;
                materialRaisedButton15.Visible = true;
                materialRaisedButton16.Visible = true;
                checkBox4.Visible = true;
                textBox8.Visible = true;
                textBox7.Visible = true;
                textBox3.Visible = true;
                textBox6.Visible = true;
                textBox5.Visible = true;
                textBox4.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                checkBox1.Visible = true;
                textBox2.Visible = true;
                textBox1.Visible = true;
                materialRaisedButton5.Enabled = true;
                if (SettingsConfig.Accslegit == true)
                {
                    materialRaisedButton5.Text = "Аккаунты проверены";
                    materialRaisedButton5.Enabled = false;
                    button1.Visible = true;
                    materialRaisedButton9.Visible = true;
                    materialRaisedButton12.Visible = true;
                }
                else
                {
                    if (isrunning != true)
                    {
                        button1.Visible = false;
                        materialRaisedButton9.Visible = false;
                        materialRaisedButton12.Visible = false;
                        materialRaisedButton5.Text = "Проверить аккаунты";
                        materialRaisedButton5.Enabled = true;
                    }
                }
            }
            else
            {
                uodatelistboxcommend();
                uodatelistboxreport();
                listBox1.SetSelected(selectblyat, true);
                listBox4.SetSelected(selectblyat, true);
                listBox5.SetSelected(selectblyat, true);
            }
            if (itslaunched != true)
            {
                try
                {
                    ulong WhenReportReady = Convert.ToUInt64(File.ReadAllText("data/" + "lastcommend.txt"));
                    if (6 * 60 * 60 >= nows() - WhenReportReady)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(6 * 60 * 60 - (nows() - WhenReportReady));
                        materialRaisedButton12.Text = string.Format("{0:D1}d:{1:D1}h:{2:D1}m:{3:D1}s", t.Days, t.Hours, t.Minutes, t.Seconds);
                        materialRaisedButton12.BackColor = System.Drawing.Color.Red;
                        materialRaisedButton7.Visible = true;
                        materialRaisedButton12.Enabled = false;
                    }
                    else
                    {
                        materialRaisedButton12.Text = "Лайкнуть";
                        materialRaisedButton12.BackColor = System.Drawing.Color.Green;
                        materialRaisedButton12.Enabled = true;
                        materialRaisedButton7.Visible = false;

                    }
                }
                catch (Exception)
                {
                    File.WriteAllText("data/" + "lastcommend.txt", "0");
                }
                try
                {
                    ulong WhenReportReady = Convert.ToUInt64(File.ReadAllText("data/" + "lastreport.txt"));
                    if (6 * 60 * 60 >= nows() - WhenReportReady)
                    {
                        TimeSpan t = TimeSpan.FromSeconds(6 * 60 * 60 - (nows() - WhenReportReady));
                        materialRaisedButton9.Text = string.Format("{0:D1}d:{1:D1}h:{2:D1}m:{3:D1}s", t.Days, t.Hours, t.Minutes, t.Seconds);
                        materialRaisedButton9.BackColor = System.Drawing.Color.Red;
                        materialRaisedButton9.Enabled = false;
                        materialRaisedButton6.Visible = true;
                    }
                    else
                    {
                        materialRaisedButton9.Text = "Репортботнуть";
                        materialRaisedButton9.BackColor = System.Drawing.Color.Green;
                        materialRaisedButton9.Enabled = true;
                        materialRaisedButton6.Visible = false;
                    }
                }
                catch (Exception)
                {
                    File.WriteAllText("data/" + "lastreport.txt", "0");
                }
            }
            if (textBox3.Text == "Логин от почты")
            {
                materialRaisedButton14.Visible = false;
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                if (SettingsConfig.emaillogin != "None")
                {
                    materialRaisedButton14.Visible = false;
                    textBox3.Enabled = false;
                    textBox7.Enabled = false;
                    textBox8.Enabled = false;
                    checkBox4.Enabled = false;
                }
                else
                {
                    materialRaisedButton14.Visible = true;
                    textBox3.Enabled = true;
                    textBox7.Enabled = true;
                    textBox8.Enabled = true;
                    checkBox4.Enabled = true;
                }
            }
            else
            {
                materialRaisedButton14.Visible = true;
            }
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            if (isrunning != true)
            {
                if (SettingsConfig.emaillogin == "None")
                {
                    materialRaisedButton15.Visible = false;
                }
                else
                {
                    materialRaisedButton15.Visible = true;
                }
            }
            updatelistbox();
            if (itslaunched == false)
            {
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                if (SettingsConfig.Accslegit != true)
                {
                    button1.Enabled = false;
                }
                else
                {
                    button1.Enabled = true;
                }
                materialRaisedButton8.Visible = true;
                launchsteam.Visible = true;
                launchcsgo.Visible = true;
            }
            else
            {
                button1.Enabled = false;
                materialRaisedButton8.Visible = false;
                launchsteam.Visible = false;
                launchcsgo.Visible = false;
            }
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            if (SettingsConfig.Steamfile.Equals("None"))
            {
                materialRaisedButton8.Visible = false;
                launchcsgo.Visible = false;
                launchsteam.Text = "Укажи путь в настройках";
                launchsteam.Enabled = false;
                launchcsgo.Text = "Укажи путь в настройках";
                launchcsgo.Enabled = false;
            }
            else
            {
                if (listBox1.SelectedItem == null)
                {
                    materialRaisedButton8.Visible = false;
                    launchsteam.Visible = false;
                    launchcsgo.Visible = false;
                }
                else
                {
                    materialRaisedButton8.Visible = true;
                    launchsteam.Visible = true;
                    launchcsgo.Visible = true;
                    materialRaisedButton8.Visible = true;
                    launchsteam.Text = "Запустить CS:GO";
                    launchcsgo.Text = "Запустить Steam";
                }
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            updatelis6box();
            if (listBox3.SelectedItem != null)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + listBox3.SelectedItem + ".json"));
                Config _Config = new Config()
                {
                    InUse = false,
                    SteamLogin = Config.SteamLogin,
                    SteamPassword = Config.SteamPassword,
                    TextREPORT = Config.TextREPORT,
                    TextCOMMEND = Config.TextCOMMEND,
                    TextOTLYOGA = Config.TextOTLYOGA,
                    TextRANK = Config.TextRANK,
                    TextVAC = Config.TextVAC,
                    cooldowntimechecked = Config.cooldowntimechecked,
                    Lastreporttime = Config.Lastreporttime,
                    Lastcommendtime = Config.Lastcommendtime,
                    emaillogin = SettingsConfig.emaillogin,
                    emailpassword = SettingsConfig.emailpassword,
                    emailserver = SettingsConfig.emailserver,
                    emailssl = SettingsConfig.emailssl,
                };
                string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
                System.IO.File.WriteAllText("data/" + Config.SteamLogin + ".json", json);
                listBox2.Items.Add(listBox3.SelectedItem);
                listBox3.Items.Remove(listBox3.SelectedItem);
                howmuchaccountsnotneed = howmuchaccountsnotneed + 1;
                howmuchaccounts = howmuchaccounts - 1;
                updatelistbox();
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void uodatelistboxcommend()
        {
            listBox5.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
                if (Config.InUse == true)
                {
                    listBox5.Items.Add(Config.SteamLogin + ":" + Config.TextCOMMEND);
                }
            }
        }
        private void uodatelistboxreport()
        {
            listBox4.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                try
                {
                    Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                }
                catch (Exception)
                {
                    Thread.Sleep(200);
                }
                if (Config.InUse == true)
                {
                    listBox4.Items.Add(Config.SteamLogin + ":" + Config.TextREPORT);
                }
            }
        }

        private void materialRaisedButton5_Click(object sender, EventArgs e)
        {
            Task.Run(() => checkcooldownall());
            button1.Text = "Проверяеться...";
            materialTabSelector1.Enabled = false;
        }
        public void checkcooldownall()
        {
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            selectblyat = 0;
            itslaunched = true;
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    itslaunched = true;
                    Otlegacheck.DoCheck("otlyoga", String.Format(Config.SteamLogin + ".json"));
                    selectblyat = selectblyat + 1;
                }
            }
            itslaunched = false;
            selectblyat = 0;
        }
        private void materialRaisedButton5_Click_1(object sender, EventArgs e)
        {
            updatelis6box();
            itslaunched = true;
            Task.Run(() => checkcoolaccs());
            materialRaisedButton2.Visible = false;
            materialRaisedButton1.Visible = false;
            materialRaisedButton3.Visible = false;
            materialRaisedButton4.Visible = false;
            materialRaisedButton14.Visible = false;
            materialRaisedButton15.Visible = false;
            materialRaisedButton16.Visible = false;
            checkBox4.Visible = false;
            textBox8.Visible = false;
            textBox7.Visible = false;
            textBox3.Visible = false;
            textBox6.Visible = false;
            textBox5.Visible = false;
            textBox4.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox1.Visible = false;
            textBox2.Visible = false;
            textBox1.Visible = false;
            materialLabel1.Visible = false;
            materialTabSelector1.Enabled = false;
            materialRaisedButton5.Enabled = false;
            materialRaisedButton5.Text = "Проверяем...";
        }
        private void checkcoolaccs()
        {
            isrunning = true;
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                Otlegacheck.DoCheck("otlyoga", String.Format(Config.SteamLogin + ".json"));
            }
            account account = new account();
            account.Accslegit(true);
            itslaunched = false;
            isrunning = false;
        }

        private void materialRaisedButton8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                int id = 0;
                DirectoryInfo d = new DirectoryInfo(@"data");
                FileInfo[] Files = d.GetFiles("*.json");
                foreach (FileInfo file in Files)
                {
                    if (listBox1.SelectedIndex == id)
                    {
                        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file.Name));
                    }
                    id = id + 1;
                }
                id = 0;
                Task.Run(() => kicknigger(Config.SteamLogin));
                itslaunched = true;
                materialRaisedButton8.Text = "Кикаем...";
                materialTabSelector1.Enabled = false;
                listBox1.SelectedIndex = -1;
            }
        }
        private void kicknigger(string configname)
        {
            Otlegacheck.DOkickow(configname + ".json");
            itslaunched = false;
            materialRaisedButton8.Text = "OW bypass";
            materialTabSelector1.Enabled = true;
        }

        private void materialRaisedButton7_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                System.Diagnostics.Process[] procs = Process.GetProcessesByName("steam"); ;
                if (procs.Length > 0)
                {
                    try
                    {
                        procs = Process.GetProcessesByName("steam");

                        Process mspaintProc = procs[0];

                        if (!mspaintProc.HasExited)
                        {
                            mspaintProc.Kill();
                        }
                    }
                    finally
                    {
                        if (procs != null)
                        {
                            foreach (Process p in procs)
                            {
                                p.Dispose();
                            }
                        }
                    }
                }
                if (listBox1.SelectedItem != null)
                {
                    int id = 0;
                    DirectoryInfo d = new DirectoryInfo(@"data");
                    FileInfo[] Files = d.GetFiles("*.json");
                    foreach (FileInfo file in Files)
                    {
                        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file.Name));
                        if (Config.InUse == true)
                        {
                            if (listBox1.SelectedIndex == id)
                            {
                                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file.Name));
                                Process steam = new Process();
                                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                                steam.StartInfo.FileName = SettingsConfig.Steamfile;
                                steam.StartInfo.Arguments = String.Format(" 730 -login " + Config.SteamLogin + " " + Config.SteamPassword);
                                steam.Start();
                                account account = new account();
                                account.TextOTLYOGA(Convert.ToUInt32(nows()), Config.SteamLogin + ".json");
                                account.cooldowntimechecked(Convert.ToUInt32(nows()), Config.SteamLogin + ".json");
                                listBox1.SelectedIndex = -1;
                            }
                            id = id + 1;
                        }
                    }
                    id = 0;
                }
            }
        }

        private void mainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            Task.Run(() => Otlegacheck.Exit());
            Task.Run(() => Handler.Exit());
            Task.Run(() => lobby.Exit());
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton3_Click(object sender, EventArgs e)
        {
            updatelis6box();
            if (checkBox2.Checked == true)
            {
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                Config _Config = new Config()
                {
                    InUse = false,
                    SteamLogin = textBox1.Text,
                    SteamPassword = textBox2.Text,
                    TextREPORT = "Не репортил",
                    TextCOMMEND = "Не Лайкал",
                    TextOTLYOGA = 0,
                    TextRANK = "Unknown",
                    TextVAC = "Unknown",
                    cooldowntimechecked = 0,
                    Lastreporttime = 0,
                    Lastcommendtime = 0,
                    emaillogin = SettingsConfig.emaillogin,
                    emailpassword = SettingsConfig.emailpassword,
                    emailserver = SettingsConfig.emailserver,
                    emailssl = SettingsConfig.emailssl,
                };
                string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);

                //write string to file
                System.IO.File.WriteAllText("data/" + textBox1.Text + ".json", json);
                account account = new account();
                account.Accslegit(false);
                textBox1.Text = "Логин";
                textBox2.Text = "Пароль";
                textBox4.Text = "Логин от почты";
                textBox5.Text = "Пароль от почты";
                textBox6.Text = "POP3 Protocol";
                checkBox3.Checked = false;
                howmuchaccountsnotneed = howmuchaccountsnotneed + 1;
                uodatelistbox();
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                    Config _Config = new Config()
                    {
                        InUse = false,
                        SteamLogin = textBox1.Text,
                        SteamPassword = textBox2.Text,
                        TextREPORT = "Не репортил",
                        TextCOMMEND = "Не Лайкал",
                        TextOTLYOGA = 0,
                        TextRANK = "Unknown",
                        TextVAC = "Unknown",
                        cooldowntimechecked = 0,
                        Lastreporttime = 0,
                        Lastcommendtime = 0,
                        emaillogin = textBox4.Text,
                        emailpassword = textBox5.Text,
                        emailserver = textBox6.Text,
                        emailssl = checkBox3.Checked,
                    };
                    string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);

                    //write string to file
                    System.IO.File.WriteAllText("data/" + textBox1.Text + ".json", json);
                    account account = new account();
                    account.Accslegit(false);
                    textBox1.Text = "Логин";
                    textBox2.Text = "Пароль";
                    textBox4.Text = "Логин от почты";
                    textBox5.Text = "Пароль от почты";
                    textBox6.Text = "POP3 Protocol";
                    checkBox3.Checked = false;
                    howmuchaccountsnotneed = howmuchaccountsnotneed + 1;
                    uodatelistbox();
                }
                else
                {
                    Config _Config = new Config()
                    {
                        InUse = false,
                        SteamLogin = textBox1.Text,
                        SteamPassword = textBox2.Text,
                        TextREPORT = "Не репортил",
                        TextCOMMEND = "Не Лайкал",
                        TextOTLYOGA = 0,
                        TextRANK = "Unknown",
                        TextVAC = "Unknown",
                        cooldowntimechecked = 0,
                        Lastreporttime = 0,
                        Lastcommendtime = 0,
                        emaillogin = "None",
                        emailpassword = "None",
                        emailserver = "None",
                        emailssl = false,
                    };
                    string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);

                    //write string to file
                    System.IO.File.WriteAllText("data/" + textBox1.Text + ".json", json);
                    account account = new account();
                    account.Accslegit(false);
                    textBox1.Text = "Логин";
                    textBox2.Text = "Пароль";
                    textBox4.Text = "Логин от почты";
                    textBox5.Text = "Пароль от почты";
                    textBox6.Text = "POP3 Protocol";
                    checkBox3.Checked = false;
                    howmuchaccountsnotneed = howmuchaccountsnotneed + 1;
                    uodatelistbox();
                }
            }
        }

        private void materialRaisedButton4_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                System.IO.File.Delete("data/" + listBox2.SelectedItem + ".json");
                System.IO.File.Delete("data/" + listBox2.SelectedItem + ".key");
                System.IO.File.Delete("data/" + listBox2.SelectedItem + ".lastmatch");
                System.IO.File.Delete("data/" + listBox2.SelectedItem + ".sentry");
                howmuchaccountsnotneed = howmuchaccountsnotneed - 1;
                uodatelistbox();
            }
            updatelis6box();
        }



        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text.Length == 5)
            {
                SteamAuthCode = textBox9.Text;
                textBox9.Text = null;
            }
        }

        private void materialRaisedButton2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + listBox2.SelectedItem + ".json"));
                Config _Config = new Config()
                {
                    InUse = true,
                    SteamLogin = Config.SteamLogin,
                    SteamPassword = Config.SteamPassword,
                    TextREPORT = Config.TextREPORT,
                    TextCOMMEND = Config.TextCOMMEND,
                    TextOTLYOGA = Config.TextOTLYOGA,
                    TextRANK = Config.TextRANK,
                    TextVAC = Config.TextVAC,
                    cooldowntimechecked = Config.cooldowntimechecked,
                    Lastreporttime = Config.Lastreporttime,
                    Lastcommendtime = Config.Lastcommendtime,
                    emaillogin = SettingsConfig.emaillogin,
                    emailpassword = SettingsConfig.emailpassword,
                    emailserver = SettingsConfig.emailserver,
                    emailssl = SettingsConfig.emailssl,
                };
                string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
                System.IO.File.WriteAllText("data/" + Config.SteamLogin + ".json", json);
                listBox1.Items.Add(listBox2.SelectedItem);
                listBox2.Items.Remove(listBox2.SelectedItem);
                howmuchaccountsnotneed = howmuchaccountsnotneed - 1;
                howmuchaccounts = howmuchaccounts + 1;
                uodatelistbox();
            }
            updatelis6box();
        }
        private void uodatelistbox()
        {
            listBox3.Items.Clear();
            listBox2.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    listBox3.Items.Add(Config.SteamLogin);
                }
                else
                {
                    listBox2.Items.Add(Config.SteamLogin);
                }
            }
            materialLabel2.Text = String.Format("Используются = " + howmuchaccounts);
            materialLabel3.Text = String.Format("Не Используются = " + howmuchaccountsnotneed);
        }

        private void materialRaisedButton12_Click(object sender, EventArgs e)
        {
            Task.Run(() => commend());
        }
        private void commend()
        {
            if (accountID > 1)
            {
                Otlegacheck Otlegacheck = new Otlegacheck();
                DirectoryInfo d = new DirectoryInfo(@"data");
                FileInfo[] Files = d.GetFiles("*.json");
                selectblyat = 0;
                foreach (FileInfo file in Files)
                {

                    Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                    if (Config.InUse == true)
                    {
                        itslaunched = true;
                        Otlegacheck.DoCheck("commend", String.Format(Config.SteamLogin + ".json"));
                        selectblyat = selectblyat + 1;
                        Thread.Sleep(500);
                    }
                }
                selectblyat = 0;

                itslaunched = false;
            }
        }

        private void materialRaisedButton16_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                account account = new account();
                account.Steamfile(folderBrowserDialog1.SelectedPath + "\\Steam.exe");
                materialRaisedButton16.Text = "Готово!";
                materialRaisedButton16.Enabled = false;
            }
        }

        private void materialRaisedButton15_Click(object sender, EventArgs e)
        {
            account account = new account();
            account.emaillogin("None");
            account.emailpassword("None");
            account.emailserver("None");
            account.emailssl(false);
            textBox3.Text = "Логин от почты";
            textBox7.Text = "Пароль от почты";
            textBox8.Text = "POP3 Protocol";
            materialRaisedButton14.Enabled = true;
            checkBox4.Enabled = true;
            textBox8.Enabled = true;
            textBox7.Enabled = true;
            textBox3.Enabled = true;
            checkBox4.Enabled = true;
        }

        private void materialRaisedButton14_Click(object sender, EventArgs e)
        {
            using (Pop3 pop3 = new Pop3())
            {
                account account = new account();
                SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
                if (checkBox4.Checked == true)
                {
                    pop3.ConnectSSL(textBox8.Text);
                }
                else
                {
                    pop3.Connect(textBox8.Text);  // or ConnectSSL for SSL 
                }
                pop3.UseBestLogin(textBox3.Text, textBox7.Text);
                MailBuilder builder = new MailBuilder();
                foreach (string uid in pop3.GetAll())
                {
                    IMail email = builder.CreateFromEml(
                      pop3.GetMessageByUID(uid));
                    account.emaillogin(textBox3.Text);
                    account.emailpassword(textBox7.Text);
                    account.emailserver(textBox8.Text);
                    if (checkBox4.Checked == true)
                    {
                        account.emailssl(true);
                    }
                    else
                    {
                        account.emailssl(true);
                    }
                    textBox8.Enabled = false;
                    textBox7.Enabled = false;
                    textBox3.Enabled = false;
                    checkBox4.Enabled = false;
                    materialRaisedButton14.Enabled = false;
                    materialRaisedButton14.Text = "Почта загружена!";
                    return;
                }
            }

        }

        private void materialRaisedButton9_Click(object sender, EventArgs e)
        {
            Task.Run(() => report());
        }
        private void report()
        {
            if (accountID > 1)
            {
                Otlegacheck Otlegacheck = new Otlegacheck();
                DirectoryInfo d = new DirectoryInfo(@"data");
                FileInfo[] Files = d.GetFiles("*.json");
                selectblyat = 0;
                foreach (FileInfo file in Files)
                {
                    Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                    if (Config.InUse == true)
                    {
                        itslaunched = true;
                        Otlegacheck.DoCheck("report", String.Format(Config.SteamLogin + ".json"));
                        selectblyat = selectblyat + 1;
                        Thread.Sleep(500);
                    }
                }
                accountID = 0;
            }
            selectblyat = 0;
        }
        private void materialRaisedButton13_Click(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Clipboard.GetText(), "^[0-9]+$"))
            {
                materialRaisedButton13.Text = Clipboard.GetText();
                accountID = UInt32.Parse(Clipboard.GetText());
            }
            else
            {
                materialRaisedButton13.Text = "Steam3ID блять";
            }
        }

        private void materialRaisedButton10_Click(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Clipboard.GetText(), "^[0-9]+$"))
            {
                materialRaisedButton10.Text = Clipboard.GetText();
                accountID = UInt32.Parse(Clipboard.GetText());
            }
            else
            {
                materialRaisedButton10.Text = "Steam3ID блять";
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton7_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText("data/" + "lastcommend.txt", "0");
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                account account = new account();
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    account.TextCOMMEND("Текст сброшен", Config.SteamLogin + ".json");
                }
            }
            uodatelistboxcommend();
        }

        private void materialRaisedButton6_Click_1(object sender, EventArgs e)
        {
            account account = new account();
            File.WriteAllText("data/" + "lastreport.txt", "0");
            DirectoryInfo d = new DirectoryInfo(@"data");
            FileInfo[] Files = d.GetFiles("*.json");
            foreach (FileInfo file in Files)
            {
                Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + file));
                if (Config.InUse == true)
                {
                    account.TextREPORT("Текст сброшен", Config.SteamLogin + ".json");
                }
            }
            uodatelistboxreport();
        }

        private void materialTabControl1_Enter(object sender, EventArgs e)
        {
            updatelis6box();
        }

        private void materialTabControl1_Leave(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }
        private void materialRaisedButton11_Click(object sender, EventArgs e)
        {
            if (listBox6.SelectedItem != null)
            {
                if (isjoinlobby == false)
                {
                    clrmsg();
                    isjoinlobby = true;
                    string nowacc = listBox6.SelectedItem.ToString();
                    Task.Run(() => lobby.joininlobby(nowacc + ".json"));
                    materialRaisedButton11.Text = "Отключиться";
                    materialRaisedButton20.Visible = true;
                    textBox10.Visible = true;
                    comboBox1.Visible = true;
                    textBox12.Visible = true;
                    textBox11.Visible = true;
                    textBox13.Visible = true;
                    button2.Enabled = true;
                    textBox15.Visible = true;
                    materialRaisedButton18.Visible = true;
                    materialRaisedButton17.Visible = true;
                    textBox16.Visible = true;
                    checkBox5.Visible = true;
                }
                else
                {
                    clrmsg();
                    lobby.statusteam = "Отключен";
                    lobbyid = 0;
                    lobbyid = Convert.ToUInt64(textBox10.Text);
                    name = null;
                    numbersofwins = null;
                    IsPrime = false;
                    Medals = false;
                    materialRaisedButton11.Text = "Подключиться";
                    isjoinlobby = false;
                    materialRaisedButton20.Visible = false;
                    textBox10.Visible = false;
                    comboBox1.Visible = false;
                    textBox12.Visible = false;
                    textBox11.Visible = false;
                    textBox13.Visible = false;
                    button2.Enabled = false;
                    textBox15.Visible = false;
                    materialRaisedButton18.Visible = false;
                    materialRaisedButton17.Visible = false;
                    textBox16.Visible = false;
                    checkBox5.Visible = false;
                }
            }
            else
            {
                materialRaisedButton11.Text = "Выбери аккаунт";
            }
        }

        private void mainform_Enter(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton19_Click(object sender, EventArgs e)
        {
            if (isjoinlobby == true)
            {
                Task.Run(() => lobby.create());
            }
        }

        private void materialRaisedButton20_Click(object sender, EventArgs e)
        {
            Handler.changingrank = false;
            if (isjoinlobby == true)
            {
                autocrash = checkBox6.Checked;
                Autokick = checkBox5.Checked;
                fakesteamid = textBox15.Text;
                clrmsg();
                Handler.whoinlobby = null;
                lobbyid = Convert.ToUInt64(textBox10.Text);
                name = textBox11.Text;
                level = Convert.ToInt32(textBox12.Text);
                rankid = comboBox1.SelectedIndex;
                numbersofwins = textBox13.Text;
                lobby.join();
            }
            else
            {
                isjoinlobby = false;
            }
        }
        public void clrmsg()
        {
            listBox7.Items.Clear();
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            currentmessage = textBox14.Text;
            textBox14.Text = null;
            lobby.SendMessage();
        }

        private void materialRaisedButton17_Click(object sender, EventArgs e)
        {
            Task.Run(() => lobby.crash());
        }

        private void materialRaisedButton18_Click(object sender, EventArgs e)
        {
            if (crashlobby == false)
            {
                crashlobby = true;
                Task.Run(() => lobby.SpamMessage());
                materialRaisedButton18.Text = "Стоп";
                materialRaisedButton17.Visible = false;
            }
            else
            {
                materialRaisedButton18.Text = "СПАМ";
                crashlobby = false;
                materialRaisedButton17.Visible = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedItem != null)
            {
                kicksteamid = Convert.ToInt32(Convert.ToUInt64(getBetween(listBox8.SelectedItem.ToString(), "SteamID:", ".")) - 76561197960265728);
                lobby.Kickguy();
                listBox8.SelectedIndex = -1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1 settingsForm = new Form1();

            // Show the settings form
            settingsForm.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox8.SelectedItem != null)
            {
                string openlink = "http://steamcommunity.com/profiles/" + getBetween(listBox8.SelectedItem.ToString(), "SteamID:", ".");
                System.Diagnostics.Process.Start(openlink);
                listBox8.SelectedIndex = -1;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }
        private void materialRaisedButton21_Click(object sender, EventArgs e)
        {
            if (autoparser == false)
            {
                autoparser = true;
                autokick = checkBox8.Checked;
                textwhenjoin = textBox18.Text;
                autocrash = checkBox9.Checked;
                nonkickablebots = checkBox10.Checked;
                Task.Run(() => lobby.GetLobbys());
                materialRaisedButton21.Text = "Ебашим";
            }
            else
            {
                autoparser = false;
                materialRaisedButton21.Text = "Разъебать CS:GO Cheats";
            }
        }

        private void materialRaisedButton25_Click(object sender, EventArgs e)
        {
            if (isjoinlobby == false)
            {
                isjoinlobby = true;
                string nowacc = listBox9.SelectedItem.ToString();
                Task.Run(() => lobby.joininlobby(nowacc + ".json"));
                materialRaisedButton25.Text = "Отключиться";
                name = "CSGO OverWatch";
                level = 1;
                rankid = 0;
                numbersofwins = "1";
            }
            else
            {
                materialRaisedButton25.Text = "Подключиться";
                isjoinlobby = false;
            }
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton19_Click_1(object sender, EventArgs e)
        {
            if (autoparser == false)
            {
                listenkech = true;
                autoparser = true;
                Task.Run(() => lobby.GetLobbys());
                materialRaisedButton19.Text = "Патрулирую...";
            }
            else
            {
                listenkech = false;
                autoparser = false;
                materialRaisedButton19.Text = "Начать патруль";
            }
            if (startyowerwatch == false)
            {
                startyowerwatch = true;
            }
            else
            {
                startyowerwatch = false;
            }
        }

        private void materialRaisedButton26_Click(object sender, EventArgs e)
        {
            try
            {
                string whitelist = File.ReadAllText("whitelist.txt") + Environment.NewLine + textBox20.Text;
                File.WriteAllText("whitelist.txt", whitelist);
            }
            catch (Exception)
            {
                File.WriteAllText("whitelist.txt", textBox20.Text);
            }
        }

        private void materialRaisedButton28_Click(object sender, EventArgs e)
        {
            if (isjoinlobby == true)
            {
                Autokick = checkBox5.Checked;
                fakesteamid = textBox15.Text;
                clrmsg();
                Handler.whoinlobby = null;
                lobbyid = Convert.ToUInt64(textBox10.Text);
                name = textBox11.Text;
                level = Convert.ToInt32(textBox12.Text);
                rankid = comboBox1.SelectedIndex;
                numbersofwins = textBox13.Text;
                lobby.join();
            }
            else
            {
                isjoinlobby = false;
            }
        }

        private void materialRaisedButton29_Click(object sender, EventArgs e)
        {
            try
            {
                string blacklist = File.ReadAllText("blacklist.txt") + Environment.NewLine + textBox20.Text;
                File.WriteAllText("blacklist.txt", blacklist);
            }
            catch (Exception)
            {
                File.WriteAllText("blacklist.txt", textBox21.Text);
            }
        }

        private void materialRaisedButton27_Click(object sender, EventArgs e)
        {
            if (listBox12.SelectedItem != null)
            {
                string whitelist = File.ReadAllText("whitelist.txt").Replace(listBox12.SelectedItem.ToString(), "");
                File.WriteAllText("whitelist.txt", whitelist);
                listBox12.SelectedIndex = -1;
            }
        }

        private void materialRaisedButton30_Click(object sender, EventArgs e)
        {
            if (listBox13.SelectedItem != null)
            {
                string blacklist = File.ReadAllText("blacklist.txt").Replace(listBox13.SelectedItem.ToString(), "");
                File.WriteAllText("blacklist.txt", blacklist);
                listBox13.SelectedIndex = -1;
            }
        }

        private void materialRaisedButton31_Click(object sender, EventArgs e)
        {
            if (startsearchlobby != true)
            {
                if (listBox9.SelectedItem != null)
                {
                    startsearchlobby = true;
                    materialRaisedButton31.Text = "Ищу...";
                    Task.Run(() => lobby.Findlobby());
                }
            }
            else
            {
                materialRaisedButton31.Text = "Искать в лобби";
                startsearchlobby = false;
            }
        }

        private void materialRaisedButton32_Click(object sender, EventArgs e)
        {
            nahlebscheck = checkBox11.Checked;
            nonahlebscheck = checkBox7.Checked;
            Task.Run(() => checkforbans());
        }
        private void checkforbans()
        {
            string[] ids = File.ReadAllLines("steamids.txt");
            string nahlebs = File.ReadAllText("nahlebs.txt");
            int id = 0;
            string nonahlebs = null;
            while (id != ids.Length)
            {
                if (nahlebs.Contains((ids[id].ToString().Replace("\r\n", "")).Replace("\n", "")) != true)
                {
                    nonahlebs = nonahlebs + Environment.NewLine + ids[id].ToString();
                }
                id = id + 1;
            }
            File.WriteAllText("stata/" + "nonahlebs.txt", nonahlebs);
            File.WriteAllText("stata/" + "nahlebs.txt", nahlebs);
            checkingnowbans = 0;
            bannedbyow = 0;
            string[] idsofstat = File.ReadAllLines("stata/" + "nonahlebs.txt");
            string[] idsofstatnahlebs = File.ReadAllLines("stata/" + "nahlebs.txt");
            if (nonahlebscheck == true)
            {
                for (int i = 0; i <= idsofstat.Count(); i++)
                {
                    checkingnowbans = checkingnowbans + 1;
                    WebClient client = new WebClient();
                    string profile = client.DownloadString("http://steamcommunity.com/profiles/" + idsofstat[i]);
                    if (profile.Contains("1 game ban on record"))
                    {
                        File.WriteAllText("stata/" + "vacedids.txt", File.ReadAllText("stata/" + "vacedids.txt") + Environment.NewLine + idsofstat[i]);
                        bannedbyow = bannedbyow + 1;
                    }
                    else
                    {
                        if (profile.Contains("1 VAC ban on record"))
                        {
                            File.WriteAllText("stata/" + "vacedids.txt", File.ReadAllText("stata/" + "vacedids.txt") + Environment.NewLine + idsofstat[i]);
                            bannedbyow = bannedbyow + 1;
                        }
                        else
                        {
                            if (profile.Contains("Multiple game bans on record"))
                            {
                                File.WriteAllText("stata/" + "vacedids.txt", File.ReadAllText("stata/" + "vacedids.txt") + Environment.NewLine + idsofstat[i]);
                                bannedbyow = bannedbyow + 1;
                            }
                            else
                            {
                                if (profile.Contains("Multiple VAC bans on record"))
                                {
                                    File.WriteAllText("stata/" + "vacedids.txt", File.ReadAllText("stata/" + "vacedids.txt") + Environment.NewLine + idsofstat[i]);
                                    bannedbyow = bannedbyow + 1;
                                }
                            }
                        }
                    }
                }
            }
            if (nahlebscheck == true)
            {
                for (int i = 0; i <= idsofstatnahlebs.Count(); i++)
                {
                    checkingnowbans = checkingnowbans + 1;
                    WebClient client = new WebClient();
                    string profile = client.DownloadString("http://steamcommunity.com/profiles/" + idsofstatnahlebs[i]);
                    if (profile.Contains("1 game ban on record"))
                    {
                        File.WriteAllText("stata/" + "vacedidsnahlebs.txt", File.ReadAllText("stata/" + "vacedidsnahlebs.txt") + Environment.NewLine + idsofstatnahlebs[i]);
                        bannedbyow = bannedbyow + 1;
                    }
                    else
                    {
                        if (profile.Contains("1 VAC ban on record"))
                        {
                            File.WriteAllText("stata/" + "vacedidsnahlebs.txt", File.ReadAllText("stata/" + "vacedidsnahlebs.txt") + Environment.NewLine + idsofstatnahlebs[i]);
                            bannedbyow = bannedbyow + 1;
                        }
                        else
                        {
                            if (profile.Contains("Multiple game bans on record"))
                            {
                                File.WriteAllText("stata/" + "vacedidsnahlebs.txt", File.ReadAllText("stata/" + "vacedidsnahlebs.txt") + Environment.NewLine + idsofstatnahlebs[i]);
                                bannedbyow = bannedbyow + 1;
                            }
                            else
                            {
                                if (profile.Contains("Multiple VAC bans on record"))
                                {
                                    File.WriteAllText("stata/" + "vacedidsnahlebs.txt", File.ReadAllText("stata/" + "vacedidsnahlebs.txt") + Environment.NewLine + idsofstatnahlebs[i]);
                                    bannedbyow = bannedbyow + 1;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void materialRaisedButton22_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("vkpublics.txt", File.ReadAllText("vkpublics.txt") + Environment.NewLine + textBox17.Text);
            }
            catch (Exception)
            {
                File.WriteAllText("vkpublics.txt", textBox17.Text + Environment.NewLine);
            }
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {

        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton33_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText("pathstatus.txt", folderBrowserDialog1.SelectedPath);
            }

        }

        private void materialRaisedButton34_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText("pathsteamids.txt", folderBrowserDialog1.SelectedPath);
            }
        }

        private void materialRaisedButton24_Click(object sender, EventArgs e)
        {
            if (isparsinglocalprofile == false)
            {
                Task.Run(() => findandcrashbithc());
                isparsinglocalprofile = true;
                materialRaisedButton24.Text = "Крашим...";
            }
            else
            {
                isparsinglocalprofile = false;
                materialRaisedButton24.Text = "Крашить бомжей";
            }
        }
        void findandcrashbithc()
        {
            while (isparsinglocalprofile != false)
            {
                lobby.statusteam = "Читаю файл";
                string[] ids = File.ReadAllLines("steamidsforattack.txt");
                for (int i = 0; i < ids.Count(); i++)
                {
                    WebClient client = new WebClient();
                    Thread.Sleep(500);
                    string profile = client.DownloadString("http://steamcommunity.com/profiles/" + ids[i]);
                    lobby.statusteam = "Проверяю профиль...";
                    string lobby1 = getBetween(profile, "steam://joinlobby/730/", "/");
                    if (lobby1 != null)
                    {
                        lobby.statusteam = "Крашу бомжа!";
                        lobbyid = Convert.ToUInt64(lobby1);
                        autocrash = true;
                        lobby.join();
                    }
                }
            }
        }

        private void materialRaisedButton23_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllText("steamidsforattack.txt", File.ReadAllText("steamidsforattack.txt") + Environment.NewLine + textBox19.Text);
            }
            catch (Exception)
            {
                File.WriteAllText("steamidsforattack.txt", textBox19.Text);
            }
            updatelistbox1();
        }

        private void materialRaisedButton35_Click(object sender, EventArgs e)
        {
            if (startsearchlobby != true)
            {
                if (listBox9.SelectedItem != null)
                {
                    if (checkBox14.Checked == true)
                    {
                        startsearchlobby = true;
                        materialRaisedButton35.Text = "Ищу...";
                        Task.Run(() => lobby.OwnSlayerStart());
                        spamsomemessages = false;
                    }
                    else
                    {
                        if (checkBox15.Checked == true)
                        {
                            joinforprotect = true;
                            startsearchlobby = true;
                            materialRaisedButton35.Text = "Ищу...";
                            Task.Run(() => lobby.OwnSlayerStart());
                            spamsomemessages = false;
                        }
                        else
                        {
                            spamsomemessages = checkBox12.Checked;
                            dontcrashihm = checkBox13.Checked;
                            spamessage = "Type -rep, he is cheater: http://steamcommunity.com/id/kzhack/";
                            startsearchlobby = true;
                            materialRaisedButton35.Text = "Спамлю...";
                            Task.Run(() => lobby.spaminalllobys());
                        }
                    }
                }
            }
            else
            {
                materialRaisedButton35.Text = "Спамить в лобби";
                startsearchlobby = false;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Handler.changingrank = true;
            lobby.changerank(Convert.ToUInt64(getBetween(listBox8.SelectedItem.ToString(), "SteamID:", ".")), textBox11.Text, Convert.ToUInt64(textBox13.Text), comboBox1.SelectedIndex, Convert.ToInt32(textBox12.Text), Convert.ToUInt64(textBox10.Text));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Handler.changingrank = true;
            lobby.join();
            for (int i = 0; i < listBox8.Items.Count; i++)
            {
                if (getBetween(listBox8.Items[i].ToString(), "Name:", "SteamID").Contains("Name") != true)
                {
                    lobby.changerank(Convert.ToUInt64(getBetween(listBox8.Items[i].ToString(), "SteamID:", ".")), textBox11.Text, Convert.ToUInt64(textBox13.Text), comboBox1.SelectedIndex, Convert.ToInt32(textBox12.Text), Convert.ToUInt64(textBox10.Text));
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void materialRaisedButton36_Click(object sender, EventArgs e)
        {
            justjointolisten = true;
            Autokick = checkBox5.Checked;
            fakesteamid = textBox15.Text;
            clrmsg();
            Handler.whoinlobby = null;
            lobbyid = Convert.ToUInt64(textBox10.Text);
            name = textBox11.Text;
            level = Convert.ToInt32(textBox12.Text);
            rankid = comboBox1.SelectedIndex;
            numbersofwins = textBox13.Text;
            lobby.join();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            File.WriteAllText("testid.txt", listBox8.SelectedItem.ToString());
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            lobby.datatest();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton39_Click(object sender, EventArgs e)
        {
            if (scaneveything != true)
            {
                if (listBox9.SelectedItem != null)
                {
                    scaneveything = true;
                    materialRaisedButton31.Text = "Ищу...";
                    Task.Run(() => lobby.scaneverything());
                }
            }
            else
            {
                materialRaisedButton31.Text = "Искать в лобби";
                scaneveything = false;
            }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}