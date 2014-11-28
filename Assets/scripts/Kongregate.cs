using UnityEngine;
using System.Collections;

public class Kongregate : MonoBehaviour {
	private static bool isLoaded = false;
	private static int uid = 0;
	//private static string auth_token = "";
	
	void Start () {
		Application.ExternalEval("if(typeof(kongregateUnitySupport) != 'undefined') { kongregateUnitySupport.initAPI('Kongregate', 'OnKongregateAPILoaded');};");
		Application.ExternalEval(
			"kongregate.services.addEventListener('login', function(){" +
			"	var services = kongregate.services;" +
			"	var params=[services.getUserId(), services.getUsername(), services.getGameAuthToken()].join('|');" +
			"	kongregateUnitySupport.getUnityObject().SendMessage('Kongregate', 'OnKongregateAPILoaded', params);" + 
			"});"
		);
	}
	public void OnKongregateAPILoaded (string userInfo) {
		isLoaded = true;
		
		string[] uData = userInfo.Split("|"[0]);
		uid = int.Parse(uData[0]);
		NetworkManager.updateNick(uData[1]);
		//auth_token = uData[2];
	}
	public static bool isAPILoaded ()
	{
		if (uid != 0 && isLoaded) {
			return true;
		} else {
			return false;
		}
	}
	public static void score (string name, int value) {
		if (!isAPILoaded()) { return;}
		
		Application.ExternalCall("kongregate.stats.submit", name, value);
	}
}