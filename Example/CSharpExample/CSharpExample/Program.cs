using Newtonsoft.Json.Linq;
using SelfDiagnosisLibrary;
using System;
using System.Threading.Tasks;

namespace CSharpExample
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static async Task<JObject> UserInfo(string cdcUrl, string orgCode, string name, string birth, string password)
        {
            v2 api = new v2();
            api.atptOfcdcConctUrl = cdcUrl;
            JObject userData = await api.findUser(orgCode, name, birth);
            string token = userData["token"].ToString();
            token = await api.loginToken(token, password);
            JArray userGroup = await api.selectUserGroup(token);
            string userPNo = userGroup[0]["userPNo"].ToString();
            JObject userInfo = await api.getUserInfo(token, orgCode, userPNo);
            return userInfo;
        }

        static async Task<string> Healthy(JObject userInfo)
        {
            if (userInfo["isHealthy"] == null)//미참여
            {
                return "미참여";
            }
            else if ((bool)userInfo["isHealthy"])//정상
            {
                return " 정상";
            }
            else if (!(bool)userInfo["isHealthy"])//유증상
            {
                return "유증상";
            }
            return "오류";
        }

        static async void Servey(v2 api, JObject userInfo)
        {
            string res = await api.registerServey(userInfo["token"].ToString());
        }
    }
}
