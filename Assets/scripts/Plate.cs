using UnityEngine;
using System.Collections;

public class Plate : MonoBehaviour {
	
	public bool hasFood = false;
	private int lastHolder = 0;
	private int currentHolder = 0;

	public bool isHolded () {
		if (this.currentHolder != 0) {
			return true;
		}
		return false;
	}

	public int getLastHolder () {
		return this.lastHolder;
	}

	public void setCurrentHolder (int holder) {
		this.currentHolder = holder;
		this.lastHolder = holder;
	}

	public void unHold () {
		this.currentHolder = 0;
	}

	void OnCollisionEnter (Collision collision) {
		Food food = collision.gameObject.GetComponent<Food>();

		if (food == null || food.type != FoodHelper.TYPE_BUN || food.ingredientAmount() < 2 || food.isHolded()) {return;}

		collision.gameObject.transform.parent = transform;
		collision.gameObject.tag = "Stacked";
		
		Destroy(collision.gameObject.GetComponent<Rigidbody>());

		// center food
		collision.transform.rotation = (Quaternion.Euler(Mathf.Clamp(collision.transform.rotation.eulerAngles.x, -2.5f, 2.5f), collision.transform.rotation.eulerAngles.y, Mathf.Clamp(collision.transform.rotation.eulerAngles.z, -2.5f, 2.5f)));
		collision.transform.position = new Vector3(transform.position.x, transform.position.y+0.01f, transform.position.z);


		transform.renderer.material = Resources.Load("Food/PlateDirty") as Material;
		hasFood = true;
	}
}
