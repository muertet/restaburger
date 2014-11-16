using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodHelper : MonoBehaviour {

	public const int TYPE_BACON = 1;
	public const int TYPE_CHEESE = 2;
	public const int TYPE_TOMATO = 3;
	public const int TYPE_LETTUCE = 4;
	public const int TYPE_PATTY = 5;
	public const int TYPE_TOPBUN = 6;
	public const int TYPE_BUN = 7;

	public const int COOKED_TIME = 20;

	private static string[] conversion = new string[8];

	public static bool needsToBeCentered (int type) {
		if (type == TYPE_CHEESE || type == TYPE_LETTUCE || type == TYPE_TOPBUN || type == TYPE_PATTY || type == TYPE_BUN) {
			return true;
		} else {
			return false;
		}
	}

	public static string getPrefabName (int type) {
		if (conversion [1] == null) {
			conversion [1] = "Bacon";
			conversion [2] = "Cheese";
			conversion [3] = "Tomatoe";
			conversion [4] = "Lettuce";
			conversion [5] = "Patty";
			conversion [6] = "Topbun";
			conversion [7] = "Bun";
		}

		return conversion [type];
	}

	public static List<MenuPlate> getMenu () {
		List<MenuPlate> menu = new List<MenuPlate>();

		menu.Add (new MenuPlate(new int[]{
			TYPE_TOPBUN,
			TYPE_PATTY,
			TYPE_BUN,
		}, 4));
		menu.Add (new MenuPlate(new int[]{
			TYPE_TOPBUN,
			TYPE_CHEESE,
			TYPE_PATTY,
			TYPE_BUN,
		}, 4));
		menu.Add (new MenuPlate(new int[]{
			TYPE_TOPBUN,
			TYPE_PATTY,
			TYPE_CHEESE,
			TYPE_PATTY,
			TYPE_CHEESE,
			TYPE_BUN,
		}, 4));

		return menu;
	}
}
