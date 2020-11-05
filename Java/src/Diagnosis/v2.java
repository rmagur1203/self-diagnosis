package Diagnosis;

import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import java.io.IOException;
import java.net.URLEncoder;

public class v2 {
    private static final String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
    public String atptOfcdcConctUrl = "hcs.eduro.go.kr";
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
        if (schulList.size() == 1)
            this.atptOfcdcConctUrl = (String)((JSONObject)schulList.get(0)).get("atptOfcdcConctUrl");
        return schulList;
    }
    public JSONObject findUser(String orgCode, String name, String birthDay) throws IOException, ParseException {
        return findUser(orgCode, name, birthDay, "school");
    }
    public JSONObject findUser(String orgCode, String name, String birthDay, String loginType) throws IOException, ParseException {
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
        JSONObject json = new JSONObject();
        json.put("orgCode", orgCode);
        json.put("name", Encrypt.encode(name, publicKey));
        json.put("birthday", Encrypt.encode(birthDay, publicKey));
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
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
        String data = Http.PostToken(
                String.format("https://%s/v2/hasPassword", atptOfcdcConctUrl),
                "",
                token
        );
        return Boolean.valueOf(data);
    }
    public boolean validatePassword(String token, String password) throws IOException {
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
        JSONObject json = new JSONObject();
        json.put("deviceUuid", "");
        json.put("password", Encrypt.encode(password, publicKey));
        String data = Http.PostToken(
                String.format("https://%s/v2/hasPassword", atptOfcdcConctUrl),
                json.toJSONString(),
                token
        );
        return Boolean.valueOf(data);
    }
    public JSONArray selectUserGroup(String token) throws IOException, ParseException {
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
        String data = Http.PostToken(
                String.format("https://%s/v2/selectUserGroup", atptOfcdcConctUrl),
                "",
                token
        );
        JSONParser jsonParser = new JSONParser();
        return (JSONArray) jsonParser.parse(data);
    }
    public JSONObject getUserInfo(String token, String orgCode, String userPNo) throws IOException, ParseException {
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
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
        if (atptOfcdcConctUrl.equals("hcs.eduro.go.kr"))
            throw new IOException("atptOfcdcConctUrl 을 searchSchool의 atptOfcdcConctUrl 로 바꿔야 합니다.");
        if (token.isEmpty())
            token = this.token;
        String url = String.format("https://%s/registerServey", atptOfcdcConctUrl);
        String data = Http.PostToken(url, Json.normalServey, token);
        return data;
    }
}
