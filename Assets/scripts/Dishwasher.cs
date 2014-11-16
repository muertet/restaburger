using UnityEngine;
using System.Collections;

public class Dishwasher : MonoBehaviour {
	
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name != "Plate(Clone)") {return;}

		other.gameObject.renderer.material = Resources.Load("Food/Plate") as Material;
	}
}
