using System.Collections;
using UnityEngine;

public class Grill : MonoBehaviour {

	private Material defaultMaterial;
	private int foodCount = 0;

	void Start () {
		if (!audio) {
			Debug.LogError("Grill requires an AudioSource");
			return;
		}
		audio.clip = Resources.Load ("AudioClip/Grill") as AudioClip;
		defaultMaterial = renderer.material;
	}

	void OnCollisionEnter (Collision collision) {

		// dont restart on each collision
		if (foodCount < 1) {
			audio.Play ();
			audio.loop = true;
			renderer.material = Resources.Load ("Materials/_Red_Glass_") as Material;
		}
		foodCount++;

		Food food = collision.gameObject.GetComponent<Food>();
		if (food != null) { food.cook (); }
	}

	void OnCollisionExit (Collision collision) {
		foodCount--;
		// stop only if there isnt food
		if (foodCount < 1) {
			audio.Stop();
			renderer.material = defaultMaterial;
		}


		Food food = collision.gameObject.GetComponent<Food>();
		if (food != null) { food.stopCooking (); }
	}
}
