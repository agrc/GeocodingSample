import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.UnsupportedEncodingException;
import java.net.URL;
import java.net.URLEncoder;
import java.nio.charset.Charset;
import org.json.*;

//Requirements: org.json library is located in json-20141113.jar and was downloaded from http://mvnrepository.com/artifact/org.json/json/20141113


public class Main {

	public static void main(String[] args) {
		// TODO Auto-generated method stub
		String api_key="insert your api key here";
		String street="123 South Main Street";
		String zone="SLC";
		geoCoder(api_key,street, zone);
	}
	
	private static void geoCoder(String api_key,String street, String zone){
		//values to pass to service
		api_key=encodeURL(api_key);
		street=encodeURL(street);
		zone=encodeURL(zone);
		
		//Create service url
		String url ="http://api.mapserv.utah.gov/api/v1/geocode/"+street+"/"+zone+"?apiKey="+api_key;
		JSONObject jobj = getJSONFromURL(url);
		JSONObject results = jobj.getJSONObject("result");
		JSONObject location = results.getJSONObject("location");
		
		//Print values from JSONObject
		System.out.println("X:"+location.getDouble("x"));
		System.out.println("Y:"+location.getDouble("y"));
	}
	
	private static JSONObject getJSONFromURL(String url){
		try{
			//Read a string from url add it to a string builder
			InputStream is = new URL(url).openStream();
			BufferedReader reader = new BufferedReader(new InputStreamReader(is, Charset.forName("UTF-8")));
		    int buff;
			StringBuilder sb = new StringBuilder();
		    while((buff=reader.read()) != -1){
		    	sb.append((char)buff);
		    }
		    System.out.println(sb.toString());
		    //Create JSON object from string
		    JSONObject obj = new JSONObject(sb.toString());
		    is.close();
			return obj;
		}catch(IOException ex2){
			ex2.printStackTrace();
		}
		return null;
		
	}
	
	private static String encodeURL(String str){
		try {
			//Java's URLEncoder uses a "+" instead of "%20" which the service likes better.
			return URLEncoder.encode(str,"UTF-8").replaceAll("\\+", "%20");
		} catch (UnsupportedEncodingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		return str;
	}

}
