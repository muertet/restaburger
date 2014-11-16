using UnityEngine;
using System.Collections;

public class MenuPlate {
	public int[] ingredients;
	public int price = 0;
	
	public MenuPlate (int[] ingredients, int price) {
		this.ingredients = ingredients;
		this.price = price;
	}

	public string getImageName () {
		string name = "";
		
		foreach (int i in ingredients) {
			name += i.ToString();
		}
		return name;
	}
}
