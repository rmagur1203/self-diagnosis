﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace SelfDiagnosisLibrary
{
    class Json
    {
        public static string normalServey = "{\"rspns00\":\"Y\",\"rspns01\":\"1\",\"rspns02\":\"1\",\"rspns03\":null,\"rspns04\":null,\"rspns05\":null,\"rspns06\":null,\"rspns07\":null,\"rspns08\":null,\"rspns09\":\"0\",\"rspns10\":null,\"rspns11\":null,\"rspns12\":null,\"rspns13\":null,\"rspns14\":null,\"rspns15\":null,\"deviceUuid\":\"\", \"upperToken\":\"\", \"upperUserNameEncpt\":\"\"}";
        public static string getNormalServey(string token, string userName)
        {
            JObject obj = JObject.Parse(normalServey);
            obj["upperToken"] = token;
            obj["upperUserNameEncpt"] = userName;
            return obj.ToString();
        }
    }
}
