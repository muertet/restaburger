using UnityEngine;
using System.Collections;

public class NetworkObject : Photon.MonoBehaviour {

	[RPC]

	public void setHold (Vector3 pos, Quaternion rot, int viewId) {

		if (rigidbody == null) { //horrible bug: user is trying to hold an object wich hasnt rigidbody
			return;
		}

		PhotonView player = PhotonView.Find (viewId);
		Component PetaTest = player.gameObject.transform.Find ("pickedView");
		PetaTest.transform.position = pos;
		PetaTest.transform.rotation = rot;
		transform.parent = PetaTest.transform;

		//transform.position = pos;
		//transform.rotation = rot;

		rigidbody.isKinematic = true;
		rigidbody.useGravity = false;


		// if its an ingredient/plate set the current holder
		Food food = GetComponent<Food>();
		Plate plate = GetComponent<Plate>();
		
		if (food != null) {
			food.setLastHolder(viewId);
		} else if (plate != null) {
			plate.setCurrentHolder(viewId);
		}
	}

	[RPC]
	public void unHold () {
		// if its an ingredient set the current holder
		Food food = GetComponent<Food>();
		Plate plate = GetComponent<Plate>();

		if (food != null) {
			food.setLastHolder(0);
		} else if (plate != null) {
			plate.unHold();
		}
		if (rigidbody != null) {
			rigidbody.isKinematic = false;
			rigidbody.useGravity = true;
		}

		transform.parent = null; //drop the object
	}

	[RPC]
	private void SetTexture(int playerID, string textureName)
	{
		PhotonView.Find(playerID).transform.renderer.material = Resources.Load("models/food/Materials/" + textureName) as Material;
	}

}
