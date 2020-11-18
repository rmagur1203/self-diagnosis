using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web;

namespace SelfDiagnosisLibrary
{
    public class v1
    {
        private static readonly string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
        public String atptOfcdcConctUrl = "hcs.eduro.go.kr";
        private String orgCode;
        private String token;

        public async Task<string> schoolCode(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String schulName)
        {
            String data = await Http.Get(String.Format("https://hcs.eduro.go.kr/v2/searchSchool?lctnScCode={0}&schulCrseScCode=%s&orgName=%s&currentPageNo=1", (int)lctnScCode, (int)schulCrseScCode, HttpUtility.UrlEncode(schulName)));
            JObject jsonObject = JObject.Parse(data);
            JArray schulList = (JArray)jsonObject["schulList"];
            JObject schulFirst = (JObject)schulList[0];
            atptOfcdcConctUrl = (String)schulFirst["atptOfcdcConctUrl"];
            orgCode = (String)schulFirst["orgCode"];
            return orgCode;
        }


        public async Task<string> loginToken(String orgCode, String Name, String Birth)
        {
            if (orgCode.isEmpty())
                orgCode = this.orgCode;
            JObject parameter = new JObject();
            parameter.Add("orgcode", orgCode);
            parameter.Add("name", await Encrypt.encryptAsync(Name, publicKey));
            parameter.Add("birthday", await Encrypt.encryptAsync(Birth, publicKey));
            String url = String.Format("https://{0}/loginwithschool", atptOfcdcConctUrl);
            String data = await Http.PostJson(url, parameter.ToString());
            JObject jsonObject = JObject.Parse(data);
            token = (String)jsonObject["token"];
            return token;
        }

        public async Task<string> servey(String token)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Deprecated warning in v1.servey function\n-   v2 의 registerServey 를 사용해주시기 바랍니다.");
            Console.ResetColor();
            v2 api = new v2();
            api.atptOfcdcConctUrl = this.atptOfcdcConctUrl;
            return await api.registerServey(token);
        }
    }
}
