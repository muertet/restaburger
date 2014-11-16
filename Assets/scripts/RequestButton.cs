using UnityEngine;
using System.Collections;

public class RequestButton : Photon.MonoBehaviour {

	private static float lastRequest = 0;

	public static void requestFood () {

		if ((Time.time - lastRequest) <= 60 && lastRequest != 0) {
			return;
		}
		lastRequest = Time.time;

		if (GameLogic.earnedMoney < 40) {
			GameLogic.localAnnounce("You don't have enough money to request more food (needed 40$)");
			return;
		}
		GameLogic.deductMoney(40);

		GameObject button = GameObject.Find ("RequestButton").gameObject;
		Vector3 pushPos = new Vector3 (button.transform.localPosition.x, button.transform.localPosition.y-0.02f, button.transform.localPosition.z);
		button.transform.localPosition = pushPos;

		PhotonNetwork.Instantiate ("Truck", new Vector3 (-7.5007f, 0.5263865f, 6.423898f), Quaternion.identity, 0);

		backToPosition ();
	}
	static IEnumerator backToPosition () {
		GameObject button = GameObject.Find ("RequestButton").gameObject;
		for (float i = 0f; i <= 0.02; i += 0.01f) {
			Vector3 pushPos = new Vector3 (button.transform.localPosition.x, button.transform.localPosition.y + i, button.transform.localPosition.z);
			button.transform.localPosition = pushPos;
			yield return new WaitForSeconds(1);
		}
	}
}
