using UnityEngine;
using System.Collections;

public class CashRegister : MonoBehaviour {

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name != "Buck(Clone)") {return;}

		PhotonNetwork.Destroy(other.gameObject);
		GameLogic.addMoney(1);
	}
}
