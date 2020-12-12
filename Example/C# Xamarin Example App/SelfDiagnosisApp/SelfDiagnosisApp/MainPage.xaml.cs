using Newtonsoft.Json.Linq;
using SelfDiagnosisLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using static SelfDiagnosisApp.UserEnumerator;

namespace SelfDiagnosisApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Title = "자가진단";
            LoadUsers();
        }

        public void LoadUsers()
        {
            Users.Children.Clear();
            InitButton();
            try
            {
                UserEnumerator userList = new UserEnumerator();
                foreach (User user in userList)
                {
                    UserPanel panel = new UserPanel(user);
                    Users.Children.Add(panel);
                }
                var enumerator = userList.GetEnumerator();
                var list = new List<User>();
                while (enumerator.MoveNext())
                    list.Add((User)enumerator.Current);
                string org = JArray.FromObject(list).ToString();
                Console.WriteLine(org);
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
            _ = Task.Factory.StartNew(() => LoadUsersFromWeb());

            async void LoadUsersFromWeb()
            {
                try
                {
                    UserEnumerator userList = new UserEnumerator();
                    List<User> users = await userList.GetUsersAsync();
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Users.Children.Clear();
                        InitButton();
                        foreach (User user in users)
                        {
                            UserPanel panel = new UserPanel(user);
                            Users.Children.Add(panel);
                        }
                    });
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError(ex);
                    _ = Task.Factory.StartNew(() => LoadUsersFromWeb());
                }
            }
        }

        public void InitButton()
        {
            Button diagnosis = new Button();
            diagnosis.Text = "전체 자가진단";
            diagnosis.Clicked += Diagnosis_Clicked;
            Users.Children.Add(diagnosis);

            Button sharestatus = new Button();
            sharestatus.Text = "상태 공유";
            sharestatus.Clicked += Sharestatus_Clicked;
            Users.Children.Add(sharestatus);

            Button reload = new Button();
            reload.Text = "다시 로드";
            reload.Clicked += Reload_Clicked;
            Users.Children.Add(reload);
        }

        private void Reload_Clicked(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private async void Sharestatus_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserEnumerator userList = new UserEnumerator();
                List<User> users = await userList.GetUsersAsync();
                foreach (User user in users)
                {
                    v2 api = new v2();
                    api.atptOfcdcConctUrl = user.cdcUrl;
                    JObject userData = await api.findUser(user.orgCode, user.name, user.birth);
                    string token = userData["token"].ToString();
                    token = await api.loginToken(token, user.password);
                    JArray userGroup = await api.selectUserGroup(token);
                    string userPNo = userGroup[0]["userPNo"].ToString();
                    JObject userInfo = await api.getUserInfo(token, user.orgCode, userPNo);
                    Webhook.SendMessage($"{user.name}: {userInfo["registerDtm"]}, 상태: {(((bool)userInfo["isHealthy"]) ? "정상" : "비정상")}");
                }
                Alert("공유", "공유했습니다.", "닫기");
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        private async void Diagnosis_Clicked(object sender, EventArgs e)
        {
            try
            {
                UserEnumerator userList = new UserEnumerator();
                List<User> users = await userList.GetUsersAsync();
                foreach (User user in users)
                {
                    v2 api = new v2();
                    api.atptOfcdcConctUrl = user.cdcUrl;
                    JObject userData = await api.findUser(user.orgCode, user.name, user.birth);
                    string token = userData["token"].ToString();
                    token = await api.loginToken(token, user.password);
                    JArray userGroup = await api.selectUserGroup(token);
                    string userPNo = userGroup[0]["userPNo"].ToString();
                    token = userGroup[0]["token"].ToString();
                    JObject userInfo = await api.getUserInfo(token, user.orgCode, userPNo);
                    token = userInfo["token"].ToString();
                    if (userInfo["isHealthy"] == null || ((bool)userInfo["isHealthy"]) == false)
                    {
                        string res = await api.registerServey(token);
                        Alert("성공", $"{user.name}: {res}", "닫기");
                        Webhook.SendMessage($"{user.name} : {res}");
                    }
                }
                LoadUsers();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        public static async void Alert(string title, string message, string cancel)
        {
            if (Application.Current.MainPage == null)
                Device.BeginInvokeOnMainThread(() => Alert(title, message, cancel));
            else
                await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
