using UnityEngine;
using System.Collections;

public class Crate : MonoBehaviour {

	public void open () {
		// decide what will spawn
		int tAmount = 3; // amount of ingredient types per crate
		int iAmount = 5; // amount of ingredients of each type per crate
		int rand = 0;
		Transform spawn = gameObject.transform;

		for (int t = 1; t <= tAmount; t++) {
			rand = Random.Range(1, 7);
			for (int i = 1; i <= iAmount; i++) {
				PhotonNetwork.Instantiate (FoodHelper.getPrefabName(rand), new Vector3 (spawn.position.x, spawn.position.y + 0.20f, spawn.position.z), Quaternion.identity, 0);
			}
		}

		PhotonNetwork.Destroy(gameObject);
	}
}
