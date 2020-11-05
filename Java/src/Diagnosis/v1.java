package Diagnosis;

import Diagnosis.lctnScCodes;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import javax.crypto.Cipher;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.security.KeyFactory;
import java.security.PublicKey;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

public class v1 {
    private static final String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
    public String atptOfcdcConctUrl = "hcs.eduro.go.kr";
    private String orgCode;
    private String token;

    /**
     * @deprecated Replaced by {@link v2#searchSchool(lctnScCodes, schulCrseScCodes, String)}
     * @param lctnScCode Select location in {@link lctnScCodes}
     * @param schulCrseScCode Select grade in {@link schulCrseScCodes}
     * @param schulName school name
     * @return school's orgCode of first in search
     * @throws ParseException in parse from http result to JSON
     * @throws IOException in http post request
     */
    @Deprecated
    public String schoolCode(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String schulName) throws ParseException, IOException {
        String data = Http.Get(String.format("https://hcs.eduro.go.kr/v2/searchSchool?lctnScCode=%s&schulCrseScCode=%s&orgName=%s&currentPageNo=1", lctnScCode.getCode(), schulCrseScCode.getCode(), URLEncoder.encode(schulName)));
        JSONParser jsonParser = new JSONParser();
        JSONObject jsonObject = (JSONObject)jsonParser.parse(data);
        JSONArray schulList = (JSONArray) jsonObject.get("schulList");
        JSONObject schulFirst = (JSONObject) schulList.get(0);
        atptOfcdcConctUrl = (String) schulFirst.get("atptOfcdcConctUrl");
        orgCode = (String) schulFirst.get("orgCode");
        return orgCode;
    }

    /**
     * @deprecated Replaced by {@link v2#findUser(String, String, String)}
     * @param orgCode At schoolCode function result
     * @param Name Your name
     * @param Birth Your birth day to YYMMDD format
     * @return return token
     * @throws ParseException in JSON parse
     * @throws IOException in http request
     */
    @Deprecated
    public String loginToken(String orgCode, String Name, String Birth) throws ParseException, IOException {
        if (orgCode.isEmpty())
            orgCode = this.orgCode;
        JSONObject parameter = new JSONObject();
        parameter.put("orgcode", orgCode);
        parameter.put("name", Encrypt.encode(Name, publicKey));
        parameter.put("birthday", Encrypt.encode(Birth, publicKey));
        String url = String.format("https://%s/loginwithschool", atptOfcdcConctUrl);
        String data = Http.Post(url, parameter.toString());
        System.out.println(data);
        JSONParser jsonParser = new JSONParser();
        JSONObject jsonObject = (JSONObject)jsonParser.parse(data);
        token = (String) jsonObject.get("token");
        return token;
    }

    /**
     * @deprecated This method was moved to v2 class<br>
     * Use {@link v2#registerServey(String)} instead.
     * @param token Token value received as a result of login
     * @return Servey result ("OK" or JSON String)
     * @throws IOException in http post request
     */
    @Deprecated
    public String servey(String token) throws IOException {
        System.out.print(Console.ANSI_YELLOW);
        System.out.print("Deprecated warning in v1.servey function\n-   v2 의 registerServey 를 사용해주시기 바랍니다.");
        System.out.println(Console.ANSI_RESET);
        v2 api = new v2();
        api.atptOfcdcConctUrl = this.atptOfcdcConctUrl;
        return api.registerServey(token);
    }
}