using Newtonsoft.Json.Linq;
using SelfDiagnosisLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SelfDiagnosisApp
{
    public class UserEnumerator
    {
        public class User
        {
            public string cdcUrl;
            public string orgCode;
            public string name;
            public string birth;
            public string password;

            public User(string cdcUrl, string orgCode, string name, string birth, string password)
            {
                (this.cdcUrl, this.orgCode, this.name, this.birth, this.password) = (cdcUrl, orgCode, name, birth, password);
            }

            public void Deconstruct(out string cdcUrl, out string orgCode, out string name, out string birth, out string password)
            {
                (cdcUrl, orgCode, name, birth, password) = (this.cdcUrl, this.orgCode, this.name, this.birth, this.password);
            }

            /*
            public lctnScCodes _lctnScCode;
            public schulCrseScCodes _schulCrseScCode;
            public string _schoolName;
            public string _name;
            public string _birth;

            public User(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, string schoolName, string name, string birth)
            {
                (_lctnScCode, _schulCrseScCode, _schoolName, _name, _birth) = (lctnScCode, schulCrseScCode, schoolName, name, birth);
            }

            public void Deconstruct(out lctnScCodes lctnScCode, out schulCrseScCodes schulCrseScCode, out string schoolName, out string name, out string birth)
            {
                (lctnScCode, schulCrseScCode, schoolName, name, birth) = (_lctnScCode, _schulCrseScCode, _schoolName, _name, _birth);
            }
            */
        }

        public IEnumerator GetEnumerator()
        {
            yield return new User("goehcs.eduro.go.kr", "**********", "***", "*****", "****");
        }

        public async Task<List<User>> GetUsersAsync()
        {
            const bool web = false;
            if (web)
                return await GetUsersAsyncFromWeb();
            else
            {
                List<User> users = new List<User>();
                var enumerator = GetEnumerator();
                while (enumerator.MoveNext())
                    users.Add(enumerator.Current as User);
                return users;
            }
        }

        public async Task<List<User>> GetUsersAsyncFromWeb()
        {
            string s = await Http.Get("");
            JArray array = JArray.Parse(s);
            List<User> users = new List<User>();
            foreach (JToken obj in array)
            {
                users.Add(obj.ToObject<User>());
            }
            return users;
        }
    }
}
