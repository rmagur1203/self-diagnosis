import org.json.simple.JSONArray;
import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;

import javax.crypto.Cipher;
import javax.net.ssl.HttpsURLConnection;
import javax.print.DocFlavor;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.security.KeyFactory;
import java.security.PublicKey;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;

public class Diagnosis {
    private final String publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA81dCnCKt0NVH7j5Oh2+SGgEU0aqi5u6sYXemouJWXOlZO3jqDsHYM1qfEjVvCOmeoMNFXYSXdNhflU7mjWP8jWUmkYIQ8o3FGqMzsMTNxr+bAp0cULWu9eYmycjJwWIxxB7vUwvpEUNicgW7v5nCwmF5HS33Hmn7yDzcfjfBs99K5xJEppHG0qc+q3YXxxPpwZNIRFn0Wtxt0Muh1U8avvWyw03uQ/wMBnzhwUC8T4G5NclLEWzOQExbQ4oDlZBv8BM/WxxuOyu0I8bDUDdutJOfREYRZBlazFHvRKNNQQD2qDfjRz484uFs7b5nykjaMB9k/EJAuHjJzGs9MMMWtQIDAQAB";
    private String atptOfcdcConctUrl = "hcs.eduro.go.kr";
    private String orgCode;
    private String Token;

    public enum lctnScCodes {
        서울특별시(1),
        부산광역시(2),
        대구광역시(3),
        인천광역시(4),
        광주광역시(5),
        대전광역시(6),
        울산광역시(7),
        세종특별자치시(8),
        경기도(10),
        강원도(11),
        충청북도(12),
        충청남도(13),
        전라북도(14),
        전라남도(15),
        경상북도(16),
        경상남도(17),
        제주특별자치도(18);

        final private int code;

        private lctnScCodes(int code) {
            this.code = code;
        }
        public int getCode() {
            return code;
        }
    }
    public enum schulCrseScCodes {
        유치원(1),
        초등학교(2),
        중학교(3),
        고등학교(4),
        특수학교(5);

        final private int code;

        private schulCrseScCodes(int code) {
            this.code = code;
        }
        public int getCode() {
            return code;
        }
    }

    public String SchoolCode(lctnScCodes lctnScCode, schulCrseScCodes schulCrseScCode, String schulName) throws Exception {
        String data = Get(String.format("https://hcs.eduro.go.kr/v2/searchSchool?lctnScCode=%s&schulCrseScCode=%s&orgName=%s&currentPageNo=1", lctnScCode.getCode(), schulCrseScCode.getCode(), URLEncoder.encode(schulName)));
        JSONParser jsonParser = new JSONParser();
        JSONObject jsonObject = (JSONObject)jsonParser.parse(data);
        JSONArray schulList = (JSONArray) jsonObject.get("schulList");
        JSONObject schulFirst = (JSONObject) schulList.get(0);
        atptOfcdcConctUrl = (String) schulFirst.get("atptOfcdcConctUrl");
        orgCode = (String) schulFirst.get("orgCode");
        return orgCode;
    }
    public String LoginToken(String orgCode, String Name, String Birth) throws Exception {
        if (orgCode.isEmpty())
            orgCode = this.orgCode;
        JSONObject parameter = new JSONObject();
        parameter.put("orgcode", orgCode);
        parameter.put("name", encode(Name, publicKey));
        parameter.put("birthday", encode(Birth, publicKey));
        String url = String.format("https://%s/loginwithschool", atptOfcdcConctUrl);
        String data = Post(url, parameter.toString());
        System.out.println(data);
        JSONParser jsonParser = new JSONParser();
        JSONObject jsonObject = (JSONObject)jsonParser.parse(data);
        Token = (String) jsonObject.get("token");
        return Token;
    }
    public String Servey(String token) throws Exception {
        if (token.isEmpty())
            token = this.Token;
        String url = String.format("https://%s/registerServey", atptOfcdcConctUrl);
        String data = PostToken(url, "{\"rspns00\":\"Y\",\"rspns01\":\"1\",\"rspns02\":\"1\",\"rspns03\":null,\"rspns04\":null,\"rspns05\":null,\"rspns06\":null,\"rspns07\":\"0\",\"rspns08\":\"0\",\"rspns09\":\"0\",\"rspns10\":null,\"rspns11\":null,\"rspns12\":null,\"rspns13\":null,\"rspns14\":null,\"rspns15\":null,\"deviceUuid\":\"\"}", token);
        return data;
    }


    private final String USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
    private String Get(String targetUrl) throws Exception {
        URL url = new URL(targetUrl);
        HttpURLConnection con = (HttpURLConnection) url.openConnection();
        con.setRequestMethod("GET");
        con.setRequestProperty("User-Agent", USER_AGENT);
        BufferedReader in = new BufferedReader(new InputStreamReader(con.getInputStream()));
        String inputLine;
        StringBuffer response = new StringBuffer();
        while ((inputLine = in.readLine()) != null)
            response.append(inputLine);
        in.close();
        return response.toString();
    }
    private String Post(String targetUrl, String parameters) throws Exception {
        URL url = new URL(targetUrl);
        HttpURLConnection conn = (HttpURLConnection) url.openConnection();
        conn.setDoOutput(true);
        conn.setRequestMethod("POST");
        conn.setRequestProperty("Content-Type", "application/json");
        conn.setRequestProperty("User-Agent", USER_AGENT);
        conn.setRequestProperty("Origin", "https://hcs.eduro.go.kr");
        conn.setRequestProperty("Referer", "https://hcs.eduro.go.kr/");
        conn.setConnectTimeout(10000);
        conn.setReadTimeout(10000);
        OutputStream os = conn.getOutputStream();
        os.write(parameters.getBytes("UTF-8"));
        os.flush();
        BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream(), "UTF-8"));
        StringBuffer outResult = new StringBuffer();
        String inputLine = null;
        while ((inputLine = in.readLine()) != null) {
            outResult.append(inputLine);
        }
        conn.disconnect();
        return outResult.toString();
    }
    private String PostToken(String targetUrl, String parameters, String token) throws Exception {
        URL url = new URL(targetUrl);
        HttpURLConnection conn = (HttpURLConnection) url.openConnection();
        conn.setDoOutput(true);
        conn.setRequestMethod("POST");
        conn.setRequestProperty("Content-Type", "application/json");
        conn.setRequestProperty("User-Agent", USER_AGENT);
        conn.setRequestProperty("Authorization", token);
        conn.setRequestProperty("Origin", "https://hcs.eduro.go.kr");
        conn.setRequestProperty("Referer", "https://hcs.eduro.go.kr/");
        conn.setConnectTimeout(10000);
        conn.setReadTimeout(10000);
        OutputStream os = conn.getOutputStream();
        os.write(parameters.getBytes("UTF-8"));
        os.flush();
        BufferedReader in = new BufferedReader(new InputStreamReader(conn.getInputStream(), "UTF-8"));
        StringBuffer outResult = new StringBuffer();
        String inputLine = null;
        while ((inputLine = in.readLine()) != null) {
            outResult.append(inputLine);
        }
        conn.disconnect();
        return outResult.toString();
    }
    private String encode(String plainData, String stringPublicKey) {
        String encryptedData = null;
        try {
            KeyFactory keyFactory = KeyFactory.getInstance("RSA");
            byte[] bytePublicKey = Base64.getDecoder().decode(stringPublicKey.getBytes());
            X509EncodedKeySpec publicKeySpec = new X509EncodedKeySpec(bytePublicKey);
            PublicKey publicKey = keyFactory.generatePublic(publicKeySpec);
            Cipher cipher = Cipher.getInstance("RSA");
            cipher.init(Cipher.ENCRYPT_MODE, publicKey);
            byte[] byteEncryptedData = cipher.doFinal(plainData.getBytes());
            encryptedData = Base64.getEncoder().encodeToString(byteEncryptedData);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return encryptedData;
    }
}
