using Newtonsoft.Json.Linq;
using SelfDiagnosisLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static SelfDiagnosisApp.UserEnumerator;

namespace SelfDiagnosisApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPanel : ContentView
    {
        private User user;

        public UserPanel(User user)
        {
            InitializeComponent();
            this.user = user;
            Load();
        }

        public async void Load()
        {
            try
            {
                UserName.Text = user.name;
                Border.BackgroundColor = Color.Gray;
                v2 api = new v2();
                api.atptOfcdcConctUrl = user.cdcUrl;
                JObject userData = await api.findUser(user.orgCode, user.name, user.birth);
                string token = userData["token"].ToString();
                token = await api.loginToken(token, user.password);
                JArray userGroup = await api.selectUserGroup(token);
                string userPNo = userGroup[0]["userPNo"].ToString();
                JObject userInfo = await api.getUserInfo(token, user.orgCode, userPNo);
                if (userInfo["isHealthy"] == null)//미참여
                {
                    Border.BackgroundColor = Color.FromRgb(180, 180, 180);
                    Status.Text = "미참여";
                }
                else if ((bool)userInfo["isHealthy"])//정상
                {
                    //Border.BackgroundColor = Color.FromRgba(0, 128, 0, 77);//"정상";
                    Border.BackgroundColor = Color.FromRgb(33, 152, 234);
                    Status.Text = " 정상";
                }
                else if (!(bool)userInfo["isHealthy"])//유증상
                {
                    //Border.BackgroundColor = Color.FromRgba(255, 165, 0, 77);//"유증상";
                    Border.BackgroundColor = Color.FromRgb(254, 147, 52);
                    Status.Text = "유증상";
                }
                UserName.Text = $"{user.name} ({userInfo["registerDtm"]})";
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
                    MainPage.Alert("성공", $"{user.name}: {res}", "닫기");
                    Webhook.SendMessage($"{user.name} : {res}");
                }
                else
                {
                    MainPage.Alert("자가진단", $"{user.name}님은 이미 자가진단을 했습니다.", "닫기");
                }
                Load();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }

        private async void Servey_Clicked(object sender, EventArgs e)
        {
            try
            {
                var mainPage = Application.Current.MainPage;
                v2 api = new v2();
                api.atptOfcdcConctUrl = user.cdcUrl;
                JObject userData = await api.findUser(user.orgCode, user.name, user.birth);
                string token = userData["token"].ToString();
                bool hasPWD = await api.hasPassword(token);
                if (hasPWD)
                {
                    string pwd = await mainPage.DisplayPromptAsync("자가진단", "비밀번호 입력", placeholder: "비밀번호");
                    if (pwd == "") return;
                    string validStr = await api.validatePassword(token, pwd);
                    bool valid = api.validPWD2bool(validStr);
                    if (!valid)
                    {
                        JObject err = JObject.Parse(validStr);
                        MainPage.Alert("자가진단", $"비밀번호가 맞지 않습니다.\n비밀번호를 {err["data"]["failCnt"]}회 틀리셨습니다.", "확인");
                        ErrorHandler.SendMessage(err.ToString());
                        return;
                    }
                }
                JArray userGroup = await api.selectUserGroup(token);
                string userPNo = userGroup[0]["userPNo"].ToString();
                JObject userInfo = await api.getUserInfo(token, user.orgCode, userPNo);

                JObject content = JObject.Parse("{\"rspns03\":null,\"rspns04\":null,\"rspns05\":null,\"rspns06\":null,\"rspns07\":null,\"rspns08\":null,\"rspns10\":null,\"rspns11\":null,\"rspns12\":null,\"rspns13\":null,\"rspns14\":null,\"rspns15\":null,\"deviceUuid\":\"\"}");
                string question01 = "학생 본인이 37.5℃ 이상 발열 또는 발열감이 있나요?\n단, 기저질환 등으로 코로나19와 관계없이 평소에 발열 증상이 계속되는 경우는 제외";
                string question02 = "학생에게 코로나19가 의심되는 아래의 임상증상*이 있나요?\n* (주요 임상증상) 기침, 호흡곤란, 오한, 근육통, 두통, 인후통, 후각·미각 소실 또는 폐렴\n단, 기저질환 등으로 코로나19와 관계없이 평소에 다음 증상이 계속되는 경우는 제외";
                string question03 = "학생 본인 또는 동거인이 방역당국에 의해 현재 자가격리가 이루어지고 있나요?\n※ < 방역당국 지침 > 최근 14일 이내 해외 입국자, 확진자와 접촉자 등은 자가격리 조치\n단, 직업특성상 잦은 해외 입·출국으로 의심증상이 없는 경우 자가격리 면제";
                string rspns01 = (await mainPage.DisplayAlert("설문 1", question01, "예", "아니요")) ? "2" : "1";
                string rspns02 = (await mainPage.DisplayAlert("설문 2", question02, "예", "아니요")) ? "0" : "1";
                string rspns09 = (await mainPage.DisplayAlert("설문 3", question03, "예", "아니요")) ? "1" : "0";
                string rspns00 = ("1" == rspns01 && "1" == rspns02 && "0" == rspns09) ? "Y" : "N";
                content.Add("rspns01", rspns01);
                content.Add("rspns02", rspns02);
                content.Add("rspns09", rspns09);
                content.Add("rspns00", rspns00);
                content.Add("upperToken", "");

                string res = await api.registerServey(token, content.ToString());
                MainPage.Alert("성공", $"{user.name}: {res}", "닫기");
                Webhook.SendMessage($"{user.name} : {res}");
                Load();
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError(ex);
            }
        }
    }
}