using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {
	
	public int type = 0;
	private int cookTime = 0;
	private int lastHolder = 0;
	private List<int> ingredientList = new List<int>();
	public bool isFather = false;

	private bool fixRequired = false;

	public int ingredientAmount () {
		return ingredientList.Count;
	}

	public void setLastHolder (int holder) {
		this.lastHolder = holder;
	}

	public int getLastHolder () {
		return this.lastHolder;
	}

	public bool isHolded () {
		if (this.lastHolder != 0) {
			return true;
		}
		return false;
	}

	public int getType () {
		return this.type;
	}

	public void cook() {
		StartCoroutine("Cook");
	}

	public bool isOverCooked () {
		if (this.cookTime > FoodHelper.COOKED_TIME) {
			return true;
		} else {
			return false;
		}
	}

	public bool isCooked () {
		if (this.cookTime == FoodHelper.COOKED_TIME && this.cookTime > (FoodHelper.COOKED_TIME-5) ) {
			return true;
		} else {
			return false;
		}
	}

	IEnumerator Cook() {
		for (float f = 1f; f >= 0; f -= 0.1f) {
			Color c = renderer.material.color;
			c.a = f;
			c.r = f;
			//c.b = 0.98f;
			//c.r = 0.98f;

			renderer.material.color = c;
			this.cookTime += 5;

			yield return new WaitForSeconds(3);
		}
	}

	public void stopCooking () {
		StopCoroutine("Cook");
	}

	public bool hasTopBun () {
		foreach (int fType in this.ingredientList)
		{
			if (fType == FoodHelper.TYPE_TOPBUN)
			{
				return true;
			}
		}
		return false;
	}

	void Update () {
		// check if needs to be fixed
		if (this.fixRequired)
		{
			if (!this.isFather) { // ensure that is not the father

				// search for childs
				foreach (Transform child in gameObject.transform) {
					if (child.tag == "Stacked") {
						child.transform.parent = gameObject.transform.parent;
					}
				}
			}
			this.fixRequired = false;
		}
	}

	void OnCollisionEnter (Collision collision) {

		// if this food already has a top bun, stop adding more ingredients
		if (this.hasTopBun()) {
			return;
		}

		Food food = collision.gameObject.GetComponent<Food>();
		
		if (food == null) {return;}

		// dont do anything until player stops holding it
		if (this.getLastHolder() != 0 || food.getLastHolder() != 0) {
			return;
		}

		if (this.type == FoodHelper.TYPE_BUN) { // dont stack bun with bun
			
			if (this.ingredientAmount() < 1 && (food.type == FoodHelper.TYPE_BUN  || food.type == FoodHelper.TYPE_TOPBUN)) {
				return;
			}

			if (food.type != FoodHelper.TYPE_BUN && food.type != FoodHelper.TYPE_TOPBUN) {
				this.isFather = true;
			} else if (food.isFather && this.isFather) {
				food.isFather = false;
				food.fixRequired = true;
			}

			float tHeight = renderer.bounds.extents.y;

			if (!food.isFather) {
				collision.gameObject.transform.parent = transform;
				collision.gameObject.tag = "Stacked";
				
				Destroy(collision.gameObject.GetComponent<Rigidbody>());
			}

			// calculate hamburguer height for the new ingredient
			foreach (Transform child in gameObject.transform) {
				if (child.tag != "Stacked") {continue;}
				tHeight += child.renderer.bounds.extents.y;
			}
			Vector3 vec = new Vector3(0, tHeight, 0);

			collision.transform.rotation = (Quaternion.Euler(Mathf.Clamp(collision.transform.rotation.eulerAngles.x, -2.5f, 2.5f), collision.transform.rotation.eulerAngles.y, Mathf.Clamp(collision.transform.rotation.eulerAngles.z, -2.5f, 2.5f)));

			if (FoodHelper.needsToBeCentered(food.getType())) {
				collision.transform.position = base.transform.position + vec;
			} else {
				collision.transform.position = new Vector3(Mathf.Clamp(collision.transform.position.x, base.transform.position.x - (base.transform.localScale.x / 3f), base.transform.position.x + (base.transform.localScale.x / 3f)), base.transform.position.y+tHeight, Mathf.Clamp(collision.transform.position.z, base.transform.position.z - (base.transform.localScale.z / 3f), base.transform.position.z + (base.transform.localScale.z / 3f)));
			}

			ingredientList.Add(food.getType());
		}
	}
}
