package Diagnosis;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

public class Http {
    public static final String USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.83 Safari/537.36";
    public static String Get(String targetUrl) throws IOException {
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
    public static String Post(String targetUrl, String parameters) throws IOException {
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
    public static String PostToken(String targetUrl, String parameters, String token) throws IOException {
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
}
