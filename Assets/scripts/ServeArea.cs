using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServeArea : MonoBehaviour {

	private bool served = false;

	public bool isServed () {
		return this.served;
	}

	public void restart () {
		this.served = false;
	}

	void OnCollisionEnter (Collision collision) {
		Plate plate = collision.gameObject.GetComponent<Plate>();
		
		if (plate == null || !plate.hasFood || plate.isHolded()) {return;}

		int price = 4;

		// delete the food from the plate
		foreach (Transform child in collision.gameObject.transform) {
			PhotonNetwork.Destroy(child.gameObject);
		}
		plate.hasFood = false;
		this.served = true;

		// spawn the bucks
		for (int i = 1; i <= price; i++) {
			PhotonNetwork.Instantiate ("Buck", new Vector3 (transform.position.x, transform.position.y + 0.20f, transform.position.z), Quaternion.identity, 0);
		}

		// score +1 to player that served it
		if (NetworkManager.myPhotonView.viewID == plate.getLastHolder()) {
			Kongregate.score("ServedClients", 1);
		}

	}
}
