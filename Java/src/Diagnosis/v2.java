package Diagnosis;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import java.io.IOException;
import java.net.URLEncoder;

public class v2 {
    private static final String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
    private String atptOfcdcConctUrl = "hcs.eduro.go.kr";
    private String orgCode;
    private String token;

    public JSONArray searchSchool(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String orgName) throws ParseException, IOException {
        return searchSchool(lctnScCode, schulCrseScCode, orgName, "school");
    }
    public JSONArray searchSchool(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String orgName, String loginType) throws ParseException, IOException {
        String data = Http.Get(
                String.format(
                        "https://hcs.eduro.go.kr/v2/searchSchool?lctnScCode=%s&schulCrseScCode=%s&orgName=%s&loginType=%s",
                        lctnScCode.getCode(),
                        schulCrseScCode.getCode(),
                        URLEncoder.encode(orgName),
                        URLEncoder.encode(loginType)
                )
        );
        JSONParser jsonParser = new JSONParser();
        JSONObject jsonObject = (JSONObject)jsonParser.parse(data);
        JSONArray schulList = (JSONArray) jsonObject.get("schulList");
        return schulList;
    }
    public JSONObject findUser(String orgCode, String name, String birthDay, String loginType) throws IOException, ParseException {
        JSONObject json = new JSONObject();
        json.put("orgCode", orgCode);
        json.put("name", name);
        json.put("birthday", birthDay);
        json.put("loginType", loginType);
        json.put("stdntPNo", null);
        String data = Http.Post(
                String.format("https://%s/v2/findUser", atptOfcdcConctUrl),
                json.toJSONString()
        );
        JSONParser jsonParser = new JSONParser();
        return (JSONObject)jsonParser.parse(data);
    }
    public boolean hasPassword(String token) throws IOException {
        String data = Http.PostToken(
                String.format("https://%s/v2/hasPassword", atptOfcdcConctUrl),
                "",
                token
        );
        return Boolean.getBoolean(data);
    }
    public boolean validatePassword(String token, String password) throws IOException {
        JSONObject json = new JSONObject();
        json.put("deviceUuid", "");
        json.put("password", Encrypt.encode(password, publicKey));
        String data = Http.PostToken(
                String.format("https://%s/v2/hasPassword", atptOfcdcConctUrl),
                json.toJSONString(),
                token
        );
        return Boolean.getBoolean(data);
    }
    public JSONArray selectUserGroup(String token) throws IOException, ParseException {
        String data = Http.PostToken(
                String.format("https://%s/v2/selectUserGroup", atptOfcdcConctUrl),
                "",
                token
        );
        JSONParser jsonParser = new JSONParser();
        return (JSONArray) jsonParser.parse(data);
    }
    public JSONObject getUserInfo(String token, String orgCode, String userPNo) throws IOException, ParseException {
        JSONObject json = new JSONObject();
        json.put("orgCode", orgCode);
        json.put("userPNo", userPNo);
        String data = Http.PostToken(
                String.format("https://%s/v2/getUserInfo", atptOfcdcConctUrl),
                json.toJSONString(),
                token
        );
        JSONParser jsonParser = new JSONParser();
        return (JSONObject) jsonParser.parse(data);
    }
    public String registerServey(String token) throws IOException {
        System.out.print(Console.ANSI_YELLOW);
        System.out.print("Deprecated warning in v2.registerServey function\n-   v1 의 registerServey 를 사용해주시기 바랍니다.");
        System.out.println(Console.ANSI_RESET);
        v1 v1 = new v1();
        v1.atptOfcdcConctUrl = this.atptOfcdcConctUrl;
        return v1.servey(token);
    }
}
