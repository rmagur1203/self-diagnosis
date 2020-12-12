using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using System.Web;

namespace SelfDiagnosisLibrary
{
    /// <summary>
    /// Api 버전2 입니다. 
    /// </summary>
    public class v2
    {
        /// <summary>
        /// 이름, 생년월일, 비밀번호 암호화 등에 사용되는 RSA 공개 키 입니다.
        /// </summary>
        public static readonly string publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
        /// <summary>
        /// 요청할 때 사용하는 호스트의 도메인입니다.
        /// </summary>
        public String atptOfcdcConctUrl = "hcs.eduro.go.kr";
        private String token;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lctnScCode"></param>
        /// <param name="schulCrseScCode"></param>
        /// <param name="orgName"></param>
        /// <seealso cref="searchSchool(lctnScCodes, schulCrseScCodes, string, string)"/>
        /// <returns></returns>
        public async Task<JArray> searchSchool(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String orgName) {
            return await searchSchool(lctnScCode, schulCrseScCode, orgName, "school");
        }
        /// <summary>
        /// 학교의 orgCode 와 <see cref="atptOfcdcConctUrl"/>를 얻기 위한 함수입니다.
        /// </summary>
        /// <param name="lctnScCode"></param>
        /// <param name="schulCrseScCode"></param>
        /// <param name="orgName"></param>
        /// <param name="loginType"></param>
        /// <returns>검색된 학교 목록을 반환합니다.</returns>
        public async Task<JArray> searchSchool(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String orgName, String loginType) {
            String data = await Http.Get(
                    String.Format(
                            "https://hcs.eduro.go.kr/v2/searchSchool?lctnScCode={0}&schulCrseScCode={1}&orgName={2}&loginType={3}",
                            (int)lctnScCode,
                            (int)schulCrseScCode,
                            HttpUtility.UrlEncode(orgName),
                            HttpUtility.UrlEncode(loginType)
                    )
            );
            JObject jsonObject = JObject.Parse(data);
            JArray schulList = (JArray)jsonObject["schulList"];
            if (schulList.Count == 1)
                this.atptOfcdcConctUrl = (String)((JObject)schulList[0])["atptOfcdcConctUrl"];
            return schulList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="name"></param>
        /// <param name="birthDay"></param>
        /// <returns></returns>
        public async Task<JObject> findUser(String orgCode, String name, String birthDay) {
            return await findUser(orgCode, name, birthDay, "school");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="name"></param>
        /// <param name="birthDay"></param>
        /// <param name="loginType"></param>
        /// <returns></returns>
        public async Task<JObject> findUser(String orgCode, String name, String birthDay, String loginType) {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            JObject json = new JObject();
            json.Add("orgCode", orgCode);
            json.Add("name", await Encrypt.encryptAsync(name, publicKey));
            json.Add("birthday", await Encrypt.encryptAsync(birthDay, publicKey));
            json.Add("loginType", loginType);
            json.Add("stdntPNo", null);
            String data = await Http.PostJson(
                    String.Format("https://{0}/v2/findUser", atptOfcdcConctUrl),
                    json.ToString()
            );
            return JObject.Parse(data);
        }
        public async Task<bool> hasPassword(String token)
        {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            String data = await Http.PostToken(
                    String.Format("https://{0}/v2/hasPassword", atptOfcdcConctUrl),
                    "",
                    token
            );
            return bool.Parse(data);
        }
        public async Task<string> validatePassword(String token, String password)
        {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            JObject json = new JObject();
            json.Add("deviceUuid", "");
            json.Add("password", await Encrypt.encryptAsync(password, publicKey));
            String data = await Http.PostToken(
                String.Format("https://{0}/v2/validatePassword", atptOfcdcConctUrl),
                json.ToString(),
                token
            );
            return data;
        }
        public bool validPWD2bool(String body)
        {
            try
            {
                return bool.Parse(body);
            }
            catch
            {
                return false;
            }
        }
        public async Task<string> loginToken(String token, String password)
        {
            bool has = await hasPassword(token);
            if (has)
            {
                string valid = await validatePassword(token, password);
                try
                {
                    JObject.Parse(valid);
                    throw new Exception("비밀번호가 틀립니다.\n"+valid);
                }
                catch (Exception)
                {
                    return valid.Replace("\"", "");
                }
            }
            else
                return token;
        }
        public async Task<JArray> selectUserGroup(String token) {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            String data = await Http.PostToken(
                    String.Format("https://{0}/v2/selectUserGroup", atptOfcdcConctUrl),
                    "",
                    token
            );
            return JArray.Parse(data);
        }
        public async Task<JObject> getUserInfo(String token, String orgCode, String userPNo) {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            JObject json = new JObject();
            json.Add("orgCode", orgCode);
            json.Add("userPNo", userPNo);
            String data = await Http.PostToken(
                    String.Format("https://{0}/v2/getUserInfo", atptOfcdcConctUrl),
                    json.ToString(),
                    token
            );
            return JObject.Parse(data);
        }
        /// <summary>
        /// 서버로 설문조사를 전송합니다.
        /// </summary>
        /// <param name="token"><see cref="v2.getUserInfo(string, string, string)"/> 의 반환값에서 Token 을 가져와 사용해야 합니다.</param>
        /// <returns></returns>
        public async Task<string> registerServey(String token)
        {
            return await registerServey(token, Json.getNormalServey(token, ""));
        }
        public async Task<string> registerServey(String token, string content)
        {
            if (atptOfcdcConctUrl == "hcs.eduro.go.kr")
                throw new Exception("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
            if (token.Trim() == "")
                token = this.token;
            String url = String.Format("https://{0}/registerServey", atptOfcdcConctUrl);
            String data = await Http.PostToken(url, content, token);
            return data;
        }
    }
}
