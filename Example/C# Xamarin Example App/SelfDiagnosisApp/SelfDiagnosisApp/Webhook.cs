using Newtonsoft.Json.Linq;
using SelfDiagnosisLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SelfDiagnosisApp
{
    public class Webhook
    {
        public static string url = "https://discordapp.com/api/webhooks/778595103211913227/58EmfmLWG8Wbz1eb5QFdobGD-Za-A3ngMYbx5qGfBHDIYq0-DTBYzTo0iUH2tIN3ixQL";

        private const string USERNAME = "자가진단 앱";
        private const string AVATAR_URL = "";

        public static void SendMessage(string content, string username = USERNAME, string avatar_url = AVATAR_URL)
        {
            SendMessage(url, content, username, avatar_url);
        }

        public static void SendMessage(string url, string content, string username = USERNAME, string avatar_url = AVATAR_URL)
        {
            JObject form = new JObject();
            form.Add("content", content);
            form.Add("username", username);
            form.Add("avatar_url", avatar_url);
            _ = Http.PostJson(url, form.ToString());
        }

        public static async Task SendMessageAsync(string url, string content, string username = USERNAME, string avatar_url = AVATAR_URL)
        {
            JObject form = new JObject();
            form.Add("content", content);
            form.Add("username", username);
            form.Add("avatar_url", avatar_url);
            await Http.PostJson(url, form.ToString());
        }

        public static async Task<string> SendMessageResult(string url, string content, string username = USERNAME, string avatar_url = AVATAR_URL)
        {
            JObject form = new JObject();
            form.Add("content", content);
            form.Add("username", username);
            form.Add("avatar_url", avatar_url);
            return await Http.PostJson(url, form.ToString());
        }
    }
}
