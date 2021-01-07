using System;

namespace IntegratedModule
{
    public static class S_StringModule
    {
        public static string format(this string s, System.Collections.Generic.KeyValuePair<string, string>[] keyValues)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> keyValue in keyValues)
                s = s.Replace("{" + keyValue.Key + "}", keyValue.Value);
            return s;
        }
    }
}
