using UnityEngine;
using System.Collections;

public class GameLogic 
{
	public const int GAME = 1;
	public const int ESC_MENU = 2;
    
	public static int CURRENT_VIEW = 1;

	public const int INITIAL_FOOD_AMOUNT = 10; // of each type

	public static bool showChat = false;
	public static int earnedMoney = 0;


	public static void addMoney (int amount) {
		earnedMoney += amount;
	}

	public static void deductMoney (int amount) {
		earnedMoney -= amount;
	}

	public static void localAnnounce (string message) {
		Chat chat = GameObject.Find ("Multiplayer").GetComponent<Chat> ();
		chat.addMessage(message, "");
	}
}
