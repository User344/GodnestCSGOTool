using System;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Drawing;

namespace GodnestCSGOTool
{
    class Config
    {
        public bool InUse { get; set; }
        public string SteamLogin { get; set; }
        public string SteamPassword { get; set; }
        public string SteamLoginToken { get; set; }
        public string TextREPORT { get; set; }
        public string TextCOMMEND { get; set; }
        public ulong TextOTLYOGA { get; set; }
        public string TextRANK { get; set; }
        public string TextVAC { get; set; }
        public ulong cooldowntimechecked { get; set; }
        public ulong Lastreporttime { get; set; }
        public ulong Lastcommendtime { get; set; }
        public string emaillogin { get; set; }
        public string emailpassword { get; set; }
        public string emailserver { get; set; }
        public bool emailssl { get; set; }
    }
    class SettingsConfig
    {
        public bool Accslegit { get; set; }
        public string Steamfile { get; set; }
        public string emaillogin { get; set; }
        public string emailpassword { get; set; }
        public string emailserver { get; set; }
        public bool emailssl { get; set; }
    }

    class account
    {
        Config Config;
        SettingsConfig SettingsConfig;
        public void Accslegit(bool InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = InUsedata,
                Steamfile = SettingsConfig.Steamfile,
                emaillogin = SettingsConfig.emaillogin,
                emailpassword = SettingsConfig.emailpassword,
                emailserver = SettingsConfig.emailserver,
                emailssl = SettingsConfig.emailssl,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }
        public void Steamfile(string InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = SettingsConfig.Accslegit,
                Steamfile = InUsedata,
                emaillogin = SettingsConfig.emaillogin,
                emailpassword = SettingsConfig.emailpassword,
                emailserver = SettingsConfig.emailserver,
                emailssl = SettingsConfig.emailssl,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }
        public void emaillogin(string InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = SettingsConfig.Accslegit,
                Steamfile = SettingsConfig.Steamfile,
                emaillogin = InUsedata,
                emailpassword = SettingsConfig.emailpassword,
                emailserver = SettingsConfig.emailserver,
                emailssl = SettingsConfig.emailssl,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }
        public void emailpassword(string InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = SettingsConfig.Accslegit,
                Steamfile = SettingsConfig.Steamfile,
                emaillogin = SettingsConfig.emaillogin,
                emailpassword = InUsedata,
                emailserver = SettingsConfig.emailserver,
                emailssl = SettingsConfig.emailssl,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }
        public void emailserver(string InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = SettingsConfig.Accslegit,
                Steamfile = SettingsConfig.Steamfile,
                emaillogin = SettingsConfig.emaillogin,
                emailpassword = SettingsConfig.emailpassword,
                emailserver = InUsedata,
                emailssl = SettingsConfig.emailssl,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }
        public void emailssl(bool InUsedata)
        {
            SettingsConfig = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText("cfg.json"));
            SettingsConfig _SettingsConfig = new SettingsConfig()
            {
                Accslegit = SettingsConfig.Accslegit,
                Steamfile = SettingsConfig.Steamfile,
                emaillogin = SettingsConfig.emaillogin,
                emailpassword = SettingsConfig.emailpassword,
                emailserver = SettingsConfig.emailserver,
                emailssl = InUsedata,
            };
            string json = JsonConvert.SerializeObject(_SettingsConfig, Formatting.Indented);
            System.IO.File.WriteAllText("cfg.json", json);
        }

        public void InUse(bool InUsedata, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = InUsedata,
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
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void SteamLogin(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = data,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void SteamPassword(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = data,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void TextREPORT(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = data,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void TextCOMMEND(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = data,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void TextOTLYOGA(ulong data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = data,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void TextRANK(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = data,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void TextVAC(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = data,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void cooldowntimechecked(uint data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = data,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void Lastreporttime(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Convert.ToUInt64(data),
                Lastcommendtime = Config.Lastcommendtime,
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
        public void Lastcommendtime(string data, string filename)
        {
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("data/" + filename));
            Config _Config = new Config()
            {
                InUse = Config.InUse,
                SteamLogin = Config.SteamLogin,
                SteamPassword = Config.SteamPassword,
                TextREPORT = Config.TextREPORT,
                TextCOMMEND = Config.TextCOMMEND,
                TextOTLYOGA = Config.TextOTLYOGA,
                TextRANK = Config.TextRANK,
                TextVAC = Config.TextVAC,
                cooldowntimechecked = Config.cooldowntimechecked,
                Lastreporttime = Config.Lastreporttime,
                Lastcommendtime = Convert.ToUInt64(data),
                emaillogin = Config.emaillogin,
                emailpassword = Config.emailpassword,
                emailserver = Config.emailserver,
                emailssl = Config.emailssl,
            };
            string json = JsonConvert.SerializeObject(_Config, Formatting.Indented);
            System.IO.File.WriteAllText("data/" + filename, json);
        }
    }
}
