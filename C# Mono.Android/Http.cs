using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SelfDiagnosisLibrary
{
    public class Http
    {
        public static async Task<string> Get(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.GetAsync(url);
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostToken(string url, string content, string token)
        {
            string newToken = token.Replace("Bearer", "").Trim();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newToken);
            HttpResponseMessage res = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<string> PostJson(string url, string content)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
            return await res.Content.ReadAsStringAsync();
        }
    }
}
