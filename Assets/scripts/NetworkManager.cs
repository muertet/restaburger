using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkManager : Photon.MonoBehaviour
{
	public static PhotonView myPhotonView;
	private static string defaultNick = "Simple Worker";
	const string GAME_VERSION = "0.1";

	void Start () {
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username", defaultNick);

		if (PhotonNetwork.connected == false && PhotonNetwork.connectionStateDetailed != PeerState.Joining) {
			Text nick = GameObject.Find("Nick").GetComponentInChildren<Text>();
			nick.text = PlayerPrefs.GetString("Username", defaultNick);
		}
	}

	void OnDestroy() {

		string name = defaultNick; // prevent saving an empty nick after playing offline

		if (PhotonNetwork.player.name != "") {
			name = PhotonNetwork.player.name;
		}

		PlayerPrefs.SetString("Username", name);
	}
	
	void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(null);
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Chat chat = GetComponent<Chat>();
		chat.sendMessage(player.name + " left the game");
	}
	
	void OnJoinedRoom()
	{
		GameObject player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
		CharacterController controller = player.GetComponent<CharacterController>();
		controller.enabled = true;

		MouseLook camera = player.GetComponent<MouseLook>();
		camera.enabled = true;

		player.transform.FindChild ("Main Camera").gameObject.SetActive(true);
		((MonoBehaviour)player.GetComponent("PlayerMovement")).enabled = true;

		myPhotonView = player.GetComponent<PhotonView> ();
		Chat chat = GetComponent<Chat>();
		chat.sendMessage(PhotonNetwork.player.name + " joined the game");

		if (PhotonNetwork.isMasterClient) {
			float x = 0;
			// spawn initial resources
			for (int i = 1; i <= GameLogic.INITIAL_FOOD_AMOUNT; i++) {
				if (i <= 5) {
					x = -20.49959f;
				} else {
					x = -20.19352f;
				}

				PhotonNetwork.Instantiate ("Bun", new Vector3 (x, 1.714955f, -16.39905f), Quaternion.identity, 0);
				PhotonNetwork.Instantiate ("Topbun", new Vector3 (x, 1.714955f, -16.20897f), Quaternion.identity, 0);
				PhotonNetwork.Instantiate ("Patty", new Vector3 (x, 1.714955f, -15.69364f), Quaternion.identity, 0);
				PhotonNetwork.Instantiate ("Cheese", new Vector3 (x, 1.714955f, -15.36852f), Quaternion.identity, 0);
				PhotonNetwork.Instantiate ("Lettuce", new Vector3 (x, 1.714955f, -15.04493f), Quaternion.identity, 0);
				PhotonNetwork.Instantiate ("Bacon", new Vector3 (x, 1.714955f, -14.7199f), Quaternion.identity, 0);
			}
			for (int i = 1; i <= GameLogic.INITIAL_FOOD_AMOUNT; i++) {
				PhotonNetwork.Instantiate ("Plate", new Vector3 (-19.72985f, 1.714955f, -17.23131f), Quaternion.identity, 0);
			}

		}
	}

	private float perCent (float percent, float original) {
		return (original * percent) / 100;
	}

	public void playSingleplayer () {
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.JoinRandomRoom();
		GameObject.Find ("MainMenu").SetActive (false);
	}

	public void Connect() {
		GameObject.Find ("MainMenu").SetActive (false);
		PhotonNetwork.ConnectUsingSettings( GAME_VERSION );
	}

	public void openAboutPage () {
		Application.OpenURL("http://yelidmod.com/burger/");
	}

	public void updateNick () {
		string nick = GameObject.Find ("Nick").GetComponentInChildren<Text> ().text;
		if (nick == "") { return; }
		PhotonNetwork.player.name = nick;
	}
	public static void updateNick (string nick) {
		if (nick == "") { return; }
		GameObject.Find ("Nick").GetComponentInChildren<Text> ().text = nick;
		PhotonNetwork.player.name = nick;
	}

	void OnJoinedLobby () {
		//RoomInfo[] rooms = PhotonNetwork.GetRoomList();
		PhotonNetwork.JoinRandomRoom();
	}

	void OnGUI()
	{
		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined)
		{
			if (PhotonNetwork.player.name == "tester") {
				GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
			} else {
				GUILayout.Label("Earnings: "+ GameLogic.earnedMoney +"$");
			}

			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();

		
			switch (GameLogic.CURRENT_VIEW) {
				case GameLogic.ESC_MENU:
					Screen.lockCursor = false;
					if( GUILayout.Button("Resume") ) {
						Debug.Log("SD");
					}
					if (PhotonNetwork.isMasterClient) {
						if( GUILayout.Button("Manage players") ) {
							Debug.Log("SD");
						}
					}
					
					if( GUILayout.Button("Options") ) {
						Debug.Log("SD");
					}
					if( GUILayout.Button("Leave game") ) {
						Application.Quit();
					}
				break;
				case GameLogic.GAME:
				if (!GameLogic.showChat) {
					Screen.lockCursor = true;
				}
				break;
			}

			GUILayout.EndVertical();
			GUILayout.EndArea();
		} else {
			GUILayout.Label("V "+GAME_VERSION+" - "+PhotonNetwork.connectionStateDetailed.ToString());
		}
	}
}
