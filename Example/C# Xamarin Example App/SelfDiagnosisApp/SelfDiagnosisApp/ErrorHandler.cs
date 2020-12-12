using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SelfDiagnosisApp
{
    public class ErrorHandler
    {
        public static async void HandleError(Exception ex)
        {
            if (Application.Current.MainPage != null)
            {
                try
                {
                    bool copy = await Application.Current.MainPage.DisplayAlert("에러", ex.ToString(), "복사", "확인");
                    if (copy)
                        await Clipboard.SetTextAsync(ex.ToString());
                    SendError(ex);
                }
                catch
                {
                    Device.BeginInvokeOnMainThread(() => HandleError(ex));
                }
            }
            else
            {
                Device.BeginInvokeOnMainThread(() => HandleError(ex));
            }
        }

        public static void SendError(Exception ex)
        {
            try
            {
                SendMessage(ex.ToString());
            }
            catch { }
        }

        public static async void SendMessage(string str)
        {
            try
            {
                string url = "https://discordapp.com/api/webhooks/778646133693218836/zkUIUU2Y7eBbjop9OW5Al8AiITg5tySooBMMwU3r0AscJiV8oi6KuuNpTLrKK52fJzgL";
                string icon = "https://image.flaticon.com/icons/png/128/221/221755.png";
                List<string> chunks = SplitChunk(str, 2000).ToList();
                foreach (string chunk in chunks)
                {
                    await Webhook.SendMessageResult(url: url, chunk, "자가진단 앱 (에러)", icon);
                }
            }
            catch { }
        }

        public static IEnumerable<string> SplitChunk(string str, int n)
        {
            if (string.IsNullOrEmpty(str) || n < 1)
                return new List<string>();

            return Enumerable.Range(0, (int)Math.Ceiling((double)str.Length / 2000))
                            .Select(i => str.Substring(i * n, Math.Min(str.Length - i * n, n)));
        }
    }
}
